using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class CalendariosDto : BaseOutDto
    {
        public List<CalendarioDto> Calendarios { get; set; }
    }

    public class CalendarioDto : BaseOutDto
    {
        public string CalendarioId { get; set; }
        public string ClaveCampus { get; set; }
        public string Campus { get; set; }
        public string LinkProspecto { get; set; }
        public string LinkCandidato { get; set; }
    }
}
