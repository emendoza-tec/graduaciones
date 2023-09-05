using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Token;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface ISemanasTecRepository
    {
        public Task<SemanasTecDto> GetSemanasTec(EndpointsDto dtosemTec, Sesion sesion);
        public Task<SemanasTecDto> GetProgramaNivel(EndpointsDto dtosemTec);
    }
}
