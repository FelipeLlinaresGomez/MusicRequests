using MvvmCross.Localization;
using MusicRequests.Core.Helpers;
using MvvmCross;
using Newtonsoft.Json;

namespace MusicRequests.Core.ViewModels
{
    public class BaseLocatizationViewModel : BaseLocatizationViewModel<string, bool>
    {
    }

    public class BaseLocatizationViewModel<TParameter> : BaseLocatizationViewModel<TParameter, bool>
    {
    }

    public class BaseLocatizationViewModel<TParameter, TResult> : MvxViewModelCustom<TParameter, TResult>, IBaseLocalizationViewModel
    {
        [JsonIgnore]
        protected readonly IMvxTextProvider _textProvider;

        [JsonIgnore]
        TParameter _inputParameter = default(TParameter);

        protected TParameter InputParameter => _inputParameter;

        public BaseLocatizationViewModel()
        {
            _textProvider = Mvx.IoCProvider.Resolve<IMvxTextProvider>();
        }

        #region ViewModel lifecycle with MvvmCross

        public override void Prepare(TParameter parameter)
        {
            _inputParameter = parameter;
        }

        #endregion

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
            get
            {
                return new MvxLanguageBinder(Constants.GeneralNamespace, GetType().Name);
            }
        }

        [JsonIgnore]
        public IMvxLanguageBinder SharedTextSource
        {
            get
            {
                return new MvxLanguageBinder(Constants.GeneralNamespace, Constants.Shared);
            }
        }

    }
}

