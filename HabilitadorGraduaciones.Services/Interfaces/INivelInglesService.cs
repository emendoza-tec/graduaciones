using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface INivelInglesService
    {
        public Task<NivelInglesDto> GetAlumnoNivelIngles(NivelInglesEntity entity);
        public Task<ProgramaDto> GetProgramas(ProgramaDto entity);
        public Task<BaseOutDto> GuardarConfiguracionNivelIngles(List<ConfiguracionNivelInglesEntity> configuracion);
    }
}
