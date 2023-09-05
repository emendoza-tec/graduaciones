using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class LogControllerTest
    {
        readonly Mock<ILogService> _logService;
        private readonly LogController _logController;

        public LogControllerTest()
        {
            _logService = new Mock<ILogService>();
            _logController = new LogController(_logService.Object);
        }

        [Fact]
        public async Task GuardarLog_Success()
        {
            BaseOutDto result = new BaseOutDto { Result = true, ErrorMessage = string.Empty };

            _logService.Setup(m => m.GuardarLog(It.IsAny<LogEnteradoDto>())).Returns(Task.FromResult(result));
            var resultado = await _logController.GuardarLog(It.IsAny<LogEnteradoDto>());
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(response);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GuardarLog_Failure()
        {
            BaseOutDto result = new BaseOutDto { Result = false, ErrorMessage = string.Empty };

            _logService.Setup(m => m.GuardarLog(It.IsAny<LogEnteradoDto>())).Returns(Task.FromResult(result));
            var resultado = await _logController.GuardarLog(It.IsAny<LogEnteradoDto>());
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(response);
            Assert.False(response.Result);
        }
    }
}
