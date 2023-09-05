using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class ReporteEstimadoGraduacionTest
    {
        Mock<IReporteEstimadoRepository> reporteData;
        private ReporteEstimadoDeGraduacionService reporteService;

        public ReporteEstimadoGraduacionTest()
        {
            reporteData = new Mock<IReporteEstimadoRepository>();
            reporteService = new ReporteEstimadoDeGraduacionService(reporteData.Object);
        }
        [Fact]
        public async Task DescargarExcelReporteEG_Success()
        {
            //Preparacion    
            var usuario = new UsuarioAdministradorDto
            {
                Nomina = "L00828911",
                IdUsuario = 1,
                Nombre = "Alejandro Tamez Galindo",
                Correo = "altaga39332045@tec.mx",
                Estatus = 1,
                ListCampus = new List<Campus> { new Campus() { ClaveCampus = "A", Descripcion = "A" } },
                Sedes = new List<Sede> { new Sede() { ClaveSede = "AGS", Descripcion = "AGS" } },
                Niveles = new List<Nivel> { new Nivel() { ClaveNivel = "05", Descripcion = "Profesional" } },
                Result = true
            };
            var list = new List<ReporteEstimadoDeGraduacionEntity>
            {
                new ReporteEstimadoDeGraduacionEntity()
                {
                    Matricula = "A01620154",
                    Nombre = "Alejandro López Macías",
                    Carrera = "Ing-Innova y Transformación",
                    Campus = "Campus Aguascalientes",
                    Sede = "Campus AGS Sede Aguascalientes",
                    PeriodoEstimado = "",
                    Confirmacion = "No",
                    PeriodoConfirmadoG = "",
                    FechaConfirmacionOCambio = new DateTime(),
                    Motivo = "",
                    CantidadCambiosRealizados = 0,
                    RegistroAsistenciaPeriodoIntensivo = "",
                    MotivoPeriodoIntensivo = ""
                },
                new ReporteEstimadoDeGraduacionEntity()
                {
                    Matricula = "A01620337",
                    Nombre = "Fabián Christoph Haubi Vila-Olmos",
                    Carrera = "Ing. Industrial y de Sistemas",
                    Campus = "Campus Aguascalientes",
                    Sede = "Campus AGS Sede Aguascalientes",
                    PeriodoEstimado = "",
                    Confirmacion = "No",
                    PeriodoConfirmadoG = "",
                    FechaConfirmacionOCambio = new DateTime(),
                    Motivo = "",
                    CantidadCambiosRealizados = 0,
                    RegistroAsistenciaPeriodoIntensivo = "",
                    MotivoPeriodoIntensivo = ""
                },
                new ReporteEstimadoDeGraduacionEntity()
                {
                    Matricula = "A00369688",
                    Nombre = "Mariana Velázquez Duarte",
                    Carrera = "Ing. Industrial y de Sistemas",
                    Campus = "Campus Aguascalientes",
                    Sede = "Campus AGS Sede Aguascalientes",
                    PeriodoEstimado = "",
                    Confirmacion = "No",
                    PeriodoConfirmadoG = "",
                    FechaConfirmacionOCambio = new DateTime(),
                    Motivo = "",
                    CantidadCambiosRealizados = 0,
                    RegistroAsistenciaPeriodoIntensivo = "",
                    MotivoPeriodoIntensivo = ""
                },
                new ReporteEstimadoDeGraduacionEntity()
                {
                    Matricula = "A01625344",
                    Nombre = "Sarah Johanna Melissa Nájera Plessmann",
                    Carrera = "Lic. en Animación y Arte Dig.",
                    Campus = "Campus Aguascalientes",
                    Sede = "Campus AGS Sede Aguascalientes",
                    PeriodoEstimado = "",
                    Confirmacion = "No",
                    PeriodoConfirmadoG = "",
                    FechaConfirmacionOCambio = new DateTime(),
                    Motivo = "",
                    CantidadCambiosRealizados = 0,
                    RegistroAsistenciaPeriodoIntensivo = "",
                    MotivoPeriodoIntensivo = ""
                },
                new ReporteEstimadoDeGraduacionEntity()
                {
                    Matricula = "A01625301",
                    Nombre = "Carlos Eduardo Torres Rodríguez",
                    Carrera = "Lic. en Tecnología y Produc.",
                    Campus = "Campus Aguascalientes",
                    Sede = "Campus AGS Sede Aguascalientes",
                    PeriodoEstimado = "",
                    Confirmacion = "No",
                    PeriodoConfirmadoG = "",
                    FechaConfirmacionOCambio = new DateTime(),
                    Motivo = "",
                    CantidadCambiosRealizados = 0,
                    RegistroAsistenciaPeriodoIntensivo = "",
                    MotivoPeriodoIntensivo = ""
                },
                new ReporteEstimadoDeGraduacionEntity()
                {
                    Matricula = "A01625437",
                    Nombre = "Saúl Valdivia Sandoval",
                    Carrera = "Lic. en Inteligencia de Neg.",
                    Campus = "Campus Aguascalientes",
                    Sede = "Campus AGS Sede Aguascalientes",
                    PeriodoEstimado = "",
                    Confirmacion = "No",
                    PeriodoConfirmadoG = "",
                    FechaConfirmacionOCambio = new DateTime(),
                    Motivo = "",
                    CantidadCambiosRealizados = 0,
                    RegistroAsistenciaPeriodoIntensivo = "",
                    MotivoPeriodoIntensivo = ""
                }
            };

            //Prueba

            reporteData.Setup(m => m.GetReporteEstimadoDeGraduacion(usuario)).Returns(Task.FromResult(list));

            var actualData = await reporteService.GetReporteEstimadoDeGraduacion(usuario);
            Assert.Equal(list, actualData);
        }
        [Fact]
        public async Task DescargarExcelReporteEG_Failure()
        {
            //Preparacion    
            var usuario = new UsuarioAdministradorDto();
            var list = new List<ReporteEstimadoDeGraduacionEntity>();

            //Prueba
            reporteData.Setup(m => m.GetReporteEstimadoDeGraduacion(usuario)).Returns(Task.FromResult(list));

            var actualData = await reporteService.GetReporteEstimadoDeGraduacion(usuario);

            Assert.IsType<List<ReporteEstimadoDeGraduacionEntity>>(actualData);
            Assert.False(actualData.Count > 0);
        }
    }
}
