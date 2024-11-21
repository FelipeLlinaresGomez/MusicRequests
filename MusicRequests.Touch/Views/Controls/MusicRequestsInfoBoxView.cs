using System;
using CoreGraphics;
using MusicRequests.Touch.Helpers;
using MusicRequests.Touch.Styles;
using UIKit;

namespace MusicRequests.Touch.Views.Controls
{
    public class MusicRequestsInfoBoxView : UIView
    {
        UIStackView _vstackContent;

        public MusicRequestsButton CloseButton { get; private set; }

        UIStackView _hstackTitle;
        UIImageView _img;
        MusicRequestsHtmlTextView _lblTitle;

        public string Message
        {
            get => _lblTitle.Text;
            set
            {
                _lblTitle.Text = value;
            }
        }

        UIColor _borderColor = Colors.Primary;
        public UIColor BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                SetStyles();
            }
        }

        public bool _closeButtonVisible = false;
        public bool CloseButtonVisible
        {
            get => _closeButtonVisible;
            set
            {
                _closeButtonVisible = value;
                CloseButton.Hidden = !value;
            }
        }

        string imgIcon;
        public string ImgIcon
        {
            get => imgIcon;
            set
            {
                imgIcon = value;
                if(value != null)
                    _img.Image = UIImage.FromBundle(value);
            }
        }


        public MusicRequestsInfoBoxView() : base() => Initialize();

        private void Initialize()
        {
            CreateControls();
            SetupLayout();

            BackgroundColor = UIColor.FromRGB(231, 242, 251);
        }

        private void CreateControls()
        {
            CloseButton = new MusicRequestsButton();
            CloseButton.SetImage(UIImage.FromBundle(Icons.Common.ASPA).ApplyTintColor(BorderColor), UIControlState.Normal);
            CloseButton.Hidden = true;

            _img = new UIImageView();
            _img.Image = UIImage.FromBundle(Icons.Common.AYUDA);

            _lblTitle = new MusicRequestsHtmlTextView();

            _hstackTitle = new UIStackView(new UIView[]{
                _img,
                _lblTitle
            })
            {
                Axis = UILayoutConstraintAxis.Horizontal,
                Alignment = UIStackViewAlignment.Top,
                Spacing = Margin.SmallMedium
            };

            _vstackContent = new UIStackView(new UIView[]{
                _hstackTitle,
            })
            {
                Axis = UILayoutConstraintAxis.Vertical,
                Spacing = Margin.Tiny,
                LayoutMarginsRelativeArrangement = true,
                LayoutMargins = new AutoLayoutHelper.Margins(margenHorizontal: Margin.SmallMedium, margenVertical: Margin.Medium).Insets
            };

            AddSubviews(
                _vstackContent,
                CloseButton
            );
        }

        private void SetupLayout()
        {
            CloseButton.ClipToTop(_vstackContent, spacing: Margin.Small);
            CloseButton.ClipToTrailing(_vstackContent, spacing: Margin.Small);

            CloseButton.SizeConstraint(32);
            CloseButton.DesiredMinHitAreaSize = Dimen.AverageFingerTapSize;

            _img.SizeConstraint(24);
            _lblTitle.ToAutoLayout();

            _hstackTitle.ToAutoLayout();

            _vstackContent.BindToView();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            SetStyles();
        }

        private void SetStyles()
        {
            Layer.CornerRadius = 2;

            Layer.BorderWidth = 1;
            Layer.BorderColor = BorderColor.CGColor;

            _lblTitle.Font = Fonts.MusicRequestsFont.OfSize(14);
            _lblTitle.FontBold = Fonts.MusicRequestsFont.MediumOfSize(14);
            _lblTitle.TextColor = BorderColor;

            _img.BackgroundColor = BorderColor;
            _img.Layer.CornerRadius = 12;
        }
    }
}
