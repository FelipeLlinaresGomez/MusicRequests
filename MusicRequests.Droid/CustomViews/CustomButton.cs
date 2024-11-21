using System;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Util;
using Android.Content.Res;
using Android.Graphics;

namespace MusicRequests.Droid
{
    public class CustomButton : Button
    {
		public CustomButton(Context context):base(context) {
			this.SetCustomTextFont (context);
		}

		public CustomButton(Context context, IAttributeSet attrs):base(context, attrs) {
			this.SetCustomTextFont(context,attrs);
		}

		public CustomButton(Context context, IAttributeSet attrs, int defStyleAttr):base(context, attrs, defStyleAttr) {
			this.SetCustomTextFont(context,attrs);
		}

	//	public CustomButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes):base(context, attrs, defStyleAttr, defStyleRes) {
	///		this.SetCustomTextFont(context,attrs);
		//}



    }
}