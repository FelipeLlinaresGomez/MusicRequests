using System;
using MusicRequests.Touch.Helpers;
using MusicRequests.Touch.Styles;
using UIKit;


namespace MusicRequests.Touch.Views.Controls
{
    public class TabView : UIStackView
    {
        public UILabel Label { get; private set; }

        UIView _title;
        UIView _bar;

        bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                SetStyles();
            }
        }

        int _fontSize = 13;
        public int FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                SetStyles();
            }
        }

        UIColor _selectedColor = Colors.Black;
        public UIColor SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                SetStyles();
            }
        }

        public TabView() : base()
        {
            Initialize();
        }

        private void Initialize()
        {
            Axis = UILayoutConstraintAxis.Vertical;

            CreateControls();
            SetupLayout();
        }

        private void CreateControls()
        {
            Label = new UILabel();
            Label.TextAlignment = UITextAlignment.Center;

            _title = new UIView();
            _bar = new UIView();

            _title.AddSubview(Label);

            AddArrangedSubview(_title);
            AddArrangedSubview(_bar);
        }

        private void SetupLayout()
        {
            _title.ToAutoLayout();
            Label.BindToView(margins: new AutoLayoutHelper.Margins(Margin.Tiny));

            _bar.HeightConstraint(2);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            SetStyles();
        }

        private void SetStyles()
        {
            if (Selected)
            {
                Label.Font = Fonts.MusicRequestsFont.MediumOfSize(FontSize);
                Label.TextColor = SelectedColor;

                _bar.BackgroundColor = SelectedColor;
            }
            else
            {
                Label.Font = Fonts.MusicRequestsFont.OfSize(FontSize);
                Label.TextColor = Colors.Black;

                _bar.BackgroundColor = Colors.Gray15;
            }
        }
    }
}
