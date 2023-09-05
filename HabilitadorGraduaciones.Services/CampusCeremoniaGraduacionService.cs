using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class CampusCeremoniaGraduacionService : ICampusCeremoniaGraduacionService
    {
        private readonly ICampusCeremoniaRepository _campusCeremoniaData;

        public CampusCeremoniaGraduacionService(ICampusCeremoniaRepository campusCeremoniaData)
        {
            _campusCeremoniaData = campusCeremoniaData;
        }

        public async Task<BaseOutDto> GuardaCeremonia(CampusCeremoniaGraduacionEntity ceremonia)
        {
            return await _campusCeremoniaData.GuardaCeremonia(ceremonia);
        }
    }
}
