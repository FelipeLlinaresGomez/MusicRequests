using System;
using System.Threading.Tasks;
using MusicRequests.Core.Helpers;
using MvvmCross;

namespace MusicRequests.Core.Services
{
	public class SessionService : ISessionService
	{
        public int MusicRequestsSessionTimeoutMilis { get; set; }
        public int IdentidadRefreshTimeoutMilis { get; set; }
        private Timer RefreshTokenIdentidadTimer;
        private Timer RefreshTokenMusicRequestsSessionTimer;

        private DateTime LastTimeAccess = DateTime.Now;
		private AccessType? _tipoAcceso;

		public void CleanSessionData()
		{
			_tipoAcceso = null;
		}

		public bool IsHCESession()
		{
			return _tipoAcceso.HasValue && _tipoAcceso.Value == AccessType.HCE;
		}

		public bool IsSessionExpired()
		{
			if (IsUserAuthenticated())
			{
				return LastTimeAccess.AddMinutes(Constants.TIMEOUT_SESSION) < DateTime.Now;
			}
			return false;
		}

		public bool IsUserAuthenticated()
		{
            return false;//return App.User != null;
		}

		public void SaveUserLastAccess()
		{
			LastTimeAccess = DateTime.Now;
		}

		public void SetAccessType(AccessType tipoAcceso)
		{
			_tipoAcceso = tipoAcceso;
		}

        #region Sesion MusicRequests

        public void StartTimerRefrescoSesionMusicRequests(int expirationTimeInSeconds)
        {
            if (expirationTimeInSeconds <= 0)
                return;

            MusicRequestsSessionTimeoutMilis = expirationTimeInSeconds * 1000;
            if (RefreshTokenMusicRequestsSessionTimer != null)
            {
                RefreshTokenMusicRequestsSessionTimer.Dispose();
            }

            //if (App.User != null &&
            //    !string.IsNullOrEmpty(App.User.TokenIdentity) &&
            //    !string.IsNullOrEmpty(App.User.SessionToken))
            //{
            //    ResetTimerRefreshTokenMusicRequestsSession();
            //}
        }

        public void StopTimerRefreshTokenMusicRequestsSession()
        {
            RefreshTokenMusicRequestsSessionTimer?.Dispose();
        }

        public void ResetTimerRefreshTokenMusicRequestsSession()
        {
            RefreshTokenMusicRequestsSessionTimer = new Timer(
                 async (state) =>
                 {
                     await CallRefreshTokenSessionIdentidad();
                 },
                null,
                (int)(MusicRequestsSessionTimeoutMilis * 0.85),
                (int)(MusicRequestsSessionTimeoutMilis * 0.85)
            );
        }

        private async Task CallRefreshTokenSessionIdentidad()
        {
            try
            {
                if (App.AppIsInBackground && (DateTime.UtcNow - App.LastActionTimestamp).TotalSeconds > App.TOMBSTONING_TIMEOUT_SECONDS)
                {
                    StopTimerRefreshTokenMusicRequestsSession();
                    return;
                }

                var MusicRequestsSessionService = Mvx.IoCProvider.Resolve<IMusicRequestsSessionService>();
                //var response = await MusicRequestsSessionService.Refresco();
            }
            catch (Exception ex)
            {
                // Se ha invalidado los tokens en el MusicRequestsHandler
            }
        }

        #endregion

        #region token identidad
        public void StartTimerRefreshTokenIdentidad(int expireIn)
        {
            if (expireIn <= 0)
                return;

            if (RefreshTokenIdentidadTimer != null)
            {
                RefreshTokenIdentidadTimer.Dispose();
            }

            // Tiempo de expiración del refresco es el que nos mandan menos un cuarto del mismo
            IdentidadRefreshTimeoutMilis = expireIn * 1000;
            //if (App.User != null &&
            //    !string.IsNullOrEmpty(App.User.TokenIdentity) &&
            //    !string.IsNullOrEmpty(App.User.RefreshTokenIdentity))
            //{
            //    ResetTimerRefreshTokenIdentidad();
            //}
        }
        public void ResetTimerRefreshTokenIdentidad()
        {
            // Init the background process to update the User Tocken
            RefreshTokenIdentidadTimer?.Dispose();
            RefreshTokenIdentidadTimer = new Timer(
                 async (state) =>
                 {
                     await CallRefreshTokenIdentidad();
                 },
                null,
                 (int)(IdentidadRefreshTimeoutMilis * 0.85),
                 (int)(IdentidadRefreshTimeoutMilis * 0.85)
            );
        }
        public void StopTimerRefreshTokenIdentidad()
        {
            RefreshTokenIdentidadTimer?.Dispose();
        }

        private async Task CallRefreshTokenIdentidad()
        {
            try
            {
                if (App.AppIsInBackground && (DateTime.UtcNow - App.LastActionTimestamp).TotalSeconds > App.TOMBSTONING_TIMEOUT_SECONDS)
                {
                    StopTimerRefreshTokenIdentidad();
                    return;
                }

                //var identityRefreshService = Mvx.IoCProvider.Resolve<IRefreshService>();
                //var refreshResponse = await identityRefreshService.Refresh(App.User.RefreshTokenIdentity);

                //App.User.RefreshTokenIdentity = refreshResponse.RefreshToken;
                //App.User.TokenIdentity = refreshResponse.AccessToken;
                //App.User.TokenIdentityExpireInSeconds = refreshResponse.ExpiresIn;
                //IdentidadRefreshTimeoutMilis = refreshResponse.ExpiresIn * 1000;
                ResetTimerRefreshTokenIdentidad();
            }
            catch (Exception ex)
            {
                // Se ha invalidado los tokens en el MusicRequestsHandler
            }
        }
        #endregion
    }
}
