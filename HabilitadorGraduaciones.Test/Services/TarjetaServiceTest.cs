using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class TarjetaServiceTest
    {
        readonly Mock<ITarjetaRepository> _tarjetaData;
        readonly TarjetaService _tarjetaService;

        public TarjetaServiceTest()
        {
            _tarjetaData = new Mock<ITarjetaRepository>();
            _tarjetaService = new TarjetaService(_tarjetaData.Object);
        }

        [Fact]
        public async Task Get_Success()
        {
            TarjetaDto result = new TarjetaDto
            {
                Contacto = string.Empty,
                Correo = string.Empty,
                Documentos = new List<DocumentosDto>(),
                ErrorMessage = string.Empty,
                IdTarjeta = 0,
                Idioma = string.Empty,
                Link = "Con tu director de programa",
                Nota = "Es un requisito de graduación demostrar un nivel de dominio B2 del idioma inglés referenciado al Marco Común Europeo de Referencia (MCER) en alguno de los <b>exámenes autorizados </b> por nuestra institución. De acuerdo a la última actualización realizada el @fecha este es el registro de tu nivel acreditado",
                Tarjeta = "Nivel de Inglés",
                Result = true
            };

            _tarjetaData.Setup(m => m.Get(It.IsAny<TarjetaEntity>())).Returns(Task.FromResult(result));
            var actualData = await _tarjetaService.Get(It.IsAny<TarjetaEntity>());
            Assert.IsType<TarjetaDto>(actualData);
            Assert.True(actualData.Result);
        }

        [Fact]
        public async Task Get_Failure()
        {
            TarjetaDto result = new TarjetaDto
            {
                ErrorMessage = string.Empty,
                Result = false
            };

            _tarjetaData.Setup(m => m.Get(It.IsAny<TarjetaEntity>())).Returns(Task.FromResult(result));
            var actualData = await _tarjetaService.Get(It.IsAny<TarjetaEntity>());
            Assert.IsType<TarjetaDto>(actualData);
            Assert.False(actualData.Result);
        }
    }
}
