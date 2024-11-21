using System;
namespace MusicRequests.Core.Services
{
	public interface IVersionTrackerService
	{
		void Track ();
		bool IsFirstLaunchForVersion { get; }
	}
}

