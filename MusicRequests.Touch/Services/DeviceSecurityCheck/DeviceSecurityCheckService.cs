using MusicRequests.Core.Services;

namespace MusicRequests.Touch
{
    public class DeviceSecurityCheckService : IDeviceSecurityCheckService
    {
        private const string JailbreakFilesDetectedErrorMessage = "Detectados ficheros relacionados con dispositivos con software modificado";
        private const string JailbreakSandboxViolationErrorMessage = "Detectado acceso de escritura en directorios del sistema";

        public bool PassSecurityChecks(IList<string> failMessage)
        {
            bool isSecuredDevice = true;
#if !DEBUG

            // Check 1 : existence of files that are common for jailbroken devices
            if (NSFileManager.DefaultManager.FileExists("/Applications/Cydia.app")
            || IsRunningOnMacOS()
            || IsRunningOnSimulator()
            || NSFileManager.DefaultManager.FileExists("/Library/MobileSubstrate/MobileSubstrate.dylib")
            || NSFileManager.DefaultManager.FileExists("/bin/bash")
            || NSFileManager.DefaultManager.FileExists("/usr/sbin/sshd")
            || NSFileManager.DefaultManager.FileExists("/etc/apt")
            || NSFileManager.DefaultManager.FileExists("/private/var/lib/cydia")
            || NSFileManager.DefaultManager.FileExists("/private/var/stash")
            || NSFileManager.DefaultManager.FileExists("/private/var/tmp/cydia.log")
            || NSFileManager.DefaultManager.FileExists("/usr/bin/sshd")
            || NSFileManager.DefaultManager.FileExists("/usr/libexec/sftp-server")//
            || NSFileManager.DefaultManager.FileExists("/Library/MobileSubstrate/DynamicLibraries/LiveClock.plist")
            || NSFileManager.DefaultManager.FileExists("/Library/MobileSubstrate/DynamicLibraries/Veency.plist")
            || NSFileManager.DefaultManager.FileExists("/System/Library/LaunchDaemons/com.ikey.bbot.plist")
            || NSFileManager.DefaultManager.FileExists("/System/Library/LaunchDaemons/com.saurik.Cydia.Startup.plist")
            //|| NSFileManager.DefaultManager.FileExists("/private/var/lib/apt")
            //|| NSFileManager.DefaultManager.FileExists("/private/var/lib/apt/") //this one persists across restores if the device is upgraded rather than erased, so you might wanna omit that.
            || UIApplication.SharedApplication.CanOpenUrl(new NSUrl("cydia://package/com.example.package")))
            {
                isSecuredDevice = false;

                failMessage.Add(JailbreakFilesDetectedErrorMessage);
            }

            // Check 2 : Reading and writing in system directories (sandbox violation)
            try
            {
                var sandboxViolation = NSFileManager.DefaultManager.CreateFile("/private/JailbreakTest.txt", new NSData(), attributes: null);

                if (sandboxViolation)
                {
                    isSecuredDevice = false;

                    failMessage.Add(JailbreakSandboxViolationErrorMessage);

                    NSFileManager.DefaultManager.Remove("/private/JailbreakTest.txt", out NSError nSError);
                }
            }
            catch { }
#endif
            return isSecuredDevice;
        }

        private bool IsRunningOnSimulator()
        {
#if TARGET_IPHONE_SIMULATOR
            return true;
#else
            return false;
#endif
        }

        private bool IsRunningOnMacOS()
        {
            return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Mac;
        }
    }
}
