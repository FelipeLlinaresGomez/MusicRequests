using MusicRequests.Core.Services;

namespace MusicRequests.Droid.Services
{
    public class PlatformHttpClientHandler : IPlatformHttpClientHandler
    {
        public HttpMessageHandler GetNativeHttpClientHandler()
        {
            return new CustomAndroidClientHandler();
        }
    }
}
