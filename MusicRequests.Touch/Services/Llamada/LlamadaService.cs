using MusicRequests.Core.Services;
using System;
using Foundation;
using UIKit;

namespace MusicRequests.Touch.Services
{
    public class LlamadaService : ILlamadaService
    {
        public void MarcarNumTelefono(string numeroTelefono)
        {
			string callURI = string.Format("telprompt:{0}", numeroTelefono.Replace(" ", ""));
			var url =   NSUrl.FromString(callURI);

			if (!UIApplication.SharedApplication.OpenUrl(url))
			{
				var av = new UIAlertView("No soportado",
				  "Las llamadas no estan activadas en este dispositivo",
				  null,
				  "OK",
				  null);
				av.Show();
			};
        }
    }
}
