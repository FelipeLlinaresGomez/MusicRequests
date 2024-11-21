using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MusicRequests.Core.Helpers;
using MusicRequests.Core.ViewModels;
using MvvmCross.IoC;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using Newtonsoft.Json;

namespace MusicRequests.Core.Managers
{
    public class NotificationsDeepLinkManager : INotificationsDeepLinkManager
    {
        private class DeeplinkTransaction
        {
            // Tipo scheme://notification/nombre_viewmodel(sin ViewModel al final)?params=
            public Uri Uri { get; set; }
            public Type Viewmodel { get; set; }
            public string Payload { get; set; }
        }

        private readonly IMvxNavigationService _navigationService = null;
        private readonly IMvxTextProvider _textProvider = null;

        public NotificationsDeepLinkManager(IMvxNavigationService navigationService, IMvxTextProvider textProvider)
        {
            _navigationService = navigationService;
            _textProvider = textProvider;
            Reset();
        }

        private DeeplinkTransaction PendingTransaction { get; set; }

        public void Reset()
        {
            PendingTransaction = null;
        }

        public async Task CrearTransaccionDeeplink(Uri uri, string payloadData)
        {
            if (!ValidateUri(uri))
            {
                await NavegacionUsuarioNoActivo();
            }
            else
            {
                PendingTransaction = new DeeplinkTransaction
                {
                    Uri = uri,
                    Viewmodel = GetViewModelTypeByName(uri.AbsolutePath.Replace("/", "")),
                    Payload = payloadData
                };
            }
        }

        public bool DeeplinkPendiente()
        {
            return PendingTransaction != null;
        }

        private bool ValidateUri(Uri uri)
        {
            bool isValid = false;

            if (App.IsAndroid())
            {
                isValid = uri.Scheme.Equals("musicrequests.pay.prod") || uri.Scheme.Equals("musicrequests.pay.pruebas");
            }

            if (App.IsiOS())
            {
                isValid = uri.Scheme.StartsWith("musicrequests.pay.", StringComparison.OrdinalIgnoreCase);
            }

            // se soporta deeplink de tipo notification y de shortcuts
            isValid = isValid &&
                (uri.Host.Equals("notification") || uri.Host.Equals("shortcuts"));

            isValid = isValid && !string.IsNullOrEmpty(uri.PathAndQuery);

            return isValid;
        }

        public async Task ProcesaDeeplinkPendiente(bool borrarTrasProcesar = true)
        {
            if (DeeplinkPendiente())
            {
                //if (App.User != null)
                //{
                //    await NavegacionUsuarioActivo();
                //}
                //else
                //{
                //    await NavegacionUsuarioNoActivo();
                //}

                // Vaciamos los datos ya utilizados para evitar reutilizarse
                if (borrarTrasProcesar)
                {
                    Reset();
                }
            }
        }

        private async Task NavegacionUsuarioNoActivo()
        {
            if (PendingTransaction == null)
            {
                if (App.IsAndroid())
                {
                    var parameters = new LoginViewModelArgs
                    {
                        EsArranqueApp = true
                    };
                    await _navigationService.Navigate<LoginViewModel, LoginViewModelArgs>(parameters);
                }
                return;
            }

            if (!string.IsNullOrEmpty(PendingTransaction.Payload))
            {
                ComprobarMetadata(PendingTransaction.Payload);
            }

            if (PendingTransaction.Viewmodel != null)
            {
                var viewmodelANavegar = PendingTransaction.Viewmodel;

                if (!EsNavegacionEspecial(viewmodelANavegar))
                {
                    if (App.IsAndroid())
                    {
                        var parameters = new LoginViewModelArgs
                        {
                            EsArranqueApp = true
                        };
                        await _navigationService.Navigate<LoginViewModel, LoginViewModelArgs>(parameters);
                    }
                }
                else
                {
                    await NavegacionEspecialOffline(viewmodelANavegar);
                }
            }
            else
            {
                if (App.IsAndroid())
                {
                    var parameters = new LoginViewModelArgs
                    {
                        EsArranqueApp = true
                    };
                    await _navigationService.Navigate<LoginViewModel, LoginViewModelArgs>(parameters);
                }
            }
        }

