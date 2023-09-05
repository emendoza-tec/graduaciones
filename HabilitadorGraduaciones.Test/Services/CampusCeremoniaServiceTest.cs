using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class CampusCeremoniaServiceTest
    {
        readonly Mock<ICampusCeremoniaRepository> _campusCeremoniaData;
        readonly CampusCeremoniaGraduacionService _campusCeremoniaService;

        public CampusCeremoniaServiceTest()
        {
            _campusCeremoniaData = new Mock<ICampusCeremoniaRepository>();
            _campusCeremoniaService = new CampusCeremoniaGraduacionService(_campusCeremoniaData.Object);
        }

        [Fact]
        public async Task GuardaCeremonia_Success()
        {
            CampusCeremoniaGraduacionEntity ceremonia = new CampusCeremoniaGraduacionEntity()
            {
                ClaveCampus = "T",
                Matricula = "A01424206",
                PeriodoGraduacion = "202311"
            };

            BaseOutDto res = new BaseOutDto { Result = true, ErrorMessage = string.Empty };

            _campusCeremoniaData.Setup(m => m.GuardaCeremonia(ceremonia)).Returns(Task.FromResult(res));

            var actualData = await _campusCeremoniaService.GuardaCeremonia(ceremonia);
            Assert.IsType<BaseOutDto>(actualData);
            Assert.True(actualData.Result);
        }

        [Fact]
        public async Task GuardaCeremonia_Failure()
        {
            CampusCeremoniaGraduacionEntity ceremonia = new();

            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };

            _campusCeremoniaData.Setup(m => m.GuardaCeremonia(ceremonia)).Returns(Task.FromResult(res));

            var actualData = await _campusCeremoniaService.GuardaCeremonia(ceremonia);
            Assert.IsType<BaseOutDto>(actualData);
            Assert.False(actualData.Result);
        }
    }
}
