using HabilitadorGraduaciones.Core.DTO;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IServicioSocialService
    {
        public Task<ServicioSocialDto> GetServicioSocial(EndpointsDto alumno);
    }
}
