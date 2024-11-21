using MvvmCross.Localization;
using MusicRequests.Core.Services;
using MusicRequests.Core.Models;
using System;
using MvvmCross;
using MvvmCross.ViewModels;
using MvvmCross.IoC;
using MvvmCross.Plugin.JsonLocalization;
using MvvmCross.Views;
using MvvmCross.Plugin;
using System.Reflection;
using System.Linq;
using Microsoft.Maui.ApplicationModel;
using MvvmCross.Navigation;
using MusicRequests.Core.Models.Common;
using MusicRequests.Core.ViewModels;

namespace MusicRequests.Core
{
    public class App : MvxApplication
    {
        public static TipoDispositivo Os;

        public static RememberUserModel RememberUser;
        public static Usuario User; 

        public static string AppId;

        #region TOMBSTONING

        public static bool AppIsInBackground = false;
        public static DateTime LastActionTimestamp;
        public const int TOMBSTONING_TIMEOUT_SECONDS = 300;

        public static DateTime RefreshTombstoningTimer()
        {
            LastActionTimestamp = DateTime.UtcNow;
            return LastActionTimestamp;
        }

        #endregion

        public void InitializeOsType(TipoDispositivo _os)
        {
            Os = _os;
        }

        public static bool IsiOS()
        {
            return Os == TipoDispositivo.IOST || Os == TipoDispositivo.IOSP;
        }
        public static bool IsAndroid()
        {
            return Os == TipoDispositivo.ANDP || Os == TipoDispositivo.ANDT;
        }

        public static byte[] GetUserAvatar()
        {
            if (RememberUser != null && RememberUser.UserAvatar != null)
                return RememberUser.UserAvatar;
            return null;
            //return User?.Foto;
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);

            // Workaround https://github.com/MvvmCross/MvvmCross/issues/4347
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Messenger.Plugin>(true);
        }

        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Manager")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            // Init api services
            InitServices();

            // Init Localization
            InitializeText();

            // Iniciamos el entorno
            ApiSettings.IniciarEntornos();

            // Init Api headers
            InitializeApiHeaders();

            // Clean data in case of a new version
            CleanDataInCaseNewVersion();

            InitializeNavigation();

            InitServiciosEntornoReal();

            RegisterCustomAppStart<AppStart>();
        }

        private static void InitServices()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IEncryptionService, EncryptionService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMusicRequestsSessionHandler, MusicRequestsSessionHandler>();

            // Servicios Refit para MusicRequests
            Mvx.IoCProvider.RegisterType<IMusicRequestsSessionService>
                (() => HttpClientHelper.CreateClient<IMusicRequestsSessionService>(Mvx.IoCProvider.Resolve<IMusicRequestsSessionHandler>()));

            //Mvx.IoCProvider.RegisterType<IRefreshService>
            //    (() => HttpClientHelper.CreateClient<IRefreshService>(Mvx.IoCProvider.Resolve<IMusicRequestsSessionHandler>()));
        }

        private void InitializeText()
        {
            var builder = new TextProviderBuilder();
            Mvx.IoCProvider.RegisterSingleton<IMvxTextProviderBuilder>(builder);
            Mvx.IoCProvider.RegisterSingleton<IMvxTextProvider>(builder.TextProvider);
        }

        public static void InitializeApiHeaders()
        {
            AppId = "MusicRequests";

            var headers = new HeaderModel()
            {
                Canal = "MOV",
                Dispositivo = Enum.GetName(typeof(TipoDispositivo), Os),
                Idioma = "ES",
                AppID = "MusicRequests",
                Version = VersionTracking.CurrentVersion
            };

            // Init our cancellation token to cancel any possible api Task
            ApiSettings.CancelTokenSource = new System.Threading.CancellationTokenSource();

            // Init HttpClient header values
            HttpClientHelper.GetHttpClient().AddHeaders(headers);
        }

        void InitializeNavigation()
        {
            IMvxViewModelLocatorCollection locatorCollection = this;
            IMvxViewModelLoader viewModelLoader = new MvxViewModelLoader(locatorCollection);
            IMvxViewDispatcher dispatcher = Mvx.IoCProvider.Resolve<IMvxViewDispatcher>();

            var service = new MvxNavigationServiceCustom(viewModelLoader, dispatcher, Mvx.IoCProvider);

            Mvx.IoCProvider.RegisterSingleton<IMvxNavigationServiceCustom>(service);
        }

        private void CleanDataInCaseNewVersion()
        {

            var versionService = Mvx.IoCProvider.Resolve<IVersionTrackerService>();
            versionService.Track();
            if (versionService.IsFirstLaunchForVersion)
            {
                // TODO: Delete all the previous data saved
            }

        }

        public static void InitServiciosEntornoReal()
        {
            var fullAssembly = typeof(App).GetTypeInfo().Assembly
                .ExceptionSafeGetTypes()
                .Select(t => t.GetTypeInfo())
                .Where(t => !t.IsAbstract)
                .Where(t => t.DeclaredConstructors.Any(c => !c.IsStatic && c.IsPublic))
                .Select(t => t.AsType());

            fullAssembly
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            InitServices();
        }
    }
}
