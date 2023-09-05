using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class ExamenIntegradorControllerTest
    {
        private readonly Mock<IExamenIntegradorService> _examenIntegradorService;
        private readonly ExamenIntegradorController _examenIntegradorController;
        private readonly Mock<IConfiguration> _configuration;

        public ExamenIntegradorControllerTest()
        {
            _examenIntegradorService = new Mock<IExamenIntegradorService>();
            _configuration = new Mock<IConfiguration>();
            _examenIntegradorController = new ExamenIntegradorController(_configuration.Object, _examenIntegradorService.Object);
        }

        [Fact]
        public async Task Get_Success()
        {
            string matricula = "A01103562";
            var expectedData = new ExamenIntegradorEntity()
            {
                PeriodoGraduacion = "201913",
                Nivel = "05",
                NombreRequisito = "INTEGRADOR",
                Estatus = "NC",
                FechaExamen = "01-01-0001",
                FechaExamenDate = Convert.ToDateTime("0001-01-01T00:00:00"),
                UltimaActualizacion = Convert.ToDateTime("2022-11-03T00:00:00"),
                Aplica = false,
                UpdateFlag = false,
                Result = true
            };

            _examenIntegradorService.Setup(m => m.GetMatricula(matricula)).Returns(Task.FromResult(expectedData));

            var responseController = await _examenIntegradorController.Get(matricula);
            var actual = responseController.Result as ObjectResult;
            var response = (ExamenIntegradorEntity)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<ExamenIntegradorEntity>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task Get_Failure()
        {
            var expectedData = new ExamenIntegradorEntity();

            _examenIntegradorService.Setup(m => m.GetMatricula(It.IsAny<string>())).Returns(Task.FromResult(expectedData));

            var responseController = await _examenIntegradorController.Get(It.IsAny<string>());
            var actual = responseController.Result as ObjectResult;
            var response = (ExamenIntegradorEntity)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<ExamenIntegradorEntity>(actual.Value);
            Assert.False(response.Result);
        }

    }
}
