using System.Threading.Tasks;
namespace MusicRequests.Core.Services
{

    public interface IDeviceInfoService
    {
        Task<string> GetDeviceId();
    }

}
