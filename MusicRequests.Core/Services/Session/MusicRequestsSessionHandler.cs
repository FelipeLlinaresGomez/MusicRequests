using System;
using System.Threading.Tasks;
using MvvmCross;

namespace MusicRequests.Core.Services
{
    public class MusicRequestsSessionHandler : IMusicRequestsSessionHandler
    {
        public HeaderMusicRequests GetHeaders()
        {
            return new HeaderMusicRequests();// App.User);
        }

        public void SetSessionToken(string st, int? timerExpiresIn = null)
        {
            //if (App.User != null)
            //{
            //    App.User.SessionToken = st;

            //    HttpClientHelper.GetHttpClient()
            //                    .ForceHeaderUpdate(HeaderKeys.TokenMusicRequests, App.User?.TokenIdentity)
            //                    .ForceHeaderUpdate(HeaderKeys.TokenSesionMusicRequests, App.User?.SessionToken);

            //    if (timerExpiresIn.HasValue && timerExpiresIn.Value > 0)
            //    {
            //        var sessionService = Mvx.IoCProvider.Resolve<ISessionService>();
            //        sessionService.StartTimerRefrescoSesionMusicRequests(timerExpiresIn.Value);
            //    }
            //}
        }

        public void SetSesionExpirada()
        {
            //if (App.User != null)
            //{
            //    App.User.TokenIdentity = string.Empty;
            //    App.User.RefreshTokenIdentity = string.Empty;
            //    App.User.SessionToken = string.Empty;

            //    var sessionService = Mvx.IoCProvider.Resolve<ISessionService>();
            //    sessionService.StopTimerRefreshTokenMusicRequestsSession();
            //    sessionService.StopTimerRefreshTokenIdentidad();
            //}
        }

        //public Task RegisterDependency(Dependency dependency)
        //{
        //    var dependencyTrack = Mvx.IoCProvider.Resolve<IDependencyTracker>();
        //    return dependencyTrack.RegisterDependency(dependency);
        //}
    }
}
