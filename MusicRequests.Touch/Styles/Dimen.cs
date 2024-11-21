namespace MusicRequests.Touch.Styles
{
    public static class Dimen
    {
        /// <summary>
        /// 40
        /// </summary>
        public static nfloat ButtonHeight => 40;

        /// <summary>
        /// 48
        /// </summary>
        public static nfloat BigButtonHeight => 48;

        public static nfloat CheckboxHeight => 18;

        public static nfloat CheckboxSize => 20;

        public static nfloat CheckboxHitAreaInset => (44 - CheckboxHeight) / 4;

        public static nfloat StepperButtonHeight => 38;

        public static nfloat TextInputHeight => 25;

        public static nfloat NewTextInputHeight => 40;

        public static nfloat MuroTextInputHeight => 48;

        public static nfloat NewSwitchWidth => 41;

        public static nfloat NewSwitchHeight => 22;

        /// <summary>
        /// 48
        /// </summary>
        public static nfloat InputHeight => 48;

        public static CGSize AverageFingerTapSize => new CGSize(44, 44);

        public static nfloat AspectRatio => UIScreen.MainScreen.Bounds.Width / UIScreen.MainScreen.Bounds.Height;

        public static nfloat WidthScreen => UIScreen.MainScreen.Bounds.Width;

        public static nfloat HeightScreen => UIScreen.MainScreen.Bounds.Height;

        public static CGRect ScreenBounds => UIScreen.MainScreen.Bounds;

        #region Isobar - Margenes y separadores

        public static nfloat IsoContainerPaddingLeft = 16;
        public static nfloat IsoContainerPaddingTop = 14;
        public static nfloat IsoContainerPaddingBottom = 20;

        public static nfloat IsoContainerMarginLeft = 14;
        public static nfloat IsoContainerMarginLeftMedium = 20;
        public static nfloat IsoContainerMarginTop = 14;
        public static nfloat IsoContainerMarginTopMedium = 20;
        public static nfloat IsoContainerMarginBottomFinal = 20;

        #endregion
    }
}
