using System;
using UIKit;
using MvvmCross.Converters;

namespace MusicRequests.Touch
{
	public class SocialUIImageConverter: MvxValueConverter<String, UIImage> 
	{
		protected override UIImage Convert(String value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string imagePath = string.Format("Social_{0}", value);
			return UIImage.FromBundle(imagePath);
		}
	}
}
