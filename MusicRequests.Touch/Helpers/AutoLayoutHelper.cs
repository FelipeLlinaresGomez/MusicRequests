using System;
using CoreGraphics;
using UIKit;

namespace MusicRequests.Touch.Helpers
{
    public static class AutoLayoutHelper
    {
        public static void ToAutoLayout(this UIView view)
        {
            view.TranslatesAutoresizingMaskIntoConstraints = false;
        }

        public static NSLayoutConstraint DebugIdentifier(this NSLayoutConstraint cstr, string name)
        {
            cstr.SetIdentifier(name);
            return cstr;
        }

        public static void ChildrenToAutoLayout(this UIStackView stack) {
            foreach (var view in stack.ArrangedSubviews) {
                view.ToAutoLayout();
            }
        }

        public static void BindToView(this UIView view, UIView otherView = null, Margins margins = null)
        {
            margins = margins ?? new Margins(0);
            otherView = otherView ?? view.Superview;
            view.ToAutoLayout();
            ActivateConstraints(
                view.TopAnchor.ConstraintEqualTo(otherView.TopAnchor, margins.Top),
                view.LeadingAnchor.ConstraintEqualTo(otherView.LeadingAnchor, margins.Left),
                view.BottomAnchor.ConstraintEqualTo(otherView.BottomAnchor, -margins.Bottom),
                view.TrailingAnchor.ConstraintEqualTo(otherView.TrailingAnchor, -margins.Right)
            );
        }

        public static void BindToViewFlexibleBottom(this UIView view, UIView otherView = null, Margins margins = null)
        {
            otherView = otherView ?? view.Superview;
            margins = margins ?? new Margins(0);
            view.ToAutoLayout();
            ActivateConstraints(
                view.TopAnchor.ConstraintEqualTo(otherView.TopAnchor, margins.Top),
                view.LeadingAnchor.ConstraintEqualTo(otherView.LeadingAnchor, margins.Left),
                view.BottomAnchor.ConstraintLessThanOrEqualTo(otherView.BottomAnchor, -margins.Bottom),
                view.TrailingAnchor.ConstraintEqualTo(otherView.TrailingAnchor, -margins.Right)
            );
        }

        public static void PlaceAtTop(this UIView view, UIView otherView = null)
        {
            view.PlaceAtTop(otherView, 0, 0);
        }

        public static void PlaceAtTop(this UIView view, UIView otherView, nfloat sideMargins, nfloat spacing)
        {
            otherView = otherView ?? view.Superview;

            view.ToAutoLayout();
            ActivateConstraints(
                view.TopAnchor.ConstraintEqualTo(otherView.TopAnchor, spacing)
            );

            if (view.Superview != null)
            {
                ActivateConstraints(
                    view.LeadingAnchor.ConstraintEqualTo(view.Superview.LeadingAnchor, sideMargins),
                    view.TrailingAnchor.ConstraintEqualTo(view.Superview.TrailingAnchor, -sideMargins)
                );
            }
        }

        public static void PlaceAtTop(this UIView view, UIView otherView, nfloat leadingMargin, nfloat trailingMargin, nfloat spacing)
        {
            otherView = otherView ?? view.Superview;

            view.ToAutoLayout();
            ActivateConstraints(
                view.TopAnchor.ConstraintEqualTo(otherView.TopAnchor, spacing)
            );

            if (view.Superview != null)
            {
                ActivateConstraints(
                    view.LeadingAnchor.ConstraintEqualTo(view.Superview.LeadingAnchor, leadingMargin),
                    view.TrailingAnchor.ConstraintEqualTo(view.Superview.TrailingAnchor, -trailingMargin)
                );
            }
        }

        public static void PlaceAtBottom(this UIView view, UIView otherView = null)
        {
            view.PlaceAtBottom(otherView, 0, 0);
        }

        public static void PlaceAtBottom(this UIView view, UIView otherView, nfloat sideMargins, nfloat spacing)
        {
            otherView = otherView ?? view.Superview;

            view.ToAutoLayout();
            ActivateConstraints(
                view.BottomAnchor.ConstraintEqualTo(otherView.BottomAnchor, -spacing)
            );

            if (view.Superview != null)
            {
                ActivateConstraints(
                    view.LeadingAnchor.ConstraintEqualTo(view.Superview.LeadingAnchor, sideMargins),
                    view.TrailingAnchor.ConstraintEqualTo(view.Superview.TrailingAnchor, -sideMargins)
                );
            }
        }

