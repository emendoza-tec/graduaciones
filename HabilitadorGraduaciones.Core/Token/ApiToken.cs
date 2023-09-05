namespace HabilitadorGraduaciones.Core.Token
{
    public class ApiToken
    {
        public string OAuthToken { get; set; }
        public string JwtToken { get; set; }
        public string IssClaim { get; set; }
        public string RutaOAuth { get; set; }
        public string RutaJWT { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string Matricula { get; set; }
    }
}
