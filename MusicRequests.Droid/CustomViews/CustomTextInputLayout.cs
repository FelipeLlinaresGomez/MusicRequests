using Android.Content;
using Android.Util;
using Java.Lang.Reflect;
using System;
using Android.Graphics;
using Android.Text;
using Android.Widget;
using Google.Android.Material.TextField;

namespace MusicRequests.Droid
{
	public class CustomTextInputLayout : TextInputLayout
	{
		public CustomTextInputLayout (Context context) :
			base (context)
		{
			//Initialize ();
		}

		public CustomTextInputLayout (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			//Initialize ();
		}

		public CustomTextInputLayout (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			//Initialize ();
		}

		#region AlertText

		string alertText;
		public string AlertText
		{
			get { return alertText; }
			set
			{
				alertText = value;
				if (!string.IsNullOrEmpty(value))
				{
					this.ErrorEnabled = true;

					SetErrorTextColor(IsErrorAlert ? Resource.Color.ic_red : Resource.Color.ic_blue);
			
					this.Error = value;
				}
				else 
				{
					this.ErrorEnabled = false;
				}
			}
		}

		#endregion


		#region IsErrorAlert

		bool isErrorAlert = true;
		public bool IsErrorAlert
		{
			get { return isErrorAlert; }
			set
			{
				isErrorAlert = value;
			}
		}

		#endregion

		public void SetErrorTextColor(int color)
		{
			if (this.ErrorEnabled) 
			{ 
				try
				{
					Field fErrorView = this.Class.Superclass.GetDeclaredField("mErrorView");
					fErrorView.Accessible = (true);
					TextView mErrorView = (TextView)fErrorView.Get(this);
					mErrorView.SetTextColor(Context.Resources.GetColor(color));
					mErrorView.RequestLayout();
					//Field fCurTextColor = Java.Lang.Class.FromType(typeof(TextView)).GetDeclaredField("mCurTextColor");
					//fCurTextColor.Accessible = (true);
					//fCurTextColor.Set(mErrorView, color);
				}
				catch (Exception e)
				{
					var msg = e.Message;
				}
			}
		}


		void Initialize ()
		{	
			Typeface verdana = Typefaces.Get (Context, Typefaces.MUSICREQUESTS_REGULAR);
			this.Typeface = verdana;

			try {
				// Retrieve the CollapsingTextHelper Field
				Field collapsingTextHelperField = (this).Class.Superclass.GetDeclaredField("mCollapsingTextHelper");
				collapsingTextHelperField.Accessible = true;

				// Retrieve an instance of CollapsingTextHelper and its TextPaint
				Java.Lang.Object collapsingTextHelper = collapsingTextHelperField.Get(this);
				Field tpf = (collapsingTextHelper).Class.GetDeclaredField("mTextPaint");
				tpf.Accessible = true;

				// Apply your Typeface to the CollapsingTextHelper TextPaint
				((TextPaint) tpf.Get(collapsingTextHelper)).SetTypeface(verdana);
			} catch (Exception e) {
				// Nothing to do
				var msg = e.Message;
			}
		}

		protected override void OnDraw (Canvas canvas)
		{
			base.OnDraw (canvas);
			Typeface verdana = Typefaces.Get (Context, Typefaces.MUSICREQUESTS_REGULAR);
			this.EditText.Typeface = verdana;

			Initialize ();
		}
	}
}

