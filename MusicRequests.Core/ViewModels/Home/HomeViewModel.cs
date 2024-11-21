using MusicRequests.Core.Managers;
using System;
using System.Threading.Tasks;
namespace MusicRequests.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IShortcutDeeplinkingManager _shortcutDeeplinkingManager;
        private readonly INotificationsDeepLinkManager _notificationsDeepLinkManager;

        public HomeViewModel(IShortcutDeeplinkingManager shortcutDeeplinkingManager,
                             INotificationsDeepLinkManager notificationsDeepLinkManager)
        {
            _shortcutDeeplinkingManager = shortcutDeeplinkingManager;
            _notificationsDeepLinkManager = notificationsDeepLinkManager;
        }

        #region ViewModel Lyfecycle from MvvmCross

        public override async Task Initialize()
        {
            try
            {
                await base.Initialize();

                Cargando = true;

                try
                {
                    //if (App.RememberUser != null)
                    //{
                    //    var dispositivoActual = Mvx.IoCProvider.Resolve<IDeviceService>().GetIdentifier();
                    //    var dispositivosRegistrados = await _notificationsBizumManager.GetDispositivosPorUsuario();
                    //    if (!dispositivosRegistrados.Any(x => x == dispositivoActual))
                    //    {
                    //        bool alta = await _notificationsBizumManager.RegistroDispositivo();
                    //    }
                    //}
                    //else
                    //{
                    //    bool baja = await _notificationsBizumManager.DesregistroDispositivo();
                    //}

                    //if (App.User != null)
                    //{
                    //    // Si tengo bizum activo comprobamos si estoy suscrito a alertas
                    //    //var dispositivoActual = Mvx.IoCProvider.Resolve<IDeviceService>().GetIdentifier();
                    //    //var suscripciones = await _notificationsBizumManager.ListadoSuscripcionesActivasUsuario();
                    //    //if (!suscripciones.Any(e => e.Evento == TipoEventoNotificaciones.Bizum && e.IdDispositivo == dispositivoActual))
                    //    //{
                    //    //    bool altaEvento = await _notificationsBizumManager.AltaAlertasBizum();
                    //    //}
                    //}
                }
                catch (Exception e)
                {
                    // No notificar al usuario
                }
            }
            catch (Exception ex)
            {
                await HandleMusicRequestsError(ex, true);
            }
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();
            try
            {
                if (_shortcutDeeplinkingManager.ShortcutDeeplinkPendiente())
                {
                    await _shortcutDeeplinkingManager.ProcesaShortcutDeeplinkPendiente(true);
                }
                else if (_notificationsDeepLinkManager.DeeplinkPendiente())
                {
                    await _notificationsDeepLinkManager.ProcesaDeeplinkPendiente();
                }
            }
            catch (Exception ex)
            {
                await this.HandleMusicRequestsError(ex);
            }
            finally
            {
                Cargando = false;
            }
        }

        #endregion
    }
}