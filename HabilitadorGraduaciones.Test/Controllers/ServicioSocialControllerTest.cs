using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class ServicioSocialControllerTest
    {
        readonly Mock<IServicioSocialService> _servicioSocialService;
        private readonly ServicioSocialController _servicioSocialController;

        public ServicioSocialControllerTest()
        {
            _servicioSocialService = new Mock<IServicioSocialService>();
            _servicioSocialController = new ServicioSocialController(_servicioSocialService.Object);
        }

        [Fact]
        public async Task ConsultarServicioSocial_Success()
        {
            var dto = new EndpointsDto()
            {
                NumeroMatricula = "A01657427",
                ClaveCampus = "2",
                ClaveCarrera = "BGB",
                ClaveEjercicioAcademico = "202213",
                ClaveNivelAcademico = "05",
                ClaveProgramaAcademico = "BGB19",
                Correo = "mirohu51528172@tec.mx"
            };

            var expectedData = new ServicioSocialDto()
            {
                Result = true,
                ErrorMessage = string.Empty,
                Carrera = "BGB",
                ClaveIdentidad = "A01657427",
                Lista_Horas = new List<HorasDto>()
                {
                    new HorasDto()
                    {
                        HoraAcreditada = "Horas acreditadas",
                        ValorAcreditada = 200,
                        HoraRequisito = "Horas requisito",
                        ValorRequisito = 480
                    }
                },
                UltimaActualizacionSS = DateTime.Now,
                isCumpleSS = false,
                isServicioSocial = true
            };

            _servicioSocialService.Setup(m => m.GetServicioSocial(dto)).Returns(Task.FromResult(expectedData));
            var resultado = await _servicioSocialController.ConsultarServicioSocial(dto);
            var actual = resultado.Result as ObjectResult;
            var response = (ServicioSocialDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<ServicioSocialDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task ConsultarServicioSocial_Failure()
        {
            var dto = new EndpointsDto()
            {
                NumeroMatricula = "A01657427",
                ClaveCampus = "2",
                ClaveCarrera = "BGB",
                ClaveEjercicioAcademico = "202213",
                ClaveNivelAcademico = "05",
                ClaveProgramaAcademico = "BGB19",
                Correo = "mirohu51528172@tec.mx"
            };

            var expectedData = new ServicioSocialDto()
            {
                Result = false,
                ErrorMessage = string.Empty
            };

            _servicioSocialService.Setup(m => m.GetServicioSocial(dto)).Returns(Task.FromResult(expectedData));
            var resultado = await _servicioSocialController.ConsultarServicioSocial(dto);
            var actual = resultado.Result as ObjectResult;
            var response = (ServicioSocialDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<ServicioSocialDto>(actual.Value);
            Assert.False(response.Result);
        }
    }
}
