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
    public class TramitesAdministrativosControllerTest
    {
        readonly Mock<IPrestamoEducativoService> _prestamoEducativoService;
        private readonly TramitesAdministrativosController _tramitesController;

        public TramitesAdministrativosControllerTest()
        {
            _prestamoEducativoService = new Mock<IPrestamoEducativoService>();
            _tramitesController = new TramitesAdministrativosController(_prestamoEducativoService.Object);
        }

        [Fact]
        public async Task GetCalendarioAlumno_Success()
        {
            PrestamoEducativoDto result = new PrestamoEducativoDto();
            result.EstatusContrato = string.Empty;
            result.TienePrestamo = true;
            result.Result = true;
            result.ErrorMessage = string.Empty;

            _prestamoEducativoService.Setup(m => m.GetPrestamoEducativo(It.IsAny<PrestamoEducativoEntity>())).Returns(Task.FromResult(result));
            var resultado = await _tramitesController.GetPrestamoAlumno(It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (PrestamoEducativoDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<PrestamoEducativoDto>(response);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GetCalendarioAlumno_Failure()
        {
            PrestamoEducativoDto result = new PrestamoEducativoDto();
            result.Result = false;
            result.ErrorMessage = string.Empty;

            _prestamoEducativoService.Setup(m => m.GetPrestamoEducativo(It.IsAny<PrestamoEducativoEntity>())).Returns(Task.FromResult(result));
            var resultado = await _tramitesController.GetPrestamoAlumno(It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (PrestamoEducativoDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<PrestamoEducativoDto>(response);
            Assert.False(response.Result);
        }
    }
}
