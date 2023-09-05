namespace HabilitadorGraduaciones.Core.Token
{
    public class EndpointInfo
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RutaOAuth { get; set; }
        public string RutaJWT { get; set; }
        public string RutaOperations { get; set; }
        public string RutaConcentraciones { get; set; }
        public string CenevalApi { get; set; }
        public string BodyApiCeneval { get; set; }
        public string ProgramaBGBApi { get; set; }
        public string BodyApiBGB { get; set; }
        public string Host { get; set; }
        public string SemanasTecApi { get; set; }
        public string BodyApiSemanasTec { get; set; }
        public string BodyApiDistinciones { get; set; }
        public string BodyApiPlanDeEstudios { get; set; }
    }
}
