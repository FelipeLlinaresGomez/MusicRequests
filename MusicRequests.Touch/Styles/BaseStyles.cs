using CoreAnimation;

namespace MusicRequests.Touch.Styles
{
    public static class BaseStyles
    {
        #region Card

        public class OuterShadow
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Blur { get; set; }
            public float Spread { get; set; }
            public CGColor Color { get; set; }
            public float Alpha { get; set; }
        }

        public static void ApplyShadow(UIView view, OuterShadow shadow)
        {
            view.Layer.ShadowColor = shadow.Color;
            view.Layer.ShadowOpacity = 1 - shadow.Alpha;
            view.Layer.ShadowOffset = new CGSize(shadow.X, shadow.Y);
            view.Layer.ShadowRadius = shadow.Blur / 2f;

            if (shadow.Spread == 0)
            {
                view.Layer.ShadowPath = null;
            }
            else
            {
                var dx = -shadow.Spread;
                var rect = view.Bounds.Inset(dx, dx);
                view.Layer.ShadowPath = UIBezierPath.FromRect(rect).CGPath;
            }
        }

        public static OuterShadow DefaultShadow => new OuterShadow()
        {
            X = 0,
            Y = 2,
            Blur = 4,
            Spread = 0,
            Color = UIColor.Black.CGColor,
            Alpha = 0.9f
        };

        public static CACornerMask NoCorners => 0;
        public static CACornerMask AllCorners => UpperCorners | LowerCorners;
        public static CACornerMask UpperCorners => CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner;
        public static CACornerMask LowerCorners => CACornerMask.MinXMaxYCorner | CACornerMask.MaxXMaxYCorner;

        public static void Card(UIView view, CACornerMask? mask = null, OuterShadow shadow = null)
        {
            view.BackgroundColor = UIColor.White;

            view.Layer.CornerRadius = 3;
            view.Layer.MaskedCorners = mask ?? AllCorners;

            ApplyShadow(view, shadow ?? DefaultShadow);
        }

        public static void Card(UIView view, nfloat cornerRadius, CACornerMask? mask = null, OuterShadow shadow = null)
        {
            view.BackgroundColor = UIColor.White;

            view.Layer.CornerRadius = cornerRadius;
            view.Layer.MaskedCorners = mask ?? AllCorners;

            ApplyShadow(view, shadow ?? DefaultShadow);
        }

        #endregion

        #region UIViewController extensions

        public static void DefaultNavigationBarAppearance(UINavigationBar navigationBar)
        {
            var appearance = new UINavigationBarAppearance();

            appearance.ConfigureWithOpaqueBackground();
            appearance.BackgroundColor = Colors.Primary;

            appearance.ShadowColor = UIColor.Clear;

            if (navigationBar is { })
            {
                navigationBar.TintColor = UIColor.White;

                navigationBar.ScrollEdgeAppearance = appearance;
                navigationBar.CompactAppearance = appearance;
                navigationBar.StandardAppearance = appearance;
            }
        }

        public static void SetNavigationStyle(UIViewController controller)
        {
            if (controller.NavigationController is { })
            {
                controller.NavigationController.NavigationBar.BarStyle = UIBarStyle.Default;
                controller.NavigationController.SetNavigationBarHidden(false, false);

                DefaultNavigationBarAppearance(
                    controller?.NavigationController?.NavigationBar
                );
            }
        }

        public static void SetNavTitle(UIViewController controller, string title, bool bold = true)
        {
            if (controller.NavigationItem != null)
            {
                if (controller.NavigationItem.TitleView is UILabel label)
                {
                    label.Text = title;
                }
                else
                {
                    var labelTitle = new UILabel(new CGRect(0, 0, 200, 44))
                    {
                        TextAlignment = UITextAlignment.Center,
                        Text = title
                    };

                    labelTitle.Font = bold ? Fonts.MusicRequestsFont.MediumOfSize(18) : Fonts.MusicRequestsFont.OfSize(18);

                    labelTitle.TextColor = UIColor.White;
                    controller.NavigationItem.TitleView = labelTitle;

                    controller.NavigationItem.TitleView = labelTitle;
                }
            }
        }

        public static void SetNavigationStyleWithImage(this UIViewController controller, string logoPath = "AppIconPruebas")
        {
            if (controller?.NavigationController is { })
            {
                controller.NavigationController.SetNavigationBarHidden(false, false);

                DefaultNavigationBarAppearance(
                    controller?.NavigationController?.NavigationBar
                );
            }

            if (controller?.NavigationItem is UINavigationItem navItem)
            {
                navItem.TitleView = new UIImageView(UIImage.FromBundle(logoPath) ?? UIImage.FromFile(logoPath))
                {
                    ContentMode = UIViewContentMode.Center
                };
            }
        }

        public static void SetNavigationStyleWithText(this UIViewController controller, string title, UIColor tintColor = null, UIColor backgroundColor = null)
        {
            if (controller?.NavigationController != null)
            {
                controller.NavigationController.SetNavigationBarHidden(false, false);

                DefaultNavigationBarAppearance(
                    controller?.NavigationController?.NavigationBar
                );

                var labelTitle = new UILabel(new CGRect(0, 0, 200, 44))
                {
                    Text = title,
                    TextAlignment = UITextAlignment.Center
                };

                labelTitle.Font = Fonts.MusicRequestsFont.MediumOfSize(18);
                labelTitle.TextColor = UIColor.White;
                controller.NavigationItem.TitleView = labelTitle;
            }
        }

        #endregion
    }
}