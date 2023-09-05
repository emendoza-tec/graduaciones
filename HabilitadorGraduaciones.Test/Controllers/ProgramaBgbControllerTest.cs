using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class ProgramaBgbControllerTest
    {
        readonly Mock<IProgramaBgbService> _programaBgbService;
        private readonly ProgramaBgbController _programaBgbController;

        public ProgramaBgbControllerTest()
        {
            _programaBgbService = new Mock<IProgramaBgbService>();
            _programaBgbController = new ProgramaBgbController(_programaBgbService.Object);
        }

        [Fact]
        public async Task GetProgramaBGB_Success()
        {
            var dto = new EndpointsDto()
            {
                NumeroMatricula = "A01657427",
                ClaveCampus = "2",
                ClaveCarrera = "BGB",
                ClaveEjercicioAcademico = "202213",
                ClaveNivelAcademico = "05",
                ClaveProgramaAcademico = "BGB19",
                Correo = "mirohu51528172@tec.mx"
            };

            var expectedData = new ProgramaBgbDto()
            {
                ClavePrograma = "BGB19",
                CreditosAprobadosProgramaInternacional = 18,
                CreditosDeInglesAprobadosCuartoSemestre = 4,
                CreditosDeInglesAprobadosQuintoSemestre = 4,
                CreditosDeInglesAprobadosOctavoSemestre = 0,
                CumpleProgramaInternacional = true,
                CumpleRequisitoInglesCuarto = true,
                CumpleRequisitoInglesQuinto = true,
                CumpleRequisitoInglesOctavo = false,
                CumpleRequisitosProgramaEspecial = false,
                ErrorMessage = string.Empty,
                Result = true,
                UltimaActualizacion = DateTime.Now
            };

            _programaBgbService.Setup(m => m.ProgramaBGBApi(dto)).Returns(Task.FromResult(expectedData));
            var resultado = await _programaBgbController.GetProgramaBGB(dto);
            var actual = resultado.Result as ObjectResult;
            var response = (ProgramaBgbDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<ProgramaBgbDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GetProgramaBGB_Failure()
        {
            var expectedData = new ProgramaBgbDto()
            {
                ClavePrograma = "BGB19",
                ErrorMessage = string.Empty,
                Result = false
            };

            EndpointsDto dto = new EndpointsDto();

            _programaBgbService.Setup(m => m.ProgramaBGBApi(dto)).Returns(Task.FromResult(expectedData));
            var resultado = await _programaBgbController.GetProgramaBGB(dto);
            var actual = resultado.Result as ObjectResult;
            var response = (ProgramaBgbDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<ProgramaBgbDto>(actual.Value);
            Assert.False(response.Result);
        }
    }
}
