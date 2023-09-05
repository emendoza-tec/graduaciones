using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class TarjetaService : ITarjetaService
    {
        private readonly ITarjetaRepository _tarjetaData;
        public TarjetaService(ITarjetaRepository tarjetaData)
        {
            _tarjetaData = tarjetaData;
        }
        public async Task<TarjetaDto> Get(TarjetaEntity entity)
        {
            return await _tarjetaData.Get(entity);
        }
    }
}