using MusicRequests.Core.ViewModels;
using MusicRequests.Core.Presenters;
using MusicRequests.Core.Services;
using MusicRequests.Touch.Views;
using MvvmCross;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MusicRequests.Touch.Presenters
{
    public class MusicRequestsIosViewPresenter : MvxIosViewPresenter
    {
        public MusicRequestsIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window)
        {
        }

        public override Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            bool changed;
            switch (hint)
            {
                case LogOutPresenterHint logouthint:
                    // Navegamos a root: Home publica
                    return HandleLogOutPresenterHint(logouthint);
                case RootViewPresentationHint rootHint:
                    // Navegamos a Home privada
                    return HandleRootViewPresentationHint(rootHint);
                case CloseWizardPresentationHint closeWizardHint:
                    HandleCloseWizardPresentationHint(closeWizardHint);
                    return Task.FromResult(true);
                default:
                    return base.ChangePresentation(hint);
            };
        }

        async Task<bool> HandleLogOutPresenterHint(LogOutPresenterHint hint)
        {
            var controllers = MasterNavigationController.ViewControllers.ToList();

            // Locate last view controller of given view model type
            var controllerToPop = controllers.FirstOrDefault(vc => vc.GetIMvxIosView().ViewModel.GetType() == typeof(LoginViewModel));
            if (controllerToPop != null)
            {
                MasterNavigationController.PopToViewController(controllerToPop, hint.Animated);
            }
            else
            {
                // Creamos un nuevo array con el primer elemento
                MasterNavigationController.PopToViewController(controllers.First(), hint.Animated);

                controllers = MasterNavigationController.ViewControllers.ToList();
                controllers.Remove(controllers.First());

                MasterNavigationController.ViewControllers = controllers.ToArray();
                await Show(new MvxViewModelRequest(typeof(LoginViewModel)));
            }

            return true;
        }

        async Task<bool> HandleRootViewPresentationHint(RootViewPresentationHint hint)
        {
            var controllers = MasterNavigationController.ViewControllers?.ToList();

            if (hint.NavigateBack)
            {
                // Locate last view controller of given view model type
                var controllerToPop = controllers?.FirstOrDefault(vc => vc.GetIMvxIosView()?.ViewModel?.GetType() == hint.ViewModelType);
                if (controllerToPop != null)
                {
                    var result = MasterNavigationController.PopToViewController(controllerToPop, hint.Animated);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // Locate last view controller of given view model type
                var controllerToPop = controllers?.FirstOrDefault(vc => vc?.GetIMvxIosView()?.ViewModel?.GetType() == hint.ViewModelType);
                if (controllerToPop != null)
                {

                    var result = MasterNavigationController.PopToViewController(controllerToPop, hint.Animated);

                    return true;
                }
                else
                {
                    var view = this.CreateViewControllerFor(MvxViewModelRequest.GetDefaultRequest(hint.ViewModelType));

                    if (view != null)
                    {
                        // Creamos un nuevo array con el primer elemento
                        MasterNavigationController.PopToViewController(controllers.First(), hint.Animated);

                        controllers = MasterNavigationController.ViewControllers.ToList();
                        controllers.Remove(controllers.First());

                        MasterNavigationController.ViewControllers = controllers.ToArray();
                        await Show(new MvxViewModelRequest(typeof(HomeViewModel)));
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }

        void HandleCloseWizardPresentationHint(CloseWizardPresentationHint hint)
        {
            if (MasterNavigationController.ViewControllers is { })
            {
                var viewControllers = new List<UIViewController>(MasterNavigationController.ViewControllers);

                // Limpiamos la pila de vistas con viewmodel null
                foreach (var view in MasterNavigationController.ViewControllers)
                {
                    if (((BaseViewController)view).ViewModel == null)
                    {
                        viewControllers.Remove(view);
                    }
                }

                // Recolecto todas las pantallas que son wizard
                var viewControllersToRemove = viewControllers
                    .Cast<BaseViewController>()
                    .Where(view => typeof(IBaseWizardViewModel).IsAssignableFrom(view?.ViewModel?.GetType()))
                    .ToList();

                // Borro las pantallas de Wizard
                foreach (var view in viewControllersToRemove)
                {
                    viewControllers.Remove(view);
                }

                // Actualizo la lista
                if (viewControllers.Any())
                {
                    MasterNavigationController.PopToViewController(viewControllers.Last(), hint.Animated);
                }
            }
        }

    }
}
