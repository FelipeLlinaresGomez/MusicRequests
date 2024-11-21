using MusicRequests.Core.Services;
using Android.Content;
using MvvmCross.Platforms.Android;
using MvvmCross;

namespace MusicRequests.Droid.Services
{
    public class LlamadaService : ILlamadaService
    {
        public void MarcarNumTelefono(string numeroTelefono)
        {
            var uri = Android.Net.Uri.Parse("tel:" + numeroTelefono);
            var intent = new Intent(Intent.ActionDial, uri);
            Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.StartActivity(intent);
        }
    }
}