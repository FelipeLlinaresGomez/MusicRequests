using Microsoft.Maui.ApplicationModel;

namespace MusicRequests.Core.Services
{
    public class VersionTrackerService : IVersionTrackerService
    {
        public VersionTrackerService() { }

        public bool IsFirstLaunchForVersion
        {
            get => VersionTracking.IsFirstLaunchForCurrentVersion;
        }

        public void Track()
        {
            VersionTracking.Track();
        }
    }
}

