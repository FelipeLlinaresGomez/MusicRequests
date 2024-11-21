using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MusicRequests.Core.ViewModels
{
    public class EmbedWebViewModelArgs
    {
        public string Title { get; set; }
        public string Url { get; set; }

        public string ActionTitle { get; set; }
        public IMvxAsyncCommand Action { get; set; }
    }

    public class EmbedWebViewModel : BaseViewModel<EmbedWebViewModelArgs>
    {
        #region ViewModel Lyfecycle from MvvmCross5

        public override async Task Initialize()
        {
            await base.Initialize();

            if (InputParameter == null)
            {
                throw new ArgumentNullException("Los parametros de navegacion son obligatorios");
            }

            Title = InputParameter.Title;
            UrlBase = InputParameter.Url;
            BackText = this.GetLocalizedText("Volver");
            ActionTitle = InputParameter.ActionTitle;
            ActionCommand = InputParameter.Action;
        }

        #endregion

        #region Property UrlBase

        string _urlBase = string.Empty;
        public string UrlBase
        {
            get { return _urlBase; }
            set { SetProperty(ref _urlBase, value); }
        }

        #endregion

        #region Property BackText

        string _backText = string.Empty;
        public string BackText
        {
            get { return _backText; }
            set { SetProperty(ref _backText, value); }
        }

        #endregion

        #region Property ActionTitle

        string _actionTitle = string.Empty;
        public string ActionTitle
        {
            get { return _actionTitle; }
            set { SetProperty(ref _actionTitle, value); }
        }

        #endregion

        #region Property ActionCommand

        IMvxAsyncCommand _actionCommand;
        public IMvxAsyncCommand ActionCommand
        {
            get { return _actionCommand; }
            set { SetProperty(ref _actionCommand, value); }
        }

        #endregion
    }
}

