
using Android.Content;
using MusicRequests.Core.Services;
using MvvmCross.Platforms.Android;
using MvvmCross;

namespace MusicRequests.Droid.Services
{

    public class DeepLinkingService : IDeepLinkingService
    {
        public void OpenAppStoreToUpdate(string url)
        {
            string packageName = string.Empty;
            if (ApiSettings.Entorno == Entornos.ProduccionEntorno)
            {
                packageName = "es.MusicRequests";
                OpenAppStoreToUpdateNoAppClose(packageName, true);
            }
            else
            {
                packageName = "es.MusicRequests.v2.piloto";
                OpenAppStoreToUpdateNoAppClose(packageName);
            }
        }

        public void OpenStoreToReview()
        {
            string packageName = "es.MusicRequests";

            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            try
            {
                activity.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + packageName)));
            }
            catch (ActivityNotFoundException anfe)
            {
                activity.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + packageName)));
            }
            catch
            {
            }
        }

        private void OpenAppStoreToUpdateNoAppClose(string appPackageName, bool aimToStore = false)
        {
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            if (aimToStore)
            {
                try
                {
                    activity.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + appPackageName)));
                }
                catch (ActivityNotFoundException anfe)
                {
                    activity.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + appPackageName)));
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    Intent i = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://install.appcenter.ms/apps"));
                    i.AddFlags(ActivityFlags.NewTask);
                    activity.StartActivity(i);
                }
                catch
                {

                }
            }
        }
    }
}