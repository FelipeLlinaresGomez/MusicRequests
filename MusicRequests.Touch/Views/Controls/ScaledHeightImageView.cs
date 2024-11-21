using CoreGraphics;
using Foundation;
using UIKit;

namespace MusicRequests.Touch.Views.Controls
{
    /// <summary>
    /// ImageView que ajusta su altura para preservar el aspect ratio de la imagen.
    /// </summary>
    [Register(nameof(ScaledHeightImageView))]
    public class ScaledHeightImageView : UIImageView
    {
        public ScaledHeightImageView() : base()
        {
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            InvalidateIntrinsicContentSize();
        }

        public override CGSize IntrinsicContentSize
        {
            get
            {
                if (Image is null)
                    return base.IntrinsicContentSize;
                var imgRatio = Image.Size.Height / Image.Size.Width;
                return new CGSize(Bounds.Size.Width, Bounds.Size.Width * imgRatio);
            }
        }
    }
}
