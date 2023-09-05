using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Data.Utils.Enums;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiltrosController : Controller
    {
        private readonly IAvisosService _avisosService;

        public FiltrosController(IAvisosService avisosService)
        {
            _avisosService = avisosService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CatalogoDto>>> GetFiltro()
            => Ok(await _avisosService.ObtenerCatalogo((int)Filtros.Campus));

        [HttpGet("{id}")]
        public async Task<ActionResult<List<CatalogoDto>>> GetFiltro(int id)
            => Ok(await _avisosService.ObtenerCatalogo(id));
    }
}
