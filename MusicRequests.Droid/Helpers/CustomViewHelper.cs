using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MusicRequests.Droid
{
	public static class CustomViewHelper
	{
		public static TextView SetCustomTextFont(this TextView view, Context context, IAttributeSet attrs)
		{
			if (!view.IsInEditMode)
			{
				var customFont = Typefaces.MUSICREQUESTS_REGULAR;

				int[] attrsArray = new int[] { Android.Resource.Attribute.TextStyle };
				TypedArray t = context.ObtainStyledAttributes(attrs, Resource.Styleable.CustomFont, 0, 0);
				var fontInt = t.GetInt(Resource.Styleable.CustomFont_customFont, 0);

				switch (fontInt)
				{
					case 1:
						customFont = Typefaces.MUSICREQUESTS_BOLD;
						break;
					case 4:
						customFont = Typefaces.MUSICREQUESTS_MEDIUM;
						break;
					case 2:
						customFont = Typefaces.MUSICREQUESTS_ITALIC;
						break;
					case 3:
						customFont = Typefaces.MUSICREQUESTS_EXTRABOLD;
						break;
					default:
						customFont = Typefaces.MUSICREQUESTS_REGULAR;
						break;
				}

                if (view.Typeface.IsItalic)
                {
                    customFont = Typefaces.MUSICREQUESTS_ITALIC;
                }

                if (!string.IsNullOrEmpty(customFont))
				{
					// Respetamos el estilo, puede que la fuente en si no soporte ese atributo
					int[] textStyleAttr = new int[] { Android.Resource.Attribute.TextStyle };
					int indexOfAttrTextStyle = 0;
					TypedArray a = context.ObtainStyledAttributes(attrs, textStyleAttr);
					int styleIndex = a.GetInt(indexOfAttrTextStyle, -1);
					a.Recycle();

					if (styleIndex == (int)TypefaceStyle.Italic) //Cursiva
					{
						view.SetTypeface(Typefaces.Get(context, customFont), TypefaceStyle.Italic);
					}
					else if (styleIndex == (int)TypefaceStyle.Bold) // negrita
					{
						view.SetTypeface(Typefaces.Get(context, customFont), TypefaceStyle.Bold);
					}
					else if (styleIndex == (int)TypefaceStyle.BoldItalic) // negrita cursiva
					{
						view.SetTypeface(Typefaces.Get(context, customFont), TypefaceStyle.BoldItalic);
					}
					else
					{
						view.SetTypeface(Typefaces.Get(context, customFont), TypefaceStyle.Normal);
					}
				}
				
			
				t.Recycle();
			}

			return view;
		}


		public static TextView SetCustomTextFont(this TextView view, Context context)
		{
			return view.SetCustomTextFont(context, Typefaces.MUSICREQUESTS_REGULAR);
		}


		public static TextView SetCustomTextFont(this TextView view, Context context, string typeFace)
		{
			if (!view.IsInEditMode)
			{
				var customFont = typeFace;
				view.SetTypeface(Typefaces.Get(context, customFont), TypefaceStyle.Normal);
			}

			return view;
		}
	}
}
