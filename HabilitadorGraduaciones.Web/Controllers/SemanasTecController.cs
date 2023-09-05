using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemanasTecController : ControllerBase
    {
        private readonly ISemanasTecService _semanasTecService;

        public SemanasTecController(ISemanasTecService semanasTecService)
        {
            _semanasTecService = semanasTecService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SemanasTecDto>> GetSemanasTec(string id)
        {
            var entity = new EndpointsDto();
            if (!string.IsNullOrEmpty(id))
            {
                entity.NumeroMatricula = id;
            }
            SemanasTecDto dto = await _semanasTecService.GetSemanasTecService(entity);

            return Ok(dto);
        }
    }
}
