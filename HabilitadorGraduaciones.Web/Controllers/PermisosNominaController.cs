using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosNominaController : Controller
    {
        private readonly IPermisosNominaService _permisosNominaService;

        public PermisosNominaController(IPermisosNominaService permisosNominaService)
        {
            _permisosNominaService = permisosNominaService;
        }

        [HttpGet("{nomina}")]
        public async Task<ActionResult<List<PermisosNominaDto>>> ObtenerPermisosPorNomina(string nomina)
            => Ok(await _permisosNominaService.ObtenerPermisosPorNomina(nomina));
    }
}
