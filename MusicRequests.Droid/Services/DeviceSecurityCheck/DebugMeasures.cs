using Android.Content;
using Android.OS;

namespace MusicRequests.Droid.Services.DeviceSecurityCheck
{
	public class DebugMeasures
	{
        public static bool IsRunningInDebug(Context context, IList<string> info)
        {
            bool debug = false;

            if (AppIsDebuggable(context))
            {
                info.Add(SecurityCheckMessages.AppIsDebuggable);
                debug = true;
            }

            if (DebuggerConnected())
            {
                info.Add(SecurityCheckMessages.DebuggerConnected);
                debug = true;
            }

            return debug;
        }

        private static bool AppIsDebuggable(Context context)
        {
            var appdebuggable = (context.ApplicationInfo.Flags & Android.Content.PM.ApplicationInfoFlags.Debuggable) != 0;

            return appdebuggable;
        }

        private static bool DebuggerConnected()
        {
            var debuggerConnected = Debug.IsDebuggerConnected;

            return debuggerConnected;
        }
    }
}

