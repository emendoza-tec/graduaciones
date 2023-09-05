using AutoMapper;
using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpedienteController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IExpedienteService _expedienteService;

        public ExpedienteController(IMapper mapper, IExpedienteService expedienteService)
        {
            _mapper = mapper;
            _expedienteService = expedienteService;
        }

        [HttpGet("{matricula}")]
        public async Task<ActionResult<ExpedienteOutDto>> GetByAlumno(string matricula)
        {
            var entity = await _expedienteService.GetByAlumno(matricula);
            return Ok(_mapper.Map<ExpedienteOutDto>(entity));
        }

        [HttpPost("procesaExpedientes/{idUsuario}")]
        public async Task<IActionResult> PostProcesaExpedientes(IFormFile archivo, int idUsuario)
        {
            if (archivo == null)
            {
                throw new CustomException("Error el archivo esta vacio", System.Net.HttpStatusCode.NoContent);
            }

            var listaProcesos = await _expedienteService.ProcesaExcel(archivo, idUsuario);

            var jsonString = JsonSerializer.Serialize(listaProcesos);

            return Ok(jsonString);
        }

        [HttpPost("guardaExpedientes/{idUsuario}")]
        public async Task<IActionResult> PostGuardaExpdientes([FromForm] ArchivoEntity archivo, int idUsuario)
        {
            if (archivo == null)
            {
                throw new CustomException("Error el archivo esta vacio", System.Net.HttpStatusCode.NoContent);
            }

            var guardaExpdientes = await _expedienteService.GuardaExpedientes(archivo, idUsuario);
            if (guardaExpdientes.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
            {
                throw new CustomException("Error el archivo esta vacio", System.Net.HttpStatusCode.NotAcceptable);
            }

            archivo.Result = "success";
            return Ok(archivo);

        }
        [HttpGet("ConsultarComentarios/{matricula}")]
        public async Task<ActionResult<List<ExpedienteOutDto>>> ConsultarComentarios(string matricula)
        {
            var entity = await _expedienteService.ConsultarComentarios(matricula);
            return Ok(_mapper.Map<List<ExpedienteOutDto>>(entity));
        }
    }
}