using System;
using System.Threading.Tasks;

namespace MusicRequests.Core.Services
{
	public interface IPermissionsService
	{
		bool HasLocationPermission();

		event EventHandler OnWritePermissionGrantedEvent;
		bool CheckPermissions();
		void OnWritePermissionGranted();
		bool HasReadPhoneStatePermission();

    }
}

