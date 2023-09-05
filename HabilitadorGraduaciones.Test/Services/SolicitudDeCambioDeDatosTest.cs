using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using HabilitadorGraduaciones.Services.Interfaces;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class SolicitudDeCambioDeDatosTest
    {
        Mock<ISolicitudDeCambioDeDatosRepository> _solicitudesData;
        SolicitudDeCambioDeDatosService _solicitudesService;
        private readonly Mock<IArchivoLocalStorageService> _archivoStorage;

        public SolicitudDeCambioDeDatosTest()
        {
            _archivoStorage = new Mock<IArchivoLocalStorageService>();
            _solicitudesData = new Mock<ISolicitudDeCambioDeDatosRepository>();
            _solicitudesService = new SolicitudDeCambioDeDatosService(_solicitudesData.Object, _archivoStorage.Object);
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

            _solicitudesData.Setup(m => m.GetEstatusSolicitudes()).Returns(Task.FromResult(list));

            var actualData = await _solicitudesService.GetEstatusSolicitudes();
            Assert.Equal(list, actualData);
        }

        [Fact]
        public async Task GetEstatusSolicitudes_Failure()
        {
            var list = new List<EstatusSolicitudDatosPersonalesEntity>();

            _solicitudesData.Setup(m => m.GetEstatusSolicitudes()).Returns(Task.FromResult(list));

            var actualData = await _solicitudesService.GetEstatusSolicitudes();
            Assert.Equal(list, actualData);
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

            _solicitudesData.Setup(m => m.GetPendientes(idUsuario)).Returns(Task.FromResult(expectedData));

            var actualData = await _solicitudesService.GetPendientes(idUsuario);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task GetPendientes_Failure()
        {
            var expectedData = new List<SolicitudDeCambioDeDatosEntity>();

            _solicitudesData.Setup(m => m.GetPendientes(It.IsAny<int>())).Returns(Task.FromResult(expectedData));

            var actualData = await _solicitudesService.GetPendientes(It.IsAny<int>());
            Assert.Equal(expectedData, actualData);
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

            _solicitudesData.Setup(m => m.Get(idEstatusSolicitud, idUsuario)).Returns(Task.FromResult(expectedData));

            var actualData = await _solicitudesService.Get(idEstatusSolicitud, idUsuario);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task Get_Failure()
        {
            var expectedData = new List<SolicitudDeCambioDeDatosEntity>();

            _solicitudesData.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(expectedData));

            var actualData = await _solicitudesService.Get(It.IsAny<int>(), It.IsAny<int>());
            Assert.Equal(expectedData, actualData);
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

            _solicitudesData.Setup(m => m.GetDetalle(idSolicitud)).Returns(Task.FromResult(expectedData));

            var actualData = await _solicitudesService.GetDetalle(idSolicitud);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task GetDetalle_Failure()
        {
            var expectedData = new List<DetalleSolicitudDeCambioDeDatosEntity>();

            _solicitudesData.Setup(m => m.GetDetalle(It.IsAny<int>())).Returns(Task.FromResult(expectedData));

            var actualData = await _solicitudesService.GetDetalle(It.IsAny<int>());
            Assert.Equal(expectedData, actualData);
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
            _solicitudesData.Setup(m => m.ModificaSolicitud(expectedData)).Returns(Task.FromResult(res));
            var actualData = await _solicitudesService.ModificaSolicitud(expectedData);

            Assert.IsType<BaseOutDto>(actualData);
            Assert.True(actualData.Result);
        }

        [Fact]
        public async Task ModificaSolicitud_Failure()
        {
            var expectedData = new ModificarEstatusSolicitudDto();

            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };
            _solicitudesData.Setup(m => m.ModificaSolicitud(expectedData)).Returns(Task.FromResult(res));
            var actualData = await _solicitudesService.ModificaSolicitud(expectedData);

            Assert.IsType<BaseOutDto>(actualData);
            Assert.False(actualData.Result);
        }

        [Fact]
        public async Task GetConteoPendientes_Success()
        {
            int idUsuario = 10;
            var expectedData = new TotalSolicitudesDto()
            {
                Total = 5,
            };

            _solicitudesData.Setup(m => m.GetConteoPendientes(idUsuario)).Returns(Task.FromResult(expectedData));

            var actualData = await _solicitudesService.GetConteoPendientes(idUsuario);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task GetConteoPendientes_Failure()
        {
            var expectedData = new TotalSolicitudesDto();

            _solicitudesData.Setup(m => m.GetConteoPendientes(It.IsAny<int>())).Returns(Task.FromResult(expectedData));

            var actualData = await _solicitudesService.GetConteoPendientes(It.IsAny<int>());
            Assert.Equal(expectedData, actualData);
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

            _solicitudesData.Setup(m => m.GetCorreo(idCorreo)).Returns(Task.FromResult(expectedData));

            var actualData = await _solicitudesService.GetCorreo(idCorreo);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task GetCorreo_Failure()
        {
            var expectedData = new CorreoSolicitudDatosPersonalesDto();

            _solicitudesData.Setup(m => m.GetCorreo(It.IsAny<int>())).Returns(Task.FromResult(expectedData));

            var actualData = await _solicitudesService.GetCorreo(It.IsAny<int>());
            Assert.Equal(expectedData, actualData);
        }
    }
}
