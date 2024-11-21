using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Java.IO;
using Java.Lang;
using Java.Util;
using File = Java.IO.File;

namespace MusicRequests.Droid.Security
{
    public class RootMeasures
    {
        private static readonly string[] knownRootAppsPackages = { "com.noshufou.android.su", "com.noshufou.android.su.elite", "eu.chainfire.supersu", "com.koushikdutta.superuser", "com.thirdparty.superuser", "com.yellowes.su" };
        private static readonly string[] knownDangerousAppsPackages = { "com.koushikdutta.rommanager", "com.dimonvideo.luckypatcher", "com.chelpus.lackypatch", "com.ramdroid.appquarantine" };
        private static readonly string[] knownRootCloakingPackages = { "com.devadvance.rootcloak", "de.robv.android.xposed.installer", "com.saurik.substrate", "com.devadvance.rootcloakplus", "com.zachspong.temprootremovejb", "com.amphoras.hidemyroot", "com.formyhm.hideroot" };
        private static readonly string[] suPaths = { "/data/local/", "/data/local/bin/", "/data/local/xbin/", "/sbin/", "/system/bin/", "/system/bin/.ext/", "/system/bin/failsafe/", "/system/sd/xbin/", "/system/usr/we-need-root/", "/system/xbin/" };
        private static readonly string[] pathsThatShouldNotBeWritable = { "/system", "/system/bin", "/system/sbin", "/system/xbin", "/vendor/bin", "/sbin", "/etc" };


        public static bool IsPhoneRooted(Context context)
        {
            return DetectRootManagementApps(context) ||
                DetectDangerousApps(context) ||
                CheckForSuBinary() ||
                CheckDangerousProps() ||
                CheckRWPaths() ||
                DetectTestKeys() ||
                CheckSuEnabled() ||
                DetectRootCloakingApps(context);

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

        private static bool DetectDangerousApps(Context context)
        {
            PackageManager pm = context.PackageManager;
            foreach (string packageName in knownDangerousAppsPackages)
            {
                try
                {
                    pm.GetPackageInfo(packageName, 0);
                    return true; // Si salta el catch es correcto
                }
                catch (PackageManager.NameNotFoundException e) { }
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
                File file = new File(completePath);
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

        private static bool DetectTestKeys()
        {
            string buildTags = Build.Tags;
            if (!string.IsNullOrEmpty(buildTags) && buildTags.Contains("test-keys"))
            {
                return true;
            }
            return false;
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

        private static bool DetectRootCloakingApps(Context context)
        {
            PackageManager pm = context.PackageManager;
            foreach (string packageName in knownRootCloakingPackages)
            {
                try
                {
                    pm.GetPackageInfo(packageName, 0);
                    return true; // Si salta el catch es correcto
                }
                catch (PackageManager.NameNotFoundException e) { }
            }
            return false;
        }
    }
}