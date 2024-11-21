using UIKit;
using MusicRequests.Touch.Styles;
using MusicRequests.Touch.Helpers;
using System;

namespace MusicRequests.Touch.Views.Controls
{
    public class InitialsView : UIView
    {
        UILabel _lblInitial;


        public string Text
        {
            get => _lblInitial.Text;
            set
            {
                _lblInitial.Text = value;
            }
        }

        private UIFont _textFont = Fonts.MusicRequestsFont.OfSize(20); 
        public UIFont TextFont
        {
            get => _textFont;
            set
            {
                _textFont = value;
                SetStyles();
            }
        }

        private UIColor _textColor = Colors.Primary;
        public UIColor TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                SetStyles();
            }
        }

        private UIColor _backgroundColor = UIColor.FromRGB(231, 242, 250);
        public UIColor InitialBackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                SetStyles();
            }
        }

        private nfloat _circleSize = 48;
        public nfloat CircleSize
        {
            get => _circleSize;
            set
            {
                _circleSize = value;
                SetStyles();
            }
        }

        private bool _MostrarBorde;
        public bool MostrarBorde
        {
            get => _MostrarBorde;
            set
            {
                _MostrarBorde = value;
                SetStyles();
            }
        }

        // Se utiliza para ajustar el padding que trae el _lblInitial 
        private nfloat _labelOffsetY = -3;
        public nfloat LabelOffsetY
        {
            get => _labelOffsetY;
            set
            {
                _labelOffsetY = value;
                _lblInitial.CenterInView(this, offsetX: 0, offsetY: value);
            }
        }

        public InitialsView() : base()
        {
            Initialize();
        }

        private void Initialize()
        {
            CreateControls();
            SetupLayout();
        }

        private void CreateControls()
        {

            _lblInitial = new UILabel
            {
                Lines = 1,
                LineBreakMode = UILineBreakMode.TailTruncation
            };

            AddSubview(_lblInitial);
        }

        private void SetupLayout()
        {
            _lblInitial.CenterInView(this, offsetX: 0, offsetY: LabelOffsetY);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            SetStyles();
        }

        private void SetStyles()
        {
            BackgroundColor = InitialBackgroundColor;

            _lblInitial.Font = TextFont;
            _lblInitial.TextColor = TextColor;

            Layer.CornerRadius = CircleSize / 2;

            if (MostrarBorde)
            {
                Layer.BorderColor = Colors.Primary.CGColor;
                Layer.BorderWidth = 1;
            }
        }
    }
}