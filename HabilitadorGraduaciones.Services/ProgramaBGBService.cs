using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Token;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class ProgramaBgbService : IProgramaBgbService
    {
        private readonly IProgramaBgbRepository _programaBGBData;
        private readonly IApiService _apiService;

        public ProgramaBgbService(IProgramaBgbRepository programaBGBData, 
            IApiService apiService)
        {
            _programaBGBData = programaBGBData;
            _apiService = apiService;
        }

        public async Task<ProgramaBgbDto> ProgramaBGBApi(EndpointsDto dtoParam)
        {            
            Sesion sesion = await _apiService.VerificaTokenUsuario(dtoParam.NumeroMatricula);         
            return await _programaBGBData.ProgramaBGBApi(dtoParam, sesion);
        }
    }
}
