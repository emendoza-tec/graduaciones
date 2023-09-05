using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface ICalendariosService
    {
        public Task<CalendarioDto> GetCalendarioAlumno(CalendarioEntity entity);
        public Task<CalendariosDto> GetCalendarios(CalendariosDto entity);
        public Task<BaseOutDto> GuardarConfiguracionCalendarios(List<CalendariosEntity> configuracion);
    }
}
