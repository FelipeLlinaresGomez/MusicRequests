using System;
using System.Net.Http;

namespace MusicRequests.Core.Services
{
    public interface IPlatformHttpClientHandler
    {
        HttpMessageHandler GetNativeHttpClientHandler();
    }
}   
