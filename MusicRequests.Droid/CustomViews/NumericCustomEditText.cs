using System;
using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Java.Lang;

namespace MusicRequests.Droid.CustomViews
{
    public class NumericCustomEditText : CustomEditText, ITextWatcher
    {
		public NumericCustomEditText(Context context):base(context) {
            this.AddTextChangedListener(this);
		}

		public NumericCustomEditText(Context context, IAttributeSet attrs):base(context, attrs) {
            this.AddTextChangedListener(this);
		}

		public NumericCustomEditText(Context context, IAttributeSet attrs, int defStyleAttr):base(context, attrs, defStyleAttr) {
            this.AddTextChangedListener(this);
		}

        void ITextWatcher.AfterTextChanged(IEditable s)
        {
			var texto = s.ToString();
			if (texto.IndexOf('.') != -1)
			{
                this.Text = texto.Replace(".", ",");
                this.SetSelection(texto.Length);
			}
        }

        void ITextWatcher.BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
            
        }

        void ITextWatcher.OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            
        }
    }
}
