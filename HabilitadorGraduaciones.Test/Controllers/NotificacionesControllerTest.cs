using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class NotificacionesControllerTest
    {
        readonly Mock<INotificacionesService> _notificacionesService;
        private readonly NotificacionesController _notificacionesController;

        public NotificacionesControllerTest()
        {
            _notificacionesService = new Mock<INotificacionesService>();
            _notificacionesController = new NotificacionesController(_notificacionesService.Object);
        }

        [Fact]
        public async Task EnviarCorreo_Success()
        {
            var dto = new CorreoDto()
            {
                Destinatario = "",
                Asunto = "Correo Prueba",
                Cuerpo = "Cuerpo correo prueba",
                Adjuntos = "",
                ConCopia = ""
            };

            BaseOutDto result = new BaseOutDto { Result = true, ErrorMessage = string.Empty };

            _notificacionesService.Setup(m => m.EnviarCorreo(dto)).Returns(Task.FromResult(result));
            var resultado = await _notificacionesController.EnviarCorreo(dto);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(response);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task EnviarCorreo_Failure()
        {
            var dto = new CorreoDto()
            {
                Destinatario = "",
                Asunto = "Correo Prueba",
                Cuerpo = "Cuerpo correo prueba",
                Adjuntos = "",
                ConCopia = ""
            };

            BaseOutDto result = new BaseOutDto { Result = false, ErrorMessage = string.Empty };

            _notificacionesService.Setup(m => m.EnviarCorreo(dto)).Returns(Task.FromResult(result));
            var resultado = await _notificacionesController.EnviarCorreo(dto);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(response);
            Assert.False(response.Result);
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

            _notificacionesService.Setup(m => m.GetNotificaciones(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.GetNotificaciones(It.IsAny<bool>(), It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.True(response.Result);
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

            _notificacionesService.Setup(m => m.GetNotificaciones(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.GetNotificaciones(It.IsAny<bool>(), It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task ActualizarEstatusNotificacion_Success()
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
                        IsConsultaNotificacionesNoLeidas = true,
                        IsModificarNotificacion = true,
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
                        IsConsultaNotificacionesNoLeidas = true,
                        IsModificarNotificacion = true,
                        IsModificarTodas = false,
                        IsNotificacion = false,
                        ListNotificacionesLeidas = string.Empty,
                        Matricula = "A01023670",
                        Titulo = "Aviso prueba 2"
                    }
                }
            };

            _notificacionesService.Setup(m => m.ActualizarEstatus(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.ActualizarEstatusNotificacion(It.IsAny<Notificacion>());
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task ActualizarEstatusNotificacion_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto()
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesService.Setup(m => m.ActualizarEstatus(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.ActualizarEstatusNotificacion(It.IsAny<Notificacion>());
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task MarcarTodasLeidas_Success()
        {
            BaseOutDto expectedData = new BaseOutDto { Result = true, ErrorMessage = string.Empty };

            _notificacionesService.Setup(m => m.MarcarTodasLeidas(It.IsAny<List<Notificacion>>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.MarcarTodasLeidas(It.IsAny<List<Notificacion>>());
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(response);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task MarcarTodasLeidas_Failure()
        {
            BaseOutDto expectedData = new BaseOutDto { Result = false, ErrorMessage = string.Empty };

            _notificacionesService.Setup(m => m.MarcarTodasLeidas(It.IsAny<List<Notificacion>>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.MarcarTodasLeidas(It.IsAny<List<Notificacion>>());
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(response);
            Assert.False(response.Result);
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

            _notificacionesService.Setup(m => m.InsertarNotificacion(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.InsertarNotificacion(It.IsAny<Notificacion>());
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task InsertarNotificacion_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesService.Setup(m => m.InsertarNotificacion(It.IsAny<Notificacion>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.InsertarNotificacion(It.IsAny<Notificacion>());
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.False(response.Result);
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

            _notificacionesService.Setup(m => m.InsertarNotificacionCorreo(It.IsAny<NotificacionCorreoDto>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.InsertarNotificacionCorreo(It.IsAny<NotificacionCorreoDto>());
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task InsertarNotificacionCorreo_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesService.Setup(m => m.InsertarNotificacionCorreo(It.IsAny<NotificacionCorreoDto>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.InsertarNotificacionCorreo(It.IsAny<NotificacionCorreoDto>());
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.False(response.Result);
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

            _notificacionesService.Setup(m => m.BienvenidoGraduacion(It.IsAny<string>(), 1)).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.BienvenidoGraduacion(It.IsAny<string>(), 1);
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task BienvenidoGraduacion_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesService.Setup(m => m.BienvenidoGraduacion(It.IsAny<string>(), 1)).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.BienvenidoGraduacion(It.IsAny<string>(), 1);
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.False(response.Result);
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

            _notificacionesService.Setup(m => m.IsCorreoEnviado(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.IsCorreoEnviado(It.IsAny<int>(), It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task IsCorreoEnviado_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesService.Setup(m => m.IsCorreoEnviado(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.IsCorreoEnviado(It.IsAny<int>(), It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.False(response.Result);
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

            _notificacionesService.Setup(m => m.EnteradoCreditosInsuficientes(It.IsAny<string>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.EnteradoCreditosInsuficientes(It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task EnteradoCreditosInsuficientes_Failure()
        {
            NotificacionesDto expectedData = new NotificacionesDto
            {
                Result = false,
                ErrorMessage = string.Empty,
                ListaNotificaciones = new List<Notificacion>()
            };

            _notificacionesService.Setup(m => m.EnteradoCreditosInsuficientes(It.IsAny<string>())).Returns(Task.FromResult(expectedData));
            var resultado = await _notificacionesController.EnteradoCreditosInsuficientes(It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (NotificacionesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NotificacionesDto>(response);
            Assert.False(response.Result);
        }
    }
}
