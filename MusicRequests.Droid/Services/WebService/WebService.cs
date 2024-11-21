using MusicRequests.Core.Services;
using Android.Content;
using MvvmCross.Platforms.Android;
using MvvmCross;

namespace MusicRequests.Droid.Services
{
    public class WebService : IWebService
	{
		private readonly IMvxAndroidCurrentTopActivity _activity;

		public WebService ()
		{
			_activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity> ();
		}
		
		public void OpenWeb (string url)
		{
			var uri = Android.Net.Uri.Parse (url);
			var intent = new Intent (Intent.ActionView, uri);
			_activity.Activity.StartActivity (intent);
		}

	}
}