        public static void PlaceAtBottom(this UIView view, UIView otherView, nfloat leftMargin, nfloat rightMargin, nfloat spacing)
        {
            otherView = otherView ?? view.Superview;

            view.ToAutoLayout();
            ActivateConstraints(
                view.BottomAnchor.ConstraintEqualTo(otherView.BottomAnchor, -spacing)
            );

            if (view.Superview != null)
            {
                ActivateConstraints(
                    view.LeadingAnchor.ConstraintEqualTo(view.Superview.LeadingAnchor, leftMargin),
                    view.TrailingAnchor.ConstraintEqualTo(view.Superview.TrailingAnchor, -rightMargin)
                );
            }
        }

        public static void PlaceBelowView(this UIView view, UIView otherView, nfloat sideMargins, nfloat spacing)
        {
            view.ToAutoLayout();
            ActivateConstraints(
                view.TopAnchor.ConstraintEqualTo(otherView.BottomAnchor, spacing)
            );

            if (view.Superview != null)
            {
                ActivateConstraints(
                    view.LeadingAnchor.ConstraintEqualTo(view.Superview.LeadingAnchor, sideMargins),
                    view.TrailingAnchor.ConstraintEqualTo(view.Superview.TrailingAnchor, -sideMargins)
                );
            }
        }

        public static void PlaceBelowView(this UIView view, UIView otherView, nfloat spacing, nfloat leftMargin, nfloat rightMargin)
        {
            view.ToAutoLayout();
            ActivateConstraints(
                view.TopAnchor.ConstraintEqualTo(otherView.BottomAnchor, spacing)
            );

            if (view.Superview != null)
            {
                ActivateConstraints(
                    view.LeadingAnchor.ConstraintEqualTo(view.Superview.LeadingAnchor, leftMargin),
                    view.TrailingAnchor.ConstraintEqualTo(view.Superview.TrailingAnchor, -rightMargin)
                );
            }
        }

        public static void PlaceAboveView(this UIView view, UIView otherView, nfloat sideMargins, nfloat spacing)
        {
            view.ToAutoLayout();
            ActivateConstraints(
                view.BottomAnchor.ConstraintEqualTo(otherView.TopAnchor, -spacing)
            );

            if (view.Superview != null)
            {
                ActivateConstraints(
                    view.LeadingAnchor.ConstraintEqualTo(view.Superview.LeadingAnchor, sideMargins),
                    view.TrailingAnchor.ConstraintEqualTo(view.Superview.TrailingAnchor, -sideMargins)
                );
            }
        }

        public static void CenterInView(this UIView view, UIView otherView)
        {
            view.ToAutoLayout();
            ActivateConstraints(
                view.CenterXAnchor.ConstraintEqualTo(otherView.CenterXAnchor),
                view.CenterYAnchor.ConstraintEqualTo(otherView.CenterYAnchor)
            );
        }

        public static void CenterInView(this UIView view, UIView otherView, nfloat? offsetX = null, nfloat? offsetY = null)
        {
            view.ToAutoLayout();
            ActivateConstraints(
                view.CenterXAnchor.ConstraintEqualTo(otherView.CenterXAnchor, offsetX ?? 0),
                view.CenterYAnchor.ConstraintEqualTo(otherView.CenterYAnchor, offsetY ?? 0)
            );
        }

        public static void CenterHorizontallyInView(this UIView view, UIView otherView)
        {
            view.ToAutoLayout();
            ActivateConstraints(
                view.CenterXAnchor.ConstraintEqualTo(otherView.CenterXAnchor)
            );
        }

        public static void CenterVerticallyInView(this UIView view, UIView otherView)
        {
            view.ToAutoLayout();
            ActivateConstraints(
                view.CenterYAnchor.ConstraintEqualTo(otherView.CenterYAnchor)
            );
        }

        public static NSLayoutConstraint ClipToTop(this UIView view, UIView otherView, nfloat? spacing = null, bool safeArea = false)
        {
            view.ToAutoLayout();

            var cstr = view.TopAnchor.ConstraintEqualTo(safeArea ? otherView.SafeAreaLayoutGuide.TopAnchor : otherView.TopAnchor, spacing ?? 0);

            ActivateConstraints(cstr);

            return cstr;
        }

