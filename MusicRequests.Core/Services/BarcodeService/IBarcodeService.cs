
namespace MusicRequests.Core.Services.BarcodeService
{
    public interface IBarcodeService
    {
        byte[] GenerateQR(string contents, int width, int height, int version = 3);
    }
}
