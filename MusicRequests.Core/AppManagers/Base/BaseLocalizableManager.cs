using System;
using MvvmCross.Localization;
using MvvmCross;
using MusicRequests.Core.Helpers;

namespace MusicRequests.Core.AppManagers.Base
{
    public class BaseLocalizableManager
    {
        public string GetLocalizedText(Type viewModelType, string key)
        {
            var textProvider = Mvx.IoCProvider.Resolve<IMvxTextProvider>();
            return textProvider.GetText(Constants.GeneralNamespace, viewModelType.Name, key);
        }

        public string GetLocalizedSharedText(string key)
        {
            var textProvider = Mvx.IoCProvider.Resolve<IMvxTextProvider>();
            return textProvider.GetText(Constants.GeneralNamespace, Constants.Shared, key);
        }
    }
}