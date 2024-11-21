using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Collections.Generic;
using MusicRequests.Core.Services.Base;
using MusicRequests.Core.Models;

namespace MusicRequests.Core.Services
{
	public static class HttpClientHelper
	{
		// Por ahora solo para Android
		public static IPlatformHttpClientHandler PlatformHttpClientHandler;

		#region GetHttpClient
		private static HttpClient _ModernHttpClient;
		public static HttpClient GetHttpClient(bool dontUseModern = false)
		{
			if (_ModernHttpClient == null)
			{
				_ModernHttpClient = new HttpClient(PlatformHttpClientHandler.GetNativeHttpClientHandler());
				_ModernHttpClient.Timeout = TimeSpan.FromMinutes(3);
			}
			return _ModernHttpClient;
		}
		#endregion

		//HEADER para todas las peticiones
		//Campo	        Obligatorio		Tipo	Tamaño	Notas
		//Canal				No	        Texto	  3		Identificador de canal (por defecto MOV)
		//Dispositivo		Sí	        Texto			Dispositivo desde donde se realiza la conexión
		//Idioma			No	        Texto			Idioma (por defecto "es")
		//AppID				Sí	        Texto			Identificador de la aplicación
		//Usuario			No	        Texto			Código de identificación o DNI
		//Ticket			No	        Texto			Ticket de seguridad
		//Version			Si			Texto			Versión de la app


		#region AddHeaders

		public static HttpClient AddHeaders(this HttpClient client, HeaderModel headers)
		{
			client.AddHeader(HeaderKeys.AppID, headers.AppID ?? "MusicRequest");
			client.AddHeader(HeaderKeys.Canal, headers.Canal ?? "MOV");
			client.AddHeader(HeaderKeys.Dispositivo, headers.Dispositivo);
			client.AddHeader(HeaderKeys.Idioma, headers.Idioma ?? "es");
			client.AddHeader(HeaderKeys.version, headers.Version);

			var guid = Guid.NewGuid();
			if (client.DefaultRequestHeaders.Contains(HeaderKeys.correlationid))
				client.DefaultRequestHeaders.Remove(HeaderKeys.correlationid);
			client.DefaultRequestHeaders.Add(HeaderKeys.correlationid, $"{ApiSettings.SessionId}-{guid.ToString().Substring(0, 8)}");

			client.AddHeader(HeaderKeys.IMEI, headers.IMEI);

			return client;
		}

		public static HttpRequestHeaders AddHeaders(HttpRequestHeaders requestHeaders, HeaderModel headers)
		{
			requestHeaders.Add(HeaderKeys.AppID, headers.AppID ?? "MusicRequest");

			requestHeaders.Add(HeaderKeys.Canal, headers.Canal ?? "MOV");

			if (!string.IsNullOrEmpty(headers.Dispositivo))
				requestHeaders.Add(HeaderKeys.Dispositivo, headers.Dispositivo);

			requestHeaders.Add(HeaderKeys.Idioma, headers.Idioma ?? "es");

			requestHeaders.Add(HeaderKeys.version, headers.Version);

			var guid = Guid.NewGuid();
			requestHeaders.Add("correlationid", $"{ApiSettings.SessionId}-{guid.ToString().Substring(0, 8)}");

			requestHeaders.Add(HeaderKeys.IMEI, headers.IMEI);
			return requestHeaders;
		}

		public static HttpRequestHeaders AddMusicRequestsHeaders(HttpRequestHeaders requestHeaders, HeaderMusicRequests headerInfo, bool anonymousCall)
		{
			if (!anonymousCall)
			{
				if (string.IsNullOrEmpty(headerInfo.TokenIdentity) && string.IsNullOrEmpty(headerInfo.SessionToken))
				{
					throw new HttpMusicRequestException((int)ApiStatusCode.InvalidToken, "");
				}
				requestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", headerInfo.TokenIdentity);
				if (!string.IsNullOrEmpty(headerInfo.SessionToken))
				{
					requestHeaders.Add("x-musicrequests-st", headerInfo.SessionToken);
				}
			}

			requestHeaders.Add("CodigoUsuario", headerInfo.CodigoUsuario);
            requestHeaders.Add("NICI", headerInfo.NICI);
            requestHeaders.Add("EsNegocio", headerInfo.EsNegocio.ToString());

			return requestHeaders;
		}
		#endregion

