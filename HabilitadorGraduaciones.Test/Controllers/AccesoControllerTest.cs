using AutoMapper;
using HabilitadorGraduaciones.Core.Automapper;
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
    public class AccesoControllerTest
    {
        Mock<IAccesosNominaService> _accesosNominaService;
        private AccesoController _accesoController;

        public AccesoControllerTest()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper();
            _accesosNominaService = new Mock<IAccesosNominaService>();
            _accesoController = new AccesoController(mapper, _accesosNominaService.Object);
        }

        [Fact]
        public async Task GetAcceso_Success()
        {
            var expectedData = new AccesosNominaEntity()
            {
                Matricula = "L00828911",
                Ambiente = "PPRD",
                Acceso = true
            };

            _accesosNominaService.Setup(m => m.GetAcceso(It.IsAny<string>())).Returns(Task.FromResult(expectedData));

            var responseController = await _accesoController.GetAcceso(It.IsAny<string>());
            var actual = responseController.Result as ObjectResult;
            var response = (AccesosNominaDto)actual?.Value;

            // Assert
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<AccesosNominaDto>(actual.Value);
            Assert.True(response.Acceso);
        }

        [Fact]
        public async Task GetAcceso_Failure()
        {
            var expectedData = new AccesosNominaEntity()
            {
                Matricula = "L00828911",
                Ambiente = "PPRD",
                Acceso = false
            };

            _accesosNominaService.Setup(m => m.GetAcceso(It.IsAny<string>())).Returns(Task.FromResult(expectedData));

            var responseController = await _accesoController.GetAcceso(It.IsAny<string>());
            var actual = responseController.Result as ObjectResult;
            var response = (AccesosNominaDto)actual?.Value;

            // Assert
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<AccesosNominaDto>(actual.Value);
            Assert.False(response.Acceso);
        }
    }
}
