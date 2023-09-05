using HabilitadorGraduaciones.Core.DTO;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IPlanDeEstudiosService
    {
        public Task<PlanDeEstudiosDto> GetPlanDeEstudios(EndpointsDto dto);
    }
}
