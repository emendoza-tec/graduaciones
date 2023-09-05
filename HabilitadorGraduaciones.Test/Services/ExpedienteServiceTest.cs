using HabilitadorGraduaciones.Core.Entities.Expediente;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class ExpedienteServiceTest
    {
        private readonly Mock<IExpedienteRepository> _expedienteData;
        private readonly ExpedienteService _expedienteService;
        private readonly Mock<IUsuarioService> _usuarioService;
        private readonly Mock<IConfiguration> _config;
        private readonly Mock<INotificacionesService> _notificacionesService;

        public ExpedienteServiceTest()
        {
            _expedienteData = new Mock<IExpedienteRepository>();
            _usuarioService = new Mock<IUsuarioService>();
            _config = new Mock<IConfiguration>();
            _notificacionesService = new Mock<INotificacionesService>();
            _expedienteService = new ExpedienteService(_config.Object, _usuarioService.Object, _expedienteData.Object, _notificacionesService.Object);
        }

        [Fact]
        public async Task GetByAlumno_Success()
        {
            string matricula = "A00828911";
            var expectedData = new ExpedienteEntity()
            {
                Estatus = "Completo",
                Detalle = "PRUEBA MENSAJE 5862",
                UltimaActualizacion = Convert.ToDateTime("2023-06-22T00:00:00"),
                Result = true
            };

            _expedienteData.Setup(m => m.GetByAlumno(matricula)).Returns(Task.FromResult(expectedData));

            var actualData = await _expedienteService.GetByAlumno(matricula);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task GetByAlumno_Failure()
        {
            var expectedData = new ExpedienteEntity();

            _expedienteData.Setup(m => m.GetByAlumno(It.IsAny<string>())).Returns(Task.FromResult(expectedData));

            var actualData = await _expedienteService.GetByAlumno(It.IsAny<string>());
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task ConsultarComentarios_Success()
        {
            string matricula = "A00828911";
            var expectedData = new List<ExpedienteEntity>()
            {
                new ExpedienteEntity()
                {
                    Detalle = "PRUEBA MENSAJE 5862",
                    UltimaActualizacion = Convert.ToDateTime("2023-06-22T00:00:00"),
                    Result = true
                },
                new ExpedienteEntity()
                {
                    Detalle = "PRUEBA MENSAJE 5863",
                    UltimaActualizacion = Convert.ToDateTime("2023-06-22T00:00:00"),
                    Result = true
                }
            };

            _expedienteData.Setup(m => m.ConsultarComentarios(matricula)).Returns(Task.FromResult(expectedData));

            var actualData = await _expedienteService.ConsultarComentarios(matricula);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task ConsultarComentarios_Failure()
        {
            var expectedData = new List<ExpedienteEntity>();

            _expedienteData.Setup(m => m.ConsultarComentarios(It.IsAny<string>())).Returns(Task.FromResult(expectedData));

            var actualData = await _expedienteService.ConsultarComentarios(It.IsAny<string>());
            Assert.Equal(expectedData, actualData);
        }
    }
}
