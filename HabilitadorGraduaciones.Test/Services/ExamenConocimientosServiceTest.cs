using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using HabilitadorGraduaciones.Services.Interfaces;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class ExamenConocimientosServiceTest
    {
        Mock<IExamenConocimientosRepository> _examenConocimientosData;
        Mock<IApiService> _apiServicie;
        ExamenConocimientosService _examenConocimientosService;
        Mock<IExamenConocimientosService> _i;
        public ExamenConocimientosServiceTest()
        {
            _examenConocimientosData = new Mock<IExamenConocimientosRepository>();
            _apiServicie = new Mock<IApiService>();
            _i = new Mock<IExamenConocimientosService>();
            _examenConocimientosService = new ExamenConocimientosService(_examenConocimientosData.Object, _apiServicie.Object);
        }

        [Fact]
        public async Task GetExamenConocimientoPorLenguaje_Success()
        {
            var entity = new TipoExamenConocimientosEntity()
            {
                IdTipoExamen = 1,
                Descripcion = "CENEVAL",
                Titulo = "Examen de Conocimientos",
                Nota = "Es un requisito de graduación haber presentado el examen externo, autorizado por el Tecnológico de Monterrey, para evaluar los conocimientos adquiridos durante tu carrera profesional.<br /><br /> Este requisito se debe registrar de manera previa y es aplicable solamente para los alumnos de las carreras profesionales para las que existan dichos exámenes.",
                Link = "Con tu director de programa",
                Result = true
            };

            int tipo = 1;
            string lenguaje = "es";

            _examenConocimientosData.Setup(x => x.GetExamenConocimientoPorLenguaje(tipo, lenguaje)).Returns(Task.FromResult(entity));
            var actualData = await _examenConocimientosService.GetExamenConocimientoPorLenguaje(tipo, lenguaje);

            Assert.IsType<TipoExamenConocimientosEntity>(actualData);
            Assert.Equal(entity, actualData);
        }

        [Fact]
        public async Task GetExamenConocimientoPorLenguaje_Failure()
        {
            var entity = new TipoExamenConocimientosEntity();

            int tipo = 1;
            string lenguaje = "es";

            _examenConocimientosData.Setup(x => x.GetExamenConocimientoPorLenguaje(tipo, lenguaje)).Returns(Task.FromResult(entity));
            var actualData = await _examenConocimientosService.GetExamenConocimientoPorLenguaje(tipo, lenguaje);

            Assert.IsType<TipoExamenConocimientosEntity>(actualData);
            Assert.Equal(entity, actualData);
        }

    }
}
