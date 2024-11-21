using System.Threading.Tasks;
using MvvmCross.Commands;

namespace MusicRequests.Core.ViewModels
{
    public class ConfirmationDialogViewModelArgs
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string OkText { get; set; }
        public string CancelText { get; set; }
        public bool Dismissible { get; set; }
        public ConfirmationButtonStyle OKButtonStyle { get; set; }
        public ConfirmationButtonStyle CancelButtonStyle { get; set; }
        public string Imagen { get; set; }
        public bool OkPequeño { get; set; }
        public string HintTextoInput { get; set; }
        public string TextoInput { get; set; }
        public TextMensajeAligment AlinearTexto { get; set; }
        public TextMensajeAligment AlinearTitulo { get; set; }
        public bool MensajeHtml { get; internal set; }
        public bool BotonesVertical { get; internal set; }
    }

    public class ConfirmationDialogViewModelReturnArgs
    {
        public bool Ok { get; set; }
        public ConfirmationDialogViewModelReturnArgs(bool ok)
        {
            Ok = ok;
        }
        public string TextoInput { get; set; }
        public ConfirmationDialogViewModelReturnArgs(string textoInput)
        {
            TextoInput = textoInput;
        }
    }

    public class ConfirmationDialogViewModel : BaseViewModel<ConfirmationDialogViewModelArgs, ConfirmationDialogViewModelReturnArgs>
    {
        public override Task Initialize()
        {
            Imagen = InputParameter?.Imagen;
            CerrarText = GetLocalizedSharedText("Cerrar");
            Titulo = InputParameter?.Titulo;
            Mensaje = InputParameter?.Mensaje;
            OkText = InputParameter?.OkText ?? GetLocalizedSharedText("Aceptar");
            CancelText = InputParameter?.CancelText;
            Dismissible = InputParameter?.Dismissible ?? false;
            OKButtonStyle = InputParameter?.OKButtonStyle ?? ConfirmationButtonStyle.Azul;
            CancelButtonStyle = InputParameter?.CancelButtonStyle ?? ConfirmationButtonStyle.Blanco;
            OkPequeño = InputParameter?.OkPequeño ?? false;
            HintTextoInput = InputParameter?.HintTextoInput ?? null;
            TextoInput = InputParameter?.TextoInput ?? null;
            AlinearTexto = InputParameter?.AlinearTexto ?? TextMensajeAligment.Left;
            AlinearTitulo = InputParameter?.AlinearTitulo ?? TextMensajeAligment.Center;
            MensajeHtml = InputParameter?.MensajeHtml ?? false;
            BotonesVertical = InputParameter?.BotonesVertical ?? false;
            return base.Initialize();
        }

        #region Property Imagen

        private string _imagen;
        public string Imagen
        {
            get => _imagen;
            set => SetProperty(ref _imagen, value);
        }

        #endregion

        #region Property OkPequeño

        private bool _OkPequeño;
        public bool OkPequeño
        {
            get => _OkPequeño;
            set => SetProperty(ref _OkPequeño, value);
        }

        #endregion

        #region Property CerrarText

        private string _cerrarText;
        public string CerrarText
        {
            get => _cerrarText;
            set => SetProperty(ref _cerrarText, value);
        }

        #endregion

        #region Property AlinearTexto

        private TextMensajeAligment _alinearTexto;
        public TextMensajeAligment AlinearTexto
        {
            get => _alinearTexto;
            set => SetProperty(ref _alinearTexto, value);
        }

        #endregion

        #region Property MensajeHtml

        private bool _MensajeHtml;
        public bool MensajeHtml
        {
            get { return _MensajeHtml; }
            set { SetProperty(ref _MensajeHtml, value); }
        }

        #endregion

        #region Property BotonesVertical

        private bool _BotonesVertical;
        public bool BotonesVertical
        {
            get { return _BotonesVertical; }
            set { SetProperty(ref _BotonesVertical, value); }
        }

        #endregion

        #region Property AlinearTitulo

        private TextMensajeAligment _alinearTitulo;
        public TextMensajeAligment AlinearTitulo
        {
            get => _alinearTitulo;
            set => SetProperty(ref _alinearTitulo, value);
        }

        #endregion

        #region Property Titulo

        private string _titulo;
        public string Titulo
        {
            get => _titulo;
            set => SetProperty(ref _titulo, value);
        }

        #endregion

        #region Property Mensaje

        private string _mensaje;
        public string Mensaje
        {
            get => _mensaje;
            set => SetProperty(ref _mensaje, value);
        }

        #endregion

        #region Property OkText

        private string _okText;
        public string OkText
        {
            get => _okText;
            set => SetProperty(ref _okText, value);
        }

        #endregion

        #region Property HintTextoInput

        private string _HintTextoInput;
        public string HintTextoInput
        {
            get => _HintTextoInput;
            set => SetProperty(ref _HintTextoInput, value);
        }

        #endregion

        #region Property OKButtonStyle

        private ConfirmationButtonStyle _okButtonStyle;
        public ConfirmationButtonStyle OKButtonStyle
        {
            get => _okButtonStyle;
            set => SetProperty(ref _okButtonStyle, value);
        }

        #endregion

        #region Property CancelText

        private string _cancelText;
        public string CancelText
        {
            get => _cancelText;
            set => SetProperty(ref _cancelText, value);
        }

        #endregion

        #region Property CancelButtonStyle

        private ConfirmationButtonStyle _cancelButton;
        public ConfirmationButtonStyle CancelButtonStyle
        {
            get => _cancelButton;
            set => SetProperty(ref _cancelButton, value);
        }

        #endregion

        #region Property Dismissible

        private bool _dismissible;
        public bool Dismissible
        {
            get => _dismissible;
            set
            {
                SetProperty(ref _dismissible, value);
                DismissCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Property TextoInput

        private string _TextoInput;
        public string TextoInput
        {
            get => _TextoInput;
            set => SetProperty(ref _TextoInput, value);
        }

        #endregion

        #region Command OKCommand

        private IMvxAsyncCommand _oKCommand;
        public IMvxAsyncCommand OKCommand => _oKCommand ??= new MvxAsyncCommand(ExecuteOKCommandAsync);

        private async Task ExecuteOKCommandAsync()
        {
            await _navigationService.Close(this, new ConfirmationDialogViewModelReturnArgs(true));
        }

        #endregion

        #region Command CerrarCommand

        private IMvxAsyncCommand _cerrarCommand;
        public IMvxAsyncCommand CerrarCommand => _cerrarCommand ??= new MvxAsyncCommand(ExecuteCerrarCommandAsync);
        private async Task ExecuteCerrarCommandAsync()
        {
            await _navigationService.Close(this, new ConfirmationDialogViewModelReturnArgs(false));
        }

        #endregion

        #region Command DismissCommand

        private IMvxCommand _dismissCommand;
        public IMvxCommand DismissCommand => _dismissCommand ??= new MvxCommand(() => CerrarCommand.ExecuteAsync(), () => Dismissible);

        #endregion

        #region Command CancelCommand

        private IMvxAsyncCommand _cancelCommand;
        public IMvxAsyncCommand CancelCommand => _cancelCommand ??= new MvxAsyncCommand(ExecuteCancelCommandAsync);
        private async Task ExecuteCancelCommandAsync()
        {
            await CerrarCommand.ExecuteAsync();
        }

        #endregion

        #region Command OKPromptCommand

        private IMvxAsyncCommand _oKPromptCommand;
        public IMvxAsyncCommand OKPromptCommand => _oKPromptCommand ??= new MvxAsyncCommand(ExecuteOKPromptCommandAsync);

        private async Task ExecuteOKPromptCommandAsync()
        {
            await _navigationService.Close(this, new ConfirmationDialogViewModelReturnArgs(TextoInput));
        }

        #endregion
    }

    public enum ConfirmationButtonStyle
    {
        Azul, Blanco
    }

    public enum TextMensajeAligment
    {
        Left, Center, Right
    }

    public static class NavigationServiceExtension
    {

        public static async Task<bool> ShowConfirmationDialogAsync(this IMvxNavigationServiceCustom navigationService,
                                                                   string mensaje,
                                                                   string titulo = null,
                                                                   string okText = null,
                                                                   string cancelText = null,
                                                                   string imagen = null,
                                                                   ConfirmationButtonStyle okButtonStyle = ConfirmationButtonStyle.Azul,
                                                                   ConfirmationButtonStyle cancelButtonStyle = ConfirmationButtonStyle.Blanco,
                                                                   bool dismissible = false,
                                                                   bool okPequeño = false,
                                                                   bool mensajeHtml = false,
                                                                   bool botonesVertical = false,
                                                                   TextMensajeAligment alinearTexto = TextMensajeAligment.Left)
        {
            var param = new ConfirmationDialogViewModelArgs
            {
                Titulo = titulo,
                Mensaje = mensaje,
                OkText = okText,
                OkPequeño = okPequeño,
                CancelText = cancelText,
                Imagen = imagen,
                Dismissible = dismissible,
                OKButtonStyle = okButtonStyle,
                CancelButtonStyle = cancelButtonStyle,
                AlinearTitulo = TextMensajeAligment.Center,
                MensajeHtml = mensajeHtml,
                BotonesVertical = botonesVertical,
                AlinearTexto = alinearTexto
            };
            var response = await navigationService.Navigate<ConfirmationDialogViewModel, ConfirmationDialogViewModelArgs, ConfirmationDialogViewModelReturnArgs>(param);
            return response != null && response.Ok;
        }



        public static async Task<bool> ShowConfirmationDialogAsyncAlign(this IMvxNavigationServiceCustom navigationService,
                                                                   string mensaje,
                                                                   string titulo = null,
                                                                   string okText = null,
                                                                   string imagen = null,
                                                                   TextMensajeAligment alinearTexto = TextMensajeAligment.Left)
        {
            var param = new ConfirmationDialogViewModelArgs
            {

                Titulo = titulo,
                Mensaje = mensaje,
                OkText = okText,
                OkPequeño = true,
                CancelText = null,
                Imagen = imagen,
                Dismissible = false,
                AlinearTexto = alinearTexto,
                OKButtonStyle = ConfirmationButtonStyle.Azul,
                CancelButtonStyle = ConfirmationButtonStyle.Azul,
                AlinearTitulo = TextMensajeAligment.Center
            };
            var response = await navigationService.Navigate<ConfirmationDialogViewModel, ConfirmationDialogViewModelArgs, ConfirmationDialogViewModelReturnArgs>(param);
            return response != null && response.Ok;
        }


        public static async Task<bool> ShowConfirmationDialogAsyncAlignDimiss(this IMvxNavigationServiceCustom navigationService,
                                                              string mensaje,
                                                              string titulo = null,
                                                              string okText = null,
                                                              string imagen = null,
                                                              TextMensajeAligment alinearTitulo = TextMensajeAligment.Center,
                                                              bool dimissible = false)
        {
            var param = new ConfirmationDialogViewModelArgs
            {
                Titulo = titulo,
                Mensaje = mensaje,
                OkText = okText,
                OkPequeño = true,
                CancelText = null,
                Imagen = imagen,
                Dismissible = dimissible,
                AlinearTitulo = alinearTitulo,
                OKButtonStyle = ConfirmationButtonStyle.Azul,
                CancelButtonStyle = ConfirmationButtonStyle.Azul,
            };
            var response = await navigationService.Navigate<ConfirmationDialogViewModel, ConfirmationDialogViewModelArgs, ConfirmationDialogViewModelReturnArgs>(param);
            return response != null && response.Ok;
        }
        public static async Task<bool> ShowAlertDialogAsync(this IMvxNavigationServiceCustom navigationService,
                                                                   string mensaje,
                                                                   string titulo = null,
                                                                   string okText = null)
        {
            var param = new ConfirmationDialogViewModelArgs
            {
                Titulo = titulo,
                Mensaje = mensaje,
                OkText = okText,
                OkPequeño = true,
                CancelText = null,
                Imagen = null,
                Dismissible = false,
                OKButtonStyle = ConfirmationButtonStyle.Azul,
                CancelButtonStyle = ConfirmationButtonStyle.Azul,
                AlinearTitulo = TextMensajeAligment.Center
            };
            var response = await navigationService.Navigate<ConfirmationDialogViewModel, ConfirmationDialogViewModelArgs, ConfirmationDialogViewModelReturnArgs>(param);
            return response != null && response.Ok;
        }

        public static async Task<bool> ShowConfirmationCustomDialogAsync(this IMvxNavigationServiceCustom navigationService,
                                                                  string mensaje,
                                                                  string titulo = null,
                                                                  string okText = null,
                                                                  string cancelText = null,
                                                                  bool dimissible = false)
        {
            var param = new ConfirmationDialogViewModelArgs
            {
                Titulo = titulo,
                Mensaje = mensaje,
                OkText = okText,
                OkPequeño = false,
                CancelText = cancelText,
                Imagen = null,
                Dismissible = dimissible,
                OKButtonStyle = ConfirmationButtonStyle.Azul,
                CancelButtonStyle = ConfirmationButtonStyle.Blanco,
                AlinearTitulo = TextMensajeAligment.Center
            };
            var response = await navigationService.Navigate<ConfirmationDialogViewModel, ConfirmationDialogViewModelArgs, ConfirmationDialogViewModelReturnArgs>(param);
            return response != null && response.Ok;
        }

        public static async Task<string> ShowCustomPromptDialogAsync(this IMvxNavigationServiceCustom navigationService,
                                                                  string mensaje,
                                                                  string titulo = null,
                                                                  string okText = null,
                                                                  string cancelText = null,
                                                                  string HintTextoInput = null)
        {
            var param = new ConfirmationDialogViewModelArgs
            {
                Titulo = titulo,
                Mensaje = mensaje,
                OkText = okText,
                OkPequeño = false,
                CancelText = cancelText,
                Imagen = null,
                Dismissible = false,
                OKButtonStyle = ConfirmationButtonStyle.Azul,
                CancelButtonStyle = ConfirmationButtonStyle.Blanco,
                HintTextoInput = HintTextoInput,
                AlinearTitulo = TextMensajeAligment.Center
            };
            var response = await navigationService.Navigate<ConfirmationDialogViewModel, ConfirmationDialogViewModelArgs, ConfirmationDialogViewModelReturnArgs>(param);
            return response.TextoInput;
        }
    }
}

