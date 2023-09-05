using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class LinksServiceTest
    {
        readonly Mock<ILinksRepository> _linksData;
        readonly LinksService _linksService;

        public LinksServiceTest()
        {
            _linksData = new Mock<ILinksRepository>();
            _linksService = new LinksService(_linksData.Object);
        }

        [Fact]
        public void GetLinks_Success()
        {
            var expectedData = new LinksDto()
            {
                DatosPersonales = "https://www.google.com.mx/",
                PrestamoEducativo = "https://sites.google.com/tec.mx/formalizacioncreditoeducativo/página-principal?authuser=0&pli=1",
                Tesoreria = "https://estadodecuentapprd.tec.mx/#/",
                Result = true,
                ErrorMessage = string.Empty
            };

            _linksData.Setup(x => x.GetLinks()).Returns(expectedData);

            var actualData = _linksService.GetLinks();
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetLinks_Failure()
        {
            var expectedData = new LinksDto()
            {
                Result = false,
                ErrorMessage = string.Empty
            };

            _linksData.Setup(x => x.GetLinks()).Returns(expectedData);

            var actualData = _linksService.GetLinks();
            Assert.IsType<LinksDto>(actualData);
            Assert.False(expectedData.Result);
        }
    }
}
