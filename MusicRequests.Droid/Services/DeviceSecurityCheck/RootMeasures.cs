using Android.Content;
using Android.Content.PM;
using Android.OS;
using Java.IO;
using Java.Lang;
using Java.Util;
using MvvmCross;

namespace MusicRequests.Droid.Services.DeviceSecurityCheck
{
	public class RootMeasures
	{
        private static readonly string[] knownRootAppsPackages = { "com.noshufou.android.su", "com.noshufou.android.su.elite", "eu.chainfire.supersu", "com.koushikdutta.superuser", "com.thirdparty.superuser", "com.yellowes.su",
                                                                   "com.topjohnwu.magisk","com.kingroot.kinguser","com.kingo.root","com.smedialink.oneclickroot","com.zhiqupk.root.global","com.alephzain.framaroot","com.devadvance.rootcloak","com.devadvance.rootcloakplus","de.robv.android.xposed.installer","com.saurik.substrate","com.zachspong.temprootremovejb","com.amphoras.hidemyroot","com.amphoras.hidemyrootadfree","com.formyhm.hiderootPremium","com.formyhm.hideroot"};
        private static readonly string[] knownDangerousAppsPackages = { "com.koushikdutta.rommanager", "com.dimonvideo.luckypatcher", "com.chelpus.lackypatch", "com.ramdroid.appquarantine" };
        private static readonly string[] knownRootCloakingPackages = { "com.devadvance.rootcloak", "de.robv.android.xposed.installer", "com.saurik.substrate", "com.devadvance.rootcloakplus", "com.zachspong.temprootremovejb", "com.amphoras.hidemyroot", "com.formyhm.hideroot" };
        private static readonly string[] suPaths = { "/data/local/", "/data/local/bin/", "/data/local/xbin/", "/sbin/", "/system/bin/", "/system/bin/.ext/", "/system/bin/failsafe/", "/system/sd/xbin/", "/system/usr/we-need-root/", "/system/xbin/" ,
                                                     "/system/bin/su","/system/xbin/su","/sbin/su","/system/su","/system/bin/.ext/su","/system/usr/we-need-root/su-backup","/system/xbin/mu"};
        private static readonly string[] pathsThatShouldNotBeWritable = { "/system", "/system/bin", "/system/sbin", "/system/xbin", "/vendor/bin", "/sbin", "/etc" };

        public static bool IsPhoneRooted(Context context, IList<string> info)
        {
            bool result = false;

            if (DetectRootManagementApps(context))
            {
                result = true;
                info.Add(SecurityCheckMessages.DetectRootManagementApps);
            }
            if (CheckForSuBinary())
            {
                result = true;
                info.Add(SecurityCheckMessages.CheckForSuBinary);
            }
            if (CheckDangerousProps())
            {
                result = true;
                info.Add(SecurityCheckMessages.CheckDangerousProps);
            }
            if (CheckRWPaths())
            {
                result = true;
                info.Add(SecurityCheckMessages.CheckRWPaths);
            }
            if (CheckSuEnabled())
            {
                result = true;
                info.Add(SecurityCheckMessages.CheckSuEnabled);
            }


            return result;
        }

        private static bool DetectRootManagementApps(Context context)
        {
            PackageManager pm = context.PackageManager;
            foreach (string packageName in knownRootAppsPackages)
            {
                try
                {
                    pm.GetPackageInfo(packageName, 0);

                    return true; // Si salta el catch es correcto
                }
                catch (PackageManager.NameNotFoundException ex) { }
            }
            return false;
        }

        private static bool CheckForSuBinary()
        {
            return CheckForSuBinary("su");
        }

        private static bool CheckForSuBinary(string binaryName)
        {
            string[] paths = suPaths;
            foreach (string path in paths)
            {
                string completePath = path + binaryName;
                Java.IO.File file = new Java.IO.File(completePath);
                if (file.Exists())
                {
                    return true;
                }
            }
            return false;
        }

        private static bool CheckDangerousProps()
        {
            Dictionary<string, string> dangerousProps = new Dictionary<string, string>();
            dangerousProps.Add("ro.debuggable", "1");
            dangerousProps.Add("ro.secure", "0");

            string[] lines = PropsReader();

            foreach (string line in lines)
            {
                foreach (string key in dangerousProps.Keys)
                {
                    if (line.Contains(key))
                    {
                        string badValue = dangerousProps[key];
                        badValue = "[" + badValue + "]";
                        if (line.Contains(badValue))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static string[] PropsReader()
        {
            System.IO.Stream inputstream = null;
            try
            {
                inputstream = Java.Lang.Runtime.GetRuntime().Exec("getprop").InputStream;
            }
            catch (Java.IO.IOException e)
            {
                e.PrintStackTrace();
            }
            string propval = string.Empty;
            try
            {
                propval = new Scanner(inputstream).UseDelimiter("\\A").Next();
            }
            catch (NoSuchElementException localNoSuchElementException) { }
            return propval.Split('\n');
        }

        private static bool CheckRWPaths()
        {
            string[] lines = MountReader();

            foreach (string line in lines)
            {
                foreach (string pathToCheck in pathsThatShouldNotBeWritable)
                {
                    if (line.Contains(pathToCheck))
                    {
                        string aux = line.Substring(line.LastIndexOf(pathToCheck) + pathToCheck.Length);
                        if (aux.Contains(" rw,"))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static string[] MountReader()
        {
            System.IO.Stream inputstream = null;
            try
            {
                inputstream = Runtime.GetRuntime().Exec("mount").InputStream;
            }
            catch (Java.IO.IOException e)
            {
                e.PrintStackTrace();
            }
            string propval = "";
            try
            {
                propval = new Scanner(inputstream).UseDelimiter("\\A").Next();
            }
            catch (NoSuchElementException e)
            {
                e.PrintStackTrace();
            }
            return propval.Split('\n');
        }

        private static bool CheckSuEnabled()
        {
            Java.Lang.Process process = null;
            try
            {
                process = Runtime.GetRuntime().Exec(new string[] { "/system/xbin/which", "su" });
                BufferedReader reader = new BufferedReader(new InputStreamReader(process.InputStream));
                if (reader.ReadLine() != null)
                {
                    return true;
                }
                return false;
            }
            catch (Throwable t)
            {
                return false;
            }
            finally
            {
                if (process != null)
                {
                    process.Destroy();
                }
            }
        }
    }
}

