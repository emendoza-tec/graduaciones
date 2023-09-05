using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamenConocimientosController : Controller
    {
        private readonly IExamenConocimientosService _examenConocimientosService;

        public ExamenConocimientosController(IExamenConocimientosService examenConocimientosService)
        {
            _examenConocimientosService = examenConocimientosService;
        }

        [HttpPost("GetExamenConocimiento")]
        public async Task<ActionResult<ExamenConocimientosDto>> GetExamenConocimiento(EndpointsDto dto)
           => Ok(await _examenConocimientosService.GetExamenConocimiento(dto));

        [HttpGet("{tipoExamen}/{lenguaje}")]
        public async Task<ActionResult<TipoExamenConocimientosEntity>> GetExamenConocimientoPorLenguaje(int tipoExamen, string lenguaje)
           => Ok(await _examenConocimientosService.GetExamenConocimientoPorLenguaje(tipoExamen, lenguaje));
    }
}
