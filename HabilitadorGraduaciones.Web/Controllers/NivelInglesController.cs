using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NivelInglesController : ControllerBase
    {
        private readonly INivelInglesService _nivelInglesService;

        public NivelInglesController(INivelInglesService nivelInglesService)
        {
            _nivelInglesService = nivelInglesService;
        }

        [HttpGet("{matricula}")]
        public async Task<ActionResult<NivelInglesDto>> GetAlumnoNivelIngles(string matricula)
        {
            var entity = new NivelInglesEntity();
            if (!string.IsNullOrEmpty(matricula))
            {
                entity.Matricula = matricula;
            }
            return Ok(await _nivelInglesService.GetAlumnoNivelIngles(entity));
        }

        [HttpGet("GetProgramas")]
        public async Task<ActionResult<ProgramaDto>> GetProgramas()
        {
            ProgramaDto entity = new();
            entity = await _nivelInglesService.GetProgramas(entity);
            return Ok(entity);
        }

        [HttpPost("ModificarNivelIngles")]
        public async Task<ActionResult<BaseOutDto>> ModificarNivelIngles(List<ConfiguracionNivelInglesEntity> configuracionIngles)
            => Ok(await _nivelInglesService.GuardarConfiguracionNivelIngles(configuracionIngles));
    }
}