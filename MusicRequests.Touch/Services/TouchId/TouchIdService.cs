using MusicRequests.Core.Services;
using MusicRequests.Touch.Styles;
using LocalAuthentication;
using Microsoft.Maui.Devices;
using MusicRequests.Touch.Helpers;

namespace MusicRequests.Touch.Services
{
    public class TouchIdService : ITouchIdService
    {
        public bool HasAccessToTouchId()
        {
            bool result = false;
            if (DeviceInfo.Current.Version.Major > 7)
            {
                var context = new LAContext();
                NSError AuthError;
                result = context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError);
            }
            return result;
        }

        public Task<bool> RequestPermissionsTouchId()
        {
            return Task.FromResult(true);
        }

        public bool IsIphoneX()
        {
            return BiometricsHelper.SupportsFaceID();
        }
    }
}

