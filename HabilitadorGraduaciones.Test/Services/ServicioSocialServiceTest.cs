using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class ServicioSocialServiceTest
    {
        readonly Mock<IServicioSocialRepository> _servicioSocialData;
        readonly ServicioSocialService _servicioSocialService;

        public ServicioSocialServiceTest()
        {
            _servicioSocialData = new Mock<IServicioSocialRepository>();
            _servicioSocialService = new ServicioSocialService(_servicioSocialData.Object);
        }

        [Fact]
        public async Task GetServicioSocial_Success()
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

            _servicioSocialData.Setup(m => m.ConsultarHorasServicioSocial(dto)).Returns(Task.FromResult(expectedData));
            var actualData = await _servicioSocialService.GetServicioSocial(dto);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task GetServicioSocial_Failure()
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

            _servicioSocialData.Setup(m => m.ConsultarHorasServicioSocial(dto)).Returns(Task.FromResult(expectedData));
            var actualData = await _servicioSocialService.GetServicioSocial(dto);
            Assert.IsType<ServicioSocialDto>(actualData);
            Assert.False(actualData.Result);
        }
    }
}
