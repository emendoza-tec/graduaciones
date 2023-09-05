using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface ITarjetaService
    {
        public Task<TarjetaDto> Get(TarjetaEntity entity);
    }
}
