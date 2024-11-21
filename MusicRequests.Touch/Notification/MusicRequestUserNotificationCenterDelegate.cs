
using MusicRequests.Core;
using MusicRequests.Core.Managers;
using MvvmCross;
using UserNotifications;

namespace MusicRequests.Touch.Notification
{
    public class MusicRequestUserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public MusicRequestUserNotificationCenterDelegate()
        {
        }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            //completionHandler(IndigitallXamarin.Indigitall.WillPresentNotification);
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            //Console.WriteLine("DidReceiveNotificationResponse push: " + push);
            //Console.WriteLine("DidReceiveNotificationResponse action: " + action.App);

            var managerDeepLinking = Mvx.IoCProvider.Resolve<INotificationsDeepLinkManager>();
            try
            {
                //managerDeepLinking.CrearTransaccionDeeplink(NSUrl.FromString(push.Action.App), push?.Data);

                if (true) //User dentro de la sesion App.User != null)
                {
                    managerDeepLinking.ProcesaDeeplinkPendiente();
                }
                else
                {
                    managerDeepLinking.ProcesaDeeplinkPendiente(false);
                }
            }
            catch (Exception ex)
            {
                //managerDeepLinking.CrearTransaccionDeeplink(new System.Uri(AppDelegate.DeeplinkAppUriScheme + "://notification/homeresumen?params="), push?.Data);
                managerDeepLinking.ProcesaDeeplinkPendiente();
            }
        }

    }
}

