using AutoMapper;
using HabilitadorGraduaciones.Core.Automapper;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities.Expediente;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class ExpedienteControllerTest
    {
        private readonly Mock<IExpedienteService> _expedienteService;
        private readonly ExpedienteController _expedienteController;

        public ExpedienteControllerTest()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper();
            _expedienteService = new Mock<IExpedienteService>();
            _expedienteController = new ExpedienteController(mapper, _expedienteService.Object);
        }

        [Fact]
        public async Task GetByAlumno_Success()
        {
            var expectedData = new ExpedienteEntity()
            {
                Estatus = "Completo",
                Detalle = "PRUEBA MENSAJE 5862",
                UltimaActualizacion = Convert.ToDateTime("2023-06-22T00:00:00"),
                Result = true
            };

            _expedienteService.Setup(m => m.GetByAlumno(It.IsAny<string>())).Returns(Task.FromResult(expectedData));

            var responseController = await _expedienteController.GetByAlumno(It.IsAny<string>());
            var actual = responseController.Result as ObjectResult;
            var response = (ExpedienteOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<ExpedienteOutDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GetByAlumno_Failure()
        {
            var expectedData = new ExpedienteEntity();

            _expedienteService.Setup(m => m.GetByAlumno(It.IsAny<string>())).Returns(Task.FromResult(expectedData));

            var responseController = await _expedienteController.GetByAlumno(It.IsAny<string>());
            var actual = responseController.Result as ObjectResult;
            var response = (ExpedienteOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<ExpedienteOutDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task ConsultarComentarios_Success()
        {
            string matricula = "A00828911";
            var expectedData = new List<ExpedienteEntity>()
            {
                new ExpedienteEntity()
                {
                    Detalle = "PRUEBA MENSAJE 5862",
                    UltimaActualizacion = Convert.ToDateTime("2023-06-22T00:00:00"),
                    Result = true
                },
                new ExpedienteEntity()
                {
                    Detalle = "PRUEBA MENSAJE 5863",
                    UltimaActualizacion = Convert.ToDateTime("2023-06-22T00:00:00"),
                    Result = true
                }
            };

            _expedienteService.Setup(m => m.ConsultarComentarios(matricula)).Returns(Task.FromResult(expectedData));

            var responseController = await _expedienteController.ConsultarComentarios(matricula);
            var actual = responseController.Result as ObjectResult;
            var response = (List<ExpedienteOutDto>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<ExpedienteOutDto>>(actual.Value);
            Assert.True(response.Count > 0);
        }

        [Fact]
        public async Task ConsultarComentarios_Failure()
        {
            var expectedData = new List<ExpedienteEntity>();

            _expedienteService.Setup(m => m.ConsultarComentarios(It.IsAny<string>())).Returns(Task.FromResult(expectedData));

            var responseController = await _expedienteController.ConsultarComentarios(It.IsAny<string>());
            var actual = responseController.Result as ObjectResult;
            var response = (List<ExpedienteOutDto>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<ExpedienteOutDto>>(actual.Value);
            Assert.False(response.Count > 0);
        }

    }
}
