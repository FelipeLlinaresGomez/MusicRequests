using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.ViewPager.Widget;

namespace MusicRequests.Droid.Views.Helpers
{
    public class ZoomFadeTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        private float offset = -1;
        private float paddingLeft;
        private float minScale;
        private float minAlpha;

        public ZoomFadeTransformer()
        {
            this.paddingLeft = paddingLeft;
            this.minAlpha = minAlpha;
            this.minScale = minScale;
        }

        public ZoomFadeTransformer(float paddingLeft, float minScale, float minAlpha)
        {
            this.paddingLeft = paddingLeft;
            this.minAlpha = minAlpha;
            this.minScale = minScale;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IntPtr Handle
        {
            get { return ((Java.Lang.Object) this).Handle; }
        }

        public void TransformPage(View page, float position)
        {
            /*
            if (offset == -1)
            {
                offset = paddingLeft / page.MeasuredWidth;
            }
            if (position < -1)
            {
                page.Alpha=0;
            }
            else if (position <= 1)
            {
                float scaleFactor = Math.Max(minScale, 1 - Math.Abs(position - offset));
                page.ScaleX=scaleFactor;
                page.ScaleY=scaleFactor;
                float alphaFactor = Math.Max(minAlpha, 1 - Math.Abs(position - offset));
                page.Alpha=alphaFactor;
            }
            else
            {
                page.Alpha=0;
            }
            */
            if (offset == -1)
            {
                offset = paddingLeft / page.MeasuredWidth;
            }
            if (position < -1)
            {
            }
            else if (position <= 1)
            {
                int translation=10;
                if (minScale < 1 - Math.Abs(position - offset))
                {
                    translation = -translation;
                }
          
            
                float scaleFactor = Math.Max(minScale, 1 - Math.Abs(position - offset));
               // page.ScaleY=scaleFactor;
                page.TranslationY = translation;
            }
            else
            {
            }
        }
    }
}