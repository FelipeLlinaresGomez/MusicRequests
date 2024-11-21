using MusicRequests.Core;
using MusicRequests.Core.Managers;
using MusicRequests.Core.ViewModels;
using MusicRequests.Touch.Services;
using MusicRequests.Touch.Styles;
using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.ViewModels;

namespace MusicRequests.Touch
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
        public static string INDeviceId { get; private set; }
        //Propiedad para capturar el token del servicio de notificaciones de Apple y pasarlo a Azure a traves de la interfaz DeviceNotificationService
        public static NSData DeviceToken { get; private set; }

        public bool RestrictRotation
        {
            get;
            set;
        }

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            base.FinishedLaunching(app, options);;

            var appearance = new UINavigationBarAppearance();
            appearance.ConfigureWithOpaqueBackground();
            appearance.BackgroundColor = Colors.Primary;
            appearance.ShadowColor = UIColor.Clear;

            UINavigationBar.Appearance.BarTintColor = UIColor.White;
            UINavigationBar.Appearance.ScrollEdgeAppearance = appearance;
            UINavigationBar.Appearance.CompactAppearance = appearance;
            UINavigationBar.Appearance.StandardAppearance = appearance;

            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);

            return true;
        }

        protected override void RunAppStart(object hint = null)
        {
            //Workaround para evitar flash negro en transition despues del splash.
            Window.RootViewController = new UIViewController();
            Window.MakeKeyAndVisible();
            if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
            {
                startup.StartAsync(GetAppStartHint(hint)).ConfigureAwait(false);
            }
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            if (this.RestrictRotation)
                return UIInterfaceOrientationMask.Portrait;
            else
                return UIInterfaceOrientationMask.All;
        }

        public override void OnActivated(UIApplication application)
        {
            App.AppIsInBackground = false;
        }

        public override void DidEnterBackground(UIApplication application)
        {
            base.DidEnterBackground(application);
            App.AppIsInBackground = true;
            App.RefreshTombstoningTimer();
        }

        public static string DeeplinkAppUriScheme
        {
            get
            {
                if (NSBundle.MainBundle.BundleIdentifier == "es.cecabank.ealia2085appstore")
                {
                    return "music.requests.prod";
                }
                else
                {
                    return "music.requests.pruebas";
                }
            }
        }

        #region Shortcuts with ForceTouch

        public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
        {
            // Perform action
            completionHandler(HandleShortcutItem(shortcutItem));
        }

        public UIApplicationShortcutItem LaunchedShortcutItem { get; set; }

        public bool HandleShortcutItem(UIApplicationShortcutItem shortcutItem)
        {
            var handled = false;

            // Anything to process?
            if (shortcutItem == null) return false;

            // Take action based on the shortcut type
            var managerShortcutDeepLinking = Mvx.IoCProvider.Resolve<IShortcutDeeplinkingManager>();

            Type viewModel = typeof(HomeViewModel);
            switch (shortcutItem.Type)
            {
                case ForceTouchShortcutUtils.ShortcutPedirCancion:
                    //viewModel = typeof(PedirCancionView);
                    break;
                case ForceTouchShortcutUtils.ShortcutAceptarCancion:
                    //viewModel = typeof(AceptarCancionView);
                    break;
                case ForceTouchShortcutUtils.ShortcutUltimaActividad:
                    //viewModel = typeof(UltimosActividadView);
                    break;
            }

            managerShortcutDeepLinking.CrearTransaccionShortcutDeeplink(viewModel);
            managerShortcutDeepLinking.ProcesaShortcutDeeplinkPendiente(false);
            handled = true;

            // Return results
            return handled;
        }

        #endregion
    }
}
