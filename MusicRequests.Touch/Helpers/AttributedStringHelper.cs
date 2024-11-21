using Foundation;
using UIKit;

namespace MusicRequests.Touch.Helpers
{
    public static class AttributedStringHelper
    {
        public static NSMutableAttributedString SetLinkAttributes(NSAttributedString attributedString, UIStringAttributes attributes)
        {
            var mutable = attributedString.MutableCopy() as NSMutableAttributedString;

            attributedString.EnumerateAttribute((NSString)"NSLink", new NSRange(0, attributedString.Length), NSAttributedStringEnumeration.LongestEffectiveRangeNotRequired,
            (NSObject v, NSRange range, ref bool stop) =>
            {
                if (v != null && v is NSUrl url)
                {
                    mutable.RemoveAttribute("NSLink", range);
                    mutable.AddAttributes(attributes, range);
                }
            });

            return mutable;
        }
    }
}
