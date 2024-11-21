using System;
using Android.Content;
using Android.Graphics;
using Java.Lang.Reflect;

namespace MusicRequests.Droid.Helpers
{
	public class FontsOverride
	{
		public static void SetDefaultFont(Context context, string staticTypefaceFieldName, string fontAssetName)
		{
			Typeface regular = Typeface.CreateFromAsset(context.Assets, fontAssetName);
			ReplaceFont(staticTypefaceFieldName, regular);
		}

		protected static void ReplaceFont(string staticTypefaceFieldName, Typeface newTypeface)
		{
			try
			{
				Field staticField = ((Java.Lang.Object)(newTypeface)).Class.GetDeclaredField(staticTypefaceFieldName);
				staticField.Accessible = true;
				staticField.Set(null, newTypeface);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}

