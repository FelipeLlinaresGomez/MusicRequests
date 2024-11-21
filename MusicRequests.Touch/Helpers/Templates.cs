using Ibercaja.Lottie.Ios;
using MusicRequests.Touch.Styles;
using MusicRequests.Touch.Views.Controls;

namespace MusicRequests.Touch.Helpers
{
    public static class Templates
    {
        public static (UIScrollView, UIView) Scroll(UIView superview)
        {
            var scrollView = new UIScrollView();
            var contentView = new UIView();


            scrollView.AddSubview(contentView);
            superview.AddSubviews(scrollView);

            scrollView.SetupScroll(contentView);

            return (scrollView, contentView);
        }

        public static UIView VerticalSeparator()
        {
            UIView separator = new UIView
            {
                BackgroundColor = Colors.Gray15
            };
            separator.WidthConstraint(1f);
            return separator;
        }

        public static UIView HorizontalSeparator(nfloat? height = null)
        {
            UIView separator = new UIView
            {
                BackgroundColor = Colors.Gray15
            };
            separator.HeightConstraint(height ?? 1f);
            return separator;
        }


        public static MusicRequestsAmountLabel AmountLabel(bool alignToRight = false)
        {
            return new MusicRequestsAmountLabel
            {
                TextAlignment = alignToRight ? UITextAlignment.Right : UITextAlignment.Left
            };
        }

        public static UILabel Label()
        {
            return new UILabel
            {
                Lines = 1,
                LineBreakMode = UILineBreakMode.TailTruncation
            };
        }

        public static UILabel MultilineLabel()
        {
            return new UILabel
            {
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap
            };
        }

        public static UIImageView Icon(string path = "", bool asTemplate = false)
        {
            var image = UIImage.FromBundle(path) ?? UIImage.FromFile(path);

            if (asTemplate)
                image = image?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

            return new UIImageView
            {
                Image = image
            };
        }

        public static CompatibleAnimationView Lottie(string name, bool loop = false, float speed = 1f)
        {
            var lott = new CompatibleAnimationView(CompatibleAnimation.Named("Lotties/" + name + ".json"));
            lott.LoopAnimationCount = loop ? -1 : 1;
            lott.AnimationSpeed = speed;

            return lott;
        }

        public static ContentSizedTableView ContentSizedTable(nfloat? rowHeight = null)
        {
            if (rowHeight is nfloat height)
            {
                return new ContentSizedTableView
                {
                    RowHeight = height
                };
            }
            else
            {
                return new ContentSizedTableView
                {
                    RowHeight = UITableView.AutomaticDimension,
                    EstimatedRowHeight = UITableView.AutomaticDimension,
                };
            }
        }

        public static FormView StackInsideCard(params UIView[] items)
        {
            var form = new FormView
            {
                TopPadding = Margin.Medium,
                SidePadding = Margin.Medium,
                BottomPadding = Margin.MediumLarge
            };

            if (items.Length >= 1)
            {
                foreach (var item in items)
                {
                    form.AddItem(item);
                }
            }

            return form;
        }

        public static Spacer VSpacer()
        {
            return new Spacer { Axis = UILayoutConstraintAxis.Vertical };
        }

        public static Spacer HSpacer()
        {
            return new Spacer { Axis = UILayoutConstraintAxis.Horizontal };
        }


        public static ContentSizedTableView ContentSizedGroupedTable()
        {
            return new ContentSizedTableView(UITableViewStyle.Grouped)
            {
                RowHeight = UITableView.AutomaticDimension,
                EstimatedRowHeight = UITableView.AutomaticDimension,
            };
        }

        public static MusicRequestsSwitch Switch()
        {
            var control = new MusicRequestsSwitch();

            control.HeightConstraint(22);
            control.WidthConstraint(41);

            return control;
        }
    }
}