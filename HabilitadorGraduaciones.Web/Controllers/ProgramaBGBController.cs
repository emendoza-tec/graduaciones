using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramaBgbController : ControllerBase
    {
        private readonly IProgramaBgbService _programaBGBService;

        public ProgramaBgbController(IProgramaBgbService programaBGBService)
        {
            _programaBGBService = programaBGBService;
        }

        [HttpPost]
        public async Task<ActionResult<ProgramaBgbDto>> GetProgramaBGB(EndpointsDto dtoParam)
        {
            ProgramaBgbDto dto = await _programaBGBService.ProgramaBGBApi(dtoParam);
            dto.ClavePrograma = dtoParam.ClaveProgramaAcademico;
            return Ok(dto);
        }
    }
}
