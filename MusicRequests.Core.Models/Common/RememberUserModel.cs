namespace MusicRequests.Core.Models
{
    public class RememberUserModel
    {
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The actual name of the user, i.e Juan Perez</value>
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets the login name
        /// </summary>
        /// <value>The name used to log in the app, according to the IsCard value</value>
        public string UserName { get; set; }

        public byte[] UserAvatar { get; set; }

        public bool IsEmpresa { get; set; }

        /// <summary>
        /// Indica el estado del accesso con touch id
        /// </summary>
        public TouchIdSelectedAccess TouchIdStatus { get; set; }

        public bool TouchID { get; set; }

        public int NICI { get; set; }
    }

    public enum TouchIdSelectedAccess
    {
        Legacy, Accepted, Refused, NotAnswered
    }
}