using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class PrestamoEducativoEntity : BaseEntity
    {
        public string EstatusContrato { get; set; }
        public bool TienePrestamo { get; set; }
    }
}
