using System;
using Android.Graphics;

namespace MusicRequests.Droid.Helpers
{
    public class ColorsHelper {
		static int[] colors2 = {Color.ParseColor("#fb0000"),
			Color.ParseColor("#fbf400"), Color.ParseColor("#00FF00")};
		private bool isAllowedMatchParent;
		static int[] colors1;

		public ColorsHelper() {
		}

		public int[] GetColors2() {
			return colors2;
		}

		public static void SetColors2(int[] colors) {
			colors1 = colors;
		}

		// GRADIENT	METHODS
		public void SetGradientPaint(Paint paint, float left, float top,
			float right, float bottom) {

			SetGradientPaint(paint, left, top, right, bottom, colors2);
		}

		public void SetGradientPaint(Paint paint, float left, float top,
			float right, float bottom, int[] colors2) {
			LinearGradient linearGradient = new LinearGradient(left, top, right,
				bottom, colors2, null, Android.Graphics.Shader.TileMode.Clamp);
			paint.SetShader(linearGradient);
			paint.AntiAlias = (true);
		}

		public void SetSweepGradientPaint(Paint paint, float width,
			float height, Color colorStart, Color colorEnd) {
			SetSweepGradientGradientPaint(paint, width, height, colorStart, colorEnd);
		}

		protected static void SetSweepGradientGradientPaint(Paint paint, float width,
			float height, Color colorStart, Color colorEnd) {
			paint.SetShader(new SweepGradient(width, height, colorStart, colorEnd));
			paint.AntiAlias = (true);
		}

		// *********************************END OF GRADIENT
		// METHODS**************************************************
		public void DrawTextCenter(Canvas canvas, String text, Color color, int size,
			int min) {
			Paint innerPaint = new Paint();
			innerPaint.AntiAlias = (true);
			innerPaint.SetStyle(Android.Graphics.Paint.Style.Fill);
			innerPaint.Color = (color);
			innerPaint.TextSize = (size);
			// int cHeight = canvas.getClipBounds().height();
			// int cWidth = canvas.getClipBounds().width();
			Rect r = new Rect();
			// setTextMatchParent(text,innerPaint,width-strokeWidth*2-25);
			innerPaint.TextAlign = (Paint.Align.Left);
			innerPaint.GetTextBounds(text, 0, text.Length, r);
			float x = min / 2f - r.Width() / 2f - r.Left;
			float y = min / 2f + r.Height() / 2f - r.Bottom;

			canvas.DrawText(text, x, y, innerPaint);
		}

		private void SetTextMatchParent(String text, Paint paint, float desiredWidth) {
			CalculateTextlength(text);
			if (isAllowedMatchParent) {
				SetTextSizeForWidth(text, paint, desiredWidth);
			}
		}

		private void CalculateTextlength(String text) {
			int textLength = text.Length;
			if (textLength > 4) {
				isAllowedMatchParent = true;
			} else {
				const float testTextSize = 38f;
			}
		}

		private static void SetTextSizeForWidth(String text, Paint paint,
			float desiredWidth) {

			const float testTextSize = 48f;
			paint.TextSize = (testTextSize);
			Rect bounds = new Rect();
			paint.GetTextBounds(text, 0, text.Length, bounds);
			float desiredTextSize = testTextSize * desiredWidth / bounds.Width();
			paint.TextSize = (desiredTextSize);
		}
			
		private void DrawText(String text, Canvas canvas) {
			Paint innerPaint = new Paint();
			innerPaint.AntiAlias = (true);
			innerPaint.SetStyle(Android.Graphics.Paint.Style.Fill);
			innerPaint.Color = (Color.Black);
			int xPos = (canvas.Width / 2);
			int yPos = (int) ((canvas.Height / 2) - ((innerPaint.Descent() + innerPaint
				.Ascent()) / 2));

			canvas.DrawText(text, xPos, yPos, innerPaint);
		}

	}
}

