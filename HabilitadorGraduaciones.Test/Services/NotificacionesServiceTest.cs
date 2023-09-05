using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class NotificacionesServiceTest
    {
        readonly Mock<INotificacionesRepository> _notificacionesData;
        readonly Mock<IEmailModuleRepository> _emailData;
        readonly NotificacionesService _notificacionesService;

        public NotificacionesServiceTest()
        {
            _notificacionesData = new Mock<INotificacionesRepository>();
            _emailData = new Mock<IEmailModuleRepository>();
            _notificacionesService = new NotificacionesService(_notificacionesData.Object, _emailData.Object);
        }

        [Fact]
        public async Task EnviarCorreo_Success()
        {
            CorreoDto correo = new CorreoDto();
            correo.Destinatario = "";
            correo.Asunto = "";
            correo.Cuerpo = "";
            correo.Adjuntos = "";
            correo.ConCopia = "";

            BaseOutDto expectedData = new BaseOutDto { Result = true, ErrorMessage = string.Empty };

            _emailData.Setup(m => m.EnviarCorreo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.EnviarCorreo(correo);
            Assert.IsType<BaseOutDto>(actualData);
            Assert.True(actualData.Result);

        }

        [Fact]
        public async Task EnviarCorreo_Failure()
        {
            CorreoDto correo = new CorreoDto();
            correo.Destinatario = "";
            correo.Asunto = "";
            correo.Cuerpo = "";
            correo.Adjuntos = "";
            correo.ConCopia = "";

            BaseOutDto expectedData = new BaseOutDto { Result = false, ErrorMessage = string.Empty };

            _emailData.Setup(m => m.EnviarCorreo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.EnviarCorreo(correo);
            Assert.IsType<BaseOutDto>(actualData);
            Assert.False(actualData.Result);

        }

        [Fact]
        public async Task GetNotificaciones_Success()
        {
            NotificacionesDto expectedData = new NotificacionesDto()
            {
                Result = true,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>
                {
                    new Notificacion()
                    {
                        Id = 1,
                        FechaRegistro = DateTime.Now,
                        Activo = true,
                        Descripcion = "Aviso de prueba de filtros de relacion campus - sedes",
                        IsConsultaNotificacionesNoLeidas = false,
                        IsModificarNotificacion = false,
                        IsModificarTodas = false,
                        IsNotificacion = false,
                        ListNotificacionesLeidas = string.Empty,
                        Matricula = string.Empty,
                        Titulo = "Aviso prueba filtros"
                    },
                    new Notificacion()
                    {
                        Id = 2,
                        FechaRegistro = DateTime.Now,
                        Activo = true,
                        Descripcion = "Descricpion de Aviso de prueba 2",
                        IsConsultaNotificacionesNoLeidas = false,
                        IsModificarNotificacion = false,
                        IsModificarTodas = false,
                        IsNotificacion = false,
                        ListNotificacionesLeidas = string.Empty,
                        Matricula = "A01023670",
                        Titulo = "Aviso prueba 2"
                    }
                }
            };

            _notificacionesData.Setup(m => m.GetNotificaciones(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.GetNotificaciones(It.IsAny<Notificacion>());
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task GetNotificaciones_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto()
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesData.Setup(m => m.GetNotificaciones(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.GetNotificaciones(It.IsAny<Notificacion>());
            Assert.IsType<NotificacionesDto>(actualData);
            Assert.False(actualData.Result);
        }

        [Fact]
        public async Task ActualizarEstatus_Success()
        {
            NotificacionesDto expectedData = new NotificacionesDto()
            {
                Result = true,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>
                {
                    new Notificacion()
                    {
                        Id = 1,
                        FechaRegistro = DateTime.Now,
                        Activo = true,
                        Descripcion = "Aviso de prueba de filtros de relacion campus - sedes",
                        IsConsultaNotificacionesNoLeidas = false,
                        IsModificarNotificacion = false,
                        IsModificarTodas = false,
                        IsNotificacion = false,
                        ListNotificacionesLeidas = string.Empty,
                        Matricula = string.Empty,
                        Titulo = "Aviso prueba filtros"
                    },
                    new Notificacion()
                    {
                        Id = 2,
                        FechaRegistro = DateTime.Now,
                        Activo = true,
                        Descripcion = "Descricpion de Aviso de prueba 2",
                        IsConsultaNotificacionesNoLeidas = false,
                        IsModificarNotificacion = false,
                        IsModificarTodas = false,
                        IsNotificacion = false,
                        ListNotificacionesLeidas = string.Empty,
                        Matricula = "A01023670",
                        Titulo = "Aviso prueba 2"
                    }
                }
            };

            _notificacionesData.Setup(m => m.GetNotificaciones(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.ActualizarEstatus(It.IsAny<Notificacion>());
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task ActualizarEstatus_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto()
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesData.Setup(m => m.GetNotificaciones(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.ActualizarEstatus(It.IsAny<Notificacion>());
            Assert.IsType<NotificacionesDto>(actualData);
            Assert.False(actualData.Result);
        }

        [Fact]
        public async Task MarcarTodasLeidas_Success()
        {
            NotificacionesDto expectedData = new NotificacionesDto()
            {
                Result = true,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>
                {
                    new Notificacion()
                    {
                        Id = 1,
                        FechaRegistro = DateTime.Now,
                        Activo = true,
                        Descripcion = "Aviso de prueba de filtros de relacion campus - sedes",
                        IsConsultaNotificacionesNoLeidas = false,
                        IsModificarNotificacion = false,
                        IsModificarTodas = false,
                        IsNotificacion = false,
                        ListNotificacionesLeidas = string.Empty,
                        Matricula = string.Empty,
                        Titulo = "Aviso prueba filtros"
                    },
                    new Notificacion()
                    {
                        Id = 2,
                        FechaRegistro = DateTime.Now,
                        Activo = true,
                        Descripcion = "Descricpion de Aviso de prueba 2",
                        IsConsultaNotificacionesNoLeidas = false,
                        IsModificarNotificacion = false,
                        IsModificarTodas = false,
                        IsNotificacion = false,
                        ListNotificacionesLeidas = string.Empty,
                        Matricula = "A01023670",
                        Titulo = "Aviso prueba 2"
                    }
                }
            };

            List<Notificacion> lista = new List<Notificacion>
            {
                new Notificacion()
                {
                    Id = 1,
                    FechaRegistro = DateTime.Now,
                    Activo = true,
                    Descripcion = "Aviso de prueba de filtros de relacion campus - sedes",
                    IsConsultaNotificacionesNoLeidas = false,
                    IsModificarNotificacion = false,
                    IsModificarTodas = false,
                    IsNotificacion = false,
                    ListNotificacionesLeidas = string.Empty,
                    Matricula = string.Empty,
                    Titulo = "Aviso prueba filtros"
                },
                new Notificacion()
                {
                    Id = 2,
                    FechaRegistro = DateTime.Now,
                    Activo = true,
                    Descripcion = "Descricpion de Aviso de prueba 2",
                    IsConsultaNotificacionesNoLeidas = false,
                    IsModificarNotificacion = false,
                    IsModificarTodas = false,
                    IsNotificacion = false,
                    ListNotificacionesLeidas = string.Empty,
                    Matricula = "A01023670",
                    Titulo = "Aviso prueba 2"
                }
            };

            _notificacionesData.Setup(m => m.GetNotificaciones(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.MarcarTodasLeidas(lista);
            Assert.IsType<BaseOutDto>(actualData);
            Assert.True(actualData.Result);
        }

        [Fact]
        public async Task MarcarTodasLeidas_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto()
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };
            List<Notificacion> notificaciones = new List<Notificacion>();
            var actualData = await _notificacionesService.MarcarTodasLeidas(notificaciones);
            Assert.IsType<BaseOutDto>(actualData);
            Assert.False(actualData.Result);
        }


        [Fact]
        public async Task InsertarNotificacion_Success()
        {
            NotificacionesDto expectedData = new NotificacionesDto
            {
                Result = true,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesData.Setup(m => m.InsertarNotificacion(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.InsertarNotificacion(It.IsAny<Notificacion>());
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task InsertarNotificacion_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto()
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesData.Setup(m => m.InsertarNotificacion(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.InsertarNotificacion(It.IsAny<Notificacion>());
            Assert.IsType<NotificacionesDto>(actualData);
            Assert.False(actualData.Result);
        }

        [Fact]
        public async Task InsertarNotificacionCorreo_Success()
        {
            NotificacionesDto expectedData = new NotificacionesDto
            {
                Result = true,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesData.Setup(m => m.InsertarNotificacionCorreo(It.IsAny<NotificacionCorreoDto>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.InsertarNotificacionCorreo(It.IsAny<NotificacionCorreoDto>());
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task InsertarNotificacionCorreo_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto()
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesData.Setup(m => m.InsertarNotificacionCorreo(It.IsAny<NotificacionCorreoDto>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.InsertarNotificacionCorreo(It.IsAny<NotificacionCorreoDto>());
            Assert.IsType<NotificacionesDto>(actualData);
            Assert.False(actualData.Result);
        }

        [Fact]
        public async Task BienvenidoGraduacion_Success()
        {
            NotificacionesDto expectedData = new NotificacionesDto
            {
                Result = true,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesData.Setup(m => m.BienvenidoGraduacion(It.IsAny<string>(), 1)).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.BienvenidoGraduacion(It.IsAny<string>(), 1);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task BienvenidoGraduacion_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto()
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesData.Setup(m => m.BienvenidoGraduacion(It.IsAny<string>(), 1)).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.BienvenidoGraduacion(It.IsAny<string>(), 1);
            Assert.IsType<NotificacionesDto>(actualData);
            Assert.False(actualData.Result);
        }

        [Fact]
        public async Task IsCorreoEnviado_Success()
        {
            NotificacionesDto expectedData = new NotificacionesDto
            {
                Result = true,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesData.Setup(m => m.IsCorreoEnviado(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.IsCorreoEnviado(It.IsAny<int>(), It.IsAny<string>());
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task IsCorreoEnviado_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto()
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesData.Setup(m => m.IsCorreoEnviado(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.IsCorreoEnviado(It.IsAny<int>(), It.IsAny<string>());
            Assert.IsType<NotificacionesDto>(actualData);
            Assert.False(actualData.Result);
        }

        [Fact]
        public async Task EnteradoCreditosInsuficientes_Success()
        {
            NotificacionesDto expectedData = new NotificacionesDto
            {
                Result = true,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesData.Setup(m => m.EnteradoCreditosInsuficientes(It.IsAny<string>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.EnteradoCreditosInsuficientes(It.IsAny<string>());
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task EnteradoCreditosInsuficientes_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto()
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesData.Setup(m => m.EnteradoCreditosInsuficientes(It.IsAny<string>())).Returns(Task.FromResult(expectedData));
            var actualData = await _notificacionesService.EnteradoCreditosInsuficientes(It.IsAny<string>());
            Assert.IsType<NotificacionesDto>(actualData);
            Assert.False(actualData.Result);
        }
    }
}
