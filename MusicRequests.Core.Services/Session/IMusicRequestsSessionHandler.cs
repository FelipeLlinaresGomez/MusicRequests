using System;
using System.Threading.Tasks;

namespace MusicRequests.Core.Services
{
    public interface IMusicRequestsSessionHandler
    {
        HeaderMusicRequests GetHeaders();

        void SetSessionToken(string st, int? timerExpiresIn = null);

        void SetSesionExpirada();
    }
}