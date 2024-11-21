using System;
namespace MusicRequests.Core.Models
{
    public class MusicRequestBackendException : Exception
    {
        public MusicRequestBackendException() { }

        public string FriendlyMessage { get; set; }
        public string Code { get; set; }

        public bool Logout { get; set; }
    }
}