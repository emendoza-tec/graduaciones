using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface ICampusCeremoniaRepository
    {
        public Task<BaseOutDto> GuardaCeremonia(CampusCeremoniaGraduacionEntity ceremonia);
    }
}
