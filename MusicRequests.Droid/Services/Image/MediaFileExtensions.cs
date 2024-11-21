using System;
using Android.Graphics;
using Android.Media;
using MusicRequests.Core.Services.Image;
using MediaOrientation = Android.Media.Orientation;
using Microsoft.Maui.Storage;

namespace MusicRequests.Droid.Services.Image
{
	public class MediaFileExtensions : IMediaFileExtensions
    {
        /// <summary>
        ///  Rotate an image if required.
        /// </summary>
        /// <param name="file">The file image</param>
        /// <returns>True if rotation occured, else fal</returns>
        async public Task<bool> FixOrientationAsync(FileResult file)
        {
            if (file == null)
                return false;
            try
            {

                var filePath = file.FullPath;
                var orientation = GetRotation(filePath);

                if (!orientation.HasValue)
                    return false;

                Bitmap bmp = RotateImage(filePath, orientation.Value);
                var quality = 100;

                using (var stream = File.Open(filePath, FileMode.OpenOrCreate))
                    await bmp.CompressAsync(Bitmap.CompressFormat.Jpeg, quality, stream);

                bmp.Recycle();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private static int? GetRotation(string filePath)
        {
            try
            {
                ExifInterface ei = new ExifInterface(filePath);
                var orientation = (MediaOrientation)ei.GetAttributeInt(ExifInterface.TagOrientation, (int)MediaOrientation.Normal);
                switch (orientation)
                {
                    case MediaOrientation.Rotate90:
                        return 90;
                    case MediaOrientation.Rotate180:
                        return 180;
                    case MediaOrientation.Rotate270:
                        return 270;
                    default:
                        return null;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        private static Bitmap RotateImage(string filePath, int rotation)
        {
            Bitmap originalImage = BitmapFactory.DecodeFile(filePath);

            Matrix matrix = new Matrix();
            matrix.PostRotate(rotation);
            var rotatedImage = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true);
            originalImage.Recycle();
            return rotatedImage;
        }
    }
}