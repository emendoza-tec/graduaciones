using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Token;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class DistincionesService : IDistincionesService
    {
        private readonly IDistincionesRepository _distincionesData;
        private readonly IApiService _apiService;

        public DistincionesService(IDistincionesRepository distincionesData, IApiService apiService)
        {
            _distincionesData = distincionesData;
            _apiService = apiService;
        }
        public async Task<DistincionesDto> GetDistincionesService(EndpointsDto dto)
        {
            Sesion sesion = await _apiService.VerificaTokenUsuario(dto.NumeroMatricula);

            return  await _distincionesData.GetDistinciones(dto, sesion);
        }
    }
}