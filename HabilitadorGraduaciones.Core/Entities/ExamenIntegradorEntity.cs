using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class ExamenIntegradorEntity : BaseEntity
    {
        public string PeriodoGraduacion { get; set; }
        public string Nivel { get; set; }
        public string NombreRequisito { get; set; }
        public string Estatus { get; set; }
        public string FechaExamen { get; set; }
        public DateTime FechaExamenDate { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public Boolean Aplica { get; set; }
        public bool UpdateFlag { get; set; }
    }
}