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
using Android.Text;

namespace MusicRequests.Droid.CustomViews
{
    public class CustomEditText : EditText
    {


        public CustomEditText(Context context) : base(context)
        {
            this.SetCustomTextFont(context);
        }

        public CustomEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.SetCustomTextFont(context, attrs);
            InitCustomEditText(context, attrs);
        }

        public CustomEditText(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            this.SetCustomTextFont(context, attrs);
            InitCustomEditText(context, attrs);
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Enter)
            {
                ClearFocus();
            }

            if (this.InputType == Android.Text.InputTypes.NumberFlagDecimal)
            {
                if (keyCode == Keycode.NumpadDot)
                {
                    return base.OnKeyDown(Keycode.NumpadComma, e);
                }
            }

            return base.OnKeyDown(keyCode, e);
        }

        private void InitCustomEditText(Context context, IAttributeSet attrs)
        {

            var styleAttrs = context.ObtainStyledAttributes(attrs, Resource.Styleable.CustomEditText);
            bool showPasswordVisible = styleAttrs.GetBoolean(Resource.Styleable.CustomEditText_showPasswordVisible, false);
            if (showPasswordVisible && TransformationMethod.GetType() == typeof(PasswordTransformationMethod))
            {
                InitializePasswordDrawable();
                InitializePasswordClickableArea();
                ForcePasswordVisible = false;
            }
        }

        #region Password visible initialization

        private void InitializePasswordDrawable()
        {
            PasswordShowDrawable = Resources.GetDrawable(Resource.Drawable.icon_eye_show);
            PasswordHideDrawable = Resources.GetDrawable(Resource.Drawable.icon_eye_hide);
        }

        private void InitializePasswordClickableArea()
        {
            this.SetOnTouchListener(new DrawableRightRegionClickListener());
        }

        #endregion

        #region Properties

        private Drawable PasswordShowDrawable;
        private Drawable PasswordHideDrawable;

        private bool _forcePasswordVisible;
        public bool ForcePasswordVisible
        {
            get
            {
                return _forcePasswordVisible;
            }
            set
            {
                var oldDrawables = GetCompoundDrawables();
                if (value)
                {
                    this.SetCompoundDrawablesWithIntrinsicBounds(oldDrawables[0], oldDrawables[1], PasswordHideDrawable, oldDrawables[3]);
                    this.TransformationMethod = new SingleLineTransformationMethod();
                }
                else
                {
                    this.SetCompoundDrawablesWithIntrinsicBounds(oldDrawables[0], oldDrawables[1], PasswordShowDrawable, oldDrawables[3]);
                    this.TransformationMethod = new PasswordTransformationMethod();
                }
                _forcePasswordVisible = value;
            }
        }

        public int MaxLength
        {
            get
            {
                foreach (IInputFilter filter in this.GetFilters())
                {
                    if (filter is InputFilterLengthFilter)
                    {
                        return ((InputFilterLengthFilter)filter).Max;
                    }
                }
                return -1;
            }
            set
            {
                IInputFilter[] FilterArray = new IInputFilter[1];
                FilterArray[0] = new InputFilterLengthFilter(value);
                this.SetFilters(FilterArray);
            }
        }

        #endregion


        class DrawableRightRegionClickListener : Java.Lang.Object, View.IOnTouchListener
        {
            private const int CLICK_SAFE_ZONE = 20;
            public bool OnTouch(View v, MotionEvent e)
            {
                CustomEditText parentView = (CustomEditText)v;
                if (e.Action == MotionEventActions.Up)
                {
                    if (e.RawX >= parentView.Right - parentView.GetCompoundDrawables()[2].Bounds.Width() - DimensionHelper.DipToPx(parentView.Context, 18) - CLICK_SAFE_ZONE)
                    {
                        parentView.ForcePasswordVisible = !parentView.ForcePasswordVisible;
                        return true;
                    }
                }
                return false;
            }
        }

    }
}

