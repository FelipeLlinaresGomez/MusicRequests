
using MusicRequests.Core.ViewModels;
using MusicRequests.Touch.Helpers;
using MusicRequests.Touch.Styles;
using MusicRequests.Touch.Views.Controls;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using static MusicRequests.Touch.Styles.BaseStyles;

namespace MusicRequests.Touch.Views.Common
{
    [MvxModalPresentation(
        ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen,
        ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve
    )]
    public class ConfirmationDialogView : BaseViewController<ConfirmationDialogViewModel>
    {
        UIView _shadow;

        UIStackView _vstackContent;

        UIStackView _hstackCerrar;
        UILabel _lblCerrar;
        UIImageView _imgCerrar;

        FormView _form;

        UIView _header;
        UIImageView _imgHeader;

        UILabel _lblTitulo;
        UILabel _lblMensaje;
        MusicRequestsHtmlTextView _htmlMensaje;

        FormInputView _inputPrompt;

        UIStackView _hstackBotones;
        UIView _spacer;
        MusicRequestsButton _btnCancelar;
        MusicRequestsButton _btnAceptar;
        MusicRequestsButton _btnAceptarPrompt;

        MusicRequestsButton _btnAceptarVertical;

        UITapGestureRecognizer _tapDismiss;

        public override void LoadView()
        {
            base.LoadView();
            CreateControls();
            SetupLayout();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            MvvmCrossBinding();

            _tapDismiss = new UITapGestureRecognizer(() => {
                if (ViewModel?.DismissCommand?.CanExecute() == true)
                    ViewModel?.DismissCommand?.Execute();
            });

            _shadow.AddGestureRecognizer(_tapDismiss);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetStyles();
        }

        public override void ViewWillAppear(bool animated)
        {
            ProvidesPresentationContextTransitionStyle = true;
            DefinesPresentationContext = true;
            ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
        }

        #region Create controls

        private void CreateControls()
        {
            _lblCerrar = Templates.Label();
            _imgCerrar = Templates.Icon(Icons.Common.ASPA);

            _hstackCerrar = new UIStackView(new UIView[] {
                Templates.HSpacer(),
                _lblCerrar,
                _imgCerrar
            })
            {
                Axis = UILayoutConstraintAxis.Horizontal,
                Alignment = UIStackViewAlignment.Center,
                Spacing = Margin.Small
            };

            _imgHeader = Templates.Icon(Icons.Common.CHECK);

            _header = new UIView();
            _header.AddSubviews(_imgHeader);

            _lblTitulo = Templates.MultilineLabel();
            _lblMensaje = Templates.MultilineLabel();
            _htmlMensaje = new MusicRequestsHtmlTextView();

            _inputPrompt = new FormInputView();

            _spacer = Templates.HSpacer();
            _spacer.Hidden = true;
            _btnCancelar = new MusicRequestsButton();
            _btnAceptar = new MusicRequestsButton();
            _btnAceptarPrompt = new MusicRequestsButton();

            _hstackBotones = new UIStackView(new UIView[]
            {
                _spacer,
                _btnCancelar,
                _btnAceptar,
                _btnAceptarPrompt,
            })
            {
                Axis = UILayoutConstraintAxis.Horizontal,
                Distribution = UIStackViewDistribution.FillEqually,
                Spacing = Margin.Medium
            };

            _btnAceptarVertical = new MusicRequestsButton();

            _form = Templates.StackInsideCard(
                _header,
                _lblTitulo,
                _lblMensaje,
                _htmlMensaje,
                _inputPrompt,
                Templates.VSpacer(),
                _hstackBotones,
                _btnAceptarVertical
            );

            _form.TopPadding = Margin.MediumLarge;

            _form.SetCustomSpacing(Margin.Large, _header);
            _form.SetCustomSpacing(Margin.Medium, _lblTitulo);
            _form.SetCustomSpacing(Margin.Large, _lblMensaje);
            _form.SetCustomSpacing(Margin.Large, _htmlMensaje);
            _form.SetCustomSpacing(Margin.Large, _inputPrompt);

            _vstackContent = new UIStackView(new UIView[]
            {
                _hstackCerrar,
                _form
            })
            {
                Axis = UILayoutConstraintAxis.Vertical,
                Spacing = Margin.Small
            };

            _shadow = new UIView();

            View.AddSubviews(
                _shadow,
                _vstackContent
            );
        }

