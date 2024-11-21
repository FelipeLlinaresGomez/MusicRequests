using System;
using MvvmCross.Converters;

namespace MusicRequests.Core.Converters
{
    public class StringToShortStringOfuscadoConverter : MvxValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return OfuscarNumero(value?.Trim());
        }

        public static string OfuscarNumero(string numero)
        {
            if (!string.IsNullOrEmpty(numero))
            {
                if (numero.Length <= 4)
                {
                    return numero;
                }
                else
                {
                    return "**** " + numero.Substring(numero.Length - 4, 4);
                }
            }

            return "";
        }
    }
}

