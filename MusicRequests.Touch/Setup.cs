using MvvmCross.ViewModels;
using System.Reflection;
using MvvmCross.Localization;
using MusicRequests.Core.Services;
using MusicRequests.Touch.Services;
using MusicRequests.Core.Models;
using MusicRequests.Core;
using MusicRequests.Core.Services.BarcodeService;
using MvvmCross.Platforms.Ios.Core;
using Serilog;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Platforms.Ios.Presenters;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using Acr.UserDialogs;
using MusicRequests.Touch.Presenters;

namespace MusicRequests.Touch
{
    public class Setup : MvxIosSetup<App>
    {
        protected override IMvxApplication CreateMvxApplication(IMvxIoCProvider iocProvider)
        {
            var app = new App();
            app.InitializeOsType(TipoDispositivo.IOSP);
            return app;
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Debug()
                .CreateLogger();
            return new SerilogLoggerFactory();
        }

        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            // We will register the services in this method
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ILocalizationService, LocalizationService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IWebService, WebService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ITouchIdService, TouchIdService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDeepLinkingService, DeepLinkingService>();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IImageService, ImageService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ILlamadaService, LlamadaService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IShareService, ShareService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ISecureStorageService, SecureStorageService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDocumentService, DocumentService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IBarcodeService, BarcodeService>();
            Mvx.IoCProvider.RegisterType<IPostNotificationPermissionService, PostNotificationPermissionService>();
            Mvx.IoCProvider.RegisterType<IDeviceSecurityCheckService, DeviceSecurityCheckService>();

            HttpClientHelper.PlatformHttpClientHandler = new PlatformHttpClientHandler();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);

            base.InitializeFirstChance(iocProvider);
        }

        protected override IMvxIosViewPresenter CreateViewPresenter()
        {
            return new MusicRequestsIosViewPresenter(ApplicationDelegate, Window);
        }

        protected override IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = base.ValueConverterAssemblies.ToList();
                toReturn.Add(typeof(MvxLanguageConverter).Assembly); // MvxLang converter to be used in the layouts
                return toReturn;
            }
        }

        public override IEnumerable<Assembly> GetPluginAssemblies()
        {
            var assemblies = base.GetPluginAssemblies().ToList();
            assemblies.Add(typeof(MvvmCross.Plugin.Visibility.Platforms.Ios.Plugin).Assembly);
            assemblies.Add(typeof(MvvmCross.Plugin.Messenger.Plugin).Assembly);
            assemblies.Add(typeof(MvvmCross.Plugin.Json.Plugin).Assembly);
            assemblies.Add(typeof(MusicRequests.Plugin.File.Platforms.Ios.Plugin).Assembly);
            return assemblies;
        }

    }
}
