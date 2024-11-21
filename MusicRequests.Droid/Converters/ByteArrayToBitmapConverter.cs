using System;
using MvvmCross.Converters;
using Android.Graphics;
using MvvmCross.Platforms.Android;
using MvvmCross;

namespace MusicRequests.Droid
{
    public class ByteArrayToBitmapConverter : MvxValueConverter<byte[], Bitmap>
	{
		protected override Bitmap Convert (byte[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value != null)
			{
                return BitmapFactory.DecodeByteArray(value, 0, value.Length);
            }
			else if (parameter == null)
			{
				var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
				return BitmapFactory.DecodeResource(activity.Resources, Resource.Drawable.ic_user_login);
			}

			return null;

		}
	}		
}

