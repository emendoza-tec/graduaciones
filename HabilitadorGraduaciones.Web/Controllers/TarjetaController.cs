using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarjetaController : Controller
    {
        private readonly ITarjetaService _tarjetaService;

        public TarjetaController(ITarjetaService tarjetaService)
        {
            _tarjetaService = tarjetaService;
        }

        [HttpGet("{id}/{idioma}")]
        public async Task<ActionResult<TarjetaDto>> Get(int id, string idioma)
        {
            var entity = new TarjetaEntity();
            entity.IdTarjeta = id;
            entity.Idioma = idioma;

            return Ok(await _tarjetaService.Get(entity));
        }
    }
}