using System;
using CoreGraphics;
using UIKit;

namespace MusicRequests.Touch.Styles
{
    public static class UIViewStyles
    {
        public static void ApplyGroupBox(UIView view, UIColor color = null)
        {
            var colorToApply = color ?? Colors.Gray15;
            view.BackgroundColor = UIColor.White;
            view.Layer.BorderWidth = 0.75f;
            view.Layer.BorderColor = colorToApply.CGColor;
        }

        public static void ApplyCircleIconInverse(UIView view)
        {
            ApplyCircleBackground(view, Colors.Primary, 0, UIColor.Clear);
        }

        public static void ApplyCircleBackground(UIView view, UIColor backgroundColor, nfloat borderWidth, UIColor borderColor)
        {
            double min = Math.Min(view.Bounds.Width, view.Bounds.Height);
            view.Layer.CornerRadius = (float)(min / 2.0);
            view.Layer.BorderColor = borderColor.CGColor;
            view.Layer.BorderWidth = borderWidth;
            view.BackgroundColor = backgroundColor;
            view.ClipsToBounds = true;
        }

        public static void ApplyBackgroundWhiteButton(UIView view)
        {
            double min = Math.Min(view.Bounds.Width, view.Bounds.Height);
            view.BackgroundColor = UIColor.White;
            view.TintColor = Colors.Primary;
            view.Layer.CornerRadius = (float)(min / 2.0);
        }

        public static void FadeAnimation(UIView view, bool isIn, double duration = 0.3, Action onFinished = null)
        {
            var minAlpha = (nfloat)0.0;
            var maxAlpha = (nfloat)1.0;
            view.Alpha = isIn ? minAlpha : maxAlpha;
            view.Transform = CGAffineTransform.MakeIdentity();
            UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
            {
                view.Alpha = isIn ? maxAlpha : minAlpha;
            }, onFinished);
        }

    }
}
