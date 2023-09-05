namespace HabilitadorGraduaciones.Core.Entities.Bases
{
    public class BaseEntity
    {
        public string Matricula { get; set; } = string.Empty;
        public string ClaveProgramaAcademico { get; set; } = string.Empty;
        public string ClaveCarrera { get; set; } = string.Empty;
        public bool Result { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}