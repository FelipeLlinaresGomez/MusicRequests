using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Widget;

namespace MusicRequests.Droid
{
	[Register("MusicRequests.Droid.HtmlTextView")]
	public class HtmlTextView : TextView
	{
		public HtmlTextView(Context context) : base(context)
		{
            this.SetCustomTextFont(context);
		}

		public HtmlTextView(Context context, IAttributeSet attrs):base(context, attrs) {
			this.SetCustomTextFont(context,attrs);
		}

		public HtmlTextView(Context context, IAttributeSet attrs, int defStyleAttr):base(context, attrs, defStyleAttr) {
			this.SetCustomTextFont(context,attrs);
		}

		#region HtmlText

		private string _HtmlText;

		public string HtmlText
		{
			get { return _HtmlText; }
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					_HtmlText = value;
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                    {
                        this.TextFormatted = Html.FromHtml(value, FromHtmlOptions.ModeLegacy);
                    }
                    else
                    {
                        this.TextFormatted = Html.FromHtml(value);
                    }

                    this.MovementMethod = LinkMovementMethod.Instance;

				}
			}
		}

		#endregion


	}
}
