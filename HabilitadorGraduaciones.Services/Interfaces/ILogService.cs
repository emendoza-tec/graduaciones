using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.DTO;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface ILogService
    {
        public Task<BaseOutDto> GuardarLog(LogEnteradoDto data);
    }
}
