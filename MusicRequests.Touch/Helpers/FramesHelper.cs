using System;
using CoreGraphics;
using UIKit;

namespace MusicRequests.Touch.Helpers
{
    public static class FramesHelper
    {
        public static void UpdateHeight(UIView view, nfloat height)
        {
            view.Frame = new CGRect(
                view.Frame.X,
                view.Frame.Y,
                view.Bounds.Width,
                height
            );
        }
    }
}
