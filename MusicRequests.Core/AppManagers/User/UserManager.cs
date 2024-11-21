using MusicRequests.Core.Services;
using MusicRequests.Core.Helpers;
using MvvmCross;
using MusicRequests.Core.Models;

namespace MusicRequests.Core.Managers
{
    public class UserManager : IUserManager
    {
        #region GetUser

        public void GetUser()
        {
            var _fileService = Mvx.IoCProvider.Resolve<IFileService>();
            App.RememberUser = _fileService.ReadTextFile<RememberUserModel>(Files.FILE_USER);
        }

        #endregion

        #region ForgetUser

        public void ForgetUser()
        {
            var _fileService = Mvx.IoCProvider.Resolve<IFileService>();
            _fileService.DeleteFile(Files.FILE_USER, false);
            App.RememberUser = null;
        }

        #endregion

        #region UpdateRememberUser

        public void UpdateRememberUser(RememberUserModel user)
        {
            var _fileService = Mvx.IoCProvider.Resolve<IFileService>();
            _fileService.WriteTextFile<RememberUserModel>(Files.FILE_USER, user);
        }

        #endregion
    }
}