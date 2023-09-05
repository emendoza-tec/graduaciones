using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class TramitesAdministrativosServiceTest
    {
        readonly Mock<IPrestamoEducativoRepository> _prestamoData;
        readonly PrestamoEducativoService _prestamoService;

        public TramitesAdministrativosServiceTest()
        {
            _prestamoData = new Mock<IPrestamoEducativoRepository>();
            _prestamoService = new PrestamoEducativoService(_prestamoData.Object);
        }

        [Fact]
        public async Task GetPrestamoEducativo_Success()
        {
            PrestamoEducativoDto result = new PrestamoEducativoDto();
            result.EstatusContrato = string.Empty;
            result.TienePrestamo = true;
            result.Result = true;
            result.ErrorMessage = string.Empty;

            _prestamoData.Setup(m => m.GetPrestamoEducativo(It.IsAny<PrestamoEducativoEntity>())).Returns(Task.FromResult(result));
            var actualData = await _prestamoService.GetPrestamoEducativo(It.IsAny<PrestamoEducativoEntity>());
            Assert.IsType<PrestamoEducativoDto>(actualData);
            Assert.True(actualData.Result);
        }

        [Fact]
        public async Task GetPrestamoEducativo_Failure()
        {
            PrestamoEducativoDto result = new PrestamoEducativoDto();
            result.Result = false;
            result.ErrorMessage = string.Empty;

            _prestamoData.Setup(m => m.GetPrestamoEducativo(It.IsAny<PrestamoEducativoEntity>())).Returns(Task.FromResult(result));
            var actualData = await _prestamoService.GetPrestamoEducativo(It.IsAny<PrestamoEducativoEntity>());
            Assert.IsType<PrestamoEducativoDto>(actualData);
            Assert.False(actualData.Result);
        }
    }
}
