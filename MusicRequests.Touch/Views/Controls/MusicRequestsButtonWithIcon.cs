using MusicRequests.Touch.Helpers;
using MusicRequests.Touch.Styles;
using MusicRequests.Touch.Views.Controls;

namespace MusicRequestscajaApp.Touch.Views.Controls
{
    public class MusicRequestsButtonWithIcon : MusicRequestsButton
    {
        private UIColor _titleColor = Colors.Black;
        public new UIColor TitleColor
        {
            get => _titleColor;
            set
            {
                _titleColor = value;
                SetStyles();
            }
        }

        private UIFont _font = Fonts.MusicRequestsFont.MediumOfSize(12);
        public UIFont Font
        {
            get => _font;
            set
            {
                _font = value;
                SetStyles();
            }
        }

        UIControlContentHorizontalAlignment _alignment;
        public UIControlContentHorizontalAlignment Alignment
        {
            get => _alignment;
            set
            {
                _alignment = value;
                HorizontalAlignment = value;
            }
        }

        string _iconPath;
        public string IconPath
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                SetImage((UIImage.FromBundle(value) ?? UIImage.FromFile(value))?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate), UIControlState.Normal);
                SetStyles();
            }
        }

        private nfloat _spacing = 16f;
        public nfloat Spacing
        {
            get => _spacing;
            set
            {
                _spacing = value;
                SetStyles();
            }
        }

        public MusicRequestsButtonWithIcon() : base() => Initialize();

        private void Initialize()
        {
            CreateControls();
        }

        private void CreateControls()
        {
            SetImage(UIImage.FromBundle(Icons.Common.ASPA).ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate), UIControlState.Normal);
            SemanticContentAttribute = UISemanticContentAttribute.ForceRightToLeft;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            SetStyles();
        }

        private void SetStyles()
        {
            SetTitleColor(TitleColor, UIControlState.Normal);
            SetImageTintColor(TitleColor, UIControlState.Normal);
            TitleLabel.Font = Font;

            ImageEdgeInsets = new UIEdgeInsets(top: 2, left: Spacing / 2, bottom: 0, right: 0);
            TitleEdgeInsets = new UIEdgeInsets(top: 0, left: 0, bottom: 0, right: Spacing / 2);
        }
    }
}
