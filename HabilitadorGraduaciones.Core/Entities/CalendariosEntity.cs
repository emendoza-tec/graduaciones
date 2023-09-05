using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class CalendariosEntity
    {
        public string ClaveCampus { get; set; }
        public string LinkProspecto { get; set; }
        public string LinkCandidato { get; set; }
        public string IdUsuario { get; set; }
    }

    public class CalendarioEntity : BaseEntity
    {
        public string CalendarioId { get; set; }
        public string ClaveCampus { get; set; }
        public string Campus { get; set; }
        public string LinkProspecto { get; set; }
        public string LinkCandidato { get; set; }
    }
}
