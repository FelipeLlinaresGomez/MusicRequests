using System;
using System.Threading.Tasks;

namespace MusicRequests.Core.Managers
{
    public interface IShortcutDeeplinkingManager
    {
        void Reset();
        bool ShortcutDeeplinkPendiente();
        void CrearTransaccionShortcutDeeplink(Type viewModel);
        Task ProcesaShortcutDeeplinkPendiente(bool borrarTrasProcesar = true);
    }
}
