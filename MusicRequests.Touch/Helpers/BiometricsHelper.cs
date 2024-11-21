using LocalAuthentication;

namespace MusicRequests.Touch.Helpers
{
    public static class BiometricsHelper
    {
        public static bool SupportsFaceID()
        {
            LAContext context = new LAContext();

            context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out NSError _);

            return context.BiometryType == LABiometryType.FaceId;
        }

        public static bool SupportsTouchID()
        {
            LAContext context = new LAContext();

            context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out NSError _);

            return context.BiometryType == LABiometryType.TouchId;
        }
    }
}
