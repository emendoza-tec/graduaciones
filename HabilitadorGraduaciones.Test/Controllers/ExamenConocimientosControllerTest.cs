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
    public class ExamenConocimientosControllerTest
    {
        Mock<IExamenConocimientosService> _examenConocimientoService;
        private ExamenConocimientosController _examenConocimientosController;

        public ExamenConocimientosControllerTest()
        {
            _examenConocimientoService = new Mock<IExamenConocimientosService>();
            _examenConocimientosController = new ExamenConocimientosController(_examenConocimientoService.Object);
        }

        [Fact]
        public async Task GetExamenConocimiento_Success()
        {
            EndpointsDto dtoEndpoints = new()
            {
                NumeroMatricula = "A00828911",
                ClaveProgramaAcademico = "IMT19",
                ClaveCarrera = "IMT",
                ClaveNivelAcademico = "05",
                ClaveEjercicioAcademico = "202311",
                ClaveCampus = "A"
            };

            var dto = new ExamenConocimientosDto()
            {
                IdTipoExamen = 1,
                DescripcionExamen = "CENEVAL",
                TituloExamen = "Examen de Conocimientos",
                CumpleRequisito = false,
                Estatus = null,
                FechaExamen = Convert.ToDateTime("2023-09-07T00:00:00"),
                FechaRegistro = Convert.ToDateTime("2023-05-12T15:50:03.8282843-06:00"),
                EsRequisito = true,
                Result = true
            };

            _examenConocimientoService.Setup(x => x.GetExamenConocimiento(dtoEndpoints)).Returns(Task.FromResult(dto));

            var responseController = await _examenConocimientosController.GetExamenConocimiento(dtoEndpoints);
            var actual = responseController.Result as ObjectResult;
            var response = (ExamenConocimientosDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<ExamenConocimientosDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GetExamenConocimiento_Failure()
        {
            EndpointsDto dtoEndpoints = new()
            {
                NumeroMatricula = "A00828911",
                ClaveProgramaAcademico = "IMT19",
                ClaveCarrera = "IMT",
                ClaveNivelAcademico = "05",
                ClaveEjercicioAcademico = "202311",
                ClaveCampus = "A"
            };

            var dto = new ExamenConocimientosDto()
            {
                IdTipoExamen = 0,
                DescripcionExamen = null,
                TituloExamen = null,
                CumpleRequisito = false,
                Estatus = null,
                FechaExamen = DateTime.Now,
                FechaRegistro = DateTime.Now,
                EsRequisito = true,
                Result = false
            };

            _examenConocimientoService.Setup(x => x.GetExamenConocimiento(dtoEndpoints)).Returns(Task.FromResult(dto));

            var responseController = await _examenConocimientosController.GetExamenConocimiento(dtoEndpoints);
            var actual = responseController.Result as ObjectResult;
            var response = (ExamenConocimientosDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<ExamenConocimientosDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task GetExamenConocimientoPorLenguaje_Success()
        {
            var entity = new TipoExamenConocimientosEntity()
            {
                IdTipoExamen = 1,
                Descripcion = "CENEVAL",
                Titulo = "Examen de Conocimientos",
                Nota = "Es un requisito de graduación haber presentado el examen externo, autorizado por el Tecnológico de Monterrey, para evaluar los conocimientos adquiridos durante tu carrera profesional.<br /><br /> Este requisito se debe registrar de manera previa y es aplicable solamente para los alumnos de las carreras profesionales para las que existan dichos exámenes.",
                Link = "Con tu director de programa",
                Result = true
            };

            int tipo = 1;
            string lenguaje = "es";

            _examenConocimientoService.Setup(x => x.GetExamenConocimientoPorLenguaje(tipo, lenguaje)).Returns(Task.FromResult(entity));

            var responseController = await _examenConocimientosController.GetExamenConocimientoPorLenguaje(tipo, lenguaje);
            var actual = responseController.Result as ObjectResult;
            var response = (TipoExamenConocimientosEntity)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<TipoExamenConocimientosEntity>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GetExamenConocimientoPorLenguaje_Failure()
        {
            var entity = new TipoExamenConocimientosEntity()
            {
                IdTipoExamen = 1,
                Descripcion = "CENEVAL",
                Titulo = "Examen de Conocimientos",
                Nota = "Es un requisito de graduación haber presentado el examen externo, autorizado por el Tecnológico de Monterrey, para evaluar los conocimientos adquiridos durante tu carrera profesional.<br /><br /> Este requisito se debe registrar de manera previa y es aplicable solamente para los alumnos de las carreras profesionales para las que existan dichos exámenes.",
                Link = "Con tu director de programa",
                Result = false
            };

            int tipo = 1;
            string lenguaje = "es";

            _examenConocimientoService.Setup(x => x.GetExamenConocimientoPorLenguaje(tipo, lenguaje)).Returns(Task.FromResult(entity));

            var responseController = await _examenConocimientosController.GetExamenConocimientoPorLenguaje(tipo, lenguaje);
            var actual = responseController.Result as ObjectResult;
            var response = (TipoExamenConocimientosEntity)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<TipoExamenConocimientosEntity>(actual.Value);
            Assert.False(response.Result);
        }

    }
}