using System.ComponentModel;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class SabanaEntity
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
        [Description("Genero")]
        public string Genero { get; set; }
        [Description("Nacionalidad")]
        public string Nacionalidad { get; set; }
        [Description("Teléfono")]
        public string Telefono { get; set; }
        [Description("Correo")]
        public string Correo { get; set; }
        [Description("Periodo")]
        public string Periodo { get; set; }
        [Description("Campus")]
        public string Campus { get; set; }
        [Description("Nivel Académico")]
        public string NivelAcademico { get; set; }
        [Description("Créditos Plan")]
        public string CreditosPlan { get; set; }
        [Description("Créditos Pendientes")]
        public string CreditosPendientes { get; set; }
        [Description("Créditos Acreditados")]
        public string CreditosAcreditados { get; set; }
        [Description("Créditos Faltantes")]
        public string CreditosFaltantes { get; set; }
        [Description("Créditos Periodo")]
        public string CreditosPeriodo { get; set; }
        [Description("Semanas Tec")]
        public string SemanasTec { get; set; }
        [Description("Servicio Social Horas Totales")]
        public string ServicioSocialHt { get; set; }
        [Description("Servicio Social Estatus")]
        public string ServicioSocialEstatus { get; set; }
        [Description("Examen Ingles")]
        public string ExamenIngles { get; set; }
        [Description("Examen Ingles Estatus")]
        public string ExamenInglesEstatus { get; set; }
        [Description("Examen Ingles Fecha")]
        public string ExamenInglesFecha { get; set; }
        [Description("Examen Ingles Puntaje")]
        public string ExamenInglesPuntaje { get; set; }
        [Description("Ceneval")]
        public string Ceneval { get; set; }
        [Description("Ceneval Estatus")]
        public string CenevalEstatus { get; set; }
        [Description("Examen Integrador")]
        public string ExamenIntegrador { get; set; }
        [Description("Nivel Idioma Requerido")]
        public string NivelIdiomaRequerido { get; set; }
        [Description("Idioma Distinto al Español")]
        public string IdiomaDistEsp { get; set; }
        [Description("Créditos Cursados Extranjero")]
        public int CreditosCursadosExtranjero { get; set; }
        [Description("Promedio")]
        public string Promedio { get; set; }
        [Description("Fecha Registro")]
        public DateTime FechaRegistro { get; set; }
        [Description("SHADEGR")]
        public string Shadegr { get; set; }
        [Description("Periodo Ceremonia")]
        public string PeriodoCeremonia { get; set; }
    }
}
