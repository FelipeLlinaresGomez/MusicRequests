using System;
using MvvmCross.Converters;
using Android.Text;
using MusicRequests.Core.ViewModels;
using MusicRequests.Core.Models;

namespace MusicRequests.Droid
{
    public class AmountViewModelToFormattedTextConverter : MvxValueConverter<AmountViewModel, SpannableStringBuilder>
	{
		protected override SpannableStringBuilder Convert (AmountViewModel value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null || value.Amount == null) return new SpannableStringBuilder("");
			var showEmptyValue = false;
			if (parameter != null && parameter is string) 
			{
				var parameterValue = (string)parameter;
				showEmptyValue = parameter.Equals("0");
			}
			return Typefaces.GetNumericSpannableStringFromDouble (value, showEmptyValue);
		}
	}
}

