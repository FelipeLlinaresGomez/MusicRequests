using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MusicRequests.Touch.Views.Controls
{
    [Register(nameof(ContentSizedTableView))]
    public class ContentSizedTableView : UITableView
    {
        public ContentSizedTableView(IntPtr handle) : base(handle)
        {

        }

        public ContentSizedTableView(CGRect frame) : base(frame)
        {

        }

        public ContentSizedTableView() : base()
        {

        }

        public ContentSizedTableView(UITableViewStyle style) : base(CGRect.Empty, style)
        {

        }

        public override CGSize ContentSize
        {
            get => base.ContentSize;
            set
            {
                base.ContentSize = value;
                InvalidateIntrinsicContentSize();
            }
        }

        public override CGSize IntrinsicContentSize
        {
            get
            {
                LayoutIfNeeded();
                return new CGSize(NoIntrinsicMetric, ContentSize.Height);
            }
        }
    }
}
