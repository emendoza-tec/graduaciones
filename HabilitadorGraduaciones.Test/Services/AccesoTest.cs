using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using Moq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class AccesoTest
    {
        Mock<IAccesosNominaRepository> accesosNominaData;
        AccesosNominaService accesosNominaService;
        public AccesoTest()
        {
            accesosNominaData = new Mock<IAccesosNominaRepository>();
            accesosNominaService = new AccesosNominaService(accesosNominaData.Object);
        }

        [Fact]
        public async Task GetAcceso_Success()
        {
            var expectedData = new AccesosNominaEntity()
            {
                Matricula = "A00828911",
                Ambiente = "PPRD",
                Acceso = true
            };

            accesosNominaData.Setup(m => m.GetAcceso(expectedData.Matricula)).Returns(Task.FromResult(expectedData));

            var actualData = await accesosNominaService.GetAcceso(expectedData.Matricula);

            // Assert
            Assert.IsType<AccesosNominaEntity>(actualData);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task GetAcceso_Failure()
        {
            var expectedData = new AccesosNominaEntity()
            {
                Matricula = "A00828911",
                Ambiente = "PPRD",
                Acceso = false
            };

            accesosNominaData.Setup(m => m.GetAcceso(It.IsAny<string>())).Returns(Task.FromResult(expectedData));

            var actualData = await accesosNominaService.GetAcceso(It.IsAny<string>());

            // Assert
            Assert.IsType<AccesosNominaEntity>(actualData);
            Assert.False(expectedData.Acceso);
        }
    }
}
