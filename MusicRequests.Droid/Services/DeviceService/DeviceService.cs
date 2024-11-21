//using MusicRequests.Core.Services;
//using Android.Util;
//using MusicRequests.Core;
//using MvvmCross.Platforms.Android;
//using MvvmCross;
//using Microsoft.Maui.Devices;

//namespace MusicRequests.Droid.Services
//{
//    public class DeviceService : IDeviceService
//	{
//        public string GetIdentifier()
//        {
//            return Android.Provider.Settings.Secure.GetString(Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
//        }

//        public string GetPhoneNumberIdentifier()
//        {
//            var numeroTelefono = App.User.Telefono.Replace(" ", "");
//            return numeroTelefono;
//        }

//		public PhoneModel GetPhoneInfo()
//		{
//			return new PhoneModel()
//			{
//				Manufacturer = DeviceInfo.Manufacturer,
//				Model = DeviceInfo.Model,
//				Type = IsTabletDevice() ? "Tablet" : "Movil",
//				DeviceOSVersion = DeviceInfo.Version,
//				DeviceOSVersionString = DeviceInfo.VersionString
//			};
//		}
        
//        public ScreenSize GetScreenSize()
//		{
//			var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>();
//			return new ScreenSize()
//			{
//				Width = (double)activity.Activity.Resources.DisplayMetrics.WidthPixels / (double)activity.Activity.Resources.DisplayMetrics.Density,
//				Height = (double)activity.Activity.Resources.DisplayMetrics.HeightPixels / (double)activity.Activity.Resources.DisplayMetrics.Density,
//				Density = (double)activity.Activity.Resources.DisplayMetrics.Density
//			};
//		}


//		private bool IsTabletDevice()
//		{
//			var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>();
//			// Compute screen size
//			DisplayMetrics dm = activity.Activity.Resources.DisplayMetrics;
//			float screenWidth = dm.WidthPixels / dm.Xdpi;
//			float screenHeight = dm.HeightPixels / dm.Ydpi;
//			double size = Math.Sqrt(Math.Pow(screenWidth, 2) + Math.Pow(screenHeight, 2));

//			//
//			// Tablet devices should have a screen size greater than 6 inches
//			return size >= 6;
//		}
//	}
//}