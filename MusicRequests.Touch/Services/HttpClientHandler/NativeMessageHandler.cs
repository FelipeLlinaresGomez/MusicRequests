using MusicRequests.Touch.Services;
using Security;

namespace MusicRequests.Touch.Services
{
    public class NativeMessageHandler : HttpClientHandler
    {
        public bool DisableCaching { get; set; }
        static bool customSSLVerificationDefault = true;

        public NativeMessageHandler() : this(false, customSSLVerificationDefault) { }
        public NativeMessageHandler(bool throwOnCaptiveNetwork, bool customSSLVerification, SslProtocol? minimumSSLProtocol = null)
        {
            var configuration = NSUrlSessionConfiguration.DefaultSessionConfiguration;
            configuration.TimeoutIntervalForResource = 240;
            configuration.TimeoutIntervalForRequest = 240;

            // System.Net.ServicePointManager.SecurityProtocol provides a mechanism for specifying supported protocol types
            // for System.Net. Since iOS only provides an API for a minimum and maximum protocol we are not able to port
            // this configuration directly and instead use the specified minimum value when one is specified.
            if (minimumSSLProtocol.HasValue)
            {
                configuration.TLSMinimumSupportedProtocol = minimumSSLProtocol.Value;
            }

            DisableCaching = false;
            //ServerCertificateCustomValidationCallback = ServicePointConfiguration.ValidateServerCertficateHandle;
        }
    }
}