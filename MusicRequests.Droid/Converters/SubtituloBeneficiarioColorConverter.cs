using System;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using MvvmCross.Converters;

namespace MusicRequests.Droid
{
    public class SubtituloBeneficiarioColorConverter : MvxValueConverter<string, SpannableStringBuilder>
    {
        protected override SpannableStringBuilder Convert (string value, Type type, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return new SpannableStringBuilder("");
            string nombreContraparte = parameter.ToString();

            SpannableStringBuilder subtitle = new SpannableStringBuilder(value);
            if (string.IsNullOrEmpty(nombreContraparte))
            {
                return subtitle;
            }

            int inicioTextoNombre = value.IndexOf(nombreContraparte, StringComparison.CurrentCulture);
            if (inicioTextoNombre != -1)
            {
				int finTextoNombre = inicioTextoNombre + nombreContraparte.Length;
				subtitle.SetSpan(new ForegroundColorSpan(Color.Rgb(0,98,174)), inicioTextoNombre, finTextoNombre, 0);
            }

            return subtitle;
        }
    }
}
