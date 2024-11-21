
using MusicRequests.Touch.Services;
using MusicRequests.Core.Services;

namespace MusicRequests.Touch.Services 
{
    public class PlatformHttpClientHandler : IPlatformHttpClientHandler
    {
        public HttpMessageHandler GetNativeHttpClientHandler()
        {
            return new NativeMessageHandler();
        }
    }
}
