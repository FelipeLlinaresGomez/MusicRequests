using MusicRequests.Touch.Helpers;
using UIKit;

namespace MusicRequests.Touch.Views.Controls
{
    public class Spacer : UIView
    {
        UILayoutConstraintAxis _axis;
        public UILayoutConstraintAxis Axis
        {
            get => _axis;
            set
            {
                _axis = value;

                if (value == UILayoutConstraintAxis.Horizontal)
                {
                    SetContentHuggingPriority((float)UILayoutPriority.Required, UILayoutConstraintAxis.Horizontal);
                    SetContentHuggingPriority((float)UILayoutPriority.FittingSizeLevel, UILayoutConstraintAxis.Vertical);
                }
                else
                {
                    SetContentHuggingPriority((float)UILayoutPriority.FittingSizeLevel, UILayoutConstraintAxis.Horizontal);
                    SetContentHuggingPriority((float)UILayoutPriority.Required, UILayoutConstraintAxis.Vertical);
                }
            }
        }

        public Spacer() : base()
        {
            this.ToAutoLayout();
        }
    }
}
