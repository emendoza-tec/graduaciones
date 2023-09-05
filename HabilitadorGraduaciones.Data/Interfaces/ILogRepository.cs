using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.DTO;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface ILogRepository
    {
        public Task<BaseOutDto> GuardarLog(LogEnteradoDto data);
    }
}
