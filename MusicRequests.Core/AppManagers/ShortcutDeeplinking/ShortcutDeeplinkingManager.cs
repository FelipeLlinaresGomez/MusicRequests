
using System;
using System.Threading.Tasks;
using MusicRequests.Core.Presenters;
using MusicRequests.Core.ViewModels;
using MvvmCross;
using MvvmCross.Navigation;

namespace MusicRequests.Core.Managers
{
    public class ShortcutDeeplinkingManager : IShortcutDeeplinkingManager
    {
        private class ShortcutDeeplinkingTransaction
        {
            public Type Viewmodel { get; set; }
        }

        private ShortcutDeeplinkingTransaction PendingTransaction { get; set; }
        private bool appMuerta = false;

        protected readonly IMvxNavigationService _navigationService = null;

        public ShortcutDeeplinkingManager()
        {
            _navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
        }

        public void Reset()
        {
            PendingTransaction = null;
        }

        public void CrearTransaccionShortcutDeeplink(Type viewModel)
        {
            PendingTransaction = new ShortcutDeeplinkingTransaction
            {
                Viewmodel = viewModel
            };
        }

        public bool ShortcutDeeplinkPendiente()
        {
            return PendingTransaction != null;
        }

        public async Task ProcesaShortcutDeeplinkPendiente(bool borrarTrasProcesar = true)
        {
            if (ShortcutDeeplinkPendiente())
            {
                //if (App.User != null)
                //{
                //    await NavegacionUsuarioActivoAsync();
                //}
                //else
                //{
                //    await NavegacionUsuarioNoActivoAsync();
                //}

                // Vaciamos los datos ya utilizados para evitar reutilizarse
                if (borrarTrasProcesar)
                {
                    Reset();
                }
            }
        }

        private async Task NavegacionUsuarioNoActivoAsync()
        {
            // Navegamos siempre a home publica, y que el mismo navegue al login
            // El deeplink se deberia guardar en este caso, para reanudarlo luego en el interior de la app
            await Task.Delay(10);
            appMuerta = true;
            //await _navigationService.Navigate<LoginViewModel>();
        }

        private async Task NavegacionUsuarioActivoAsync()
        {
            if (PendingTransaction.Viewmodel != null)
            {
                var viewmodelANavegar = PendingTransaction.Viewmodel;
                Reset();

                if (!appMuerta)
                {
                    _navigationService.ChangePresentation(new RootViewPresentationHint<HomeViewModel>());
                }

                //if (App.Usuario.IsBizumActivado())
                //{
                //    await _navigationService.Navigate(viewmodelANavegar);
                //}
                //else
                //{
                //    var args = new BizumActivarBizumViewModelArgs()
                //    {
                //        CloseSession = false
                //    };
                //    await _navigationService.Navigate<BizumActivarBizumViewModel, BizumActivarBizumViewModelArgs>(args);
                //}
            }
            else
            {
                await _navigationService.Navigate<HomeViewModel>();
            }
            appMuerta = false;
        }
    }
}
