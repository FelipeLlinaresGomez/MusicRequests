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
using Android.Util;

namespace MusicRequests.Droid
{
    class NonScrollableExpandableListView : ExpandableListView
    {
        bool expanded = true;

        public NonScrollableExpandableListView(Context context) :base(context)
        {
        }

        public NonScrollableExpandableListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public NonScrollableExpandableListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        public bool IsExpanded()
        {
            return expanded;
        }

        override protected void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if (IsExpanded())
            {
                // Calculate entire height by providing a very large height hint.
                // View.MEASURED_SIZE_MASK represents the largest height possible.
                int expandSpec = MeasureSpec.MakeMeasureSpec(View.MeasuredSizeMask, MeasureSpecMode.AtMost);
                base.OnMeasure(widthMeasureSpec, expandSpec);

                ViewGroup.LayoutParams prms = LayoutParameters;
                prms.Height = MeasuredHeight;
            }
            else
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
        }
    }
}