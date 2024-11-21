using MusicRequests.Core.Services;
using MusicRequests.Touch.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using System.Threading.Tasks;

namespace MusicRequests.Touch.Services
{
    public class ImageService : IImageService
    {
        public ImageService()
        {
        }

        public byte[] CreateThumbnail(byte[] imgArray, ImageSize size, int maxSize = 60000)
        {
            UIImage img;
            float percentage = 0.5f;
            switch (size)
            {
                case ImageSize.Small:
                    percentage = 0.25f;
                    break;
                case ImageSize.Thumbnail:
                    percentage = 0.15f;
                    break;
                case ImageSize.Tiny:
                    percentage = 0.1f;
                    break;
            }

            img = imgArray.ToImage();
            img = img.MaxResizeImage((float)img.Size.Width * percentage, (float)img.Size.Height * percentage);

            return img.AsPNG().ToArray();
        }

        public Task<byte[]> ResizeToMaxSize(byte[] imgArray, int maxSize = 200000)
        {
            return Task.FromResult(CreateThumbnail(imgArray, ImageSize.Small));
        }
    }
}
