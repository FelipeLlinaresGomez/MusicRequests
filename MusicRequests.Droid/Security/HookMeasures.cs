using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Content.PM;
using Dalvik.SystemInterop;
using Java.IO;
using Java.Lang;
using Java.Lang.Reflect;

namespace MusicRequests.Droid.Security
{
    public class HookMeasures
    {
        private static readonly string[] knownHookApps = { "de.robv.android.xposed.installer", "com.saurik.substrate" };
        private static readonly string[] knownHookLibraries = { "com.saurik.substrate", "xposedbridge.jar" };

        public static bool IsPhoneHooked(Context context)
        {
            return DetectHookApps(context) ||
                DetectHookStackTrace() ||
                DetectHookNative(context) ||
                DetectHookJar();
        }

        private static bool DetectHookApps(Context context)
        {
            PackageManager pm = context.PackageManager;
            List<ApplicationInfo> applicationInfoList = (List<ApplicationInfo>)pm.GetInstalledApplications(PackageInfoFlags.MetaData);
            foreach (ApplicationInfo appinfo in applicationInfoList)
            {
                if (knownHookApps.FirstOrDefault(x => x.Equals(appinfo.PackageName.ToLower())) != null)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool DetectHookStackTrace()
        {
            try
            {
                throw new Java.Lang.Exception("Exception on purpose");
            }
            catch (Java.Lang.Exception ex)
            {
                int zygoteInitCallCount = 0;
                foreach (StackTraceElement stackTraceElement in ex.GetStackTrace())
                {
                    if (stackTraceElement.ClassName.ToLower().Equals("com.android.internal.os.zygoteinit"))
                    {
                        zygoteInitCallCount++;
                        if (zygoteInitCallCount == 2)
                        {
                            return true;
                        }
                        if (stackTraceElement.ClassName.ToLower().Equals("com.saurik.substrate.MS$2") && stackTraceElement.MethodName.ToLower().Equals("invoked"))
                        {
                            return true;
                        }
                        if (stackTraceElement.ClassName.ToLower().Equals("de.robv.android.xposed.XposedBridge") && stackTraceElement.MethodName.ToLower().Equals("main"))
                        {
                            return true;
                        }
                        if (stackTraceElement.ClassName.ToLower().Equals("de.robv.android.xposed.XposedBridge") && stackTraceElement.MethodName.ToLower().Equals("handlehookedmethod"))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static bool DetectHookNative(Context context)
        {
            PackageManager pm = context.PackageManager;
            List<ApplicationInfo> applicationInfoList = (List<ApplicationInfo>)pm.GetInstalledApplications(PackageInfoFlags.MetaData);
            foreach (ApplicationInfo applicationInfo in applicationInfoList)
            {
                if (applicationInfo.ProcessName.ToLower().Equals(context.PackageName.ToLower()))
                {
                    var classes = new HashSet<string>();
                    try
                    {
                        DexFile dex = new DexFile(applicationInfo.SourceDir);
                        var entries = dex.Entries();
                        while (entries.HasMoreElements)
                        {
                            string entry = (string)entries.NextElement();
                            classes.Add(entry);
                        }
                        dex.Close();
                    }
                    catch (System.IO.IOException ex) { }

                    foreach (string className in classes)
                    {
                        if (className.StartsWith(nameof(HookMeasures)))
                        {
                            try
                            {
                                var classInstance = Java.Lang.Class.ForName(className);
                                foreach (Method method in classInstance.GetDeclaredMethods())
                                {
                                    if (Modifier.IsNative(method.Modifiers))
                                    {
                                        return true;
                                    }
                                }
                            }
                            catch (ClassNotFoundException ex) { }
                        }
                    }
                }
            }
            return false;
        }

        private static bool DetectHookJar()
        {
            try
            {
                var libraries = new HashSet<string>();
                string mapsFilename = "proc" + Android.OS.Process.MyPid() + "/maps";
                BufferedReader reader = new BufferedReader(new FileReader(mapsFilename));
                string line;
                int n;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.EndsWith(".so") || line.EndsWith(".jar"))
                    {
                        n = line.LastIndexOf(" ");
                        libraries.Add(line.Substring(n + 1));
                    }
                }

                foreach (string library in libraries)
                {
                    if (knownHookLibraries.FirstOrDefault(x => x.ToLower().Contains(library.ToLower())) != null)
                    {
                        return true;
                    }
                }
                reader.Close();
            }
            catch (Java.Lang.Exception ex) { }
            return false;

        }
    }
}
