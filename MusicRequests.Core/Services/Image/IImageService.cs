using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRequests.Core.Services
{
    public enum ImageSize
    {
        Normal,
        Small,
        Thumbnail,
        Tiny,
		Custom
    }

    public interface IImageService
    {
        byte[] CreateThumbnail(byte[] imgArray, ImageSize size, int maxSize = 200000);
        Task<byte[]> ResizeToMaxSize(byte[] imgArray, int maxSize = 200000);
    }
}
