using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IAvisosService
    {
        public Task<AvisosDto> Get3AvisosService(AvisosEntity entity);
        public Task<BaseOutDto> SetAvisosService(AvisoGuardar entity);
        public Task<AvisosDto> GetAvisosService(AvisosEntity entity);
        public Task<List<CatalogoDto>> ObtenerCatalogo(int opcion);
        public Task<List<CatalogoDto>> ObtenerCatalogoMatricula(FiltrosMatriculaDto filtros);
    }
}
