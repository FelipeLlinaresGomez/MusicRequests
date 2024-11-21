namespace MusicRequests.Core.Services
{
    public interface IDeepLinkingService
    {
        void OpenAppStoreToUpdate(string url);
        void OpenStoreToReview();
    }
}