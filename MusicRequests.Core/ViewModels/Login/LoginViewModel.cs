using System;
using System.Threading.Tasks;
using MusicRequests.Core.Managers;
using MvvmCross;
using MvvmCross.Commands;

namespace MusicRequests.Core.ViewModels
{
    public class LoginViewModelArgs
    {
        public bool EsArranqueApp { get; set; }
    }

    public class LoginViewModel : BaseViewModel<LoginViewModelArgs>
    {
        public LoginViewModel()
        {
        }

        #region ViewModel Lyfecycle from MvvmCross5

        public override async Task Initialize()
        {
            await base.Initialize();

            IsBusySplash = true;

            try
            {
                // Get the user data stored
                var _userManager = Mvx.IoCProvider.Resolve<IUserManager>();
                _userManager.GetUser();

                var app = GetLocalizedText("AccesoClientes");

                // Comprobamos si debemos solicitar permisos de acceso al teléfono
                // await RequestAppPermissions();
            }
            catch (Exception ex)
            {
                await this.HandleMusicRequestsError(ex);
            }

            IsBusySplash = false;
        }

        #endregion

        #region Propiedades

        #region Usuario

        private string _Usuario;
        public string Usuario
        {
            get => _Usuario;
            set
            {
                SetProperty(ref _Usuario, value);
                IniciarSesionCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Contraseña

        private string _Contraseña;
        public string Contraseña
        {
            get => _Contraseña;
            set
            {
                SetProperty(ref _Contraseña, value);
                IniciarSesionCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region UsuarioRecordado

        private bool _UsuarioRecordado;
        public bool UsuarioRecordado
        {
            get => _UsuarioRecordado;
            set => SetProperty(ref _UsuarioRecordado, value);
        }

        #endregion

        #region IsBusySplash

        private bool _IsBusySplash;
        public bool IsBusySplash
        {
            get { return _IsBusySplash; }
            set { SetProperty(ref _IsBusySplash, value); }
        }

        #endregion

        #region Property SplashScreen

        private byte[] splashScreen;
        public byte[] SplashScreen
        {
            get { return splashScreen; }
            set { SetProperty(ref splashScreen, value); }
        }

        #endregion

        #endregion

        #region RecordarUsuarioCommand

        IMvxAsyncCommand _RecordarUsuarioCommand;
        public IMvxAsyncCommand RecordarUsuarioCommand
        {
            get
            {
                return _RecordarUsuarioCommand ?? (_RecordarUsuarioCommand = new MvxAsyncCommand(
                    async () => await this.ExecuteRecordarUsuarioCommand(),
                    () => CanExecuteRecordarUsuarioCommand()));
            }
        }

        async Task ExecuteRecordarUsuarioCommand()
        {
            try
            {
                UsuarioRecordado = !UsuarioRecordado;
            }
            catch (Exception ex)
            {
                await HandleMusicRequestsError(ex);
            }
        }

        bool CanExecuteRecordarUsuarioCommand()
        {
            return true;
        }

        #endregion

        #region IniciarSesionCommand

        IMvxAsyncCommand _IniciarSesionCommand;
        public IMvxAsyncCommand IniciarSesionCommand
        {
            get
            {
                return _IniciarSesionCommand ?? (_IniciarSesionCommand = new MvxAsyncCommand(
                    async () => await this.ExecuteIniciarSesionCommand(),
                    () => CanExecuteIniciarSesionCommand()));
            }
        }

        async Task ExecuteIniciarSesionCommand()
        {
            try
            {
                Cargando = true;
                CargandoStr = "Iniciando sesión";
                await Task.Delay(2000);
                await _navigationService.Navigate<HomeViewModel>();
            }
            catch (Exception ex)
            {
                await HandleMusicRequestsError(ex);
            }
        }

        bool CanExecuteIniciarSesionCommand()
        {
            return !string.IsNullOrEmpty(Usuario) && !string.IsNullOrEmpty(Contraseña);
        }

        #endregion
    }
}