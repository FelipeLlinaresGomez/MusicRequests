using System;
using Android.Content;
using Android.Util;
using Javax.Crypto.Spec;

namespace MusicRequests.Droid.Helpers
{
	public static class SharedPreferencesHelper
	{
		public static IvParameterSpec GetIVParamsForDecryption(this Context context)
		{
			if (context != null)
			{
				var mPreferences = context.GetSharedPreferences("MusicRequest", FileCreationMode.Private);
				string encodedIv = mPreferences?.GetString("encryption_iv", null);
				if (encodedIv != null)
				{
					byte[] bytes = Base64.Decode(encodedIv, Base64Flags.Default);
					IvParameterSpec ivParams = new IvParameterSpec(bytes);
					return ivParams;
				}
				else
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}
	}
}
