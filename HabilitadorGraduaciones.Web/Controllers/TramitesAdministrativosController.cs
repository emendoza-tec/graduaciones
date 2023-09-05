using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TramitesAdministrativosController : ControllerBase
    {
        private readonly IPrestamoEducativoService _prestamoEducativoService;

        public TramitesAdministrativosController(IPrestamoEducativoService prestamoEducativoService)
        {
            _prestamoEducativoService = prestamoEducativoService;
        }

        [HttpGet("{matricula}")]
        public async Task<ActionResult<PrestamoEducativoDto>> GetPrestamoAlumno(string matricula)
        {
            var entity = new PrestamoEducativoEntity();
            if (!string.IsNullOrEmpty(matricula))
            {
                entity.Matricula = matricula;
            }
            return Ok(await _prestamoEducativoService.GetPrestamoEducativo(entity));
        }
    }
}
