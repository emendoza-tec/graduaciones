using HabilitadorGraduaciones.Core.DTO;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface ISemanasTecService
    {
        public Task<SemanasTecDto> GetSemanasTecService(EndpointsDto dtosemTec);
    }
}
