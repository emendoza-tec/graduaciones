using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class ReporteSabanaControllerTest
    {
        Mock<IReporteSabanaService> reporteService;
        private SabanaController reporteController;

        public ReporteSabanaControllerTest()
        {
            reporteService = new Mock<IReporteSabanaService>();
            reporteController = new SabanaController(reporteService.Object);
        }
        [Fact]
        public async Task DescargarExcelReporteSabana_Success()
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
            var list = new List<SabanaEntity>
            {
                new SabanaEntity()
                {
                    Matricula = "A01612358",
                    NombreCompleto = "Sebastián Domínguez López",
                    ClaveProgramaAcademico = "NEG19",
                    ConcentracionUno = "Conc o Acen no elegida",
                    ConcentracionDos = "",
                    ConcentracionTres = "",
                    Ulead = "No",DiplomaInternacional = "",  Genero = "M",Nacionalidad = "0001",Telefono = "4444901997552",Correo = "sedolo54757931@tec.mx",Periodo = "202013",Campus = "Campus San Luis Potosí",
                    NivelAcademico = "Profesional", CreditosPlan = "54", CreditosPendientes = "33", CreditosAcreditados = "21",CreditosFaltantes = "33",CreditosPeriodo = "",SemanasTec = "3",
                    ServicioSocialHt = "",ServicioSocialEstatus = "NC",ExamenIngles = "",ExamenInglesEstatus = "",ExamenInglesFecha = "",ExamenInglesPuntaje = "", Ceneval = "",
                    CenevalEstatus = "",ExamenIntegrador = "",NivelIdiomaRequerido = "",
                    IdiomaDistEsp = "",CreditosCursadosExtranjero = 0, Promedio = "",  FechaRegistro = new DateTime(),Shadegr = "SO",PeriodoCeremonia = ""
                }
            };

            //Prueba
            reporteService.Setup(m => m.GetReporteSabana(usuario)).Returns(Task.FromResult(list));
            var resultado = await reporteController.DescargarExcelReporteSabana(usuario);

            //Verificacion
            Assert.NotNull(resultado);
        }
        [Fact]
        public async Task DescargarExcelReporteSabana_Failure()
        {
            //Preparacion    
            var usuario = new UsuarioAdministradorDto();
            var list = new List<SabanaEntity>();

            //Prueba
            reporteService.Setup(m => m.GetReporteSabana(usuario)).Returns(Task.FromResult(list));
            var resultado = await reporteController.DescargarExcelReporteSabana(usuario);
            var actual = resultado as ObjectResult;

            //Verificacion
            Assert.Null(actual);
        }
    }
}
