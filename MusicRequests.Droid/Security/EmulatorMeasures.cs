using System;
using Android.Content;
using Android.OS;
using Android.Telephony;
using Java.IO;
using File = Java.IO.File;

namespace MusicRequests.Droid.Security
{
    public class EmulatorMeasures
    {
        private static readonly string[] emulatorNumbers = { "15555215554", "15555215556", "15555215558", "15555215560", "15555215562", "15555215564", "15555215566", "15555215568", "15555215570", "15555215572", "15555215574", "15555215576", "15555215578", "15555215580", "15555215582", "15555215584" };
        private static readonly string[] emulatorDeviceIds = { "000000000000000", "012345678912345", "e21833235b6eef10" };
        private static readonly string[] emulatorFiles = { "/system/lib/libc_malloc_debug_qemu.so", "/sys/qemu_trace" };
        private static readonly string[] emulatorDrivers = { "goldfish" };


        public static bool IsAppInEmulation(Context context)
        {
            return HasPhoneNumber(context) ||
                HasKnownDeviceId(context) ||
                HasKnownImsi(context) ||
                HasQEmuProps() ||
                HasQEmuDriver() ||
                HasEmulatorBuild(context) ||
                HasQEmuFiles();

        }

        private static bool HasPhoneNumber(Context context)
        {
            TelephonyManager telephonyManager = (TelephonyManager)context.GetSystemService(Context.TelephonyService);
            string phoneNumber = telephonyManager.Line1Number;
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                foreach (string emunumber in emulatorNumbers)
                {
                    if (phoneNumber.ToLower().Equals(emunumber.ToLower()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool HasKnownDeviceId(Context context)
        {
            TelephonyManager telephonyManager = (TelephonyManager)context.GetSystemService(Context.TelephonyService);
            string deviceId = telephonyManager.DeviceId;
            if (!string.IsNullOrEmpty(deviceId))
            {
                foreach (string knownDeviceId in emulatorDeviceIds)
                {
                    if (deviceId.ToLower().Equals(knownDeviceId.ToLower()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool HasKnownImsi(Context context)
        {
            TelephonyManager telephonyManager = (TelephonyManager)context.GetSystemService(Context.TelephonyService);
            string imsi = telephonyManager.SubscriberId;
            if (!string.IsNullOrEmpty(imsi) && imsi.Equals("310260000000000"))
            {
                return true;
            }
            return false;

        }

        private static bool HasQEmuProps()
        {
            try
            {
                bool[] props = new bool[14];
                props[0] = GetSystemProperty("init.svc.qemud").ContentEquals("null");
                props[1] = GetSystemProperty("init.svc.qemu-props").ContentEquals("null");
                props[2] = GetSystemProperty("qemu.sf.fake_camera").ContentEquals("null");
                props[3] = GetSystemProperty("qemu.sf.lcd_density").ContentEquals("null");
                props[4] = GetSystemProperty("ro.bootloader").ContentEquals("unknown");
                props[5] = GetSystemProperty("ro.bootmode").ContentEquals("unknown");
                props[6] = GetSystemProperty("ro.hardware").ContentEquals("goldfish");
                props[7] = GetSystemProperty("ro.kernel.android.qemud").ContentEquals("null");
                props[8] = GetSystemProperty("ro.kernel.qemu.gles").ContentEquals("null");
                props[9] = (GetSystemProperty("ro.kernel.qemu").Length() > 0 ? true : false);
                props[10] = GetSystemProperty("ro.product.device").ContentEquals("generic");
                props[11] = GetSystemProperty("ro.product.model").ContentEquals("sdk");
                props[12] = GetSystemProperty("ro.product.name").ContentEquals("sdk");
                props[13] = GetSystemProperty("ro.serialno").ContentEquals("null");

                int counter = 0;
                foreach (bool prop in props)
                {
                    if (counter > 2)
                    {
                        return true;
                    }
                    if (prop)
                    {
                        counter++;
                    }
                }
            }
            catch (Java.Lang.Exception ex) { }
            return false;
        }

        private static Java.Lang.String GetSystemProperty(string name)
        {
            Java.Lang.Class systemPropertyClass = Java.Lang.Class.ForName("android.os.SystemProperties");
            return (Java.Lang.String)systemPropertyClass.GetMethod("get", new Java.Lang.Class[] { Java.Lang.Class.ForName("String") }).Invoke(systemPropertyClass, new Java.Lang.Object[] { name });
        }

        private static bool HasQEmuDriver()
        {
            File driverFile = new File("/proc/tty/drivers");
            if (driverFile.Exists() && driverFile.CanRead())
            {
                byte[] data = new byte[driverFile.Length()];
                try
                {
                    Java.IO.InputStream istream = new FileInputStream(driverFile);
                    istream.Read(data);
                    istream.Close();
                }
                catch (Java.Lang.Exception) { }

                string driverData = System.Text.Encoding.Default.GetString(data);
                foreach (string knownDriver in emulatorDrivers)
                {
                    if (!string.IsNullOrEmpty(driverData) && driverData.IndexOf(knownDriver) != 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool HasEmulatorBuild(Context context)
        {
            string brand = Build.Brand;
            string device = Build.Device;
            string hardware = Build.Hardware;
            string model = Build.Model;
            string product = Build.Product;

            if ((!string.IsNullOrEmpty(brand) && brand.Equals("generic")) ||
                (!string.IsNullOrEmpty(device) && device.Equals("generic")) ||
                (!string.IsNullOrEmpty(model) && model.Equals("sdk")) ||
                (!string.IsNullOrEmpty(product) && product.Equals("sdk")) ||
                (!string.IsNullOrEmpty(hardware) && hardware.Equals("goldfish")))
            {
                return true;
            }
            return false;
        }

        private static bool HasQEmuFiles()
        {
            foreach (string pipe in emulatorFiles)
            {
                File qemu_file = new File(pipe);
                if (qemu_file.Exists())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
