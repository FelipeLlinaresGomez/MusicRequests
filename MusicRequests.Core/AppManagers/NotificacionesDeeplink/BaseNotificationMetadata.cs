using System;
namespace MusicRequests.Core.Managers
{
    public class BaseNotificationMetadata
    {
        public NotificationAction Action { get; set; }

        public DateTime SentDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool IsSilent { get; set; }

        public string Metadata { get; set; }

        public string IdTransaccion { get; set; }
    }

    public enum NotificationAction
    {
        NONE, // default
        SONG_SENT, // CANCION ENVIADA
        SONG_RECEIVED // CANCION RECIBIDO
    }
}
