using System;
using MusicRequests.Core.Services;
using MusicRequests.Core.Models;
using Acr.UserDialogs;
using System.Threading.Tasks;
using MusicRequests.Core.Messages;
using System.Diagnostics;
using MusicRequests.Core.Presenters;
using System.Collections.Generic;
using MvvmCross.Plugin.Messenger;
using MvvmCross;
using System.Text;

namespace MusicRequests.Core.ViewModels
{
    public abstract class BaseViewModel : BaseViewModel<string, BoolViewModelResult>
    {
    }

    public abstract class BaseViewModel<TParameter> : BaseViewModel<TParameter, BoolViewModelResult>
    {
    }

    public abstract class BaseViewModel<TParameter, TResult> : BaseLocatizationViewModel<TParameter, TResult>, IBaseViewModel
    {
        public virtual bool UsarAspaParaCancelar => false;
        public virtual bool UsarFlechaAtras => true;

        protected readonly IMvxNavigationServiceCustom _navigationService = null;
        readonly MvxSubscriptionToken _noConnectionToken;

        public BaseViewModel()
        {
            _navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationServiceCustom>();

            var _dialog = Mvx.IoCProvider.Resolve<IUserDialogs>();
            var _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            // Message subscriptions to display toast when we have no connection but we can work with some local data
            _noConnectionToken = _messenger.Subscribe<NoConnectionMessage>((mess) =>
            {
                switch (mess.Type)
                {
                    case ConnStatus.NoConnection:
                        _dialog.AlertAsync(
                            this.GetLocalizedSharedText("ErrorNoConnection"),
                            this.GetLocalizedSharedText("ErrorTitle"),
                            "OK");
                        break;
                    case ConnStatus.NoAPIResponse:
                        _dialog.AlertAsync(
                            this.GetLocalizedSharedText("ErrorNoApiResponse"),
                            this.GetLocalizedSharedText("ErrorTitle"),
                            "OK");
                        break;
                }
            });
        }

        #region ViewModel lifecycle with MvvmCross

        public override void Prepare()
        {
            base.Prepare();
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
#if DEBUG
            Debug.WriteLine($"Showing ViewModel: {this.GetType().Name}");
#endif
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
        }

        public override void ViewDisappearing()
        {
            base.ViewDisappearing();
        }

        public override void ViewDisappeared()
        {
            _noConnectionToken.Dispose();
            base.ViewDisappeared();
        }

        #endregion

        #region CargandoStr
        private string _CargandoStr;
        public string CargandoStr
        {
            get { return _CargandoStr; }
            set => SetProperty(ref _CargandoStr, value);
        }
        #endregion

        #region Cargando
        private bool _Cargando;
        public bool Cargando
        {
            get => _Cargando;
            set => SetProperty(ref _Cargando, value);
        }

        #endregion

        #region Title
        private string _Title;
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        #endregion

        #region Navigate

        public void Navigate(string Title, TipoAccion type, string targetUrl)
        {
            switch (type)
            {
                case TipoAccion.WebExterna:
                    var _webService = Mvx.IoCProvider.Resolve<IWebService>();
                    _webService.OpenWeb(targetUrl);
                    break;
                case TipoAccion.WebEmbebida:
                    _navigationService.Navigate<EmbedWebViewModel, EmbedWebViewModelArgs>(new EmbedWebViewModelArgs()
                    {
                        Title = Title,
                        Url = targetUrl
                    });
                    break;
                case TipoAccion.Aplicacion:
                    try
                    {
                        var viewModelName = targetUrl.Split(':')[1];

                        var currentNamespace = this.GetType().Namespace;

                        Type t = Type.GetType(string.Format("{0}.{1}ViewModel", currentNamespace, viewModelName));

                        _navigationService.Navigate(t);
                    }
                    catch (Exception ex)
                    {
                        //TODO: ventana error / defecto?
                        _navigationService.Navigate<EmbedWebViewModel, EmbedWebViewModelArgs>(new EmbedWebViewModelArgs()
                        {
                            Title = Title,
                            Url = targetUrl
                        });
                    }
                    break;
                case TipoAccion.Llamada:
                    var _llamadaService = Mvx.IoCProvider.Resolve<ILlamadaService>();
                    _llamadaService.MarcarNumTelefono(targetUrl);
                    break;
            }
        }

        #endregion

        #region HandleApiError

