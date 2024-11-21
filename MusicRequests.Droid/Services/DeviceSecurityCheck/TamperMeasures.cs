using Android.Content;
using Android.Content.PM;
using Java.Security;

namespace MusicRequests.Droid.Services.DeviceSecurityCheck
{
	public class TamperMeasures
	{
        private const string APP_SIGNATURE = "F6A8A8B45B8483E13F7022269F68997D9EE927CB"; // CERT PRUEBAS
        //private const string APP_SIGNATURE = "50FBE8921D7FE25C65D3D2916A61CEF121616D84"; // CERT PROD

        public static bool CheckValidAppSignature(Context context, IList<string> info)
        {
            try
            {
                PackageInfo packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, PackageInfoFlags.Signatures);
                foreach (Android.Content.PM.Signature signature in packageInfo.Signatures)
                {
                    // SHA1 the signature
                    string sha1 = GetSHA1(signature.ToByteArray());
                    // check is matches hardcoded value
                    var validSignature = APP_SIGNATURE.Equals(sha1);

                    if (!validSignature)
                    {
                        info.Add(SecurityCheckMessages.CheckValidAppSignature);
                    }

                    return validSignature;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

        public static string GetSHA1(byte[] sig)
        {
            MessageDigest digest = MessageDigest.GetInstance("SHA1");
            digest.Update(sig);
            byte[] hashtext = digest.Digest();
            return BytesToHex(hashtext);
        }

        //util method to convert byte array to hex string
        public static string BytesToHex(byte[] bytes)
        {
            char[] hexArray = { '0', '1', '2', '3', '4', '5', '6', '7', '8',
                '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            char[] hexChars = new char[bytes.Length * 2];
            int v;
            for (int j = 0; j < bytes.Length; j++)
            {
                v = bytes[j] & 0xFF;
                hexChars[j * 2] = hexArray[v >> 4];
                hexChars[j * 2 + 1] = hexArray[v & 0x0F];
            }
            return new string(hexChars);
        }
    }
}

