using System;
using System.Threading.Tasks;

namespace MusicRequests.Core.Services
{
	public interface ITouchIdService
	{
		bool HasAccessToTouchId();
		Task<bool> RequestPermissionsTouchId();
        bool IsIphoneX();

	}
}