        public async Task HandleMusicRequestsError(Exception ex, bool navigateBack = false)
        {
            try
            {
                var _dialog = Mvx.IoCProvider.Resolve<IUserDialogs>();

                if (typeof(HttpMusicRequestException) == ex.GetType())
                {
                    var ibex = (HttpMusicRequestException)ex;
                    switch (ibex.StatusCode)
                    {
                        case ApiStatusCode.InvalidToken:
                            await Task.Run(async() =>
                            await _dialog.AlertAsync(
                                string.Format("La sesión ha expirado."),
                                this.GetLocalizedSharedText("ErrorTitle"),
                                "OK"));
                            // Redirect to root
                            CloseSesionExecute();
                            break;
                        default:
                            await _dialog.AlertAsync(
                                ex.Message,
                                this.GetLocalizedSharedText("ErrorTitle"),
                                "OK");

                            break;
                    }
                }
                else if (typeof(GPSMusicRequestException) == ex.GetType())
                {
                    var gps = (GPSMusicRequestException)ex;
                    switch (gps.ErrorType)
                    {
                        case GPSError.PositionUnavailable:
                            await _dialog.AlertAsync(
                                    this.GetLocalizedSharedText("ErrorGPSUnableToGetLocation"));
                            break;
                        case GPSError.Unauthorized:
                            await _dialog.AlertAsync(
                                    this.GetLocalizedSharedText("ErrorGPSUnauthorized"));
                            break;
                        case GPSError.Disabled:
                            await _dialog.AlertAsync(
                                    this.GetLocalizedSharedText("ErrorGPSDisabled"));
                            break;
                    }
                }
                else if (typeof(ConnMusicRequestException) == ex.GetType())
                {
                    var conn = (ConnMusicRequestException)ex;
                    switch (conn.Code)
                    {
                        case ConnStatus.NoConnection:
                            await _dialog.AlertAsync(
                                this.GetLocalizedSharedText("ErrorNoConnection"),
                                this.GetLocalizedSharedText("ErrorTitle"),
                            "OK");
                            break;
                        case ConnStatus.NoAPIResponse:
                            await _dialog.AlertAsync(
                                this.GetLocalizedSharedText("ErrorNoApiResponse"),
                                this.GetLocalizedSharedText("ErrorTitle"),
                            "OK");
                            break;
                        case ConnStatus.DownloadFileFailed:
                            await _dialog.AlertAsync(
                                this.GetLocalizedSharedText("ErrorDownloadFile"),
                                this.GetLocalizedSharedText("ErrorTitle"),
                            "OK");
                            break;
                    }
                }
                else if (typeof(MusicRequestBackendException) == ex.GetType())
                {
                    var backendEx = (MusicRequestBackendException)ex;
                    await MusicRequestBackendExceptionHandler(backendEx);
                }
                else if ((typeof(TaskCanceledException) == ex.GetType()) ||
                         (typeof(OperationCanceledException) == ex.GetType()))
                {
                    Debug.WriteLine("API Call Task cancelada");
                    Debug.WriteLine(ex.Message);
                }
                else if (typeof(System.Net.WebException) == ex.GetType())
                {
                    Debug.WriteLine("ErrorNoApiResponse");
                    Debug.WriteLine(ex.Message);
                    await _dialog.AlertAsync(this.GetLocalizedSharedText("ErrorNoApiResponse"),
                        this.GetLocalizedSharedText("ErrorTitle"),
                        "OK");
                }
                else if (ex.GetType()?.ToString() == "Java.Net.UnknownHostException")
                {
                    Debug.WriteLine("ErrorNoApiResponse");
                    Debug.WriteLine(ex.Message);
                    await _dialog.AlertAsync(this.GetLocalizedSharedText("ErrorNoApiResponse"),
                        this.GetLocalizedSharedText("ErrorTitle"),
                        "OK");
                }
                else if(typeof(DeviceSecurityCheckException) == ex.GetType()){
                    Debug.WriteLine("ERROR MUSIC REQUEST");
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);

                    var deviceSecurity = (DeviceSecurityCheckException)ex;

                    var sb = new StringBuilder("El dispositivo no cumple con las características necesarias de seguridad:");
                    sb.AppendLine("");
                    foreach (string info in deviceSecurity.Causas)
                    {
                        sb.AppendLine("- " + info);
                    }

                    await _dialog.AlertAsync(
                        sb.ToString(),
                        this.GetLocalizedSharedText("ErrorTitle"),
                        "OK");
                }
                else
                {
                    Debug.WriteLine("ERROR MUSIC REQUEST");
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);

                    await _dialog.AlertAsync(
                        ex.Message,
                        this.GetLocalizedSharedText("ErrorTitle"),
                        "OK");
                }

                if (navigateBack)
                {
                    await _navigationService.Close(this);
                }
            }
            catch (Exception excep)
            {
            }
        }

        #endregion

        #region CloseSesionExecute

        public void CloseSesionExecute(bool completeLogout = true)
        {
            //_userManager.LogOut(completeLogout);
            _navigationService.ChangePresentation(new LogOutPresenterHint<LoginViewModel>());
        }

        #endregion

        #region MusicRequestBackendException

        private async Task MusicRequestBackendExceptionHandler(MusicRequestBackendException ex)
        {
            try
            {
                var _dialog = Mvx.IoCProvider.Resolve<IUserDialogs>();

                if (!string.IsNullOrEmpty(ex.FriendlyMessage))
                {
                    await _dialog.AlertAsync(
                        string.Format("{0}", ex.FriendlyMessage),
                        this.GetLocalizedSharedText("ErrorTitle"),
                        "OK");
                }
                else
                {
                    await _dialog.AlertAsync(this.GetLocalizedSharedText("ErrorGenericMessage"),
                                             this.GetLocalizedSharedText("ErrorTitle"),
                                             "OK");
                }
            }
            catch (Exception ex2)
            {
                Debug.WriteLine(ex2.Message);
            }
        }

        #endregion

        #region NavigateToInit

        public void NavigateToInit(IEnumerable<IDisposable> managersToDispose)
        {
            // Dispose the managers
            if (managersToDispose != null)
            {
                foreach (var manager in managersToDispose)
                {
                    manager?.Dispose();
                }
            }

            NavigateToInit();
        }

        public void NavigateToInit()
        {
            if (App.User != null)
            {
                if (App.IsiOS())
                {
                    _navigationService.ChangePresentation(new RootViewPresentationHint<HomeViewModel>(true));
                }
                else
                {
                    _navigationService.Navigate<MainViewModel>();
                }
            }
        }

        public void RequestNavigateToHomePublica()
        {
            ApiSettings.CancelApiCalls();
            _navigationService.ChangePresentation(new LogOutPresenterHint<LoginViewModel>());
        }

        #endregion

        public virtual async Task OnBackNavigation()
        {
            await _navigationService.Close(this);
        }
    }
}