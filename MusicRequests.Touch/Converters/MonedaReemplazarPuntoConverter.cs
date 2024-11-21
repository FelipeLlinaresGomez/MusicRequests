using System;
using UIKit;
using MvvmCross.Converters;
using System.Globalization;

namespace MusicRequests.Touch
{
    public class MonedaReemplazarPuntoConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var cadena = value as string;
                return cadena.Replace(".", ",");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var cadena = value as string;
                return cadena.Replace(".", ",");
            }
            return string.Empty;
        }
    }
}