using System;
using System.Threading.Tasks;
using MusicRequests.Core.Managers;

namespace MusicRequests.Core.ViewModels
{
    public class NotificationsDeeplinkViewModel : BaseViewModel
    {
        readonly INotificationsDeepLinkManager _notificationsDeepLinkManager;

        public NotificationsDeeplinkViewModel(INotificationsDeepLinkManager notificationsDeepLinkManager)
        {
            _notificationsDeepLinkManager = notificationsDeepLinkManager;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            if (!App.IsAndroid())
            {
                throw new NotSupportedException("Este ViewModel solo debe ser usado por Android");
            }
        }

        public async Task ProcessDeepLinkData(Uri url, string payloadData)
        {
            await _notificationsDeepLinkManager.CrearTransaccionDeeplink(url, payloadData);

            // Creamos un falso retraso para evitar parpadeos
            await Task.Delay(500);
            await _notificationsDeepLinkManager.ProcesaDeeplinkPendiente(false);
        }

    }
}

