using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class CalendariosService : ICalendariosService
    {
        private readonly ICalendariosRepository _calendariosData;
        public CalendariosService(ICalendariosRepository calendariosData)
        {
            _calendariosData = calendariosData;
        }

        public async Task<CalendarioDto> GetCalendarioAlumno(CalendarioEntity entity)
        {
            return await _calendariosData.GetCalendarioAlumno(entity);
        }
        public async Task<CalendariosDto> GetCalendarios(CalendariosDto entity)
        {
            return await _calendariosData.GetCalendarios(entity);
        }
        public async Task<BaseOutDto> GuardarConfiguracionCalendarios(List<CalendariosEntity> configuracion)
        {
            return await _calendariosData.ModificarCalendarios(configuracion);
        }
    }
}
