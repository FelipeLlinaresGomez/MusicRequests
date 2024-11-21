using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using Android.Views;
using Android.Runtime;
using Android.Graphics.Drawables;
using Android.Text.Method;

namespace MusicRequests.Droid.CustomViews
{
    public class CustomWhiteEditText : CustomEditText
    {
        private Drawable BorderBottomDashed;

        public CustomWhiteEditText(Context context) : base(context)
        {
            InitializeBorders();
        }

        public CustomWhiteEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            InitializeBorders();
        }

        public CustomWhiteEditText(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            InitializeBorders();
        }

        private void InitializeBorders()
        {
            BorderBottomDashed = Resources.GetDrawable(Resource.Drawable.dashed_line_white);

            //     var oldDrawables = GetCompoundDrawables();
            //    this.SetCompoundDrawablesWithIntrinsicBounds(BorderBottomDashed, BorderBottomDashed, BorderBottomDashed, BorderBottomDashed);
            //   this.TransformationMethod = new SingleLineTransformationMethod();

            Background.SetColorFilter(Color.Red, PorterDuff.Mode.SrcIn);
        }
    }
}

