using Android.Graphics;
using MusicRequests.Core.Services;
using System;
using System.IO;
using MusicRequests.Core.Helpers;
using System.Threading.Tasks;
using Java.IO;
using MvvmCross.Logging;
using Microsoft.Extensions.Logging;

namespace MusicRequests.Droid.Services
{
    public class ImageService : IImageService
    {
        private readonly ILogger<ImageService> _log;

        public ImageService(ILogger<ImageService> log)
        {
            _log = log;
        }

        public byte[] CreateThumbnail(byte[] imgArray, ImageSize size, int maxSize = 60000)
        {
            if (imgArray == null || imgArray.Length == 0)
            {
                return null;
            }

            byte[] newImg;

            double percentage = 0.5;
			switch (size)
			{
			    case ImageSize.Small:
			        percentage = 0.3;
			        break;
			    case ImageSize.Thumbnail:
			        percentage = 0.15;
			        break;
			    case ImageSize.Tiny:
			        percentage = 0.1;
			        break;
			}

            var quality = 25;

            using (var originalImage = BitmapFactory.DecodeByteArray(imgArray, 0, imgArray.Length))
            {
				using (var compressedImage = Bitmap.CreateScaledBitmap(originalImage, (int)(originalImage.Width * percentage), (int)(originalImage.Height * percentage), false))
                {
                    using (var stream = new MemoryStream())
                    {
						compressedImage.Compress(Bitmap.CompressFormat.Jpeg, quality, stream);
                        newImg = stream.ToArray();
                        stream.Close();
                    }
                    compressedImage.Recycle();
                }
                originalImage.Recycle();

                // Dispose of the Java side bitmap.
                GC.Collect();
            }

            return newImg;

        }

        public Task<byte[]> ResizeToMaxSize(byte[] imgArray, int maxSize = 200000)
        {
            byte[] newImg;
			var quality = 90;
            var percentage = 0.5;
            var imgGeneratedSize = Convert.ToBase64String(imgArray).Length;
            _log.LogDebug($"Tamano imagen original: {imgGeneratedSize}");

            using (var originalImage = BitmapFactory.DecodeByteArray(imgArray, 0, imgArray.Length))
            {
				
                using (var compressedImage = Bitmap.CreateScaledBitmap(originalImage, 
                                                                       (int)(originalImage.Width * percentage), 
                                                                       (int)(originalImage.Height * percentage), false))
                {
					do
					{
                        using (var stream = new MemoryStream())
                        {
                            compressedImage.Compress(Bitmap.CompressFormat.Jpeg, quality, stream);
                            newImg = stream.ToArray();
                            imgGeneratedSize = Convert.ToBase64String(newImg).Length;
                            _log.LogDebug($"Tamano imagen generada:{imgGeneratedSize}. MaxSize: {maxSize}");
                            stream.Close();
                            quality = quality - 5;
                        }
                    } while (maxSize < imgGeneratedSize && quality > 0);

                    compressedImage.Recycle();
			    }
                
				originalImage.Recycle();

				// Dispose of the Java side bitmap.
				GC.Collect();
            }
            
            return Task.FromResult(newImg);
        }
    }
}