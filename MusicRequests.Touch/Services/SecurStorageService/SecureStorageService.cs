using System;
using MusicRequests.Core.Services;
using MusicRequests.Touch.Helpers;

namespace MusicRequests.Touch.Services
{
	public class SecureStorageService : ISecureStorageService
	{
		public event EventHandler OnFingerprintConfirmation;
		public event EventHandler OnFingerprintError;
		public event EventHandler OnFingerprintIncompatible;

		private string ServiceId = "MusicRequests";

		public void DeleteUser (string user)
		{
			GetServiceIdByEntorno();
			KeychainHelpers.DeletePasswordForUsername (user, ServiceId, true);
		}

		public string GetPasswordForUser (string user)
		{
			GetServiceIdByEntorno();
			return KeychainHelpers.GetPasswordForUsername (user, ServiceId, true);
		}

		public void SavePasswordForUser (string user, string password)
		{
			GetServiceIdByEntorno();
			KeychainHelpers.SetPasswordForUsername (user, password, ServiceId, Security.SecAccessible.Always, true);
		}


		private void GetServiceIdByEntorno() 
		{
			if (ApiSettings.Entorno == "vNEXT")
				ServiceId = "MusicRequestsVNext";
			else 
				ServiceId = "MusicRequestsApp";
		}

	}
}

