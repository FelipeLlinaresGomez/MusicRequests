using Android.OS;
using MusicRequests.Core.Services;
using Android;
using Microsoft.Maui.ApplicationModel;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace MusicRequests.Droid.Services
{
    public class PostNotificationPermissionService : IPostNotificationPermissionService
    {
        public async Task<bool> CheckAndRequestPermissions()
        {
            // Tiramisu is Android v13
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                var status = await Permissions.CheckStatusAsync<PostNotificationsPermission>();
                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                status = await Permissions.RequestAsync<PostNotificationsPermission>();
                return status == PermissionStatus.Granted;
            }
            return true;
        }
    }

    internal class PostNotificationsPermission : BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string androidPermission, bool isRuntime)>
            {
          (Manifest.Permission.PostNotifications, true)
            }.ToArray();
    }
}