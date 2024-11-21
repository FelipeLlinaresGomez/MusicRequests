using System;
using MusicRequests.Core.Services;
using Foundation;
using System.Linq;

namespace MusicRequests.Touch.Services
{
	public class LocalizationService : ILocalizationService
	{
		string culture;

		public string GetCurrent ()
		{
			//var iosLanguage = NSLocale.AutoUpdatingCurrentLocale.LanguageCode;
			//return iosLanguage;
			return "es";
		}


		public string GetCurrentCulture ()
		{
			//if (string.IsNullOrEmpty (culture)) {

			//	var locale = $"{NSLocale.AutoUpdatingCurrentLocale.LanguageCode}-{NSLocale.AutoUpdatingCurrentLocale.CountryCode}";
			//	var result = Array.FindLast (NSLocale.AvailableLocaleIdentifiers, s => s.Equals (NSLocale.CurrentLocale.Identifier));

			//	if (string.IsNullOrEmpty (result))
			//		culture = "es-ES";
			//	else
			//		culture = locale;
					
			//}

			return "es-ES";

		}
	}
}