        #endregion

        NSLayoutConstraint _cstrButtonOkWidth;
        NSLayoutConstraint _cstrButtonOkPromptWidth;

        private void SetupLayout()
        {
            _shadow.BindToView();

            _form.CenterVerticallyInView(View);

            _vstackContent.ClipToLeading(View);
            _vstackContent.ClipToTrailing(View);

            _hstackCerrar.ToAutoLayout();
            _lblCerrar.ToAutoLayout();
            _imgCerrar.SizeConstraint(48);

            _form.ToAutoLayout();

            _imgHeader.CenterHorizontallyInView(_header);
            _imgHeader.SizeConstraint(105);
            _header.HeightConstraint(_imgHeader, 1);

            _lblTitulo.ToAutoLayout();
            _lblMensaje.ToAutoLayout();
            _htmlMensaje.ToAutoLayout();
            _inputPrompt.ToAutoLayout();

            _hstackBotones.ToAutoLayout();
            _spacer.ToAutoLayout();
            _btnCancelar.HeightConstraint(Dimen.ButtonHeight);
            _btnAceptar.HeightConstraint(Dimen.ButtonHeight);
            _btnAceptarPrompt.HeightConstraint(Dimen.ButtonHeight);

            _cstrButtonOkWidth = _btnAceptar.WidthAnchor.ConstraintEqualTo(_hstackBotones.WidthAnchor, 0.4f);
            _cstrButtonOkPromptWidth = _btnAceptarPrompt.WidthAnchor.ConstraintEqualTo(_hstackBotones.WidthAnchor, 0.4f);

            _btnAceptarVertical.HeightConstraint(Dimen.ButtonHeight);
        }

        string _iconPathForHeader;
        public string IconPathForHeader
        {
            get => _iconPathForHeader;
            set
            {
                _iconPathForHeader = value;

                if (string.IsNullOrEmpty(value))
                    _imgHeader.Image = null;
                else if (Icons.ConfirmationDialog.TryGetValue(value, out string actualPath))
                    _imgHeader.Image = UIImage.FromBundle(actualPath) ?? UIImage.FromFile(actualPath);
            }
        }

        bool _okPequeño;
        public bool OkPequeño
        {
            get => _okPequeño;
            set
            {
                _okPequeño = value;

                if (value)
                {
                    _spacer.Hidden = false;
                    _cstrButtonOkWidth.Active = true;
                    _cstrButtonOkPromptWidth.Active = true;
                }
            }
        }

        TextMensajeAligment _alinearTexto;
        public TextMensajeAligment AlinearTexto
        {
            get => _alinearTexto;
            set
            {
                _alinearTexto = value;

                if (value == TextMensajeAligment.Left)
                {
                    _lblMensaje.TextAlignment = UITextAlignment.Left;
                    _htmlMensaje.TextAlignment = UITextAlignment.Left;
                }
                else if (value == TextMensajeAligment.Center)
                {
                    _lblMensaje.TextAlignment = UITextAlignment.Center;
                    _htmlMensaje.TextAlignment = UITextAlignment.Center;
                }
                else if (value == TextMensajeAligment.Right)
                {
                    _lblMensaje.TextAlignment = UITextAlignment.Right;
                    _htmlMensaje.TextAlignment = UITextAlignment.Right;

                }
            }
        }

        TextMensajeAligment _alinearTitulo;
        public TextMensajeAligment AlinearTitulo
        {
            get => _alinearTitulo;
            set
            {
                _alinearTitulo = value;

                if (value == TextMensajeAligment.Left)
                    _lblTitulo.TextAlignment = UITextAlignment.Left;
                else if (value == TextMensajeAligment.Center)
                    _lblTitulo.TextAlignment = UITextAlignment.Center;
                else if (value == TextMensajeAligment.Right)
                    _lblTitulo.TextAlignment = UITextAlignment.Right;
            }
        }

