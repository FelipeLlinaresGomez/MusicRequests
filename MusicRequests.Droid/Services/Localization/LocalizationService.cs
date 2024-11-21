using System;
using MusicRequests.Core.Services;

namespace MusicRequests.Droid.Services
{
    public class LocalizationService : ILocalizationService
    {
        public LocalizationService()
        {
        }

        public string GetCurrent()
        {
			//var language = Java.Util.Locale.Default;
			//return language.Language;
			return "es";
        }

        public string GetCurrentCulture()
        {
			//var language = Java.Util.Locale.Default;
			//return $"{language.Language}-{language.Country}";

            return "es-ES";
        }
	}
}