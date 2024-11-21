using System;
using Android.Views;
using MvvmCross.Converters;

namespace MusicRequests.Droid
{
	public class InverseBoolToInvisibleConverter : MvxValueConverter<bool, ViewStates>
	{
		protected override ViewStates Convert(bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value ? ViewStates.Invisible : ViewStates.Visible;
		}
	}
}
