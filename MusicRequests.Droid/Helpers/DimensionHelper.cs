using System;
using Android.Content;
using Android.Util;
using Android.Views;

namespace MusicRequests.Droid
{
	public class DimensionHelper
	{
		public static int DipToPx(Context context, float dipValue)
		{
			DisplayMetrics metrics = context.Resources.DisplayMetrics;
			return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dipValue, metrics);
		}

		public static float PxToDp(Context context, float pixels)
		{
			var resources = context.Resources;
			DisplayMetrics metrics = resources.DisplayMetrics;
			float dp = pixels / ((float)metrics.DensityDpi / (int)DisplayMetricsDensity.Default);
			return dp;
		}

		public static bool DeviceHasSoftKeys(IWindowManager windowManager)
		{
			bool hasSoftwareKeys = true;

			Display d = windowManager.DefaultDisplay;

			DisplayMetrics realDisplayMetrics = new DisplayMetrics();
			d.GetRealMetrics(realDisplayMetrics);

			int realHeight = realDisplayMetrics.HeightPixels;
			int realWidth = realDisplayMetrics.WidthPixels;

			DisplayMetrics displayMetrics = new DisplayMetrics();
			d.GetMetrics(displayMetrics);

			int displayHeight = displayMetrics.HeightPixels;
			int displayWidth = displayMetrics.WidthPixels;

			hasSoftwareKeys = (realWidth - displayWidth) > 0 || (realHeight - displayHeight) > 0;

			return hasSoftwareKeys;
		}
	}
}
