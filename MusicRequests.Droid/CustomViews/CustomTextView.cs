using System;
using Android.Content;
using Android.Widget;
using Android.Util;
using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using Android.OS;
using Android.Text.Method;
using Android.Views;

namespace MusicRequests.Droid
{
    public class CustomTextView : TextView
    {
        public CustomTextView(Context context):base(context)
        {
			this.SetCustomTextFont(context);
        }

		public CustomTextView(Context context, IAttributeSet attrs):base(context, attrs) {
			this.InitCustomTextView(context,attrs);
		}

		public CustomTextView(Context context, IAttributeSet attrs, int defStyleAttr):base(context, attrs, defStyleAttr) {
			this.InitCustomTextView(context,attrs);
		}

		#region DrawableLeftIcon

		string _DrawableLeftIcon;

		public string DrawableLeftIcon
		{
			get { return _DrawableLeftIcon; }
			set
			{
				_DrawableLeftIcon = value;
			    if (!string.IsNullOrEmpty(value))
			    {
			        SetIconDrawableFromResourceName(value, DrawablePosition.Left);
			    }
			    else
			    {
			        RemoveLateralDrawables();
			    }
			}
		}
		#endregion

        void RemoveLateralDrawables()
        {
            this.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
        }

        #region DrawableRightIcon

		string _DrawableRightIcon;

		public string DrawableRightIcon {
			get { return _DrawableRightIcon; }
			set {
				_DrawableRightIcon = value;
				if (!string.IsNullOrEmpty (value)){
					SetIconDrawableFromResourceName (value, DrawablePosition.Right);
				}
                else
                {
                    RemoveLateralDrawables();
                }
            }
		}

		#endregion

		private enum DrawablePosition { Left,Right}

		private void SetIconDrawableFromResourceName(string resourceName, DrawablePosition position)
		{
			try
			{
				if (resourceName.Contains(".png"))
				{
					resourceName = resourceName.Replace(".png", "");
				}
				var identifier = Context.Resources.GetIdentifier(resourceName, "drawable", Context.PackageName);
				if (position == DrawablePosition.Right)
				{
					this.SetCompoundDrawablesWithIntrinsicBounds(0, 0, identifier, 0);
				}
				else 
				{
					this.SetCompoundDrawablesWithIntrinsicBounds(identifier, 0, 0, 0);
				}
			}
			catch (Exception ex)
			{
				var msg = ex.Message;
			}

		}

		#region FormattedText

		private SpannableStringBuilder _FormattedText;

		public SpannableStringBuilder FormattedText {
			get { return _FormattedText; }
			set {
				_FormattedText = value;
				this.TextFormatted = value;
			}
		}

		#endregion

		private void InitCustomTextView(Context context, IAttributeSet attrs ){
			if(!IsInEditMode){

				this.SetCustomTextFont(context, attrs);

				var textViewAttrsStyle = context.ObtainStyledAttributes(attrs, Resource.Styleable.CustomTextView, 0, 0);
				var attrDrawableLeft = textViewAttrsStyle.GetDrawable(Resource.Styleable.CustomTextView_android_drawableLeft);
				if (attrDrawableLeft != null) 
				{
					this.SetCompoundDrawablesWithIntrinsicBounds(attrDrawableLeft,null,null,null);
				}

				var attrDrawableRight = textViewAttrsStyle.GetDrawable(Resource.Styleable.CustomTextView_android_drawableRight);
				if (attrDrawableRight != null)
				{
					this.SetCompoundDrawablesWithIntrinsicBounds(null, null, attrDrawableRight, null);
				}

				textViewAttrsStyle.Recycle();

			}
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
					FormatHtlmText(value);
					MovementMethod = HtmlLinkMovementMethod.Instance;
				}
			}
		}

		private void FormatHtlmText(string value)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
				this.TextFormatted = Html.FromHtml(value, FromHtmlOptions.ModeLegacy);
			else
				this.TextFormatted = Html.FromHtml(value);
		}

		#endregion

		#region HtmlTextNoClickable

		private string _HtmlTextNoClickable;

		public string HtmlTextNoClickable
		{
			get { return _HtmlTextNoClickable; }
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					_HtmlTextNoClickable = value;
					FormatHtlmText(value);
				}
			}
		}

		#endregion

		#region Overridables

		public override void SetTextAppearance(int resId)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
			{
				base.SetTextAppearance(Context, resId);
			}
			else
			{
				base.SetTextAppearance(resId);
			}
		}

		#endregion

		private class HtmlLinkMovementMethod : LinkMovementMethod
		{
			public override bool OnTouchEvent(TextView widget, ISpannable buffer, MotionEvent e)
			{
				try
				{
					return base.OnTouchEvent(widget, buffer, e);
				}
				catch (Exception ex)
				{
					return true;
				}
			}
		}
	}
}