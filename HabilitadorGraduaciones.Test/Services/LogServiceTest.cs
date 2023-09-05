using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class LogServiceTest
    {
        readonly Mock<ILogRepository> _logData;
        readonly LogService _logService;

        public LogServiceTest()
        {
            _logData = new Mock<ILogRepository>();
            _logService = new LogService(_logData.Object);
        }

        [Fact]
        public async Task GuardarLog_Success()
        {
            BaseOutDto expectedData = new BaseOutDto { Result = true, ErrorMessage = string.Empty };

            _logData.Setup(m => m.GuardarLog(It.IsAny<LogEnteradoDto>())).Returns(Task.FromResult(expectedData));
            var actualData = await _logService.GuardarLog(It.IsAny<LogEnteradoDto>());
            Assert.IsType<BaseOutDto>(actualData);
            Assert.True(actualData.Result);
        }

        [Fact]
        public async Task GuardarLog_Failure()
        {
            BaseOutDto expectedData = new BaseOutDto { Result = false, ErrorMessage = string.Empty };

            _logData.Setup(m => m.GuardarLog(It.IsAny<LogEnteradoDto>())).Returns(Task.FromResult(expectedData));
            var actualData = await _logService.GuardarLog(It.IsAny<LogEnteradoDto>());
            Assert.IsType<BaseOutDto>(actualData);
            Assert.False(actualData.Result);
        }
    }
}
