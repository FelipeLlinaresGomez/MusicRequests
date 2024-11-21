using MusicRequests.Touch.Helpers;
using MusicRequests.Touch.Styles;

namespace MusicRequests.Touch.Views.Controls
{
    public class FormView : UIStackView
    {
        public nfloat TopPadding
        {
            get => LayoutMargins.Top;
            set => LayoutMargins = new UIEdgeInsets(value, SidePadding, BottomPadding, SidePadding);
        }

        public nfloat BottomPadding
        {
            get => LayoutMargins.Bottom;
            set => LayoutMargins = new UIEdgeInsets(TopPadding, SidePadding, value, SidePadding);
        }

        public nfloat SidePadding
        {
            get => LayoutMargins.Left;
            set => LayoutMargins = new UIEdgeInsets(TopPadding, value, BottomPadding, value);
        }

        public nfloat LeftPadding
        {
            get => LayoutMargins.Left;
            set => LayoutMargins = new UIEdgeInsets(TopPadding, value, BottomPadding, RightPadding);
        }

        public nfloat RightPadding
        {
            get => LayoutMargins.Right;
            set => LayoutMargins = new UIEdgeInsets(TopPadding, LeftPadding, BottomPadding, value);
        }

        UIView _backgroundView;

        public FormView(NSCoder decoder) : base(decoder)
        {
            SetupView();
        }

        public FormView(CGRect frame) : base(frame)
        {
            SetupView();
        }

        public FormView(bool boxStyle = true)
        {
            SetupView(boxStyle);
        }

        private void SetupView(bool boxStyle = true)
        {
            TranslatesAutoresizingMaskIntoConstraints = false;

            Axis = UILayoutConstraintAxis.Vertical;
            Spacing = Margin.Medium;
            LayoutMarginsRelativeArrangement = true;
            LayoutMargins = new UIEdgeInsets(top: Margin.Large,
                                             left: Margin.Medium,
                                             bottom: Margin.MediumLarge,
                                             right: Margin.Medium);
            _backgroundView = new UIView();

            AddSubview(_backgroundView);

            SetupLayout();
            SetStyles(boxStyle);
        }

        private void SetupLayout()
        {
            _backgroundView.BindToView();
        }

        private void SetStyles(bool boxStyle)
        {
            if (boxStyle)
                Theme.Card(_backgroundView);
        }

        public void AddItems(params UIView[] items)
        {
            foreach (var item in items)
            {
                AddItem(item, Spacing);
            }
        }

        public void AddItem(UIView item)
        {
            AddItem(item, Spacing);
        }

        public void AddItem(UIView item, nfloat customSpacing)
        {
            item.ToAutoLayout();
            AddArrangedSubview(item);
            SetCustomSpacing(customSpacing, item);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            InvalidateIntrinsicContentSize();
        }
    }
}
