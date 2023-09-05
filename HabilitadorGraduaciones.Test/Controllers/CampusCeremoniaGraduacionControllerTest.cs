using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class CampusCeremoniaGraduacionControllerTest
    {
        readonly Mock<ICampusCeremoniaGraduacionService> _campusCeremoniaService;
        private readonly CampusCeremoniaGraduacionController _campusCeremoniaController;

        public CampusCeremoniaGraduacionControllerTest()
        {
            _campusCeremoniaService = new Mock<ICampusCeremoniaGraduacionService>();
            _campusCeremoniaController = new CampusCeremoniaGraduacionController(_campusCeremoniaService.Object);
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

            _campusCeremoniaService.Setup(m => m.GuardaCeremonia(ceremonia)).Returns(Task.FromResult(res));
            var resultado = await _campusCeremoniaController.GuardaCeremonia(ceremonia);
            var actual = resultado.Result as ObjectResult;
            var resopnse = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.True(resopnse.Result);
        }

        [Fact]
        public async Task GuardaCeremonia_Failure()
        {
            CampusCeremoniaGraduacionEntity ceremonia = new();

            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };

            _campusCeremoniaService.Setup(m => m.GuardaCeremonia(ceremonia)).Returns(Task.FromResult(res));
            var resultado = await _campusCeremoniaController.GuardaCeremonia(ceremonia);
            var actual = resultado.Result as ObjectResult;
            var resopnse = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.False(resopnse.Result);
        }
    }
}
