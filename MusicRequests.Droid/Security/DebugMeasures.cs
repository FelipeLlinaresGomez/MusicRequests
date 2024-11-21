using System;
using Android.Content;
using Android.OS;

namespace MusicRequests.Droid.Security
{
    public class DebugMeasures
    {
        public static bool IsRunningInDebug(Context context)
        {
            return AppIsDebuggable(context) || DebuggerConnected();
        }

        private static bool AppIsDebuggable(Context context)
        {
            return (context.ApplicationInfo.Flags & Android.Content.PM.ApplicationInfoFlags.Debuggable) != 0;
        }

        private static bool DebuggerConnected()
        {
            return Debug.IsDebuggerConnected;
        }

        public static bool IsDeviceAdbEnabled(Context context)
        {
            return Android.Provider.Settings.System.GetInt(context.ContentResolver, "adb_enabled", 0) == 1;
        }

    }
}
