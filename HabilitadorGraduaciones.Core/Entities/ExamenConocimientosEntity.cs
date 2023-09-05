namespace HabilitadorGraduaciones.Core.Entities
{
    public class ExamenConocimientosEntity
    {
        public int IdTipoExamen { get; set; }
        public string DescripcionExamen { get; set; }
        public string TituloExamen { get; set; }
        public bool CumpleRequisito { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaExamen { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool EsRequisito { get; set; }
        public bool Result { get; set; }
    }
}