        private void MvvmCrossBinding()
        {
            var set = this.CreateBindingSet<ConfirmationDialogView, ConfirmationDialogViewModel>();

            set.Bind(_lblCerrar.Tap()).For(v => v.Command).To(vm => vm.CerrarCommand);
            set.Bind(_imgCerrar.Tap()).For(v => v.Command).To(vm => vm.CerrarCommand);

            set.Bind(_lblCerrar).To(vm => vm.CerrarText);

            set.Bind(this).For(v => v.IconPathForHeader).To(vm => vm.Imagen);
            set.Bind(_header).For("Visibility").To(vm => vm.Imagen).WithConversion("Visibility");

            set.Bind(_lblTitulo).To(vm => vm.Titulo);
            set.Bind(_lblTitulo).For("Visibility").To(vm => vm.Titulo).WithConversion("Visibility");
            set.Bind(this).For(v => v.AlinearTitulo).To(vm => vm.AlinearTitulo);

            set.Bind(_lblMensaje).To(vm => vm.Mensaje);
            set.Bind(_htmlMensaje).To(vm => vm.Mensaje);
            set.Bind(this).For(v => v.AlinearTexto).To(vm => vm.AlinearTexto);
            set.Bind(_lblMensaje).For("Visibility").To(vm => vm.MensajeHtml).WithConversion("InvertedVisibility");
            set.Bind(_htmlMensaje).For("Visibility").To(vm => vm.MensajeHtml).WithConversion("Visibility");

            set.Bind(_inputPrompt.Input).For(v => v.Placeholder).To(vm => vm.HintTextoInput);
            set.Bind(_inputPrompt.Input).To(vm => vm.TextoInput);
            set.Bind(_inputPrompt).For("Visibility").To(vm => vm.HintTextoInput).WithConversion("Visibility");

            set.Bind(_btnAceptar).To(vm => vm.OKCommand);
            set.Bind(_btnAceptar).For("TitleText").To(vm => vm.OkText);
            set.Bind(this).For(v => v.OkPequeño).To(vm => vm.OkPequeño);
            set.Bind(_btnAceptar).For("Visibility").To(vm => vm.HintTextoInput).WithConversion("InvertedVisibility");

            set.Bind(_btnCancelar).To(vm => vm.CancelCommand);
            set.Bind(_btnCancelar).For("TitleText").To(vm => vm.CancelText);
            set.Bind(_btnCancelar).For("Visibility").To(vm => vm.CancelText).WithConversion("Visibility");

            set.Bind(_btnAceptarPrompt).To(vm => vm.OKPromptCommand);
            set.Bind(_btnAceptarPrompt).For("TitleText").To(vm => vm.OkText);
            set.Bind(_btnAceptarPrompt).For("Visibility").To(vm => vm.HintTextoInput).WithConversion("Visibility");

            set.Bind(_btnAceptarVertical).To(vm => vm.OKCommand);
            set.Bind(_btnAceptarVertical).For("TitleText").To(vm => vm.OkText);

            set.Bind(_hstackBotones).For("Visibility").To(vm => vm.BotonesVertical).WithConversion("InvertedVisibility");
            set.Bind(_btnAceptarVertical).For("Visibility").To(vm => vm.BotonesVertical).WithConversion("Visibility");

            set.Apply();
        }

        private void SetStyles()
        {
            View.BackgroundColor = UIColor.Clear;
            View.Opaque = false;

            _shadow.BackgroundColor = UIColor.FromRGB(70, 76, 80).ColorWithAlpha(0.9f);

            _lblCerrar.Font = Fonts.MusicRequestsFont.OfSize(14);
            _lblCerrar.TextColor = Colors.White;

            _lblTitulo.Font = Fonts.MusicRequestsFont.MediumOfSize(26);
            _lblTitulo.TextColor = Colors.Black;

            _lblMensaje.Font = Fonts.MusicRequestsFont.OfSize(14);
            _lblMensaje.TextColor = Colors.Black;

            _htmlMensaje.Font = Fonts.MusicRequestsFont.OfSize(14);
            _htmlMensaje.TextColor = Colors.Black;

            Theme.PrimaryGhostButton(_btnCancelar);
            Theme.PrimaryRoundedButton(_btnAceptar);
            Theme.PrimaryRoundedButton(_btnAceptarPrompt);

            Theme.PrimaryRoundedButton(_btnAceptarVertical);
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                View.RemoveGestureRecognizer(_tapDismiss);

                _tapDismiss?.Dispose();

                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
