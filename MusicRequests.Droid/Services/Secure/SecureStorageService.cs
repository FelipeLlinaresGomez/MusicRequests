using Android.App;
using Javax.Crypto;
using Android.Content;
using Android.Util;
using System.Text;
using MusicRequests.Droid.Helpers;
using MvvmCross.Platforms.Android;
using MvvmCross;
using AndroidX.Core.Hardware.Fingerprint;

namespace MusicRequests.Core.Services
{
    public class SecureStorageService : ISecureStorageService//, FingerPrintDialogFragment.ICallback
	{
		Activity activity;
		string KeyName;
		string data;
		Cipher mCipher;


		public Activity Activity
		{
			get
			{
				return activity ?? Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
			}
		}

		public void DeleteUser(string user)
		{
			var sharedPreferences = Activity.GetSharedPreferences("MusicRequest", FileCreationMode.Private);
			if (sharedPreferences.Contains(user))
			{
				sharedPreferences.Edit().Remove(user);
			}
		}

		public string GetPasswordForUser(string user)
		{
			KeyName = user;
			GenerateCipher(CipherMode.DecryptMode);

			var mPreferences = Activity.GetSharedPreferences("MusicRequest", FileCreationMode.Private);
			string base64EncryptedPassword = mPreferences.GetString(user, null);
			byte[] encryptedPassword = Base64.Decode(base64EncryptedPassword, Base64Flags.Default);

			// decrypt the password
			var password = (mCipher.DoFinal(encryptedPassword));

			var passwordStr = Encoding.UTF8.GetString(password);

			return passwordStr;
		}

		FingerprintManagerCompat mFingerprintManager
		{
			get
			{
				
				return (FingerprintManagerCompat)Activity.GetSystemService(Context.FingerprintService);
			}
		}

		public void SavePasswordForUser(string user, string password)
		{
			KeyName = user;
			data = password;

			if (CurrentAndroidVersionSupportFingerprintAuth())
			{
				GenerateCipher(CipherMode.EncryptMode);
				var value = TryEncrypt(data);
				SaveEncryptValue(value);
			}
		}

		public static bool CurrentAndroidVersionSupportFingerprintAuth()
		{
			return Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M;
		}

		void GenerateCipher(CipherMode mode = CipherMode.EncryptMode)
		{
			var cryptoObjectHelper = new CryptoObjectHelper();
			if (mode == CipherMode.DecryptMode)
			{
				mCipher = cryptoObjectHelper.CreateCipher(true, mode, KeyName, Activity.GetIVParamsForDecryption());
			}
			else
			{
				mCipher = cryptoObjectHelper.CreateCipher(true, mode, KeyName);
			}
		}

		byte[] TryEncrypt(string text)
		{
			try
			{
				byte[] encrypted = mCipher.DoFinal(System.Text.Encoding.UTF8.GetBytes(text));
				return encrypted;
			}
			catch (BadPaddingException e)
			{
				var msg = e.Message;
				return null;
			}
			catch (IllegalBlockSizeException e)
			{
				var msg = e.Message;
				return null;
			}
		}

		public void SaveEncryptValue(byte[] passwordbytes)
		{
			try
			{
				byte[] encryptionIv = mCipher.GetIV();
				string encrypted = Base64.EncodeToString(passwordbytes, Base64Flags.Default);
				Activity.GetSharedPreferences("MusicRequest", FileCreationMode.Private).Edit()
						.PutString(KeyName, encrypted)
						.PutString("encryption_iv", Base64.EncodeToString(encryptionIv, Base64Flags.Default))
						.Commit();

			}
			catch (System.Exception e)
			{
				var msg = e.Message;
			}
		}
	}
}
