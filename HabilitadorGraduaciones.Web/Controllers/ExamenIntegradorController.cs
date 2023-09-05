using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamenIntegradorController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IExamenIntegradorService _examenIntegradorService;

        public ExamenIntegradorController(IConfiguration configuration, IExamenIntegradorService examenIntegradorService)
        {
            Configuration = configuration;
            _examenIntegradorService = examenIntegradorService;
        }

        [HttpPost("procesaExamenesIntegrador/{idUsuario}")]
        public async Task<IActionResult> PostProcesaExpedientes(IFormFile archivo, int idUsuario)
        {
            if (archivo == null)
            {
                return BadRequest();
            }

            var listaProcesos = _examenIntegradorService.ProcesaExcel(archivo, idUsuario);

            var jsonString = JsonSerializer.Serialize(listaProcesos.Result);

            return Ok(jsonString);
        }

        [HttpPost("guardaExamenesIntegrador/{idUsuario}")]
        public async Task<IActionResult> PostGuardaExamenesIntegrador([FromForm] ArchivoEntity archivo, int idUsuario)
        {
            if (archivo == null)
            {
                throw new CustomException("Error el archivo esta vacio", System.Net.HttpStatusCode.NoContent);
            }

            var guardaExamenesIntegrador = await _examenIntegradorService.GuardaExamenesIntegrador(archivo, idUsuario);
            if (guardaExamenesIntegrador.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
            {
                throw new CustomException("Error al guardar el archivo", System.Net.HttpStatusCode.NotAcceptable);
            }

            archivo.Result = "success";
            return Ok(archivo);
        }

        [HttpGet("{matricula}")]
        public async Task<ActionResult<ExamenIntegradorEntity>> Get(string matricula)
            => Ok(await _examenIntegradorService.GetMatricula(matricula));
    }
}