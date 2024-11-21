using Android.Content;
using MusicRequests.Core.Services;

namespace MusicRequests.Droid.Services.DeviceSecurityCheck
{
    public class DeviceSecurityCheckService : IDeviceSecurityCheckService
    {
        public bool PassSecurityChecks(IList<string> failMessages)
        {
#if DEBUG
            return true;
#else
			Context context = MainApplication.Context;

            bool isRunningInDebug = DebugMeasures.IsRunningInDebug(context, failMessages);
            bool isAppInEmulation = EmulatorMeasures.IsAppInEmulation(context, failMessages);
            bool isPhoneRooted = RootMeasures.IsPhoneRooted(context, failMessages);
            bool isValidAppSignature = TamperMeasures.CheckValidAppSignature(context, failMessages);

            return !isRunningInDebug
                && !isAppInEmulation
                && !isPhoneRooted
                && isValidAppSignature;
#endif
        }
    }
}