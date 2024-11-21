using System;
using Android.Graphics;
using Android.Text.Style;

namespace MusicRequests.Droid
{
	public class CustomTypefaceSpan : TypefaceSpan
	{

		private Typeface newType;

		public CustomTypefaceSpan(String family, Typeface type) : base(family)
		{
			newType = type;
		}

		public override void UpdateDrawState(Android.Text.TextPaint ds)
		{
			applyCustomTypeFace(ds, newType);
		}

		public override void UpdateMeasureState(Android.Text.TextPaint paint)
		{
			applyCustomTypeFace(paint, newType);
		}


		private static void applyCustomTypeFace(Paint paint, Typeface tf)
		{
			int oldStyle;
			Typeface old = paint.Typeface;
			if (old == null)
			{
				oldStyle = 0;
			}
			else {
				oldStyle = (int)old.Style;
			}

			//int fake = oldStyle & ~tf.Style;
			//if ((fake & Typeface.BOLD) != 0)
			//{
			//	paint.setFakeBoldText(true);
			//}

			//if ((fake & Typeface.ITALIC) != 0)
			//{
			//	paint.se
			//	paint.setTextSkewX(-0.25f);
			//}

			paint.SetTypeface(tf);
		}
	}
}