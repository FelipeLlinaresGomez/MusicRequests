using System;
using MusicRequests.Touch.Views.Controls;
using UIKit;

namespace MusicRequests.Touch.Styles
{
    public class ButtonStyle
    {
        public UIFont Font { get; set; }

        public UIColor BackgroundColor { get; set; }
        public UIColor HighlightedBackgroundColor { get; set; }
        public UIColor DisabledBackgroundColor { get; set; }

        public UIColor TitleColor { get; set; }
        public UIColor HighlightedTitleColor { get; set; }
        public UIColor DisabledTitleColor { get; set; }

        public nfloat CornerRadius { get; set; }

        public bool IsRounded { get; set; }

        public nfloat BorderWidth { get; set; }

        public nfloat IconPadding { get; set; }

        public void Apply(MusicRequestsButton button, string icon = null, ControlColors imageColors = null)
        {
            if (Font is { })
                button.TitleLabel.Font = Font;

            button.SetBackgroundColor(BackgroundColor, UIControlState.Normal);

            button.SetBackgroundColor(HighlightedBackgroundColor, UIControlState.Highlighted);
            button.SetBackgroundColor(DisabledBackgroundColor, UIControlState.Disabled);

            button.SetTitleColor(TitleColor, UIControlState.Normal);
            button.SetTitleColor(HighlightedTitleColor, UIControlState.Highlighted);
            button.SetTitleColor(DisabledTitleColor, UIControlState.Disabled);

            button.SetBorderColor(TitleColor, UIControlState.Normal);
            button.SetBorderColor(HighlightedTitleColor, UIControlState.Highlighted);
            button.SetBorderColor(UIColor.Clear, UIControlState.Disabled);

            button.SetBorderWidth(BorderWidth, UIControlState.Normal);
            button.SetBorderWidth(BorderWidth, UIControlState.Highlighted);
            button.SetBorderWidth(0, UIControlState.Disabled);

            button.SetImageTintColor(imageColors?.DefaultColor ?? TitleColor, UIControlState.Normal);
            button.SetImageTintColor(imageColors?.HighlightedColor ?? HighlightedTitleColor, UIControlState.Highlighted);
            button.SetImageTintColor(imageColors?.DisabledColor ?? DisabledTitleColor, UIControlState.Disabled);

            if (!string.IsNullOrEmpty(icon))
            {
                button.SetImage(UIImage.FromBundle(icon)?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate), UIControlState.Normal);
                button.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            }

            button.Layer.CornerRadius = CornerRadius;
            button.ClipsToBounds = true;

            if (button.IsRounded)
                button.IsRounded = true;
        }

        public enum ImageViewPosition
        {
            TOP,
            BOTTOM,
            LEFT,
            RIGHT
        }

        public static void SetImagePosition(MusicRequestsButton button, ImageViewPosition position, nfloat spacing)
        {
            var imageViewSize = button.ImageView.Bounds.Size;
            var titleLabelSize = button.TitleLabel.Bounds.Size;

            nfloat totalHeight = 0f;

            switch (position)
            {
                case ImageViewPosition.TOP:
                    totalHeight = imageViewSize.Height + titleLabelSize.Height + 2 * spacing;

                    button.ImageEdgeInsets = new UIEdgeInsets(
                        top: (nfloat)Math.Max(0, -(totalHeight - imageViewSize.Height)),
                        left: 0,
                        bottom: 0,
                        right: -titleLabelSize.Width
                    );

                    button.TitleEdgeInsets = new UIEdgeInsets(
                        top: 0,
                        left: -imageViewSize.Width,
                        bottom: -(totalHeight - titleLabelSize.Height),
                        right: 0
                    );

                    button.ContentEdgeInsets = new UIEdgeInsets(
                        top: 0,
                        left: 0,
                        bottom: titleLabelSize.Height,
                        right: 0
                    );
                    break;

                case ImageViewPosition.LEFT:
                    button.ImageEdgeInsets = new UIEdgeInsets(
                        top: 0,
                        left: 0,
                        bottom: 0,
                        right: spacing
                    );

                    button.TitleEdgeInsets = new UIEdgeInsets(
                        top: 0,
                        left: spacing,
                        bottom: 0,
                        right: 0
                    );

                    button.ContentEdgeInsets = UIEdgeInsets.Zero;

                    break;

                case ImageViewPosition.RIGHT:
                    var dx = imageViewSize.Width + spacing + titleLabelSize.Width;
                    var insetAmount = dx / 2f;

                    button.TitleEdgeInsets = new UIEdgeInsets(
                        top: 0,
                        left: 0,
                        bottom: 0,
                        right: 2 * insetAmount
                    );

                    button.ImageEdgeInsets = new UIEdgeInsets(
                        top: 0,
                        left: 2 * insetAmount,
                        bottom: 0,
                        right: 0
                    );

                    button.ContentEdgeInsets = new UIEdgeInsets(
                        0,
                        insetAmount + 100,
                        0,
                        -insetAmount
                    );

                    break;
            }
        }
    }
}

