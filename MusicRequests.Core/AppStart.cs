using System;
using System.Threading.Tasks;
using MusicRequests.Core.ViewModels;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MusicRequests.Core
{
    public class AppStart : MvxAppStart
    {
        public AppStart(IMvxApplication app, IMvxNavigationService mvxNavigationService)
               : base(app, mvxNavigationService)
        {
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            var parameters = new LoginViewModelArgs
            {
                EsArranqueApp = true
            };
            return NavigationService.Navigate<LoginViewModel, LoginViewModelArgs>(parameters);
        }
    }
}
