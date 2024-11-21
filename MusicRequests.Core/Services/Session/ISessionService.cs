using System;
namespace MusicRequests.Core.Services
{
	public enum AccessType
	{
		HCE,
		Sticker
	}

	public interface ISessionService
	{
		/// <summary>
		/// Validamos si el usuario está logado 
		/// </summary>
		/// <returns><c>true</c>, if user authenticated was ised, <c>false</c> otherwise.</returns>
		bool IsUserAuthenticated();

		/// <summary>
		/// Comprobamos que el tiempo de sesión haya expirado o no
		/// </summary>
		/// <returns><c>true</c>, if session expired was ised, <c>false</c> otherwise.</returns>
		bool IsSessionExpired();

		/// <summary>
		/// Guarda la última vez que el usuario accede a la API
		/// </summary>
		void SaveUserLastAccess();

		/// <summary>
		/// Guarda el tipo de acceso que ha realizado el usuario
		/// </summary>
		void SetAccessType(AccessType tipoAcceso);

		/// <summary>
		/// Comprueba si el usuario ha iniciado sesión de HCE 
		/// </summary>
		/// <returns><c>true</c>, if user logged as hce was ised, <c>false</c> otherwise.</returns>
		bool IsHCESession();

		/// <summary>
		/// Limpia la sesión del usuario
		/// </summary>
		void CleanSessionData();

		void StartTimerRefreshTokenIdentidad(int expiresIn);
		void StartTimerRefrescoSesionMusicRequests(int expiresIn);
		void StopTimerRefreshTokenIdentidad();
		void StopTimerRefreshTokenMusicRequestsSession();
		void ResetTimerRefreshTokenIdentidad();
		void ResetTimerRefreshTokenMusicRequestsSession();
	}
}
