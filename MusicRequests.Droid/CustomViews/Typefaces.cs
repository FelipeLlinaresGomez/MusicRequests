using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using System.Runtime.CompilerServices;
using Android.Text;
using Android.Text.Style;
using MusicRequests.Core.ViewModels;
using MusicRequests.Core.Helpers;
using MusicRequests.Core.Services;
using MvvmCross;

namespace MusicRequests.Droid
{
    public class Typefaces
	{
		private static readonly Dictionary<String, Typeface> cache = new Dictionary<string, Typeface>();

		public const string MUSICREQUESTS_BOLD = "fonts/musicRequests-Bold";
		public const string MUSICREQUESTS_REGULAR = "fonts/musicRequests-Regular";
		public const string MUSICREQUESTS_MEDIUM = "fonts/musicRequests-Medium";
		public const string MUSICREQUESTS_ITALIC = "fonts/musicRequests-Italic";
		public const string MUSICREQUESTS_EXTRABOLD = "fonts/musicRequests-ExtraBold";


		[MethodImpl(MethodImplOptions.Synchronized)]
		public static Typeface Get(Context c, String name){
			if(!cache.ContainsKey(name)){
				Typeface t = Typeface.CreateFromAsset(c.Assets, String.Format("{0}.ttf", name));
				cache.Add(name, t);
			}
			return cache[name];
		}  
		public static SpannableStringBuilder  GetNumericSpannableStringFromDouble(AmountViewModel amountViewModel, bool showEmptyValue = false){
			if (showEmptyValue) return new SpannableStringBuilder("--");
			if (amountViewModel == null) return new SpannableStringBuilder("");
			return GetNumericSpannableStringFromDouble(amountViewModel.AmountValue, amountViewModel.Currency);
		}

		private static SpannableStringBuilder  GetNumericSpannableStringFromDouble(double ammount, string moneda){
			//TODO: change this according to especs
			var _localizationService = Mvx.IoCProvider.Resolve<ILocalizationService> ();

			var amountFormat = ammount.ToAmountFormat(
				_localizationService.GetCurrentCulture(), 
				moneda);

			var formatText= amountFormat.Item1;
			var separatorPosition = amountFormat.Item2;

			var finalPoistion = formatText.Count ();

			var builder = new SpannableStringBuilder(formatText);

			var sizeSpan = new RelativeSizeSpan(0.75f); 

			builder.SetSpan(sizeSpan, separatorPosition, finalPoistion, SpanTypes.InclusiveInclusive);

			return builder;
		}
	}
}

