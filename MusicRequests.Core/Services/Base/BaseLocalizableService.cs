using MusicRequests.Core.Helpers;
using MvvmCross;
using MvvmCross.Localization;

namespace MusicRequests.Core.Services.Base
{
    public abstract class BaseLocalizableService
    {
        public string GetLocalizedText(string key) => GetText(GetType().Name, key);

        public string GetLocalizedSharedText(string key) => GetText(Constants.Shared, key);

        private string GetText(string typeKey, string key)
        {
            var textProvider = Mvx.IoCProvider.Resolve<IMvxTextProvider>();
            return textProvider.GetText(Constants.GeneralNamespace, typeKey, key);
        }
    }
}