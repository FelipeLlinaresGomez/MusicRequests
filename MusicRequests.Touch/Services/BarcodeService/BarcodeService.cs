using System.Runtime.InteropServices;
using MusicRequests.Core.Services.BarcodeService;
using Microsoft.Maui.Graphics;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Rendering;

namespace MusicRequests.Touch.Services
{
    public class BarcodeService : IBarcodeService
    {
        public byte[] GenerateQR(string content, int width, int height, int version = 3)
        {
            var writer = new BarcodeWriter<UIImage>
            {
                Format = BarcodeFormat.QR_CODE,
                Renderer = new CustomBitmapRenderer()
            };

            var barcodeBitmap = writer.Write(content);
            var img = barcodeBitmap.AsPNG();

            byte[] pixels = new byte[img.Length];

            Marshal.Copy(img.Bytes, pixels, 0, Convert.ToInt32(img.Length));

            return pixels;
        }

        private class CustomBitmapRenderer : IBarcodeRenderer<UIImage>
        {
            public UIImage Render(BitMatrix matrix, BarcodeFormat format, string content)
            {
                return RenderMatrix(matrix);
            }

            public UIImage Render(BitMatrix matrix, BarcodeFormat format, string content, EncodingOptions options)
            {
                return RenderMatrix(matrix);
            }

            private static UIImage RenderMatrix(BitMatrix matrix)
            {
                var width = matrix.Width;
                var height = matrix.Height;

                var black = new CGColor(0f, 0f, 0f);
                var white = new CGColor(1.0f, 1.0f, 1.0f);

                UIGraphics.BeginImageContext(new CGSize(width, height));
                var context = UIGraphics.GetCurrentContext();

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        context.SetFillColor(matrix[x, y] ? black : white);
                        context.FillRect(new CGRect(x, y, 1, 1));
                    }
                }

                var img = UIGraphics.GetImageFromCurrentImageContext();

                UIGraphics.EndImageContext();

                return img;
            }
        }
    }

}
