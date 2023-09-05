using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using HabilitadorGraduaciones.Services.Interfaces;
using Moq;

namespace HabilitadorGraduaciones.Test.Services
{
    public class ProgramaBgbTest
    {
        readonly Mock<IProgramaBgbRepository> _programaBgbData;
        readonly ProgramaBgbService _programaBgbService;
        readonly Mock<IApiService> _apiService;

        public ProgramaBgbTest()
        {
            _programaBgbData = new Mock<IProgramaBgbRepository>();
            _apiService = new Mock<IApiService>();
            _programaBgbService = new ProgramaBgbService(_programaBgbData.Object, _apiService.Object);
        }

        //[Fact]
        //public void ProgramaBGBApi_Succsess()
        //{
        //    var dto = new EndpointsDto()
        //    {
        //        NumeroMatricula = "A01657427",
        //        ClaveCampus = "2",
        //        ClaveCarrera = "BGB",
        //        ClaveEjercicioAcademico = "202213",
        //        ClaveNivelAcademico = "05",
        //        ClaveProgramaAcademico = "BGB19",
        //        Correo = "mirohu51528172@tec.mx"
        //    };

        //    var sesion = new Sesion()
        //    {
        //        FechaCreacion = DateTime.Now,
        //        FechaExpiracion = DateTime.Today,
        //        JwtToken = "eyJraWQiOiJoczI1Ni1rZXkiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJBMDE2NTc0MjciLCJzdWIiOiJzdWIiLCJhdWQiOiJhdWQiLCJleHAiOjE2ODkxMTg0MDAsImlhdCI6MTY4OTExNDgwMH0.8EEFaMPS4Lq30BYrFFlBljXXPDlk3o_V9CRhgcDVHSs",
        //        OAuthToken = "AAIgNjQwM2U0Y2MwYTEwNDZlZGM1NTI2NjhhYjU1ZGM5MGLcD6Ip0knMgQt0A2hEP5y9_k7zHYqSuxw9IYwo1Vkz79wiMPIrD_0xamzGimv4Yc5YP65z3b8YeYmG1V_-4qHV0EsbF42st9FpeH45lYZ2i65lbS97THa8OWgIUEU7O8Q",
        //        Matricula = "A01657427"
        //    };

        //    var expectedData = new ProgramaBgbDto()
        //    {
        //        ClavePrograma = "BGB19",
        //        CreditosAprobadosProgramaInternacional = 18,
        //        CreditosDeInglesAprobadosCuartoSemestre = 4,
        //        CreditosDeInglesAprobadosQuintoSemestre = 4,
        //        CreditosDeInglesAprobadosOctavoSemestre = 0,
        //        CumpleProgramaInternacional = true,
        //        CumpleRequisitoInglesCuarto = true,
        //        CumpleRequisitoInglesQuinto = true,
        //        CumpleRequisitoInglesOctavo = false,
        //        CumpleRequisitosProgramaEspecial = false,
        //        ErrorMessage = string.Empty,
        //        Result = true,
        //        UltimaActualizacion = DateTime.Now
        //    };
        //    expectedData.Result = true;
        //    expectedData.ErrorMessage = string.Empty;

        //    _programaBgbData.Setup(m =>m.ProgramaBGBApi(dto, sesion)).Returns(expectedData);

        //    var actualData = _programaBgbService.ProgramaBGBApi(dto);
        //    Assert.Equal(expectedData, actualData);
        //}
    }
}
