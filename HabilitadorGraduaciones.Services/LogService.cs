using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Data;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HabilitadorGraduaciones.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logData;

        public LogService(ILogRepository logData)
        {
            _logData = logData;
        }
        public async Task<BaseOutDto> GuardarLog(LogEnteradoDto data)
        {
            return await _logData.GuardarLog(data);
        }
    }
}
