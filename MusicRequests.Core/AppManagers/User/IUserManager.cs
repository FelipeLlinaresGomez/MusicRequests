using MusicRequests.Core.Models;

namespace MusicRequests.Core.Managers
{
    public interface IUserManager
    {
        void GetUser();
        void ForgetUser();
        void UpdateRememberUser(RememberUserModel user);
    }
}