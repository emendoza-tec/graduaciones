using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface INivelInglesRepository
    {
        public Task<NivelInglesDto> GetAlumnoNivelIngles(NivelInglesEntity entity);
        public Task<ProgramaDto> GetProgramas(ProgramaDto entity);
        public Task<BaseOutDto> ModificarNivelIngles(List<ConfiguracionNivelInglesEntity> guardarNiveles);
    }
}
