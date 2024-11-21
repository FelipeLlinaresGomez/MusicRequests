using Android.Views;
using AndroidX.RecyclerView.Widget;
using Android.Graphics;

namespace MusicRequests.Droid.Views
{
    public class HorizontalRecyclerSpacingItemDecoration : RecyclerView.ItemDecoration
    {
        readonly int _spacing;
        public HorizontalRecyclerSpacingItemDecoration(int spacing)
        {
            this._spacing = spacing;
        }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            var position = parent.GetChildAdapterPosition(view);
            var size = parent.Width;

            if (position > 0)
            {
                outRect.Left = _spacing;
            }
        }
    }
}