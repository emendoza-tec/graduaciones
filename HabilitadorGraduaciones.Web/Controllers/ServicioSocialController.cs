using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioSocialController : Controller
    {
        private readonly IServicioSocialService _servicioSocialService;

        public ServicioSocialController(IServicioSocialService servicioSocialService)
        {
            _servicioSocialService = servicioSocialService;
        }


        [HttpPost]
        public async Task<ActionResult<ServicioSocialDto>> ConsultarServicioSocial(EndpointsDto dtoParam)
            => Ok(await _servicioSocialService.GetServicioSocial(dtoParam));      
    }
}