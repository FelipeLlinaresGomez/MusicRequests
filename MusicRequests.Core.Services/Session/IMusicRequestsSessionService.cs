using System.Threading.Tasks;
using Refit;

namespace MusicRequests.Core.Services
{
    public interface IMusicRequestsSessionService
    {
        //[Get("/api/login")]
        //Task<IMuicRequestsSessionLoginResponse> Crear();

        //[Get("/api/refresh")]
        //Task<IMuicRequestsSessionLoginResponse> Refresco();

        [Get("/api/logout")]
        [Headers("Anonymous: true")]
        Task Logout([Header("Authorization")] string bearerToken, [Header("x-musicrequests-st")] string tokenSesion);
    }
}
