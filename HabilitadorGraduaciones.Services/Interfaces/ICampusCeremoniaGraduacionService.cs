using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface ICampusCeremoniaGraduacionService
    {
        public Task<BaseOutDto> GuardaCeremonia(CampusCeremoniaGraduacionEntity ceremonia);
    }
}
