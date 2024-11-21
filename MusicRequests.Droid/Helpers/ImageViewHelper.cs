using System;
using Android.Widget;

namespace MusicRequests.Droid
{
	public static class ImageViewHelper
	{
		public static ImageView SetImageSourceByDrawableName(this ImageView view, string drawableName)
		{
			var resourceId = (int)typeof(Resource.Drawable).GetField(drawableName).GetValue(null);
			if (resourceId != 0)
			{
				view.SetImageResource(resourceId);
			}
			return view;
		}
	}
}
