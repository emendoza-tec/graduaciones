using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class PeriodoGraduacionEntity : BaseEntity
    {
        public int Id { get; set; }
        public int PeriodoGraduacion { get; set; }
        public string DescPeriodoGraduacion { get; set; }
    }
}
