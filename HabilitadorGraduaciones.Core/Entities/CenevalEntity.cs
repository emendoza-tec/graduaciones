using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class CenevalEntity : BaseEntity
    {
        public bool CumpleRequisitoCeneval { get; set; }
        public DateTime FechaExamen { get; set; }
        public string FechaRegistro { get; set; }
    }
}