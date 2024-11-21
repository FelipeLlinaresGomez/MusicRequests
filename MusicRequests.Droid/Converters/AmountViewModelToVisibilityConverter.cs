using System;
using Android.Views;
using MusicRequests.Core.ViewModels;
using MvvmCross.Converters;

namespace MusicRequests.Droid
{
    public class AmountViewModelToVisibilityConverter : MvxValueConverter<AmountViewModel, ViewStates>
    {
        protected override ViewStates Convert(AmountViewModel value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value != null)
            {
                return value.Amount > 0 ? ViewStates.Visible : ViewStates.Gone;
            }
            else
            {
                return ViewStates.Gone;
            }
            
        }
    }

	public class InvertedAmountViewModelToVisibilityConverter : MvxValueConverter<AmountViewModel, ViewStates>
	{
		protected override ViewStates Convert(AmountViewModel value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            if (value != null)
            {
                return value.Amount > 0 ? ViewStates.Gone : ViewStates.Visible;
            }
            else
            {
                return ViewStates.Gone;
            }
        }
	}
}

