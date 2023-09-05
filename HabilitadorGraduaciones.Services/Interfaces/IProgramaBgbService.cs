using HabilitadorGraduaciones.Core.DTO;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IProgramaBgbService
    {
        public Task<ProgramaBgbDto> ProgramaBGBApi(EndpointsDto dtoParam);
    }
}
