using MusicRequests.Touch.Helpers;
using MusicRequests.Touch.Styles;
using MvvmCross.Commands;

namespace MusicRequests.Touch.Views.Controls
{
    public enum FormInputType
    {
        PLAIN,
        DATE_PICKER,
        AMOUNT
    }

    public class FormInputView : UIView
    {
        UIStackView _vstackContent;

        UIStackView _hstackTitulo;
        UIView _titulo;
        MusicRequestsRadioButton _rbRadio;
        UILabel _lblTitulo;
        MusicRequestsButton _btnShowHideContent;

        public MusicRequestsButton ButtonTooltip { get; private set; }
        public UILabel LabelTooltip { get; private set; }

        public TextInputView Input { get; private set; }

        UILabel _lblAyuda;

        UILabel _lblError;

        public string Titulo
        {
            get => _lblTitulo.Text;
            set
            {
                _lblTitulo.Text = value;
                _lblTitulo.Hidden = string.IsNullOrEmpty(value);
            }
        }

        public bool MostrarBarraSuperior
        {
            get => !_hstackTitulo.Hidden;
            set => _hstackTitulo.Hidden = !value;
        }

        public string Ayuda
        {
            get => _lblAyuda.Text;
            set
            {
                _lblAyuda.Text = value;
                _lblAyuda.Hidden = string.IsNullOrEmpty(value);
            }
        }

        UIFont _ayudaFont = Fonts.MusicRequestsFont.OfSize(10);
        public UIFont AyudaFont
        {
            get => _ayudaFont;
            set
            {
                _ayudaFont = value;
                SetStyles();
            }
        }

        UIColor _ayudaTextColor = Colors.Black;
        public UIColor AyudaTextColor
        {
            get => _ayudaTextColor;
            set
            {
                _ayudaTextColor = value;
                SetStyles();
            }
        }

        public string Error
        {
            get => _lblError.Text;
            set
            {
                _lblError.Text = value;
                _lblError.Hidden = string.IsNullOrEmpty(value);
                if (OcultarAyudaConError)
                    _lblAyuda.Hidden = !_lblError.Hidden;
                SetStyles();
            }
        }

        bool _errorSinTexto;
        public bool ErrorSinTexto
        {
            get => _errorSinTexto;
            set
            {
                _errorSinTexto = value;
                SetStyles();
            }
        }

        bool _ocultarAyudaConError;
        public bool OcultarAyudaConError
        {
            get => _ocultarAyudaConError;
            set
            {
                _ocultarAyudaConError = value;
            }
        }

        bool _secureEntry;
        public bool SecureEntry
        {
            get => _secureEntry;
            set
            {
                _secureEntry = value;
                _btnShowHideContent.Hidden = !value;
                _btnShowHideContent.SetImage(UIImage.FromBundle(Icons.Common.OCULTAR_CLAVE), UIControlState.Normal);
                Input.SecureTextEntry = value;
            }
        }

        public bool ShowTooltip
        {
            get => !ButtonTooltip.Hidden;
            set => ButtonTooltip.Hidden = !value;
        }

        public bool ReadOnly
        {
            get => !Input.Enabled;
            set
            {
                Input.Enabled = !value;
                SetStyles();
            }
        }

        bool _enableAutoCorrection;
        public bool EnableAutocorrection
        {
            get => _enableAutoCorrection;
            set
            {
                _enableAutoCorrection = value;

                if (value)
                {
                    Input.SpellCheckingType = UITextSpellCheckingType.Yes;
                    Input.AutocapitalizationType = UITextAutocapitalizationType.Sentences;
                    Input.AutocorrectionType = UITextAutocorrectionType.Yes;
                }
                else
                {
                    Input.SpellCheckingType = UITextSpellCheckingType.No;
                    Input.AutocapitalizationType = UITextAutocapitalizationType.None;
                    Input.AutocorrectionType = UITextAutocorrectionType.No;
                }
            }
        }

        FormInputType _inputType = FormInputType.PLAIN;
        public FormInputType InputType
        {
            get => _inputType;
            set
            {
                _inputType = value;
                SetStyles();
            }
        }

        bool _showCalendar;
        public bool ShowCalendar
        {
            get => _showCalendar;
            set
            {
                _showCalendar = value;
                SetStyles();
            }
        }

        bool _showRadio = false;
        public bool ShowRadio
        {
            get => _showRadio;
            set
            {
                _showRadio = value;
                SetStyles();
            }
        }

        IMvxCommand _radioCommand;
        public IMvxCommand RadioCommand
        {
            get => _radioCommand;
            set
            {
                _radioCommand = value;
                _rbRadio.Command = _radioCommand;
            }
        }

        bool _radioSelected = false;
        public bool RadioSelected
        {
            get => _radioSelected;
            set
            {
                _radioSelected = value;
                _rbRadio.Selected = _radioSelected;
                Input.Enabled = _radioSelected;
                if (!_radioSelected)
                    Input.Text = "";
                SetStyles();
            }
        }

