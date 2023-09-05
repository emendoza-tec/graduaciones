using System.ComponentModel;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class SabanaDto
    {
        public string Matricula { get; set; }
        [Description("Nombre Completo")]
        public string NombreCompleto { get; set; }
        [Description("Clave Programa Académico")]
        public string ClaveProgramaAcademico { get; set; }
        [Description("Concentración Uno")]
        public string ConcentracionUno { get; set; }
        [Description("Concentración Dos")]
        public string ConcentracionDos { get; set; }
        [Description("Concentración Tres")]
        public string ConcentracionTres { get; set; }
        [Description("ULEAD")]
        public string Ulead { get; set; }
        [Description("Diploma Internacional")]
        public string DiplomaInternacional { get; set; }
        public string Genero { get; set; }
        public string Nacionalidad { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Periodo { get; set; }
        public string Campus { get; set; }
        public string NivelAcademico { get; set; }
        public string CreditosPlan { get; set; }
        public string CreditosPendientes { get; set; }
        public string CreditosAcreditados { get; set; }
        public string CreditosFaltantes { get; set; }
        public string CreditosPeriodo { get; set; }
        public string SemanasTec { get; set; }
        public string ServicioSocialHt { get; set; }
        public string ServicioSocialEstatus { get; set; }
        public string ExamenIngles { get; set; }
        public string ExamenInglesEstatus { get; set; }
        public string ExamenInglesFecha { get; set; }
        public string ExamenInglesPuntaje { get; set; }
        public string Ceneval { get; set; }
        public string CenevalEstatus { get; set; }
        public string ExamenIntegrador { get; set; }
        public string NivelIdiomaRequerido { get; set; }
        public string IdiomaDistEsp { get; set; }
        public string CreditosCursadosExtranjero { get; set; }
        public string Promedio { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Shadegr { get; set; }
        public string PeriodoCeremonia { get; set; }
    }
}
