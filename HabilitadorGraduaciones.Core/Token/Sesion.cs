namespace HabilitadorGraduaciones.Core.Token
{
    public class Sesion
    {
        public string Matricula { get; set; }
        public string OAuthToken { get; set; }
        public string JwtToken { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
    }
}
