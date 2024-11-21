using System;
using MusicRequests.Touch.Helpers;
using MusicRequests.Touch.Styles;
using UIKit;

namespace MusicRequests.Touch.Views.Login.Controls
{
    public class FieldRecordarUsuarioView : UIView
    {
        UIStackView _hstackContent;

        UIImageView _imgCheck;
        UILabel _lblRecordarUsuario;

        bool _checked;
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                _imgCheck.Image = UIImage.FromBundle(value ? "icon_checkbox_white_checked" : "icon_checkbox_white_unchecked");
            }
        }

        public string RecordarUsuario
        {
            get => _lblRecordarUsuario.Text;
            set => _lblRecordarUsuario.Text = value;
        }

        public FieldRecordarUsuarioView() : base() => Initialize();

        private void Initialize()
        {
            CreateControls();
            SetupLayout();
        }

        private void CreateControls()
        {
            _imgCheck = new UIImageView();

            _lblRecordarUsuario = new UILabel
            {
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap
            };

            _hstackContent = new UIStackView(new UIView[]{
                _imgCheck,
                _lblRecordarUsuario
            })
            {
                Axis = UILayoutConstraintAxis.Horizontal,
                Alignment = UIStackViewAlignment.Center,
                Spacing = Margin.Tiny
            };

            AddSubviews(_hstackContent);
        }

        private void SetupLayout()
        {
            _imgCheck.SizeConstraint(25);

            _lblRecordarUsuario.ToAutoLayout();

            _hstackContent.BindToView();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            SetStyles();
        }

        private void SetStyles()
        {
            _lblRecordarUsuario.Font = Fonts.MusicRequestsFont.MediumOfSize(12);
            _lblRecordarUsuario.TextColor = Colors.White;
        }
    }
}
