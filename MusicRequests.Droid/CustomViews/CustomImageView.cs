using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using static Android.Resource;
namespace MusicRequests.Droid
{
    public class CustomImageView : ImageView
    {
        public CustomImageView(Context context) : base(context)
        {
        }

        public CustomImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CustomImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public CustomImageView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected CustomImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #region ImageSource

        string _imageSource;

        public string ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int? idresourcedrawable = Resources.GetIdentifier(value, "drawable", this.Context.PackageName);
                    if (idresourcedrawable.HasValue)
                    {
                        SetImageResource((int)idresourcedrawable);
                        _imageSource = value;
                    }
                }

            }
        }
        #endregion

        #region TintColor

        string _tintColor;

        public string TintColor
        {
            get
            {
                return _tintColor;
            }
            set
            {
                int? idResourceColor = Resources.GetIdentifier(value, "color", this.Context.PackageName);
                if (idResourceColor.HasValue)
                {
                    //SetColorFilter((Android.Graphics.ColorFilter)idResourceColor);
                    //SetColorFilter(ContextCompat.GetColor(this.Context, idResourceColor), Android.Graphics.PorterDuff.Mode.Multiply);
                    SetColorFilter(Context.Resources.GetColor(idResourceColor.Value));

                    _tintColor = value;
                }
            }
        }

        Bitmap _bitmap;

        public Bitmap Bitmap
        {
            get
            {
                return _bitmap;
            }

            set
            {
                _bitmap = value;
            }
        }

        #endregion
    }
}