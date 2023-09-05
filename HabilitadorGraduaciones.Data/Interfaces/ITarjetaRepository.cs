using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface ITarjetaRepository
    {
        public Task<TarjetaDto> Get(TarjetaEntity entity);
        public Task<List<DocumentosDto>> GetDocumentos(string idioma);
    }
}
