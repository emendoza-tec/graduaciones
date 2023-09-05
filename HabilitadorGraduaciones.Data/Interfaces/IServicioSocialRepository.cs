using HabilitadorGraduaciones.Core.DTO;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IServicioSocialRepository
    {
        public Task<ServicioSocialDto> ConsultarHorasServicioSocial(EndpointsDto alumno);
    }
}
