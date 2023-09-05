using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet()]
        public async Task<ActionResult<List<RolesEntity>>> ObtenRoles()
            => Ok(await _rolesService.ObtenRoles());

        [HttpGet("{idRol}")]
        public async Task<ActionResult<RolesEntity>> ObtenerRolesPorId(int idRol)
            => Ok(await _rolesService.ObtenerRolesPorId(idRol));

        [HttpGet("ObtenerSecciones")]
        public async Task<ActionResult<List<SeccionesPermisosDto>>> ObtenerSecciones()
           => Ok(await _rolesService.ObtenerSecciones());

        [HttpPost("GuardaRol")]
        public async Task<ActionResult<BaseOutDto>> GuardaRol(RolesEntity rol)
            => Ok(await _rolesService.GuardaRol(rol));

        [HttpPost("ModificaRol")]
        public async Task<ActionResult<BaseOutDto>> ModificaRol(RolesEntity rol)
            => Ok(await _rolesService.ModificaRol(rol));

        [HttpPost("CambiaEstatusRol")]
        public async Task<ActionResult<BaseOutDto>> CambiaEstatusRol(RolesEntity rol)
            => Ok(await _rolesService.CambiaEstatusRol(rol));

        [HttpPost("EliminaRol")]
        public async Task<ActionResult<BaseOutDto>> EliminaRol(RolesEntity rol)
            => Ok(await _rolesService.EliminaRol(rol));

        [HttpGet("ObtenerDescripcionRoles")]
        public async Task<ActionResult<List<RolesDto>>> ObtenerDescripcionRoles()
            => Ok(await _rolesService.ObtenerDescripcionRoles());
    }
}
