using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class UsuarioDto : BaseOutDto
    {
        public UsuarioDto()
        {
            Matricula = string.Empty;
            Nombre = string.Empty;
            ApeidoPaterno = string.Empty;
            ApeidoMaterno = string.Empty;
            Curp = string.Empty;
            Correo = string.Empty;
            ProgramaAcademico = string.Empty;
            DiplomasAdcionales = string.Empty;
            Telefono = string.Empty;
            TelefonoCasa = string.Empty;
            Concentracion = string.Empty;
            Mentor = string.Empty;
            DirectorPrograma = string.Empty;
            CarreraId = string.Empty;
            Carrera = string.Empty;
            ClaveCampus = string.Empty;
            ClaveEstatusGraduacion = string.Empty;
        }
        public string Matricula { get; set; }
        public string Nombre { get; set; }
        public string ApeidoPaterno { get; set; }
        public string ApeidoMaterno { get; set; }
        public string Curp { get; set; }
        public string Correo { get; set; }
        public string ProgramaAcademico { get; set; }
        public string DiplomasAdcionales { get; set; }
        public string Concentracion { get; set; }
        public string Telefono { get; set; }
        public string TelefonoCasa { get; set; }
        public string Mentor { get; set; }
        public string DirectorPrograma { get; set; }
        public string CorreoMentor { get; set; }
        public string CorreoDirector { get; set; }
        public string CarreraId { get; set; }
        public string Carrera { get; set; }
        public string PeriodoActual { get; set; }
        public string NivelAcademico { get; set; }
        public string ClaveProgramaAcademico { get; set; }
        public string ClaveCampus { get; set; }
        public string PeriodoGraduacion { get; set; }
        public string ClaveEstatusGraduacion { get; set; }
        public string PeriodoTranscurridoActual { get; set; }
    }
}