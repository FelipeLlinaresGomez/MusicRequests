using MvvmCross.Localization;
using MvvmCross;
using MvvmCross.ViewModels;
using System;
using MusicRequests.Core.Helpers;
using Newtonsoft.Json;

namespace MusicRequests.Core.ViewModels.Base
{
    public class ParcialBaseViewModel : MvxViewModel
    {
        [JsonIgnore]
        protected readonly IMvxTextProvider _textProvider;

        [JsonIgnore]
        protected readonly IFormatProvider _formatProvider;

        public ParcialBaseViewModel()
        {
            _textProvider = Mvx.IoCProvider.Resolve<IMvxTextProvider>();

            var service = Mvx.IoCProvider.Resolve<Services.ILocalizationService>();
            _formatProvider = new System.Globalization.CultureInfo(service.GetCurrentCulture());
        }

        public string GetLocalizedText(string key)
        {
            return _textProvider.GetText(Constants.GeneralNamespace, GetType().Name, key);
        }

        public string GetLocalizedSharedText(string key)
        {
            return _textProvider.GetText(Constants.GeneralNamespace, Constants.Shared, key);
        }

        [JsonIgnore]
        public IMvxLanguageBinder TextSource
        {
            get { return new MvxLanguageBinder(Constants.GeneralNamespace, GetType().Name); }
        }

        [JsonIgnore]
        public IMvxLanguageBinder SharedTextSource
        {
            get { return new MvxLanguageBinder(Constants.GeneralNamespace, Constants.Shared); }
        }
    }
}
