using CoreAnimation;
using MusicRequests.Touch.Views.Controls;

namespace MusicRequests.Touch.Styles
{
    public class Theme
    {
        public static UIColor BackgroundColor => UIColor.FromRGB(244, 247, 253);

        public static void Background(UIView view) => view.BackgroundColor = BackgroundColor;

        public static void WhiteBackground(UIView view) => view.BackgroundColor = Colors.White;

        public static void Card(UIView view, CACornerMask? mask = null)
        {
            BaseStyles.Card(view, mask);
        }

        public static void PlainCard(UIView view, CACornerMask? mask = null)
        {
            view.Layer.BorderWidth = 1;
            view.Layer.BorderColor = UIColor.FromRGB(218, 226, 233).CGColor;

            view.BackgroundColor = UIColor.White;

            view.Layer.CornerRadius = 2;
            view.Layer.MaskedCorners = mask ?? BaseStyles.AllCorners;
        }

        public static void TextButton(MusicRequestsButton button, string icon = null, ControlColors imageColors = null) => TextButtonStyle.Apply(button, icon, imageColors);

        public static void TextButtonPrimary(MusicRequestsButton button, string icon = null, ControlColors imageColors = null) => TextButtonPrimaryStyle.Apply(button, icon, imageColors);

        #region Text inputs

        protected static TextInputStyle TextInputStyle => new TextInputStyle
        {
            HorizontalPadding = Margin.Medium,
            BorderWidth = 1,
            BorderColor = Colors.Gray50,
            Font = Fonts.MusicRequestsFont.OfSize(13),
            TextColor = Colors.Black,
            PlaceholderColor = Colors.Gray50,
            FloatingPlaceholder = false,
            FloatingPlaceholderFontSize = 13,
            BackgroundColor = Colors.White,
            UnderlineColor = UIColor.Clear,
            ImageColor = Colors.Black
        };

        protected static TextInputStyle WhiteTextInputStyle => new TextInputStyle
        {
            HorizontalPadding = Margin.Medium,
            BorderWidth = 1,
            BorderColor = Colors.Gray50,
            Font = Fonts.MusicRequestsFont.OfSize(13),
            TextColor = Colors.Black,
            PlaceholderColor = Colors.Gray50,
            FloatingPlaceholder = false,
            FloatingPlaceholderFontSize = 13,
            BackgroundColor = Colors.White,
            UnderlineColor = UIColor.Clear,
            ImageColor = Colors.Black
        };

        protected static TextInputStyle ErrorTextInputStyle => new TextInputStyle
        {
            HorizontalPadding = Margin.Medium,
            BorderWidth = 1,
            BorderColor = Colors.Gray50,
            Font = Fonts.MusicRequestsFont.OfSize(13),
            TextColor = UIColor.FromRGB(221, 76, 64),
            PlaceholderColor = Colors.Gray50,
            FloatingPlaceholder = false,
            FloatingPlaceholderFontSize = 13,
            UnderlineColor = UIColor.Clear,
            ImageColor = UIColor.FromRGB(221, 76, 64)
        };

        protected static TextInputStyle DisabledTextInputStyle => new TextInputStyle
        {
            BackgroundColor = UIColor.FromRGB(227, 230, 236),
            HorizontalPadding = Margin.Medium,
            BorderWidth = 0,
            Font = Fonts.MusicRequestsFont.OfSize(13),
            TextColor = Colors.Gray54,
            PlaceholderColor = Colors.Gray50,
            FloatingPlaceholder = false,
            FloatingPlaceholderFontSize = 13,
            UnderlineColor = UIColor.Clear,
            ImageColor = Colors.Gray16
        };
        
        public static void  FormInput(TextInputView input, FormInputType inputType = FormInputType.PLAIN, bool OjosBlancos = false)
        {
            if (input is TextInputPicker picker)
            {
                TextInputStyle.Apply(picker);
            }
            else
            {
                if (OjosBlancos)
                    WhiteTextInputStyle.Apply(input);
                else
                    TextInputStyle.Apply(input);
            }

            switch (inputType)
            {
                case FormInputType.DATE_PICKER:

                    var searchIcon = new UIImageView(new CGRect(Margin.Medium, 0, 24, 24))
                    {
                        Image = UIImage.FromBundle(Helpers.Icons.Common.CALENDARIO),
                        ContentMode = UIViewContentMode.ScaleAspectFit
                    };

                    var iconView = new UIView(new CGRect(0, 0, searchIcon.Bounds.Width + Margin.Large, searchIcon.Bounds.Height));
                    iconView.AddSubview(searchIcon);

                    input.RightView = iconView;
                    input.RightViewMode = UITextFieldViewMode.Always;

                    break;
                default:
                    break;
            }
        }

        public static void ErrorFormInput(TextInputView input)
        {
            if (input is TextInputPicker picker)
            {
                ErrorTextInputStyle.Apply(picker);
            }
            else
            {
                ErrorTextInputStyle.Apply(input);
            }
        }

