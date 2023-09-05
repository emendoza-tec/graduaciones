using AutoMapper;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Services;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.Office2013.Excel;
using HabilitadorGraduaciones.Core.CustomException;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPeriodosService _periodosService;

        public PeriodosController( IMapper mapper,  IPeriodosService periodosRepository)
        {
            _mapper = mapper;
            _periodosService = periodosRepository;
        }
       
        [HttpPost("GetPeriodos/")]
        public async Task<ActionResult<List<PeriodosDto>>> GetPeriodos(EndpointsDto dto)
        {
            List<PeriodosEntity> result = await _periodosService.GetPeriodos(dto);

            List<PeriodosDto> data = _mapper.Map<List<PeriodosDto>>(result);
          
            return Ok(data);
        }
        [HttpPost("GetPeriodoPronostisco/")]
        public async Task<ActionResult<PeriodosDto>> GetPeriodoPronostisco(EndpointsDto dto)
        {
            var result = await _periodosService.GetPeriodoPronostico(dto);

            PeriodosDto data = _mapper.Map<PeriodosDto>(result);
            return Ok(data);
        }
        [HttpGet("GetPeriodoAlumno/{matricula}")]
        public async Task<ActionResult<PeriodosDto>> GetPeriodoAlumno(string matricula)
        {
            var result = await _periodosService.GetPeriodoAlumno(matricula);
            PeriodosDto data = _mapper.Map<PeriodosDto>(result);
            return Ok(data);
        }
        [HttpPost("GuardarPeriodo/")]
        public async Task<ActionResult<BaseOutDto>> GuardarPeriodo(PeriodosDto data) => Ok(await _periodosService.GuardarPeriodo(data));

        [HttpPost("EnviarCorreo/")]
        public async Task<ActionResult<BaseOutDto>> EnviarCorreo(UsuarioDto correo)
        {
            BaseOutDto result = await _periodosService.EnviarCorreo(correo);
            return Ok(result);
        }
    }
}
