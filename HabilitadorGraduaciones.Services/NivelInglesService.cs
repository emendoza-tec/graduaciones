using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;


namespace HabilitadorGraduaciones.Services
{
    public class NivelInglesService : INivelInglesService
    {
        private readonly INivelInglesRepository _nivelInglesData;
        public NivelInglesService(INivelInglesRepository nivelInglesData)
        {
            _nivelInglesData = nivelInglesData;
        }

        public async Task<NivelInglesDto> GetAlumnoNivelIngles(NivelInglesEntity entity)
        {
            return await _nivelInglesData.GetAlumnoNivelIngles(entity);
        }

        public async Task<ProgramaDto> GetProgramas(ProgramaDto entity)
        {
            return await _nivelInglesData.GetProgramas(entity);
        }

        public async Task<BaseOutDto> GuardarConfiguracionNivelIngles(List<ConfiguracionNivelInglesEntity> configuracion)
        {
            return await _nivelInglesData.ModificarNivelIngles(configuracion);
        }
    }
}