        public static void DisabledFormInput(TextInputView input)
        {
            if (input is TextInputPicker picker)
            {
                DisabledTextInputStyle.Apply(picker);
            }
            else
            {
                DisabledTextInputStyle.Apply(input);
            }
        }

        #endregion

        #region Buttons

        public static ButtonStyle TextButtonStyle => new ButtonStyle
        {
            Font = Fonts.MusicRequestsFont.MediumOfSize(12),
            BackgroundColor = UIColor.Clear,
            DisabledBackgroundColor = UIColor.Clear,
            TitleColor = Colors.Black,
            HighlightedTitleColor = Colors.Primary,
            DisabledTitleColor = Colors.Gray32,
            CornerRadius = 0,
            BorderWidth = 0,
            IconPadding = Margin.Medium
        };

        public static ButtonStyle TextButtonPrimaryStyle => new ButtonStyle
        {
            Font = Fonts.MusicRequestsFont.MediumOfSize(12),
            BackgroundColor = UIColor.Clear,
            DisabledBackgroundColor = UIColor.Clear,
            TitleColor = Colors.Primary,
            HighlightedTitleColor = Colors.PrimaryDarker,
            DisabledTitleColor = Colors.Gray32,
            CornerRadius = 0,
            BorderWidth = 0,
            IconPadding = Margin.Medium
        };

        public static ButtonStyle PrimaryRoundedButtonStyle => new ButtonStyle
        {
            Font = Fonts.MusicRequestsFont.MediumOfSize(18),
            BackgroundColor = Colors.Primary,
            HighlightedBackgroundColor = UIColor.FromRGB(133, 188, 231),
            DisabledBackgroundColor = Colors.Gray15, 
            TitleColor = Colors.White,
            HighlightedTitleColor = Colors.Gray3,
            DisabledTitleColor = Colors.Gray32,
            CornerRadius = 6,
            BorderWidth = 0,
            IconPadding = Margin.Medium
        };

        public static ButtonStyle SecondaryRoundedButtonStyle => new ButtonStyle
        {
            Font = Fonts.MusicRequestsFont.MediumOfSize(12),
            BackgroundColor = Colors.White,
            HighlightedBackgroundColor = UIColor.FromRGB(133, 188, 231),
            DisabledBackgroundColor = Colors.Gray15,
            TitleColor = Colors.Black,
            HighlightedTitleColor = Colors.Gray3,
            DisabledTitleColor = Colors.Gray32,
            CornerRadius = 6,
            BorderWidth = 0,
            IconPadding = Margin.Medium
        };

        public static ButtonStyle PrimaryGhostButtonStyle => new ButtonStyle
        {
            Font = Fonts.MusicRequestsFont.MediumOfSize(18),
            BackgroundColor = UIColor.Clear,
            HighlightedBackgroundColor = UIColor.FromRGB(133, 188, 231),
            DisabledBackgroundColor = Colors.Gray15,
            TitleColor = Colors.Black,
            HighlightedTitleColor = Colors.Black,
            DisabledTitleColor = Colors.Gray32,
            CornerRadius = 6,
            BorderWidth = 1,
            IconPadding = Margin.Medium
        };

        public static ButtonStyle SecondaryGhostButtonStyle => new ButtonStyle
        {
            Font = Fonts.MusicRequestsFont.MediumOfSize(12),
            BackgroundColor = UIColor.Clear,
            HighlightedBackgroundColor = UIColor.FromRGB(133, 188, 231),
            DisabledBackgroundColor = Colors.Gray15,
            TitleColor = Colors.White,
            HighlightedTitleColor = Colors.Gray3,
            DisabledTitleColor = Colors.Gray32,
            CornerRadius = 6,
            BorderWidth = 1,
            IconPadding = Margin.Medium
        };

        public static void PrimaryRoundedButton(MusicRequestsButton button, string icon = null) => PrimaryRoundedButtonStyle.Apply(button, icon);

        public static void SecondaryRoundedButton(MusicRequestsButton button, string icon = null) => SecondaryRoundedButtonStyle.Apply(button, icon);

        public static void PrimaryGhostButton(MusicRequestsButton button, string icon = null) => PrimaryGhostButtonStyle.Apply(button, icon);

        public static void SecondaryGhostButton(MusicRequestsButton button, string icon = null) => SecondaryGhostButtonStyle.Apply(button, icon);

        #endregion

        public static void Switch(MusicRequestsSwitch control)
        {
            if (control.Enabled)
            {
                control.OffTintColor = UIColor.FromRGB(192, 206, 217);
                control.OnTintColor = Colors.Primary;
            }
            else
            {
                control.OffTintColor = Colors.Gray5;
                control.OnTintColor = Colors.Gray5;
            }

            control.ThumbOffTintColor = control.ThumbOnTintColor = Colors.White;
            control.ThumbShadowOpacity = 0;

            control.ThumbSize = new CGSize(18, 18);
            control.Padding = 2;
        }
    }
}