        public static NSLayoutConstraint ClipToBottom(this UIView view, UIView otherView, nfloat? spacing = null, bool safeArea = false)
        {
            view.ToAutoLayout();

            var cstr = view.BottomAnchor.ConstraintEqualTo(safeArea ? otherView.SafeAreaLayoutGuide.BottomAnchor : otherView.BottomAnchor, -spacing ?? 0);

            ActivateConstraints(cstr);

            return cstr;
        }

        public static NSLayoutConstraint ClipToLeading(this UIView view, UIView otherView, nfloat? spacing = null)
        {
            view.ToAutoLayout();

            var cstr = view.LeadingAnchor.ConstraintEqualTo(otherView.LeadingAnchor, spacing ?? 0);

            ActivateConstraints(cstr);

            return cstr;
        }

        public static NSLayoutConstraint ClipToTrailing(this UIView view, UIView otherView, nfloat? spacing = null)
        {
            view.ToAutoLayout();

            var cstr = view.TrailingAnchor.ConstraintEqualTo(otherView.TrailingAnchor, -spacing ?? 0);

            ActivateConstraints(cstr);

            return cstr;
        }

        public static void SetupScroll(this UIScrollView scrollView, UIView contentView)
        {
            scrollView.ToAutoLayout();
            scrollView.BindToView();
            contentView.ToAutoLayout();
            contentView.WidthAnchor.ConstraintEqualTo(scrollView.WidthAnchor).Active = true;
            contentView.BindToView(scrollView);
        }

        public static void SetupScroll(this UIScrollView scrollView, UIView contentView, nfloat marginBottom)
        {
            scrollView.ToAutoLayout();
            ActivateConstraints(
                scrollView.LeadingAnchor.ConstraintEqualTo(scrollView.Superview.LeadingAnchor),
                scrollView.TrailingAnchor.ConstraintEqualTo(scrollView.Superview.TrailingAnchor),
                scrollView.TopAnchor.ConstraintEqualTo(scrollView.Superview.TopAnchor),
                scrollView.BottomAnchor.ConstraintEqualTo(scrollView.Superview.BottomAnchor, -marginBottom)
            );
            contentView.ToAutoLayout();
            contentView.WidthAnchor.ConstraintEqualTo(scrollView.WidthAnchor).Active = true;
            contentView.BindToView(scrollView);
        }

        public static void SetupScroll(this UIScrollView scrollView, UIView contentView, NSLayoutYAxisAnchor topAnchorViewBelowScroll)
        {
            scrollView.ToAutoLayout();
            ActivateConstraints(
                scrollView.LeadingAnchor.ConstraintEqualTo(scrollView.Superview.LeadingAnchor),
                scrollView.TrailingAnchor.ConstraintEqualTo(scrollView.Superview.TrailingAnchor),
                scrollView.TopAnchor.ConstraintEqualTo(scrollView.Superview.TopAnchor),
                scrollView.BottomAnchor.ConstraintEqualTo(topAnchorViewBelowScroll)
            );
            contentView.ToAutoLayout();
            contentView.WidthAnchor.ConstraintEqualTo(scrollView.WidthAnchor).Active = true;
            contentView.BindToView(scrollView);
        }

        public static void ActivateConstraints(params NSLayoutConstraint[] constraints)
        {
            NSLayoutConstraint.ActivateConstraints(constraints);
        }

        public static void HeightConstraint(this UIView view, nfloat equalTo)
        {
            view.ToAutoLayout();
            view.HeightAnchor.ConstraintEqualTo(equalTo).Active = true;
        }

        public static NSLayoutConstraint HeightConstraint(this UIView view, UIView otherView, nfloat multiplier)
        {
            view.ToAutoLayout();
            var cstr = view.HeightAnchor.ConstraintEqualTo(otherView.HeightAnchor, multiplier);
            cstr.Active = true;
            return cstr;
        }

        public static void MinHeightConstraint(this UIView view, UIView otherView, nfloat multiplier)
        {
            view.ToAutoLayout();
            view.HeightAnchor.ConstraintGreaterThanOrEqualTo(otherView.HeightAnchor, multiplier).Active = true;
        }

