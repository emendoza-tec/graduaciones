using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class LinksControllerTest
    {
        readonly Mock<ILinksService> _linksService;
        private readonly LinksController _linksController;

        public LinksControllerTest()
        {
            _linksService = new Mock<ILinksService>();
            _linksController = new LinksController(_linksService.Object);
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

            _linksService.Setup(m => m.GetLinks()).Returns(expectedData);
            var resultado = _linksController.GetLinks();
            var actual = resultado.Result as ObjectResult;
            var response = (LinksDto)actual.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<LinksDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public void GetLinks_Failure()
        {
            var expectedData = new LinksDto()
            {
                Result = false,
                ErrorMessage = string.Empty
            };

            _linksService.Setup(m => m.GetLinks()).Returns(expectedData);
            var resultado = _linksController.GetLinks();
            var actual = resultado.Result as ObjectResult;
            var response = (LinksDto)actual.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<LinksDto>(actual.Value);
            Assert.False(response.Result);
        }
    }
}
