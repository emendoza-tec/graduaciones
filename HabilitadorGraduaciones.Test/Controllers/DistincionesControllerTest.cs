

using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class DistincionesControllerTest
    {
        readonly Mock<IDistincionesService> _distincionesService;
        private readonly DistincionesController _distincionesController;

        public DistincionesControllerTest()
        {
            _distincionesService = new Mock<IDistincionesService>();
            _distincionesController = new DistincionesController(_distincionesService.Object);
        }

        [Fact]
        public async Task GetDistinciones_Success()
        {
            EndpointsDto dto = new EndpointsDto
            {
                ClaveCampus = "A",
                ClaveCarrera = "ITC",
                ClaveEjercicioAcademico = "202213",
                ClaveNivelAcademico = "05",
                ClaveProgramaAcademico = "ITC19",
                Correo = "vamoel42686857@tec.mx",
                NumeroMatricula = "A01280603"
            };

            DistincionesDto result = new DistincionesDto
            {
                Result = true,
                ErrorMessage = string.Empty,
                Diploma = string.Empty,
                DiplomaOk = true,
                HasDiploma = true,
                Ulead = string.Empty,
                UleadOk = true,
                HasUlead = true,
                LstConcentracion = new List<string>
                {
                    "Concentración de prueba 1"
                },
                ClaveEjercicioAcademico = "202213",
                ClaveNivelAcademico = "05",
                NumeroMatricula = "A01280603"
            };

            _distincionesService.Setup(m => m.GetDistincionesService(dto)).Returns(Task.FromResult(result));
            var resultado = await _distincionesController.GetDistinciones(dto);
            var actual = resultado.Result as ObjectResult;
            var response = (DistincionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<DistincionesDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GetDistinciones_Failure()
        {
            EndpointsDto dto = new EndpointsDto
            {
                ClaveCampus = "A",
                ClaveCarrera = "ITC",
                ClaveEjercicioAcademico = "202213",
                ClaveNivelAcademico = "05",
                ClaveProgramaAcademico = "ITC19",
                Correo = "vamoel42686857@tec.mx",
                NumeroMatricula = "A01280603"
            };

            DistincionesDto result = new DistincionesDto
            {
                Result = false,
                ErrorMessage = string.Empty
            };

            _distincionesService.Setup(m => m.GetDistincionesService(dto)).Returns(Task.FromResult(result));
            var resultado = await _distincionesController.GetDistinciones(dto);
            var actual = resultado.Result as ObjectResult;
            var response = (DistincionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<DistincionesDto>(actual.Value);
            Assert.False(response.Result);
        }
    }
}
