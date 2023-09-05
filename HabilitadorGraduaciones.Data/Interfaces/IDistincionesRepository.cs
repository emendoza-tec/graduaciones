using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Token;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IDistincionesRepository
    {
        public Task<DistincionesDto> GetDistinciones(EndpointsDto dto, Sesion sesion);
    }
}
