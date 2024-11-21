namespace MusicRequests.Core.Helpers
{
    public static class Constants
    {
        public const string RootFolderForResources = "TextResources/Text";
        public const string GeneralNamespace = "MusicRequests";
        public const string Shared = "Shared";

        public const int CACHE_7_DAYS = 10080; // 7 DAYS
        public const int CACHE_LONGER = 1440; // 24 HOURS
        public const int CACHE_LONG = 720; // 12 HOURS
        public const int CACHE_MEDIUM = 360; // 6 HOURS
        public const int CACHE_SHORT = 60; // 1 HOUR
        public const int CACHE_SHORTER = 15; // 15 MINUTES
        public const int CACHE_TINY = 5; // 5 MINUTES
        public const int CACHE_FOREVER = 0; // 0 MINUTES WILL MEAN FOREVER

        /// <summary>
        /// The timeout session in minutes
        /// </summary>
        public const int TIMEOUT_SESSION = 7;
        public const string DEEPLINKING_CALL_SECRET = "8732CFFF-75BA-4E80-BE3A-84D3EF2E1119";
    }
}