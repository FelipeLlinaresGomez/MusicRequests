using MvvmCross.ViewModels;
using System.Reflection;
using MvvmCross.Localization;
using MusicRequests.Core.Models;
using MusicRequests.Core.Services;
using MusicRequests.Droid.Services;
using MusicRequests.Core;
using MusicRequests.Droid.Presenters;
using Acr.UserDialogs;
using MusicRequests.Core.Services.BarcodeService;
using Serilog;
using MvvmCross;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.IoC;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using Google.Android.Material.Navigation;
using Google.Android.Material.FloatingActionButton;
using AndroidX.DrawerLayout.Widget;
using MvvmCross.Plugin;
using Plugin.CurrentActivity;
using MusicRequests.Core.Services.Image;
using MusicRequests.Droid.Services.Image;
using MusicRequests.Droid.Services.DeviceSecurityCheck;

namespace MusicRequests.Droid
{
    public class Setup : MvxAndroidSetup<App>
	{
        protected override IMvxApplication CreateMvxApplication(IMvxIoCProvider iocProvider)
        {
            var app = new App();
            app.InitializeOsType(TipoDispositivo.ANDP);
            return app;
        }

        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            // serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.AndroidLog()
                // add more sinks here
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            InitServices();

            Android.Util.Log.Info("MusicRequest", "MvvmCross Platform services initialization");

            base.InitializeFirstChance(iocProvider);
        }

        protected override void RegisterDefaultSetupDependencies(IMvxIoCProvider iocProvider)
        {
            //Bug check: github.com/MvvmCross/MvvmCross/issues/3572
            var nm = CreateViewToViewModelNaming();
            Mvx.IoCProvider.RegisterSingleton(nm);

            base.RegisterDefaultSetupDependencies(iocProvider);
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Visibility.Platforms.Android.Plugin>();
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Json.Plugin>();
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.ResourceLoader.Platforms.Android.Plugin>();
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Messenger.Plugin>();
        }

        /// <summary>
        /// This is very important to override. The default view presenter does not know how to show fragments!
        /// </summary>
        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MusicRequestAndroidViewPresenter(AndroidViewAssemblies);
        }

        private void InitServices()
		{
			Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ILocalizationService, LocalizationService>();
			Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IWebService, WebService>();
			Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IPermissionsService, PermissionsService>();
			Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDeepLinkingService, DeepLinkingService>();
			Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IImageService, ImageService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMediaFileExtensions, MediaFileExtensions>();

			Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ILlamadaService, LlamadaService>();
			Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IShareService, ShareService>();
			Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ITouchIdService, TouchIdService>();
			Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ISecureStorageService, SecureStorageService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDocumentService, DocumentService>();
			Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IBarcodeService, BarcodeService>();
			Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IPostNotificationPermissionService, PostNotificationPermissionService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDeviceSecurityCheckService, DeviceSecurityCheckService>();

			HttpClientHelper.PlatformHttpClientHandler = new PlatformHttpClientHandler();
            var currentact = CrossCurrentActivity.Current.Activity;
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
        }

        protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
		{
			typeof(NavigationView).Assembly,
			typeof(FloatingActionButton).Assembly,
			typeof(AndroidX.AppCompat.Widget.Toolbar).Assembly,
			typeof(DrawerLayout).Assembly,
		};

		protected override IEnumerable<Assembly> ValueConverterAssemblies {
			get {
				var toReturn = base.ValueConverterAssemblies.ToList();
				toReturn.Add(typeof(MvxLanguageConverter).Assembly); // MvxLang converter to be used in the layouts
				return toReturn;
			}
		}
        
        protected override void FillTargetFactories(MvvmCross.Binding.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
        {
            //MvxAppCompatSetupHelper.FillTargetFactories(registry);
            base.FillTargetFactories(registry);
        }

        public static bool IsInitialized { get; set; }

    }
}
