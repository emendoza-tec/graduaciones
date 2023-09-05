using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class TarjetaControllerTest
    {
        readonly Mock<ITarjetaService> _tarjetaService;
        private readonly TarjetaController _tarjetaController;

        public TarjetaControllerTest()
        {
            _tarjetaService = new Mock<ITarjetaService>();
            _tarjetaController = new TarjetaController(_tarjetaService.Object);
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

            _tarjetaService.Setup(m => m.Get(It.IsAny<TarjetaEntity>())).Returns(Task.FromResult(result));
            var resultado = await _tarjetaController.Get(It.IsAny<int>(), It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (TarjetaDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<TarjetaDto>(response);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task Get_Failure()
        {
            TarjetaDto result = new TarjetaDto
            {
                ErrorMessage = string.Empty,
                Result = false
            };

            _tarjetaService.Setup(m => m.Get(It.IsAny<TarjetaEntity>())).Returns(Task.FromResult(result));
            var resultado = await _tarjetaController.Get(It.IsAny<int>(), It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (TarjetaDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<TarjetaDto>(response);
            Assert.False(response.Result);
        }
    }
}
