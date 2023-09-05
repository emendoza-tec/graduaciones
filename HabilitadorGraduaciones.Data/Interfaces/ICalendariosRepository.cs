using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface ICalendariosRepository
    {
        public Task<CalendarioDto> GetCalendarioAlumno(CalendarioEntity entity);
        public Task<CalendariosDto> GetCalendarios(CalendariosDto entity);
        public Task<BaseOutDto> ModificarCalendarios(List<CalendariosEntity> guardarCalendarios);
    }
}
