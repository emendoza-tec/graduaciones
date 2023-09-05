using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class ServicioSocialService : IServicioSocialService
    {
        private readonly IServicioSocialRepository _servicioSocialData;

        public ServicioSocialService(IServicioSocialRepository servicioSocialData)
        {
            _servicioSocialData = servicioSocialData;
        }

        public async Task<ServicioSocialDto> GetServicioSocial(EndpointsDto alumno)
        {
            return await _servicioSocialData.ConsultarHorasServicioSocial(alumno);
        }
    }
}