        public static void MaxHeightConstraint(this UIView view, UIView otherView, nfloat multiplier)
        {
            view.ToAutoLayout();
            view.HeightAnchor.ConstraintLessThanOrEqualTo(otherView.HeightAnchor, multiplier).Active = true;
        }

        public static void WidthConstraint(this UIView view, nfloat equalTo)
        {
            view.ToAutoLayout();
            view.WidthAnchor.ConstraintEqualTo(equalTo).Active = true;
        }

        public static void WidthConstraint(this UIView view, UIView otherView, nfloat multiplier)
        {
            view.ToAutoLayout();
            view.WidthAnchor.ConstraintEqualTo(otherView.WidthAnchor, multiplier).Active = true;
        }

        public static void SizeConstraint(this UIView view, nfloat size)
        {
            view.ToAutoLayout();
            view.WidthAnchor.ConstraintEqualTo(size).Active = true;
            view.HeightAnchor.ConstraintEqualTo(view.WidthAnchor).Active = true;
        }

        public static void SizeConstraint(this UIView view, nfloat width, nfloat height)
        {
            view.ToAutoLayout();
            view.WidthAnchor.ConstraintEqualTo(width).Active = true;
            view.HeightAnchor.ConstraintEqualTo(height).Active = true;
        }

        public static UIEdgeInsets Set(this UIEdgeInsets insets, nfloat? top = null, nfloat? bottom = null, nfloat? left = null, nfloat? right = null)
        {
            return new UIEdgeInsets(
                top ?? insets.Top,
                left ?? insets.Left,
                bottom ?? insets.Bottom,
                right ?? insets.Right
            );
        }

        public static void ConfigureCompressionX(this UIView view, nfloat resistance)
        {
            view.SetContentCompressionResistancePriority((float)resistance, UILayoutConstraintAxis.Horizontal);
            view.SetContentHuggingPriority(Math.Max(0f, (float)UILayoutPriority.DefaultHigh - (float)resistance), UILayoutConstraintAxis.Horizontal);
        }

        public static void ConfigureCompressionY(this UIView view, nfloat resistance)
        {
            view.SetContentCompressionResistancePriority((float)resistance, UILayoutConstraintAxis.Vertical);
            view.SetContentHuggingPriority(Math.Max(0f, (float)UILayoutPriority.DefaultHigh - (float)resistance), UILayoutConstraintAxis.Vertical);
        }

        #region Uso de componentes basados en AutoLayout en pantallas con Frames

        public static void SetFrame(UIView view, nfloat x, nfloat y, nfloat width)
        {
            // Desactivamos AutoLayout en la vista si estaba activado
            if (!view.TranslatesAutoresizingMaskIntoConstraints)
                view.TranslatesAutoresizingMaskIntoConstraints = true;

            var estimatedSize = view.SystemLayoutSizeFittingSize(UIView.UILayoutFittingExpandedSize);
            view.Frame = new CGRect(
              new CGPoint(x, y),
              new CGSize(width, estimatedSize.Height)
            );
        }

        #endregion

        public class Margins 
        {
            private UIEdgeInsets _insets;
            public UIEdgeInsets Insets => _insets;

            public Margins(nfloat margen) => _insets = new UIEdgeInsets(margen, margen, margen, margen);

            public Margins(nfloat margenHorizontal, nfloat margenVertical) => _insets = new UIEdgeInsets(margenVertical,
                                                                                                      margenHorizontal,
                                                                                                      margenVertical,
                                                                                                      margenHorizontal);

            public Margins(nfloat top, nfloat left, nfloat bottom, nfloat right) => _insets = new UIEdgeInsets(top,
                                                                                                      left,
                                                                                                      bottom,
                                                                                                      right);

            public Margins(nfloat all, nfloat? top = null, nfloat? left = null, nfloat? bottom = null, nfloat? right = null) => _insets = new UIEdgeInsets(top ?? all, left ?? all, bottom ?? all, right ?? all);

            public Margins(UIEdgeInsets insets) => this._insets = insets;

            public nfloat Top => _insets.Top;

            public nfloat Left => _insets.Left;

            public nfloat Bottom => _insets.Bottom;

            public nfloat Right => _insets.Right;
        }
    }
}
