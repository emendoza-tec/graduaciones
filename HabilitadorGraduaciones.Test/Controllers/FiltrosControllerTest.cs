using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Data.Utils.Enums;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class FiltrosControllerTest
    {
        Mock<IAvisosService> _filtrosService;
        private FiltrosController _filtrosController;

        public FiltrosControllerTest()
        {
            _filtrosService = new Mock<IAvisosService>();
            _filtrosController = new FiltrosController(_filtrosService.Object);
        }

        [Fact]
        public async Task GetFiltro_Success()
        {
            var list = new List<CatalogoDto>()
            {
                new CatalogoDto()
                {
                     Clave = "A62",
                     Descripcion = "Administración de Riesgos",
                     Result = true,
                     ErrorMessage = string.Empty
                },
                new CatalogoDto()
                {
                     Clave = "1",
                     Descripcion = "Campus Aguascalientes",
                     Result = true,
                     ErrorMessage = string.Empty
                },
                new CatalogoDto()
                {
                     Clave = "R",
                     Descripcion = "Campus Chiapas",
                     Result = true,
                     ErrorMessage = string.Empty
                }
            };

            _filtrosService.Setup(m => m.ObtenerCatalogo((int)Filtros.Campus)).Returns(Task.FromResult(list));

            var responseController = await _filtrosController.GetFiltro((int)Filtros.Campus);
            var actual = responseController.Result as ObjectResult;
            var response = (List<CatalogoDto>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<CatalogoDto>>(actual.Value);
            Assert.True(response.Count > 0);
        }

        [Fact]
        public async Task GetAcceso_Failure()
        {
            var list = new List<CatalogoDto>();

            _filtrosService.Setup(m => m.ObtenerCatalogo(It.IsAny<int>())).Returns(Task.FromResult(list));

            var responseController = await _filtrosController.GetFiltro(It.IsAny<int>());
            var actual = responseController.Result as ObjectResult;
            var response = (List<CatalogoDto>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<CatalogoDto>>(actual.Value);
            Assert.False(response.Count > 0);
        }


        [Fact]
        public async Task ObtenerCatalogo_Success()
        {
            var list = new List<CatalogoDto>()
            {
                new CatalogoDto()
                {
                     Clave = "A62",
                     Descripcion = "Administración de Riesgos",
                     Result = true,
                     ErrorMessage = string.Empty
                },
                new CatalogoDto()
                {
                     Clave = "1",
                     Descripcion = "Campus Aguascalientes",
                     Result = true,
                     ErrorMessage = string.Empty
                },
                new CatalogoDto()
                {
                     Clave = "R",
                     Descripcion = "Campus Chiapas",
                     Result = true,
                     ErrorMessage = string.Empty
                }
            };

            _filtrosService.Setup(m => m.ObtenerCatalogo((int)Filtros.Campus)).Returns(Task.FromResult(list));

            var responseController = await _filtrosController.GetFiltro((int)Filtros.Campus);
            var actual = responseController.Result as ObjectResult;
            var response = (List<CatalogoDto>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<CatalogoDto>>(actual.Value);
            Assert.True(response.Count > 0);
        }

        [Fact]
        public async Task ObtenerCatalogo_Failure()
        {
            var list = new List<CatalogoDto>();

            _filtrosService.Setup(m => m.ObtenerCatalogo(It.IsAny<int>())).Returns(Task.FromResult(list));

            var responseController = await _filtrosController.GetFiltro(It.IsAny<int>());
            var actual = responseController.Result as ObjectResult;
            var response = (List<CatalogoDto>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<CatalogoDto>>(actual.Value);
            Assert.False(response.Count > 0);
        }
    }
}
