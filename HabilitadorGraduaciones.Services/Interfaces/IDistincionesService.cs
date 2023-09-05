using HabilitadorGraduaciones.Core.DTO;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IDistincionesService
    {
        public Task<DistincionesDto> GetDistincionesService(EndpointsDto dto);
    }
}
