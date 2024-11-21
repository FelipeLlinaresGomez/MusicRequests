using Android.Content;
using Android.Graphics;
using Android.Text.Method;
using Android.Widget;

namespace MusicRequests.Droid.Helpers
{
    public class InputTextHelper
    {
        /// <summary>
        /// Needed for password inputs, otherwise, it wont get the custom font
        /// </summary>
        /// <param name="context">Current Context</param>
        /// <param name="etPassword">The input field that will be customized</param>
        public static void SetPasswordStyle(Context context, EditText etPassword)
        {
			SetPasswordStyle (context, etPassword, Typefaces.MUSICREQUESTS_REGULAR);
        }

		public static void SetPasswordStyle (Context context, EditText etPassword, string font)
		{
			Typeface verdana = Typefaces.Get (context, font);
			if (etPassword != null) {
				etPassword.TransformationMethod = new PasswordTransformationMethod ();
				etPassword.Typeface = verdana;
				etPassword.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.TextFlagNoSuggestions;
			}
		}
    }
}