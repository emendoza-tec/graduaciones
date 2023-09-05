using System.ComponentModel;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class ReporteEstimadoDeGraduacionEntity
    {
        [Description("Matrícula")]
        public string Matricula { get; set; }

        [Description("Nombre Completo")]
        public string Nombre { get; set; }

        [Description("Carrera(Plan de Estudios)")]
        public string Carrera { get; set; }

        [Description("Campus al que Pertenece el Alumno")]
        public string Campus { get; set; }

        [Description("Sede")]
        public string Sede { get; set; }

        [Description("Periodo Estimado")]
        public string PeriodoEstimado { get; set; }

        [Description("Confirmación del Periodo Estimado")]
        public string Confirmacion { get; set; }

        [Description("Periodo Confirmado de Graduación")]
        public string PeriodoConfirmadoG { get; set; }

        [Description("Fecha de Confirmación o Cambio")]
        public DateTime FechaConfirmacionOCambio { get; set; }

        [Description("Motivo")]
        public string Motivo { get; set; }

        [Description("Cantidad de Cambios Realizados por el Estudiante")]
        public int CantidadCambiosRealizados { get; set; }

        [Description("Registro Asistencia Ceremonia por Titulación Periodo Intensivo")]
        public string RegistroAsistenciaPeriodoIntensivo { get; set; }

        [Description("Motivo No Asistencia a Ceremonia por Titulación Periodo Intensivo")]
        public string MotivoPeriodoIntensivo { get; set; }
    }
}
