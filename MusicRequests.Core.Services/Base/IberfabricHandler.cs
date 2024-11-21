using MusicRequests.Core.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicRequests.Core.Services.Base
{
    public class MusicRequestsHandler : DelegatingHandler
    {
        public IMusicRequestsSessionHandler Handler { get; set; }
        public MusicRequestsHandler()
        {
        }
        public MusicRequestsHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }
        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            System.Threading.CancellationToken cancellationToken)
        {
            //Si el endpoint lo etiquetamos como [Headers("Anonymous: true")] no comprobamos el token de identidad
            bool anonymousCall = request.Headers.Remove("Anonymous");

            HttpClientHelper.AddMusicRequestsHeaders(request.Headers, Handler.GetHeaders(), anonymousCall);
            DateTime timeStampStart = DateTime.Now;
           
            try
            {
#if DEBUG
                string input = request.Content != null ? await request.Content.ReadAsStringAsync() : null;
#endif
                var response = await base.SendAsync(request, cancellationToken);

                bool hayTokenSesion = response.Headers.TryGetValues("x-musicrequests-st", out var tokenSesion);
                bool hayTokenSesionExpiresIn = response.Headers.TryGetValues("x-musicrequests-st-expirein", out var tokenSesionExpireIn);
                if (hayTokenSesion && !string.IsNullOrEmpty(tokenSesion.FirstOrDefault()))
                {
                    if (hayTokenSesionExpiresIn && !string.IsNullOrEmpty(tokenSesionExpireIn.FirstOrDefault()))
                        Handler.SetSessionToken(tokenSesion.FirstOrDefault(),
                                                Int32.Parse(tokenSesionExpireIn.FirstOrDefault()));
                    else
                        Handler.SetSessionToken(tokenSesion.FirstOrDefault());
                }

                if (await SesionExpirada(response))
                {
                    Handler.SetSesionExpirada();
                    throw new MusicRequestBackendException()
                    {
                        FriendlyMessage = "La sesión ha expirado. Comprueba que no has iniciado sesión en otro dispositivo e inténtalo de nuevo",
                        Code = "0001",
                        Logout = true
                    };
                }

                throw new Exception();
            }
            catch (OperationCanceledException cancelEx)
            {
                Debug.WriteLine($"{cancelEx.Message} - {request.RequestUri}");
                cancelEx.Data.Add("request", $"{request.Method}: {request.RequestUri}");
                throw;
            }
            catch (MusicRequestBackendException bex)
            {
                Debug.WriteLine($"Reason: {bex.FriendlyMessage} Hidden Reason: {bex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ex.Data.Add("request", $"{request.Method}: {request.RequestUri}");
                throw;
            }
        }


        private async Task<bool> SesionExpirada(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                string content = await response.Content.ReadAsStringAsync();
                if (content == "19")
                {
                    // Si se devuelve un error 401, y en el content hay un "19" significa que ha expirado la sesion y hay que llamar al refresh y volverlo a intentar
                    return true;
                }
                else
                {
                    //Si no es "19" es que el token de identidad no es válido, lo consideramos como sesión expirada.
                    return true;
                }
            }
            return false;
        }
    }
}
