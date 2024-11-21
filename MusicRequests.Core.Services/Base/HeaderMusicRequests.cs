namespace MusicRequests.Core.Services
{
    public class HeaderMusicRequests
    {
        public HeaderMusicRequests()//UsuariosInicioSesionResponse info)
        {
            //if (info != null)
            //{
            //    this.TokenIdentity = info.TokenIdentity;
            //    this.EsNegocio = info.TipoUsuario == TipoUsuario.EMPRESA;
            //    this.CodigoUsuario = info.CodigoUsuario;
            //    this.NICI = info.Nici.ToString();
            //    this.SessionToken = info.SessionToken;
            //}
        }

        public string TokenIdentity { get; set; }
        public string SessionToken { get; set; }
        public bool EsNegocio { get; set; }
        public string CodigoUsuario { get; set; }
        public string NICI { get; set; }
    }
}