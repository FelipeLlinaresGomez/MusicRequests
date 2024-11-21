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
    public class RiseTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        private int risedp;
        private float offset = -1;
        private float paddingLeft;

        public RiseTransformer(float paddingLeft, int risedp)
        {
            this.paddingLeft = paddingLeft;
            this.risedp = risedp;
        }

        public RiseTransformer()
        {
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
            bool up = false;
            if (offset == -1)
            {
                offset = paddingLeft/page.MeasuredWidth;
            }

            if (position < -1)
            {
            }
            else if (position <= 1)
            {
                if (0.95f < 1 - Math.Abs(position - offset))
                {
                    up = true;
                }

                if (up)
                {
                    risePage(page);
                }
                else
                {
                    downPage(page);
                }
            }
            else
            {
            }


        
        }

        private void risePage(View page)
        {
            if (page.TranslationY > -risedp)
            {
                page.TranslationY -= 15;
            }
        }

        private void downPage(View page)
        {
            if (page.TranslationY < risedp)
            {
                page.TranslationY += 15;
            }
           
        }

    }
}