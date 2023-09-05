using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class CalendariosControllerTest
    {
        readonly Mock<ICalendariosService> _calendariosService;
        private readonly CalendariosController _calendariosController;

        public CalendariosControllerTest()
        {
            _calendariosService = new Mock<ICalendariosService>();
            _calendariosController = new CalendariosController(_calendariosService.Object);
        }

        [Fact]
        public async Task GetCalendarioAlumno_Success()
        {
            //Preparacion 
            var dto = new CalendarioDto
            {
                CalendarioId = "28",
                ClaveCampus = "M",
                Campus = "Campus Querétaro",
                LinkProspecto = "www.google.com",
                LinkCandidato = "www.youtube.com",
                ErrorMessage = string.Empty,
                Result = true
            };

            //Prueba            
            _calendariosService.Setup(m => m.GetCalendarioAlumno(It.IsAny<CalendarioEntity>())).Returns(Task.FromResult(dto));
            var resultado = await _calendariosController.GetCalendarioAlumno(It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (CalendarioDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<CalendarioDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GetCalendarioAlumno_Failure()
        {
            //Preparacion  
            var dto = new CalendarioDto
            {
                ErrorMessage = string.Empty,
                Result = false
            };

            //Prueba            
            _calendariosService.Setup(m => m.GetCalendarioAlumno(It.IsAny<CalendarioEntity>())).Returns(Task.FromResult(dto));
            var resultado = await _calendariosController.GetCalendarioAlumno(It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (CalendarioDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<CalendarioDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task GetCalendarios_Success()
        {
            //Preparacion 
            CalendariosDto dto = new CalendariosDto();
            dto.Result = true;
            dto.ErrorMessage = string.Empty;
            dto.Calendarios = new List<CalendarioDto>()
            {
                new CalendarioDto
                {
                    CalendarioId = "28",
                    ClaveCampus = "M",
                    Campus = "Campus Querétaro",
                    LinkProspecto = "www.google.com",
                    LinkCandidato = "www.youtube.com",
                    ErrorMessage = string.Empty,
                    Result = true
                },
                new CalendarioDto
                {
                    CalendarioId = "14",
                    ClaveCampus = "W",
                    Campus = "Campus Guadalajara",
                    LinkProspecto = "https://www.google.com/",
                    LinkCandidato = "https://tec.mx/es/conocenos",
                    ErrorMessage = string.Empty,
                    Result = true
                },
                new CalendarioDto
                {
                    CalendarioId = "11",
                    ClaveCampus = "S",
                    Campus = "Campus Estado de México",
                    LinkProspecto = string.Empty,
                    LinkCandidato = string.Empty,
                    ErrorMessage = string.Empty,
                    Result = true
                }
            };
            //Prueba            
            _calendariosService.Setup(m => m.GetCalendarios(It.IsAny<CalendariosDto>())).Returns(Task.FromResult(dto));
            var resultado = await _calendariosController.GetCalendarios();
            var actual = resultado.Result as ObjectResult;
            var response = (CalendariosDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<CalendariosDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GetCalendarios_Failure()
        {
            //Preparacion 
            CalendariosDto dto = new CalendariosDto();
            dto.Result = false;
            dto.ErrorMessage = string.Empty;

            //Prueba            
            _calendariosService.Setup(m => m.GetCalendarios(It.IsAny<CalendariosDto>())).Returns(Task.FromResult(dto));
            var resultado = await _calendariosController.GetCalendarios();
            var actual = resultado.Result as ObjectResult;
            var response = (CalendariosDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<CalendariosDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task ModificarCalendarios_Success()
        {
            //Preparacion 
            List<CalendariosEntity> calendarios = new List<CalendariosEntity>()
            {
                new CalendariosEntity()
                {
                    ClaveCampus = "W",
                    IdUsuario = "L03533706",
                    LinkCandidato = "www.google.com",
                    LinkProspecto = "www.tec.mx"
                },
                new CalendariosEntity()
                {
                    ClaveCampus = "S",
                    IdUsuario = "L03533706",
                    LinkCandidato = "www.youtube.com",
                    LinkProspecto = "www.mitec.mx"
                }
            };
            BaseOutDto res = new BaseOutDto { Result = true, ErrorMessage = string.Empty };

            //Prueba            
            _calendariosService.Setup(m => m.GuardarConfiguracionCalendarios(calendarios)).Returns(Task.FromResult(res));
            var resultado = await _calendariosController.ModificarCalendarios(calendarios);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task ModificarCalendarios_Failure()
        {
            //Preparacion 
            List<CalendariosEntity> calendarios = new List<CalendariosEntity>();

            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };

            //Prueba            
            _calendariosService.Setup(m => m.GuardarConfiguracionCalendarios(calendarios)).Returns(Task.FromResult(res));
            var resultado = await _calendariosController.ModificarCalendarios(calendarios);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.False(response.Result);
        }
    }
}
