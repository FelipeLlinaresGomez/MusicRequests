using MusicRequests.Core.Services;

namespace MusicRequests.Core.Managers
{
    public class MusicRequestDeepLinkingManager : IMusicRequestDeepLinkingManager
    {
        private readonly IDeepLinkingService _deeplinkingService;

        public MusicRequestDeepLinkingManager(IDeepLinkingService deepLinkingService)
        {
            _deeplinkingService = deepLinkingService;
        }

        public void OpenStoreToEnterReview()
        {
            _deeplinkingService.OpenStoreToReview();        
        }
    }
}