		#region AddHeader
		public static HttpClient AddHeader(this HttpClient client, string key, string value)
		{
			if (!client.DefaultRequestHeaders.Contains(key))
			{
				if (!string.IsNullOrEmpty(value))
					client.DefaultRequestHeaders.Add(key, value);
			}
			return client;
		}
		#endregion

		#region AddHeaderApiVersion

		public static HttpClient AddHeaderApiVersion(this HttpClient client, string version = "1")
		{
			client.ForceHeaderUpdate(HeaderKeys.ApiVersion, version);
			return client;
		}

		#endregion

		#region AddSecurityHeaders

		public static HttpClient AddSecurityHeaders(this HttpClient client, string ticket, string userCode, string codigoContrato)
		{
			if (string.IsNullOrEmpty(ticket))
				throw new Exception("Request must have a non empty value for Ticket");

			return client.AddHeader(HeaderKeys.Ticket, ticket)
						 .AddHeader(HeaderKeys.Usuario, userCode)
						 .AddHeader(HeaderKeys.contrato, codigoContrato);
		}

		#endregion

		#region RemoveHeader
		public static HttpClient RemoveHeader(this HttpClient client, string key)
		{
			client.DefaultRequestHeaders.Remove(key);
			return client;
		}
		#endregion

		#region RemoveSecurityHeaders
		public static HttpClient RemoveSecurityHeaders(this HttpClient client)
		{
			return client.RemoveHeader("Ticket")
						  .RemoveHeader("Usuario")
						  .RemoveHeader("contrato")
						 .RemoveHeader(HeaderKeys.BizumSesionId)
						 .RemoveHeader(HeaderKeys.HceSesionId);
		}

		public static void RemoveSecurityHeaders()
		{
			_ModernHttpClient.RemoveSecurityHeaders();
		}
		#endregion

		#region ForceHeaderUpdate
		public static HttpClient ForceHeaderUpdate(this HttpClient client, string key, string value)
		{
			return client.RemoveHeader(key)
						 .AddHeader(key, value);
		}
		#endregion

		#region ExistsHeader
		public static bool ExistsHeader(this HttpClient client, string key)
		{
			return client.DefaultRequestHeaders.Contains(key);
		}
		#endregion

		private static readonly TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromMinutes(3);
		private static object _getHttpWithHandlerClientLock = new object();
		private static object _getHttpClientLock = new object();
		private static Dictionary<string, Tuple<HttpClient, HttpClientHandler>> _httpClientsWithHandler = new Dictionary<string, Tuple<HttpClient, HttpClientHandler>>();
		private static Dictionary<string, HttpClient> _httpClients = new Dictionary<string, HttpClient>();

		public static T CreateClient<T>(IMusicRequestsSessionHandler handler)
		{
			HttpClient httpClient = GetHttpClient(ApiSettings.MusicRequestsAPIBaseUrl, handler);

			var service = Refit.RestService.For<T>(httpClient);
			return service;
		}

		private static HttpClient GetHttpClient(string baseUri, IMusicRequestsSessionHandler handler)
		{
			var clave = baseUri;
			//Bloqueamos para no acceder en paralelo a los clientes y que si se tiene que generar sólo se haga una vez
			lock (_getHttpClientLock)
			{
				if (_httpClients.TryGetValue(clave, out HttpClient httpClient))
				{
					return httpClient;
				}
				else
				{
                    //El cliente no estaba generado, lo creamos y añadimos a la coleccion de httpclient
                    var MusicRequestsHandler = new MusicRequestsHandler(PlatformHttpClientHandler.GetNativeHttpClientHandler())
                    {
                        Handler = handler
					};

					httpClient = new HttpClient(MusicRequestsHandler)
					{ BaseAddress = new Uri(baseUri) };

					httpClient.Timeout = DEFAULT_TIMEOUT;

					_httpClients.Add(clave, httpClient);

					return httpClient;
				}
			}
		}

