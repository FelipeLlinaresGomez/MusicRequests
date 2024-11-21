using Android.Graphics;
using Android.OS;
using MusicRequests.Core.Services.BarcodeService;
using ZXing;
using ZXing.QrCode;

namespace MusicRequests.Droid.Services
{
    public class BarcodeService : IBarcodeService
    {
        public byte[] GenerateQR(string contents, int width, int height, int version = 3)
        {
            byte[] bitmapData;
            var barcodeWriter = new QRCodeWriter();
            var matrix = barcodeWriter.encode(contents, BarcodeFormat.QR_CODE, width, height);

            var w = matrix.Width;
            var h = matrix.Height;
            var pixels = new int[w * h];

            for (int y = 0; y < h; y++)
            {
                int offset = y * w;
                for (int x = 0; x < w; x++)
                {
                    pixels[offset + x] = matrix[x, y] ? Color.Black : Color.White;
                }
            }

            Bitmap bitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
            bitmap.SetPixels(pixels, 0, width, 0, 0, w, h);

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                return stream.ToArray();
            }
        }
    }
}
