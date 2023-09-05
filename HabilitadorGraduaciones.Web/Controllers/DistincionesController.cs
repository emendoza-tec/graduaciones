
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistincionesController : ControllerBase
    {
        private readonly IDistincionesService _distincionesService;

        public DistincionesController(IDistincionesService distincionesService)
        {
            _distincionesService = distincionesService;

        }

        [HttpPost]
        public async Task<ActionResult<BaseOutDto>> GetDistinciones(EndpointsDto dto) 
            => Ok(await _distincionesService.GetDistincionesService(dto));
    }
}