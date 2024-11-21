using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Java.Lang;

namespace MusicRequests.Droid
{
    //public class NonScrollableListView : ListView
    public class NonScrollableListView : RecyclerView
    {
        public NonScrollableListView(Context context) : base(context)
        {
            
        }

        public NonScrollableListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            
        }

        public NonScrollableListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int heightMeasureSpec_custom = MeasureSpec.MakeMeasureSpec(
            Integer.MaxValue >> 2, MeasureSpecMode.AtMost);
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec_custom);
            this.LayoutParameters.Height = MeasuredHeight;
        }
    }
}
