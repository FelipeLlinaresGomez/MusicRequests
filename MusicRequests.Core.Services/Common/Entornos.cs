namespace MusicRequests.Core.Services
{
	public static class Entornos
	{
		// Pruebas
		public const string PruebasEntorno = "Entorno: PRUEBAS";
		public const string PruebasEntornoToShow = "PRUEBAS";
		public const string PruebasBaseUrl = "https://epypru.musicrequestsdirecto.com/";
		public const string PruebasAPIDomain = "epypru.musicrequestsdirecto.com";
		public const int PruebasPort = 443;

		public static readonly string PruebasMusicRequestsAPIBaseUrl = "https://ebepru.musicrequests.es/omnicanalidad/canales/bancamovil/apigateway/v1/";
		public static readonly string PruebasMusicRequestsIdentidadBaseUrl = "https://identidadpru.musicrequests.es/soporte/plataforma/identidad/api/v1/";
		public static readonly string PruebasMusicRequestsAPIDomain = "https://ebepru.musicrequests.es/";

		// Produccion
		public const string ProduccionEntorno = "Entorno: PRODUCCIÓN";
		public const string ProduccionEntornoToShow = "PRODUCCIÓN";
		public const string ProduccionBaseUrl = "https://epy.musicrequestsdirecto.com/";
		public const string ProduccionAPIDomain = "epy.musicrequestsdirecto.com";
		public const int ProduccionPort = 443;

		public static readonly string ProduccionMusicRequestsAPIBaseUrl = "https://ebe.musicrequests.es/omnicanalidad/canales/bancamovil/apigateway/v1/";
		public static readonly string ProduccionMusicRequestsIdentidadBaseUrl = "https://identidad.musicrequests.es/soporte/plataforma/identidad/api/v1/";
		public static readonly string ProduccionMusicRequestsAPIDomain = "https://ebe.musicrequests.es/";
	}
}
