using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using HabilitadorGraduaciones.Web.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class SolicitudDeCambioDeDatosControllerTest
    {
        private readonly Mock<ISolicitudDeCambioDeDatosService> _solicitudesService;
        private readonly SolicitudDeCambioDeDatosController _solicitudesController;
        private readonly Mock<IWebHostEnvironment> _env;
        private readonly Mock<IArchivoStorage> _archivoStorage;

        public SolicitudDeCambioDeDatosControllerTest()
        {
            _env = new Mock<IWebHostEnvironment>();
            _archivoStorage = new Mock<IArchivoStorage>();
            _solicitudesService = new Mock<ISolicitudDeCambioDeDatosService>();
            _solicitudesController = new SolicitudDeCambioDeDatosController(_env.Object, _solicitudesService.Object);

        }

        [Fact]
        public async Task GetEstatusSolicitudes_Success()
        {
            var list = new List<EstatusSolicitudDatosPersonalesEntity>()
            {
                new EstatusSolicitudDatosPersonalesEntity()
                {
                    IdEstatusSolicitud = 1,
                    Descripcion = "No abierta",
                },
                new EstatusSolicitudDatosPersonalesEntity()
                {
                    IdEstatusSolicitud = 2,
                    Descripcion = "Solicitud en revisión",
                },
                new EstatusSolicitudDatosPersonalesEntity()
                {
                    IdEstatusSolicitud = 3,
                    Descripcion = "Solicitud aprobada",
                },
                new EstatusSolicitudDatosPersonalesEntity()
                {
                    IdEstatusSolicitud = 4,
                    Descripcion = "Solicitud no aprobada",
                }
            };

            _solicitudesService.Setup(m => m.GetEstatusSolicitudes()).Returns(Task.FromResult(list));

            var responseController = await _solicitudesController.GetEstatusSolicitudes();
            var actual = responseController.Result as ObjectResult;
            var response = (List<EstatusSolicitudDatosPersonalesEntity>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<EstatusSolicitudDatosPersonalesEntity>>(actual.Value);
            Assert.True(response.Count > 0);


        }

        [Fact]
        public async Task GetEstatusSolicitudes_Failure()
        {
            var list = new List<EstatusSolicitudDatosPersonalesEntity>();

            _solicitudesService.Setup(m => m.GetEstatusSolicitudes()).Returns(Task.FromResult(list));

            var responseController = await _solicitudesController.GetEstatusSolicitudes();
            var actual = responseController.Result as ObjectResult;
            var response = (List<EstatusSolicitudDatosPersonalesEntity>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<EstatusSolicitudDatosPersonalesEntity>>(actual.Value);
            Assert.False(response.Count > 0);
        }

        [Fact]
        public async Task GetPendientes_Success()
        {
            int idUsuario = 10;
            var expectedData = new List<SolicitudDeCambioDeDatosEntity>()
            {
                new SolicitudDeCambioDeDatosEntity()
                {
                    IdSolicitud = 44,
                    NumeroSolicitud = 44,
                    Matricula = "A00828911",
                    PeriodoGraduacion = "202411",
                    IdDatosPersonales = 1,
                    Descripcion = "Nombre",
                    FechaSolicitud = Convert.ToDateTime("2023-06-22"),
                    UltimaActualizacion = Convert.ToDateTime("2023-03-22"),
                    IdEstatusSolicitud = 1,
                    Estatus = "No abierta"
                }
            };

            _solicitudesService.Setup(m => m.GetPendientes(idUsuario)).Returns(Task.FromResult(expectedData));

            var responseController = await _solicitudesController.GetPendientes(idUsuario);
            var actual = responseController.Result as ObjectResult;
            var response = (List<SolicitudDeCambioDeDatosEntity>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<SolicitudDeCambioDeDatosEntity>>(actual.Value);
            Assert.True(response.Count > 0);
        }

        [Fact]
        public async Task GetPendientes_Failure()
        {
            var expectedData = new List<SolicitudDeCambioDeDatosEntity>();

            _solicitudesService.Setup(m => m.GetPendientes(It.IsAny<int>())).Returns(Task.FromResult(expectedData));

            var responseController = await _solicitudesController.GetPendientes(It.IsAny<int>());
            var actual = responseController.Result as ObjectResult;
            var response = (List<SolicitudDeCambioDeDatosEntity>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<SolicitudDeCambioDeDatosEntity>>(actual.Value);
            Assert.True(response.Count == 0);
        }

        [Fact]
        public async Task Get_Success()
        {
            int idUsuario = 10;
            int idEstatusSolicitud = 1;
            var expectedData = new List<SolicitudDeCambioDeDatosEntity>()
            {
                new SolicitudDeCambioDeDatosEntity()
                {
                    IdSolicitud = 44,
                    NumeroSolicitud = 44,
                    Matricula = "A00828911",
                    PeriodoGraduacion = "202411",
                    IdDatosPersonales = 1,
                    Descripcion = "Nombre",
                    FechaSolicitud = Convert.ToDateTime("2023-06-22"),
                    UltimaActualizacion = Convert.ToDateTime("2023-03-22"),
                    IdEstatusSolicitud = 1,
                    Estatus = "No abierta"
                },
                new SolicitudDeCambioDeDatosEntity()
                {
                    IdSolicitud = 45,
                    NumeroSolicitud = 45,
                    Matricula = "A00828911",
                    PeriodoGraduacion = "202411",
                    IdDatosPersonales = 1,
                    Descripcion = "Nombre",
                    FechaSolicitud = Convert.ToDateTime("2023-06-22"),
                    UltimaActualizacion = Convert.ToDateTime("2023-03-22"),
                    IdEstatusSolicitud = 1,
                    Estatus = "No abierta"
                }
            };

            _solicitudesService.Setup(m => m.Get(idUsuario, idEstatusSolicitud)).Returns(Task.FromResult(expectedData));

            var responseController = await _solicitudesController.Get(idUsuario, idEstatusSolicitud);
            var actual = responseController.Result as ObjectResult;
            var response = (List<SolicitudDeCambioDeDatosEntity>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<SolicitudDeCambioDeDatosEntity>>(actual.Value);
            Assert.True(response.Count > 0);
        }

        [Fact]
        public async Task Get_Failure()
        {
            var expectedData = new List<SolicitudDeCambioDeDatosEntity>();

            _solicitudesService.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(expectedData));

            var responseController = await _solicitudesController.Get(It.IsAny<int>(), It.IsAny<int>());
            var actual = responseController.Result as ObjectResult;
            var response = (List<SolicitudDeCambioDeDatosEntity>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<SolicitudDeCambioDeDatosEntity>>(actual.Value);
            Assert.True(response.Count == 0);
        }

        [Fact]
        public async Task GetDetalle_Success()
        {
            int idSolicitud = 44;
            var expectedData = new List<DetalleSolicitudDeCambioDeDatosEntity>()
            {
                new DetalleSolicitudDeCambioDeDatosEntity()
                {
                   IdSolicitud = 44,
                   IdDetalleSolicitud = 20,
                   IdDatosPersonales = 1,
                   FechaSolicitud = Convert.ToDateTime("2023-06-22"),
                   Descripcion = "Nombre",
                   DatoIncorrecto = "Andrea Luna Roldán",
                   DatoCorrecto = "Andrea Luna Rooldán",
                   IdEstatusSolicitud = 1,
                   Estatus = "No abierta",
                   Documento = "Captura de pantalla 2023-03-09 112337.png",
                   Extension = "png",
                   AzureStorage = "Solicitudes/cd53ba96-f98c-4d6e-960d-7545859ca654.png"
                }
            };

            _solicitudesService.Setup(m => m.GetDetalle(idSolicitud)).Returns(Task.FromResult(expectedData));

            var responseController = await _solicitudesController.GetDetalle(idSolicitud);
            var actual = responseController.Result as ObjectResult;
            var response = (List<DetalleSolicitudDeCambioDeDatosEntity>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<DetalleSolicitudDeCambioDeDatosEntity>>(actual.Value);
            Assert.True(response.Count > 0);
        }

        [Fact]
        public async Task GetDetalle_Failure()
        {
            var expectedData = new List<DetalleSolicitudDeCambioDeDatosEntity>();

            _solicitudesService.Setup(m => m.GetDetalle(It.IsAny<int>())).Returns(Task.FromResult(expectedData));

            var responseController = await _solicitudesController.GetDetalle(It.IsAny<int>());
            var actual = responseController.Result as ObjectResult;
            var response = (List<DetalleSolicitudDeCambioDeDatosEntity>)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<DetalleSolicitudDeCambioDeDatosEntity>>(actual.Value);
            Assert.True(response.Count == 0);
        }

        [Fact]
        public async Task ModificaSolicitud_Success()
        {
            var expectedData = new ModificarEstatusSolicitudDto()
            {
                IdSolicitud = 44,
                Matricula = "A00828911",
                IdEstatusSolicitud = 2,
                UsarioRegistro = "L00828911",
                Comentarios = "Pruebas"
            };

            BaseOutDto res = new BaseOutDto { Result = true, ErrorMessage = string.Empty };
            _solicitudesService.Setup(m => m.ModificaSolicitud(expectedData)).Returns(Task.FromResult(res));

            var resultado = await _solicitudesController.ModificaSolicitud(expectedData);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task ModificaSolicitud_Failure()
        {
            var expectedData = new ModificarEstatusSolicitudDto();

            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };
            _solicitudesService.Setup(m => m.ModificaSolicitud(expectedData)).Returns(Task.FromResult(res));

            var resultado = await _solicitudesController.ModificaSolicitud(expectedData);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task GetConteoPendientes_Success()
        {
            int idUsuario = 10;
            var expectedData = new TotalSolicitudesDto()
            {
                Total = 5,
            };

            _solicitudesService.Setup(m => m.GetConteoPendientes(idUsuario)).Returns(Task.FromResult(expectedData));

            var responseController = await _solicitudesController.GetConteoPendientes(idUsuario);
            var actual = responseController.Result as ObjectResult;
            var response = (TotalSolicitudesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<TotalSolicitudesDto>(actual.Value);
            Assert.True(response.Total > 0);
        }

        [Fact]
        public async Task GetConteoPendientes_Failure()
        {
            int idUsuario = 10;
            var expectedData = new TotalSolicitudesDto();

            _solicitudesService.Setup(m => m.GetConteoPendientes(idUsuario)).Returns(Task.FromResult(expectedData));

            var responseController = await _solicitudesController.GetConteoPendientes(idUsuario);
            var actual = responseController.Result as ObjectResult;
            var response = (TotalSolicitudesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<TotalSolicitudesDto>(actual.Value);
            Assert.True(response.Total == 0);
        }

        [Fact]
        public async Task GetCorreo_Success()
        {
            int idCorreo = 1;
            var expectedData = new CorreoSolicitudDatosPersonalesDto()
            {
                IdCorreo = 1,
                IdSolicitud = 6,
                Destinatario = "darega45186159@tec.mx",
                Comentarios = "Pruebas",
                Enviado = true,
            };

            _solicitudesService.Setup(m => m.GetCorreo(idCorreo)).Returns(Task.FromResult(expectedData));

            var responseController = await _solicitudesController.GetCorreo(idCorreo);
            var actual = responseController.Result as ObjectResult;
            var response = (CorreoSolicitudDatosPersonalesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<CorreoSolicitudDatosPersonalesDto>(actual.Value);
            Assert.True(response.Enviado);
        }

        [Fact]
        public async Task GetCorreo_Failure()
        {
            var expectedData = new CorreoSolicitudDatosPersonalesDto();
                
            _solicitudesService.Setup(m => m.GetCorreo(It.IsAny<int>())).Returns(Task.FromResult(expectedData));

            var responseController = await _solicitudesController.GetCorreo(It.IsAny<int>());
            var actual = responseController.Result as ObjectResult;
            var response = (CorreoSolicitudDatosPersonalesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<CorreoSolicitudDatosPersonalesDto>(actual.Value);
            Assert.False(response.Enviado);


        }

        [Fact]
        public void DownloadFile_Success()
        {
            string _fileName = "TemplateExpediente.xlsx";

            ArchivoController archivoController = new ArchivoController(_archivoStorage.Object, _env.Object);
            var resultado = archivoController.DownloadFile(_fileName);

            Assert.NotNull(resultado);
        }

        [Fact]
        public void DownloadFile_Failure()
        {
            string _fileName = string.Empty;

            ArchivoController archivoController = new ArchivoController(_archivoStorage.Object, _env.Object);
            var resultado = archivoController.DownloadFile(_fileName);

            Assert.NotNull(resultado);
        }
    }
}
