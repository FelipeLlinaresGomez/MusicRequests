using System.Collections.Generic;

namespace MusicRequests.Core.Services
{
    public interface IDeviceSecurityCheckService
    {
        bool PassSecurityChecks(IList<string> failMessage);
    }
}