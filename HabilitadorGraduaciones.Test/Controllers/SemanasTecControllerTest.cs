using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class SemanasTecControllerTest
    {
        readonly Mock<ISemanasTecService> _semanasTecService;
        private readonly SemanasTecController _semanasTecController;

        public SemanasTecControllerTest()
        {
            _semanasTecService = new Mock<ISemanasTecService>();
            _semanasTecController = new SemanasTecController(_semanasTecService.Object);
        }

        [Fact]
        public async Task GetSemanasTec_Success()
        {
            var expectedData = new SemanasTecDto()
            {
                NumeroMatricula = "A01657427",
                ClaveProgramaAcademico = "PBE20",
                ClaveNivelAcademico = "05",
                SemanasMaximas = 21,
                SemanasObtenidas = 20,
                UltimaActualizacion = Convert.ToDateTime("2023-05-24"),
                Result = true

            };

            _semanasTecService.Setup(m => m.GetSemanasTecService(It.IsAny<EndpointsDto>())).Returns(Task.FromResult(expectedData));
            var resultado = await _semanasTecController.GetSemanasTec(It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (SemanasTecDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(resultado.Result);
            Assert.NotNull(actual.Value);
            Assert.IsType<SemanasTecDto>(actual.Value);
            Assert.True(response.Result);
        }


        [Fact]
        public async Task GetSemanasTec_Failure()
        {

            var expectedData = new SemanasTecDto()
            {

                ClaveProgramaAcademico = "PBE20",
                ErrorMessage = string.Empty,
                Result = false
            };

            _semanasTecService.Setup(m => m.GetSemanasTecService(It.IsAny<EndpointsDto>())).Returns(Task.FromResult(expectedData));
            var resultado = await _semanasTecController.GetSemanasTec(It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (SemanasTecDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<SemanasTecDto>(actual.Value);
            Assert.False(response.Result);
        }

    }
}