        private async Task NavegacionUsuarioActivo()
        {
            if (!string.IsNullOrEmpty(PendingTransaction.Payload))
            {
                ComprobarMetadata(PendingTransaction.Payload);
            }

            if (PendingTransaction.Viewmodel != null)
            {
                var viewmodelANavegar = PendingTransaction.Viewmodel;

                if (!EsNavegacionEspecial(viewmodelANavegar))
                {
                    Reset();
                    await _navigationService.Navigate(viewmodelANavegar);
                }
                else
                {
                    await NavegacionEspecial(viewmodelANavegar);
                }
            }
            else
            {
                await _navigationService.Navigate<HomeViewModel>();
            }
        }

        private void ComprobarMetadata(string payload)
        {
            //var real = JsonConvert.DeserializeObject<BaseNotificationMetadata>(payload);
            //switch (real.Action)
            //{
            //    case NotificationAction.SONG_RECEIVED:
            //        PendingTransaction.Viewmodel = typeof(DetalleCancionViewModel);
            //        break;
            //    default:
            //        break;
            //}
        }


        /// <summary>
        /// Devuelve si es un viewmodel condicional, como mis finanzas que podria necesitar activarse primero antes de acceder.
        /// </summary>
        /// <returns> true si es un viewmodel que necesita controlarse</returns>
        /// <param name="viewmodel">Viewmodel.</param>
        private bool EsNavegacionEspecial(Type viewmodel)
        {
            //return viewmodel == typeof(DetalleMovimientoBizumViewModel);
            return false;
        }

        private async Task NavegacionEspecial(Type viewmodel)
        {
            //if (viewmodel == typeof(DetalleMovimientoBizumViewModel))
            //{
            //    var objReal = JsonConvert.DeserializeObject<BaseNotificationMetadata>(PendingTransaction.Payload);
            //    var metadata = JsonConvert.DeserializeObject<BaseNotificationMetadata>(objReal.Metadata);

            //    if (metadata.Action == NotificationAction.SONG_SENT) // Solicitud de Pago
            //    {
            //        var parameters = new DetalleMovimientoBizumViewModelArgs
            //        {
            //        };
            //        await _navigationService.Navigate<DetalleMovimientoBizumViewModel, DetalleMovimientoBizumViewModelArgs>(parameters);
            //    }
            //    else if (metadata.Action == NotificationAction.SONG_RECEIVED)
            //    {
            //        var parameters = new BizumIDInicioSesionFirmaViewModelArgs()
            //        {
            //        };
            //        await _navigationService.Navigate<BizumIDInicioSesionFirmaViewModel, BizumIDInicioSesionFirmaViewModelArgs>(parameters);
            //    }
            //    else
            //    {
            //        Reset();
            //        await _navigationService.Navigate<HomeViewModel>();
            //    }

            //    // Reseteamos el deeplink pendiente para no mostrarlo mas
            //    Reset();
            //}
        }

        private async Task NavegacionEspecialOffline(Type viewmodel)
        {
            // Sin casos todavia. Habria que comprobar si es el tipo que se busca y hacer un Reset() al final para marcarlo como procesado

            if (App.IsAndroid())
            {
                var parameters = new LoginViewModelArgs
                {
                    EsArranqueApp = true
                };
                await _navigationService.Navigate<LoginViewModel, LoginViewModelArgs>(parameters);
            }
        }

        private Type GetViewModelTypeByName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string viewModelName = name;

                if (!name.ToLowerInvariant().EndsWith("viewmodel"))
                {
                    viewModelName = name + "ViewModel";
                }

                Assembly assembly = GetType().GetTypeInfo().Assembly;

                return assembly.CreatableTypes()
                               .EndingWith("ViewModel")
                               .FirstOrDefault(t => t.Name.Equals(viewModelName, StringComparison.OrdinalIgnoreCase));
            }
            return null;
        }

        public string GetLocalizedSharedText(string key)
        {
            return _textProvider.GetText(Constants.GeneralNamespace, Constants.Shared, key);
        }
    }
}