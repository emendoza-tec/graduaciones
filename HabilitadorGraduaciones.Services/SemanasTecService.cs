using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Token;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HabilitadorGraduaciones.Services
{
    public class SemanasTecService : ISemanasTecService
    {
        public IConfiguration _configuration { get; set; }
        private readonly ISemanasTecRepository _semanasTecData;
        private readonly IApiService _apiService;

        public SemanasTecService(IConfiguration configuration, ISemanasTecRepository semanasTecData, IApiService apiService)
        {
            _configuration = configuration;
            _semanasTecData = semanasTecData;
            _apiService = apiService;
        }
        public async Task<SemanasTecDto> GetSemanasTecService(EndpointsDto dtosemTec)

        {
            Sesion sesion = await _apiService.VerificaTokenUsuario(dtosemTec.NumeroMatricula);

            SemanasTecDto dto = await _semanasTecData.GetSemanasTec(dtosemTec, sesion);
            return dto;
        }

    }
}