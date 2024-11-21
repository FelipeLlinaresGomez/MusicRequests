using System;
using CoreGraphics;
using MusicRequests.Touch.Views.Controls;
using UIKit;

namespace MusicRequests.Touch.Styles
{
    public class TextInputStyle
    {
        public UIColor BackgroundColor { get; set; } = UIColor.Clear;

        public nfloat HorizontalPadding { get; set; }

        public nfloat CornerRadius { get; set; }

        public nfloat BorderWidth { get; set; }

        public UIColor BorderColor { get; set; } = UIColor.Clear;

        public UIFont Font { get; set; }

        public UIColor TextColor { get; set; }

        public UIColor PlaceholderColor { get; set; }

        public bool FloatingPlaceholder { get; set; }

        public nfloat FloatingPlaceholderFontSize { get; set; }

        public UIColor UnderlineColor { get; set; }

        public UIColor ImageColor { get; set; }

        public void Apply(TextInputView input)
        {
            input.BackgroundColor = BackgroundColor;

            input.LeftView = new UIView(new CGRect(0, 0, HorizontalPadding, 1));
            input.LeftViewMode = UITextFieldViewMode.Always;

            if (input.RightView is null)
            {
                input.RightView = new UIView(new CGRect(0, 0, HorizontalPadding, 1));
                input.RightViewMode = UITextFieldViewMode.Always;
            }

            input.Layer.BorderWidth = BorderWidth;
            input.Layer.BorderColor = BorderColor.CGColor;
            input.Layer.CornerRadius = CornerRadius;

            input.Font = Font;
            input.TextColor = TextColor;
            input.PlaceHolderColor = PlaceholderColor;

            input.FloatingPlaceHolder = FloatingPlaceholder;
            input.FloatingPlaceHolderFontSize = FloatingPlaceholderFontSize;
            input.UnderLineColor = UnderlineColor;
        }

        public void Apply(TextInputPicker input)
        {
            Apply(input as TextInputView);

            var image = UIImage.FromBundle(Helpers.Icons.Common.ARROW_DOWN);
            input.ArrowImageView.Image = image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

            input.ArrowImageView.TintColor = ImageColor;
        }
    }
}

