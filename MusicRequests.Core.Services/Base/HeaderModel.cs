using System;

namespace MusicRequests.Core.Services
{
	public class HeaderModel
	{
		public string Canal { get; set; }
		public string Dispositivo { get; set; }
		public string Idioma { get; set; }
		public string AppID { get; set; }
		public string Usuario { get; set; }
		public string Version { get; set; }
		public string IMEI { get; set; }
    }
}