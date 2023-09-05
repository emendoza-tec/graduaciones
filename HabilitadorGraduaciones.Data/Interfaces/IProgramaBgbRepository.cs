using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Token;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IProgramaBgbRepository
    {
        public Task<ProgramaBgbDto> ProgramaBGBApi(EndpointsDto dto, Sesion sesion);
    }
}
