using System;
using Android.Views;
using MvvmCross.Converters;

namespace MusicRequests.Droid
{
    public class DateTimeToConverter : MvxValueConverter<DateTime, string>
    {
        protected override string Convert(DateTime value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string returnString = "";

            if(parameter.ToString().CompareTo("time")==0)   //Dar formato aqui
            {
                string format = "hh:mm";
                returnString = value.ToString(format);
            }
            else
            {
                string format = "dd/MM/yyyy";
                returnString = value.ToString(format);
            }
            
            return returnString;
        }
    }
}