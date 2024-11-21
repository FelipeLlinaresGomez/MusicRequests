using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using MusicRequests.Core.Models;
using MusicRequests.Core.Services;
using Newtonsoft.Json;
using UIKit;

namespace MusicRequests.Touch.Services
{
    public class DeepLinkingService : IDeepLinkingService
    {
        string GetStoreUrl()
        {
            if (ApiSettings.Entorno == Entornos.ProduccionEntorno)
            {
                return "itms-apps://itunes.apple.com/es/app/music-requests/id1080621381?mt=8";
            }
            else
            {
                return @"https://rink.hockeyapp.net";
            }
        }

        public void OpenAppStoreToUpdate(string url)
        {
            UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(url));
        }

        public void OpenStoreToReview()
        {
            UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(GetStoreUrl()));
        }
    }
}
