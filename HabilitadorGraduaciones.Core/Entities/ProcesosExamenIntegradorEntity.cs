namespace HabilitadorGraduaciones.Core.Entities
{
    public class ProcesosExamenIntegradorEntity
    {
        public List<ExamenIntegradorEntity> ExamenesIntegradorNuevos { get; set; }
        public List<ExamenIntegradorEntity> ExamenesIntegradorAModificar { get; set; }
        public List<ExamenIntegradorEntity> ExamenesIntegradorNcAScYNP { get; set; }
        public List<ExamenIntegradorEntity> ExamenesIntegradorScANcYNP { get; set; }
        public List<ExamenIntegradorEntity> ExamenesIntegradorFormatoInvalido { get; set; }
        public List<ExamenIntegradorEntity> ExamenesIntegradorNumeroInvalido { get; set; }
        public List<ExamenIntegradorEntity> ExamenesIntegradorAnioInvalido { get; set; }
        public List<ExamenIntegradorEntity> ExamenesIntegradorPeriodoInvalido { get; set; }
        public List<ExamenIntegradorEntity> ExamenesIntegradorFechaInvalida { get; set; }
        public List<ExamenIntegradorEntity> ExamenesIntegradorErrorFiltro { get; set; }
    }
}