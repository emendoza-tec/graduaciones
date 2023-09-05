using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class ExamenIntegradorServiceTest
    {
        private readonly Mock<IExamenIntegradorRepository> _examenIntegradorData;
        private readonly ExamenIntegradorService _examenIntegradorService;
        private readonly Mock<IConfiguration> _config;
        private readonly Mock<IPeriodosRepository> _periodoData;

        public ExamenIntegradorServiceTest()
        {
            _examenIntegradorData = new Mock<IExamenIntegradorRepository>();
            _periodoData = new Mock<IPeriodosRepository>();
            _config = new Mock<IConfiguration>();
            _examenIntegradorService = new ExamenIntegradorService(_examenIntegradorData.Object, _config.Object, _periodoData.Object);
        }

        [Fact]
        public async Task GetMatricula_Success()
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

            _examenIntegradorData.Setup(m => m.GetMatricula(matricula)).Returns(Task.FromResult(expectedData));

            var actualData = await _examenIntegradorService.GetMatricula(matricula);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task GetMatricula_Failure()
        {
            var expectedData = new ExamenIntegradorEntity();

            _examenIntegradorData.Setup(m => m.GetMatricula(It.IsAny<string>())).Returns(Task.FromResult(expectedData));

            var actualData = await _examenIntegradorService.GetMatricula(It.IsAny<string>());
            Assert.Equal(expectedData, actualData);
        }
    }
}
