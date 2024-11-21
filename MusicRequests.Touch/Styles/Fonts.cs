namespace MusicRequests.Touch.Styles
{
    public static class Fonts
    {
        public static class MusicRequestsFont
        {
            public const string REGULAR = "TT Neoris Trial DemiBold";
            public const string MEDIUM = "TT Neoris Trial Medium";
            public const string ITALIC = "TT Neoris Trial DemiBold";
            public const string EXTRA_BOLD = "TT Neoris Trial Black";
            public const string BOLD = "TT Neoris Trial Bold";

            public static UIFont OfSize(float size) => UIFont.FromName(REGULAR, size);

            public static UIFont ItalicOfSize(float size) => UIFont.FromName(ITALIC, size);

            public static UIFont MediumOfSize(float size) => UIFont.FromName(MEDIUM, size);

            public static UIFont BoldOfSize(float size) => UIFont.FromName(BOLD, size);

            public static UIFont ExtraBoldOfSize(float size) => UIFont.FromName(EXTRA_BOLD, size);
        }
    }
}