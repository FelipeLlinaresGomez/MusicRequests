using System;
using System.Collections.Generic;

namespace MusicRequests.Core.Models
{
    public class DeviceSecurityCheckException : Exception
    {
        public DeviceSecurityCheckException(IEnumerable<string> causas)
        {
            Causas = causas;
        }

        public IEnumerable<string> Causas { get; private set; }
    }
}
