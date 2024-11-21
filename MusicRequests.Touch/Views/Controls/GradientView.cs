using System;
using CoreAnimation;
using Foundation;
using UIKit;
using MusicRequests.Touch.Styles;
namespace MusicRequests.Touch.Views.Controls
{
    public static class GradientView
    {
        //Crea una capa de gradiente semitrasnparente, uso GradientView.SetGradient(_myView);
        public static void SetGradientBlackTransparent(UIView view)
        {
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new[] { Colors.Transparent.CGColor, UIColor.Black.CGColor };
            gradientLayer.Locations = new NSNumber[] { 0.4, 0.9 };
            gradientLayer.Frame = view.Frame;

            view.BackgroundColor = UIColor.Clear;
            view.Layer.AddSublayer(gradientLayer);
        }

        public static void SetGradientBlackTransparentQuarterBottom(UIView view, float relativeHeight = 0.2f)
        {
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new[] { Colors.Transparent.CGColor, UIColor.Black.CGColor };
            gradientLayer.Locations = new NSNumber[] { 1 - relativeHeight, 1 };
            gradientLayer.Frame = view.Frame;

            view.BackgroundColor = UIColor.Clear;
            view.Layer.AddSublayer(gradientLayer);
        }

        /// <param name="color1">Color de la parte superior</param>
        /// <param name="color2">Color de la parte inferior</param>
        public static void SetGradient(UIView view, UIColor color1, UIColor color2)
        {
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new[] { color1.CGColor, color2.CGColor };
            gradientLayer.Locations = new NSNumber[] { 0, 1 };
            gradientLayer.Frame = view.Frame;

            view.BackgroundColor = UIColor.White;
            view.Layer.AddSublayer(gradientLayer);
        }
    }
}
