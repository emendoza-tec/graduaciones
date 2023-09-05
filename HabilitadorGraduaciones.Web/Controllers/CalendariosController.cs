using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendariosController : ControllerBase
    {
        private readonly ICalendariosService _calendariosService;

        public CalendariosController(ICalendariosService calendariosService)
        {
            _calendariosService = calendariosService;
        }

        [HttpGet("{matricula}")]
        public async Task<ActionResult<CalendarioDto>> GetCalendarioAlumno(string matricula)
        {
            var entity = new CalendarioEntity();
            if (!string.IsNullOrEmpty(matricula))
            {
                entity.Matricula = matricula;
            }

            CalendarioDto data = await _calendariosService.GetCalendarioAlumno(entity);
            return Ok(data);
        }

        [HttpGet("GetCalendarios")]
        public async Task<ActionResult<CalendariosDto>> GetCalendarios()
        {
            CalendariosDto entity = new();
            entity = await _calendariosService.GetCalendarios(entity);
            return Ok(entity);
            
        }

        [HttpPost("ModificarCalendarios")]
        public async Task<ActionResult<BaseOutDto>> ModificarCalendarios(List<CalendariosEntity> configuracionCalendario)
        {
            BaseOutDto result = await _calendariosService.GuardarConfiguracionCalendarios(configuracionCalendario);
            return Ok(result);
        }
    }
}