		public static HttpClient GetIdentityHttpClient()
		{
			var httpClientHandler = new HttpClientHandler()
			{
				//AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
				AllowAutoRedirect = false,
				//UseCookies = true,
				//CookieContainer = new CookieContainer()
			};
			HttpClient httpClient = GetPlainHttpClientCustomHandler(ApiSettings.MusicRequestsIdentidadBaseUrl, httpClientHandler);
			return httpClient;
		}

		public static (HttpClient, HttpClientHandler) GetIdentityLoginHttpClient(int timeoutMilliseconds)
		{
			var cookieContainer = new CookieContainer();
			var httpClientHandler = new HttpClientHandler()
			{
				//AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
				AllowAutoRedirect = false,
				UseCookies = true,
				CookieContainer = cookieContainer
			};
			var (httpClient, httpClientHandlerReturn) = GetPlainHttpClientAndCustomHandler(ApiSettings.MusicRequestsIdentidadBaseUrl, httpClientHandler, timeoutMilliseconds);
			return (httpClient, httpClientHandlerReturn);
		}

		private static (HttpClient, HttpClientHandler) GetPlainHttpClientAndCustomHandler(string baseUri, HttpClientHandler handler, int timeoutMilliseconds)
		{
			var timeout = timeoutMilliseconds != 0 ? timeoutMilliseconds : DEFAULT_TIMEOUT.TotalMilliseconds;

			var clave = $"plainwithcustomhandler_{baseUri}";
			//Bloqueamos para no acceder en paralelo a los clientes y que si se tiene que generar sólo se haga una vez
			lock (_getHttpWithHandlerClientLock)
			{
				if (_httpClientsWithHandler.TryGetValue(clave, out Tuple<HttpClient, HttpClientHandler> httpClientWithHandler))
				{
					return (httpClientWithHandler.Item1, httpClientWithHandler.Item2);
				}
				else
				{
					//El cliente no estaba generado, lo creamos y añadimos a la coleccion de httpclient

					var client = new HttpClient(handler);
					httpClientWithHandler = new Tuple<HttpClient, HttpClientHandler>(client, handler);

					client.Timeout = TimeSpan.FromMilliseconds(timeout);

					_httpClientsWithHandler.Add(clave, httpClientWithHandler);

					return (client, handler);
				}
			}
		}

		private static HttpClient GetPlainHttpClientCustomHandler(string baseUri, HttpClientHandler handler)
		{
			var clave = $"plaincustomhandler_{baseUri}";
			//Bloqueamos para no acceder en paralelo a los clientes y que si se tiene que generar sólo se haga una vez
			lock (_getHttpClientLock)
			{
				if (_httpClients.TryGetValue(clave, out HttpClient httpClient))
				{
					return httpClient;
				}
				else
				{
					//El cliente no estaba generado, lo creamos y añadimos a la coleccion de httpclient

					httpClient = new HttpClient(handler);

					httpClient.Timeout = DEFAULT_TIMEOUT;

					_httpClients.Add(clave, httpClient);

					return httpClient;
				}
			}
		}
	}

	public static class HeaderKeys
	{
		public const string AppID = "AppID";
		public const string Canal = "Canal";
		public const string DireccionIP = "DireccionIP";
		public const string Dispositivo = "Dispositivo";
		public const string Entidad = "Entidad";
		public const string Idioma = "Idioma";
		public const string version = "version";
		public const string PlayBackMode = "PlayBackMode";
		public const string correlationid = "correlationid";
		public const string IMEI = "IMEI";
		public const string Nfc = "Nfc";
		public const string HceSesionId = "HceSesionId";
		public const string Ticket = "Ticket";
		public const string Usuario = "Usuario";
		public const string contrato = "contrato";
		public const string ApiVersion = "X-Api-Version";
		public const string BizumSesionId = "BizumSesionId";
		public const string TokenMusicRequests = "tokenMusicRequests";
		public const string TokenSesionMusicRequests = "x-musicrequests-st";
	}
}

