using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicRequests.Core.Services
{
    public interface IPostNotificationPermissionService
    {
        Task<bool> CheckAndRequestPermissions();
    }
}