        public int MaxLength
        {
            get => Input.MaxLength;
            set => Input.MaxLength = value;
        }

        public FormInputView() : base()
        {
            Initialize();
        }

        private void Initialize()
        {
            CreateControls();
            SetupLayout();

            EnableAutocorrection = false;
        }

        private void CreateControls()
        {
            _titulo = new UIView();
            _lblTitulo = Templates.MultilineLabel();
            _titulo.AddSubview(_lblTitulo);

            _btnShowHideContent = new MusicRequestsButton();
            _btnShowHideContent.Hidden = true;
            _btnShowHideContent.TouchUpInside += OnTouchShowHideContent;

            ButtonTooltip = new MusicRequestsButton();
            ButtonTooltip.Hidden = true;

            LabelTooltip = Templates.MultilineLabel();

            _rbRadio = new MusicRequestsRadioButton();
            _rbRadio.Hidden = true;

            _hstackTitulo = new UIStackView(new UIView[]{
                _rbRadio,
                _titulo,
                _btnShowHideContent,
                ButtonTooltip
            })
            {
                Axis = UILayoutConstraintAxis.Horizontal,
                Alignment = UIStackViewAlignment.Center,
                Spacing = Margin.Small
            };

            Input = new TextInputView();

            _lblAyuda = Templates.MultilineLabel();

            _lblError = Templates.MultilineLabel();

            _vstackContent = new UIStackView(new UIView[]{
                _hstackTitulo,
                Input,
                _lblError,
                _lblAyuda,
            })
            {
                Axis = UILayoutConstraintAxis.Vertical,
                Spacing = Margin.Small
            };

            _vstackContent.SetCustomSpacing(0, _hstackTitulo);

            _vstackContent.SetCustomSpacing(2, Input);
            _vstackContent.SetCustomSpacing(2, _lblError);

            AddSubviews(
                _vstackContent,
                LabelTooltip
            );
        }

        private void OnTouchShowHideContent(object sender, EventArgs e)
        {
            if (!_btnShowHideContent.Hidden)
            {
                Input.SecureTextEntry = !Input.SecureTextEntry;
                var iconPath = Input.SecureTextEntry ? Icons.Common.OCULTAR_CLAVE : Icons.Common.MOSTRAR_CLAVE;

                _btnShowHideContent.SetImage(UIImage.FromBundle(iconPath), UIControlState.Normal);
            }
        }

        private void SetupLayout()
        {
            _rbRadio.SizeConstraint(20);
            _hstackTitulo.ToAutoLayout();
            _titulo.ToAutoLayout();
            _lblTitulo.BindToView(margins: new AutoLayoutHelper.Margins(bottom: Margin.Small, top: 0, left: 0, right: 0));
            _btnShowHideContent.SizeConstraint(22);
            ButtonTooltip.SizeConstraint(16);
            LabelTooltip.ToAutoLayout();

            Input.HeightConstraint(Dimen.InputHeight);

            _lblAyuda.ToAutoLayout();
            _lblError.ToAutoLayout();

            _vstackContent.BindToView();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            SetStyles();
        }

        private void SetStyles()
        {
            _lblTitulo.Font = Fonts.MusicRequestsFont.MediumOfSize(14);
            _lblTitulo.TextColor = Colors.Black;

            if (ReadOnly)
            {
                Theme.DisabledFormInput(Input);
            }
            else if (string.IsNullOrEmpty(Error) && !ErrorSinTexto)
            {
                Theme.FormInput(Input, InputType);
            }
            else
            {
                Theme.ErrorFormInput(Input);
            }

            _rbRadio.Hidden = !ShowRadio;

            LabelTooltip.Font = Fonts.MusicRequestsFont.OfSize(12);
            LabelTooltip.TextColor = Colors.White;

            _lblAyuda.Font = AyudaFont;
            _lblAyuda.TextColor = AyudaTextColor;

            if (ShowCalendar)
            {
                var calendarIcon = new UIImageView(UIImage.FromBundle(Icons.Common.CALENDARIO))
                {
                    ContentMode = UIViewContentMode.ScaleAspectFit,
                };

                calendarIcon.Frame = new CGRect(Margin.Small, 0, calendarIcon.Bounds.Width, calendarIcon.Bounds.Height);
                var calendarView = new UIView(new CGRect(0, 0, calendarIcon.Bounds.Width + 2 * Margin.Small, calendarIcon.Bounds.Height));
                calendarView.AddSubview(calendarIcon);

                Input.RightView = calendarView;
                Input.RightViewMode = UITextFieldViewMode.Always;
            }

            _lblError.Font = Fonts.MusicRequestsFont.OfSize(10);
            _lblError.TextColor = UIColor.FromRGB(221, 76, 64);

            Theme.TextButton(ButtonTooltip, Icons.Common.AYUDA);
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _btnShowHideContent.TouchUpInside -= OnTouchShowHideContent;
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}