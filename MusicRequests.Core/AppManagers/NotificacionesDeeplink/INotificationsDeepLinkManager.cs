using System;
using System.Threading.Tasks;

namespace MusicRequests.Core.Managers
{
    public interface INotificationsDeepLinkManager
    {
        void Reset();
        bool DeeplinkPendiente();
        Task CrearTransaccionDeeplink(Uri uri, string payloadData);
        Task ProcesaDeeplinkPendiente(bool borrarTrasProcesar = true);
    }
}
