namespace HabilitadorGraduaciones.Core.Entities
{
    public class PeriodosEntity
    {
        public string PeriodoId { get; set; }
        public string Matricula { get; set; }
        public string Descripcion { get; set; }
        public bool IsRegular { get; set; }
        public int TipoPeriodo { get; set; }
        public string EstatusEjercicio { get; set; }
        public string AnioAcademico { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Estatus { get; set; }
        public string PeriodoCeremonia { get; set; }
        public decimal CreditosPeriodo { get; set; }

    }
}
