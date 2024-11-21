using Foundation;
using MusicRequests.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace MusicRequests.Touch.Services
{
    public class PostNotificationPermissionService : IPostNotificationPermissionService
    {
        public async Task<bool> CheckAndRequestPermissions()
        {
            // Lo gestiona la propia SDK de Indigitall
            return true;
        }
    }
}