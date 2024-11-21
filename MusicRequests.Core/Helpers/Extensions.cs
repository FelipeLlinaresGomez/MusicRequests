using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using MvvmCross;
using MusicRequests.Core.Services;
using System.Text.RegularExpressions;
using MusicRequests.Core.ViewModels;
using MvvmCross.Localization;

namespace MusicRequests.Core.Helpers
{
	public static class Extensions
	{
		public static byte[] ToArray(this Stream stream)
		{
			using(var memoryStream = new MemoryStream())
			{
				stream.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
		}

		#region ToAmmountFormat

		public static Tuple<string, int> ToAmountFormat (this double? value, string culture, string currency)
		{
			if (value.HasValue == false)
				return new Tuple<string, int> (string.Empty, 0);
			return ToAmountFormat(value.Value, culture, currency);
		}


		public static Tuple<string, int> ToAmountFormat(this double value, string culture, string currency)
		{
			var cultureInfo = new CultureInfo(culture);
			string text = string.Format("{0} {1}",
				value.ToString("N2", cultureInfo),
				!string.IsNullOrWhiteSpace(currency)?currency:string.Empty);

			string decimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;
			
			string[] splitAmount = text.Split(decimalSeparator.ToCharArray());
			int iAmountSize = splitAmount[0].Length;

			return new Tuple<string, int> (text, iAmountSize);

		}

		/// <summary>
		/// Devuelve un string con el formato de ammount utilizado en la app
		/// </summary>
		/// <returns>El texto formateado, con la cantidad y la moneda</returns>
		/// <param name="value">El valor del ammount</param>
		/// <param name="currency">La moneda del ammount</param>
		public static string ToAmmountFormat(this double value, string currency) 
		{ 
			var _localizationService = Mvx.IoCProvider.Resolve<ILocalizationService>();
			var culture = _localizationService.GetCurrentCulture();

			var cultureInfo = new CultureInfo(culture);

			string text = string.Format("{0} {1}",
				value.ToString("N2", cultureInfo),
				currency);
			return text;
		}

		/// <summary>
		/// Devuelve un importe formateado a la cultura de la app
		/// </summary>
		/// <returns>The  ammount format.</returns>
		/// <param name="value">Value.</param>
		public static string ToAmmountFormat (this double value)
		{
			var _localizationService = Mvx.IoCProvider.Resolve<ILocalizationService> ();
			var culture = _localizationService.GetCurrentCulture ();

			var cultureInfo = new CultureInfo (culture);

			string text = value.ToString ("N2", cultureInfo);
			return text;
		}

		public static string ToAmmountFormat(this AmountViewModel value)
		{
			if (value == null || value.Amount == null) return "";
			
			var cantidad = value.AmountValue;
			var currency = value.Currency;

			var _localizationService = Mvx.IoCProvider.Resolve<ILocalizationService>();
			var culture = _localizationService.GetCurrentCulture();

			var cultureInfo = new CultureInfo(culture);

			string text = string.Format("{0} {1}",
				cantidad.ToString("N2", cultureInfo),
				currency);
			return text;
		}




		#endregion

		#region ToString4Decimals
		/// <summary>
		/// Tos the string4 decimals.
		/// </summary>
		/// <returns>The string4 decimals.</returns>
		/// <param name="value">Value.</param>
		public static string ToString4Decimals (this double? value)
		{
			if (value.HasValue == false)
				return "--";
			var _localizationService = Mvx.IoCProvider.Resolve<ILocalizationService> ();
			var culture = _localizationService.GetCurrentCulture ();

			var cultureInfo = new CultureInfo (culture);

			string text = value.Value.ToString (cultureInfo);
			return text;
		}
        #endregion

        #region ToString4Decimals
        /// <summary>
        /// Tos the string4 decimals.
        /// </summary>
        /// <returns>The string4 decimals.</returns>
        /// <param name="value">Value.</param>
        public static string ToString4Decimals(this double value, bool IsVisible)
        {
            if (IsVisible == false)
                return "--";
            var _localizationService = Mvx.IoCProvider.Resolve<ILocalizationService>();
            var culture = _localizationService.GetCurrentCulture();

            var cultureInfo = new CultureInfo(culture);

            string text = value.ToString(cultureInfo);
            return text;
        }
        #endregion

        #region IsEqual
        public static bool IsEqual<T> (this List<T> list1, List<T> list2)
		{
			if ((list1 == null) || (list2 == null)) return false;

			foreach (T t in list1)
				if (!list2.Contains (t))
					return false;

			foreach (T t in list2)
				if (!list1.Contains (t))
					return false;

			return true;


		}
		#endregion

		#region DateTime Extensions

		public static DateTime GetActualYearBegin (this DateTime date)
		{
			return new DateTime (DateTime.Today.Year, 1, 1);
		}

		public static DateTime GetLast90DaysBegin (this DateTime date)
		{
			return new DateTime (DateTime.Today.AddDays (-90).Year,
							   DateTime.Today.AddDays (-90).Month,
							  DateTime.Today.AddDays (-90).Day);
		}

		public static DateTime GetLastMonthBegin (this DateTime date)
		{
			return new DateTime (DateTime.Today.AddMonths (-1).Year,
												   DateTime.Today.AddMonths (-1).Month,
								 1);
		}

		public static DateTime GetActualMonthBegin (this DateTime date)
		{
			return new DateTime (DateTime.Today.Year,
												   DateTime.Today.Month,
								 1);
		}

		public static DateTime GetLast3MonthsBegin (this DateTime date)
		{
			return new DateTime (DateTime.Today.AddMonths(-3).Year,
								DateTime.Today.AddMonths (-3).Month,
								 1);
		}

		public static DateTime GetLastMonthEnd (this DateTime date)
		{
			return new DateTime (DateTime.Today.AddMonths (-1).Year,
								 DateTime.Today.AddMonths (-1).Month,
								 new DateTime (DateTime.Today.Year,
								   				DateTime.Today.Month,
											   1).AddDays (-1).Day);
		}

		public static bool SameDayMonthYear (this DateTime date1, DateTime date2)
		{
			return (date1.Year == date2.Year) && (date1.Month == date2.Month) && (date1.Day == date2.Day);
		}

		public static string ToddMMMyyyyFormat (this DateTime date)
		{
			var localizationService = Mvx.IoCProvider.Resolve<ILocalizationService> ();
			var dateString = date.ToString ("dd-MMM-yyyy", new CultureInfo (localizationService.GetCurrentCulture ()));
			return dateString;
		}

		public static string ToHHmmFormat (this DateTime date)
		{
			var localizationService = Mvx.IoCProvider.Resolve<ILocalizationService> ();
			var dateString = date.ToString ("HH:mm", new CultureInfo (localizationService.GetCurrentCulture ()));
			return dateString;
		}

		public static DateTime ddMMMyyyyFormatToDateTime(this string strDate)
		{
			DateTime result;
			string[] format = { "dd-MMM-yyyy" };
			var localizationService = Mvx.IoCProvider.Resolve<ILocalizationService>();
			DateTime.TryParseExact(strDate,
								   format,
			                       new CultureInfo(localizationService.GetCurrentCulture()),
								   System.Globalization.DateTimeStyles.None,
								   out result);
			return result;
		}

		public static string ToddMMMyyyyHHmmFormat(this DateTime date)
		{
			var localizationService = Mvx.IoCProvider.Resolve<ILocalizationService>();
			var dateString = date.ToString("dd-MMM-yyyy HH:mm", new CultureInfo(localizationService.GetCurrentCulture()));
			return dateString;
		}

		public static string ToLongDateFormat(this DateTime date)
		{
			var localizationService = Mvx.IoCProvider.Resolve<ILocalizationService>();
			var dayName = date.ToString("dddd", new CultureInfo(localizationService.GetCurrentCulture()));
			var dateString = date.ToString("dd MMM yyyy", new CultureInfo(localizationService.GetCurrentCulture()));
			return string.Format("{0}, {1}", dayName, dateString);
		}

		#endregion

		public static string UpperFirst(this string source)
		{
			return string.IsNullOrEmpty(source) ? 
                string.Empty : 
                source.Substring (0, 1).ToUpper () + source.Substring (1).ToLower ();
		}

		/// <summary>
		/// Metodo de extensión de nuestro servicio de localización que valida un importe segun la cultura del teléfono
		/// </summary>
		/// <returns>The importe.</returns>
		/// <param name="service">Service.</param>
		/// <param name="importe">Importe.</param>
		public static bool ValidarImporte (this ILocalizationService service, string importe)
		{
			if (string.IsNullOrEmpty (importe))
				return false;

			var cultureInfo = new CultureInfo (service.GetCurrentCulture ());
			string decimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;
			string millions = cultureInfo.NumberFormat.NumberGroupSeparator;

			var pattern = "^\\(?\\s*\\(?(((\\d{1,3}((\\" + millions + "\\d{3})*|\\d*))?(\\" + decimalSeparator 
				+ "\\d{1,2})?)|((\\d{1,3}((\\" + millions + "\\d{3})*|\\d*))(\\" + decimalSeparator + "\\d{0,2})?))\\)?$";

			var regex = new Regex (pattern);
			var match = regex.Match (importe);

			return match.Success;
		}

		/// <summary>
		/// Metodo de extensión de nuestro servicio de localización que devuelve el separador decimal según la cultura del teléfono
		/// </summary>
		/// <returns>The decimal separator.</returns>
		/// <param name="service">Service.</param>
		public static string GetDecimalSeparator (this ILocalizationService service)
		{
			var cultureInfo = new CultureInfo (service.GetCurrentCulture ());
			return cultureInfo.NumberFormat.NumberDecimalSeparator;
		}

		public static double ToDouble (this string input)
		{
			Double doubleInput;
			Double.TryParse (input, 
			                 NumberStyles.Number, 
			                 new CultureInfo(Mvx.IoCProvider.Resolve<ILocalizationService>().GetCurrentCulture()), 
			                 out doubleInput);
			return doubleInput;
		}

		public static DateTime ToDateTime (this ILocalizationService service, string input)
		{
			DateTime date = DateTime.MinValue;
			var cultureInfo = new CultureInfo (service.GetCurrentCulture ());
			DateTime.TryParseExact (input, "dd-MMM-yyyy", cultureInfo, DateTimeStyles.None, out date);
			return date;
		}

		public static string ToIbanFormat (this string IBAN)
		{
			return !string.IsNullOrWhiteSpace (IBAN) ? Regex.Replace (IBAN, ".{4}", "$0 ") : IBAN;
		}

		public static bool IsNotNullOrEmpty<T> (this IEnumerable<T> collection)
		{
			return collection != null && collection.Any ();
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
		{
			return collection == null || collection.Any() == false;
		}

		/// <summary>
		/// Validación que utilizamos para comprobar que un double es multiplo del valor minimum y 
		/// tiene igual o menor número de decimales que numDecimals
		/// </summary>
		/// <returns><c>true</c>, if valid double with decimals was ised, <c>false</c> otherwise.</returns>
		/// <param name="amount">Amount.</param>
		/// <param name="numDecimals">Number decimals.</param>
		/// <param name="minimum">Minimum.</param>
		public static bool IsValidDoubleWithDecimals (this double amount, int numDecimals, double minimum)
		{
			var power = Math.Pow (10, minimum.GetNumberOfDecimals ());
			var result = (amount * power) % (minimum * power);
			return result == 0 && amount.GetNumberOfDecimals() <= numDecimals;
		}

		/// <summary>
		/// Devuelve el numero de caracteres decimales que posee un double
		/// </summary>
		/// <returns>The number of decimals.</returns>
		/// <param name="amount">Amount.</param>
		public static int GetNumberOfDecimals (this double amount)
		{
			var localizationService = Mvx.IoCProvider.Resolve<ILocalizationService> ();
			string decimalSeparator = localizationService.GetDecimalSeparator ();
			string amountString = amount.ToString4Decimals (true);

			if (amountString.IndexOf (decimalSeparator) < 1)
				return 0;
			
			var val = amountString.Length - amountString.IndexOf (decimalSeparator) - 1;
			return val;
		}

		/// <summary>
		/// Calculamos el tamaño de un array de bytes basado en su representación base64
		/// </summary>
		/// <returns>The size base64 string.</returns>
		/// <param name="bytes">Bytes.</param>
		public static int CalculateSizeBase64String(this byte[] bytes)
		{
			if (bytes.Length > 0)
			{
				string base64 = Convert.ToBase64String(bytes);
				return base64.Length;
			}
			return 0;
		}


		public static string ToPhoneNumberFormat(this string val)
		{
			var chars = val.ToCharArray();
			string newString = string.Empty;
			for (int i = 0; i < chars.Length; i++)
			{
				if (i % 3 == 0)
					newString += " ";
				newString += chars[i];
			}
			return newString;
		}

		/// <summary>
		/// Convierte un string en un AmountViewModel teniendo en cuenta la cultura del teléfono
		/// </summary>
		/// <returns>The amount view model.</returns>
		/// <param name="value">Value.</param>
		/// <param name="currency">Currency.</param>
		public static AmountViewModel ToAmountViewModel(this string value, string currency)
		{
			var _localizationService = Mvx.IoCProvider.Resolve<ILocalizationService>();
			if (_localizationService.ValidarImporte(value))
			{
				return new AmountViewModel()
				{
					Amount = value.ToDouble(),
					Currency = currency
				};
			}
			return null;
		}

		/// <summary>
		/// Método de extensión de IMvxTextProvider para recuperar el valor de un recurso
		/// </summary>
		/// <returns>The resource.</returns>
		/// <param name="provider">Provider.</param>
		/// <param name="viewModel">View model.</param>
		/// <param name="key">Key.</param>
		public static string GetResource(this IMvxTextProvider provider, string viewModel, string key)
		{
			return provider.GetText(Constants.GeneralNamespace, viewModel, key);
		}


		/// <summary>
		/// Método de extensión de IMvxTextProvider para recuperar el valor de un recurso en el archivo shared
		/// </summary>
		/// <returns>The resource.</returns>
		/// <param name="provider">Provider.</param>
		/// <param name="key">Key.</param>
		public static string GetSharedResource(this IMvxTextProvider provider, string key)
		{
			return provider.GetText(Constants.GeneralNamespace, Constants.Shared, key);
		}

	}
}

