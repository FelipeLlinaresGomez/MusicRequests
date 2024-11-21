using System.ComponentModel;
using MusicRequests.Touch.Helpers;

namespace MusicRequests.Touch.Views
{
    [Register("MusicRequestsCheckBox")]
    public class MusicRequestsCheckBox : UIView, INotifyPropertyChanged
    {
        UIImageView _MusicRequestsCheckBoxImage;
        UITapGestureRecognizer _tapRecognizer;

        NSLayoutConstraint _legacyConstraintY;
        NSLayoutConstraint _constraintY;

        private readonly string imageChecked = "icons/icon_selected.png";
        private readonly string imageUnChecked = "icons/icon_unchecked_circle.png";

        /// <summary>
        /// Espacio adicional que se introduce en cada lateral del MusicRequestsCheckBox para aumentar
        /// la region en la que podemos pulsar para cambiar el estado del MusicRequestsCheckBox.
        /// </summary>
        public nfloat InsetHitArea { get; set; }

        /// <summary>
        /// Valor minimo recomendado para la property InsetHitArea. Apple recomienda que al menos
        /// tenga un ancho y alto de 44. Si el MusicRequestsCheckBox es superior o igual a ese valor, esta propiedad
        /// valdra 0.
        /// </summary>
        public nfloat MinRecommendedInsetHitArea => (nfloat)Math.Max(0, (44 - Bounds.Width) / 2f);

        private bool _checked = false;
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                SetImage();
                OnPropertyChanged(nameof(Checked));
            }
        }

        public MusicRequestsCheckBox(IntPtr handle) : base(handle)
        {

            InitControl();
        }

        public MusicRequestsCheckBox()
        {
            InitControl();
        }

        public MusicRequestsCheckBox(string pathImageChecked, string pathImageUnChecked)
        {
            imageChecked = pathImageChecked;
            imageUnChecked = pathImageUnChecked;
            InitControl();
        }

        private void InitControl()
        {
            Initialize();
            SetImage();
        }

        void Initialize()
        {
            _MusicRequestsCheckBoxImage = new UIImageView(new CGRect(0, 0, 25, 25))
            {
                ContentMode = UIViewContentMode.ScaleAspectFit
            };

            _tapRecognizer = new UITapGestureRecognizer(() => Checked = !Checked);

            AddGestureRecognizer(_tapRecognizer);

            Frame = new CGRect(
                x: 0,
                y: 0,
                width: 30,
                height: 30);

            AddSubviews(_MusicRequestsCheckBoxImage);
            SetupLayout();
        }

        void SetupLayout()
        {
            _legacyConstraintY = _MusicRequestsCheckBoxImage.TopAnchor.ConstraintEqualTo(TopAnchor);
            _legacyConstraintY.Active = true;

            _constraintY = _MusicRequestsCheckBoxImage.CenterYAnchor.ConstraintEqualTo(CenterYAnchor);

            _MusicRequestsCheckBoxImage.ToAutoLayout();
            AutoLayoutHelper.ActivateConstraints(
                _MusicRequestsCheckBoxImage.CenterXAnchor.ConstraintEqualTo(CenterXAnchor),

                _MusicRequestsCheckBoxImage.WidthAnchor.ConstraintEqualTo(WidthAnchor)
            );
        }

        /// <summary>
        /// Para la correcta visualizacion de las pantallas anteriores a seguro de hogar
        /// el UIImageView del MusicRequestsCheckBox no debe estar centrado verticalmente en este componente.
        ///
        /// En las nuevas pantallas utilizamos autolayout, por lo que el modo debe ser CENTER.
        /// En las pantallas antiguas se utiliza el enfoque basado en UpdateFrames, asi que el modo sera TOP
        /// </summary>
        public override bool TranslatesAutoresizingMaskIntoConstraints
        {
            get => base.TranslatesAutoresizingMaskIntoConstraints;
            set
            {
                base.TranslatesAutoresizingMaskIntoConstraints = value;

                if (value)
                {
                    _constraintY.Active = false;
                    _legacyConstraintY.Active = true;
                }
                else
                {
                    _legacyConstraintY.Active = false;
                    _constraintY.Active = true;
                }
            }
        }

        public override bool PointInside(CGPoint point, UIEvent uievent)
        {
            var area = Bounds.Inset(-InsetHitArea, -InsetHitArea);
            return area.Contains(point);
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                RemoveGestureRecognizer(_tapRecognizer);
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #region INotifyPropertyChange Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        void SetImage()
        {
            _MusicRequestsCheckBoxImage.Image = UIImage.FromBundle(_checked ? imageChecked : imageUnChecked);
        }
    }
}

