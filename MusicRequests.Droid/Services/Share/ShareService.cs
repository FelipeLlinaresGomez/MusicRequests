using Android.Content;

using MusicRequests.Core.Services;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MusicRequests.Droid.Services
{
    public class ShareService : IShareService
    {
        public void ShareContactoEnlace(string name, string number)
        {
            var myIntent = new Intent(Android.Content.Intent.ActionSend);
            myIntent.SetType("text/plain");
            myIntent.PutExtra(Intent.ExtraText, name+" "+number);
            Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.StartActivity(Intent.CreateChooser(myIntent, ""));
        }
    }
}