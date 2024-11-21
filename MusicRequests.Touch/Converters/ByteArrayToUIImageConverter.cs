using System;
using MvvmCross.Converters;
using UIKit;
using MusicRequests.Touch.Helpers;
using Foundation;

namespace MusicRequests.Touch.Converters
{
    public class ByteArrayToUIImageConverter : MvxValueConverter<byte[], UIImage>
    {
        protected override UIImage Convert(byte[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var fallBack = parameter is string fallBackImage ? fallBackImage : "products/picture_avatar_inverse.png";
            return value.ToImage(fallBack);
        }

        public static UIImage Convert(byte[] value)
        {
            return value.ToImage();
        }
    }
}

