using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using AndroidX.Core.Content;
using AndroidX.Core.Hardware.Fingerprint;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MusicRequests.Core.Services
{
    public class TouchIdService : ITouchIdService
    {
        #region ITouchIdService implementation

        public bool HasAccessToTouchId()
        {
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            if (activity != null)
            {
                return DeviceIsFingerPrintAuthCapable(activity)
                    && SecuritySettingsAreEnabled(activity)
                    && FingerPrintIsRegistered(activity);
            }
            return false;
        }

        public bool HasTouchIdPermissions()
        {
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            return activity != null &&
                SecuritySettingsAreEnabled(activity) &&
                FingerPrintIsRegistered(activity);
        }

        public async Task<bool> RequestPermissionsTouchId()
        {
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            return await CheckExplicitPermission(activity);
        }

        #endregion

        public bool DeviceIsFingerPrintAuthCapable(Context context)
        {
            FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(context);
            return fingerprintManager != null && fingerprintManager.IsHardwareDetected;
        }

        public bool SecuritySettingsAreEnabled(Context context)
        {
            KeyguardManager keyguardManager = (KeyguardManager)context.GetSystemService(Context.KeyguardService);
            return keyguardManager != null && !keyguardManager.IsKeyguardLocked;
        }

        public bool FingerPrintIsRegistered(Context context)
        {
            FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(context);
            if (!fingerprintManager.HasEnrolledFingerprints)
            {
                // Can't use fingerprint authentication - notify the user that they need to 
                // enroll at least one fingerprint with the device.
                return false;
            }
            return true;
        }

        public Task<bool> CheckExplicitPermission(Context context)
        {
            Android.Content.PM.Permission permissionResult = ContextCompat.CheckSelfPermission(context, Manifest.Permission.UseFingerprint);
            if (permissionResult == Android.Content.PM.Permission.Granted)
            {
                // Permission granted - go ahead and start the fingerprint scanner.
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
                // No permission. Go and ask for permissions and don't start the scanner. See
                // http://developer.android.com/training/permissions/requesting.html
            }
        }

        public bool IsIphoneX()
        {
            return false;
        }
    }
}