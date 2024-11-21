using System;
using CoreGraphics;
using MusicRequests.Touch.Converters;
using UIKit;

namespace MusicRequests.Touch.Views.Controls
{
    public class SplashBackgroundView : UIImageView
    {
        byte[] _imageBytes;
        public byte[] ImageBytes
        {
            get => _imageBytes;
            set
            {
                var oldValue = ImageBytes;
                _imageBytes = value;

                var image = ByteArrayToUIImageConverter.Convert(value);

                if (value != oldValue)
                {
                    Transition(this, 0.3f, UIViewAnimationOptions.TransitionCrossDissolve, () => { Image = image; }, null);
                }
            }
        }

        UIImage? _image;
        public override UIImage? Image
        {
            get => _image;
            set
            {
                var oldValue = Image;
                _image = value;

                if (value != oldValue)
                {
                    Transition(this, 0.3f, UIViewAnimationOptions.TransitionCrossDissolve, () => { Image = value; }, null);
                }
            }
        }

        public SplashBackgroundView() : base() => Initialize();

        public SplashBackgroundView(CGRect frame) : base(frame) => Initialize();

        private void Initialize()
        {
            ContentMode = UIViewContentMode.ScaleToFill;
            UserInteractionEnabled = true;
        }
    }
}
