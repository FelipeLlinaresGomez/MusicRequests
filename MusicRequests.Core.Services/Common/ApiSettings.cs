using System;
using System.Threading;

namespace MusicRequests.Core.Services
{
	public static class ApiSettings
	{
		public static string SessionId = "00000000";
		public static CancellationTokenSource CancelTokenSource;//generic cancellation token to cancel any async calls

        public static string Entorno;
		public static string EntornoToShow;
		public static string APIBaseUrl;
		public static string APIDomain;
		public static int APIPort;
		public static string MusicRequestsAPIBaseUrl;
		public static string MusicRequestsIdentidadBaseUrl;
		public static string MusicRequestsAPIDomain;

		public static void CancelApiCalls ()
		{
			ApiSettings.CancelTokenSource.Cancel ();
			ApiSettings.CancelTokenSource = new CancellationTokenSource ();
		}

		public static void IniciarEntornos()
		{

#if DEBUG
            EntornoToShow = Entornos.PruebasEntornoToShow;
            Entorno = Entornos.PruebasEntorno;
            APIBaseUrl = Entornos.PruebasBaseUrl;
            APIDomain = Entornos.PruebasAPIDomain;
            APIPort = Entornos.PruebasPort;

			MusicRequestsAPIBaseUrl = Entornos.PruebasMusicRequestsAPIBaseUrl;
			MusicRequestsIdentidadBaseUrl = Entornos.PruebasMusicRequestsIdentidadBaseUrl;
			MusicRequestsAPIDomain = Entornos.PruebasMusicRequestsAPIDomain;;
#else
            EntornoToShow = Entornos.ProduccionEntornoToShow;
			Entorno = Entornos.ProduccionEntorno;
			APIBaseUrl = Entornos.ProduccionBaseUrl;
			APIDomain = Entornos.ProduccionAPIDomain;
			APIPort = Entornos.ProduccionPort;

			MusicRequestsAPIBaseUrl = Entornos.ProduccionMusicRequestsAPIBaseUrl;
			MusicRequestsIdentidadBaseUrl = Entornos.ProduccionMusicRequestsIdentidadBaseUrl;
			MusicRequestsAPIDomain = Entornos.ProduccionMusicRequestsAPIDomain;
#endif
		}
	}
}

