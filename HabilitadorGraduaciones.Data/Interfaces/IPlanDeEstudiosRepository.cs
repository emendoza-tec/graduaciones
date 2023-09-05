using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Token;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IPlanDeEstudiosRepository
    {
        public Task<PlanDeEstudiosDto> ConsultarApiPlanDeEstudios(EndpointsDto dtoPE, Sesion sesion);
    }
}
