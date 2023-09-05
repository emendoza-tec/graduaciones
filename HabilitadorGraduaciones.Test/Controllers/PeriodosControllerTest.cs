using AutoMapper;
using HabilitadorGraduaciones.Core.Automapper;
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
    public class PeriodosControllerTest
    {
        Mock<IPeriodosService> periodoService;
        private PeriodosController periodosController;

        public PeriodosControllerTest()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper();
            periodoService = new Mock<IPeriodosService>();
            periodosController = new PeriodosController(mapper, periodoService.Object);
        }

        [Fact]
        public async Task GetPeriodos_Success()
        {
            //Preparacion    
            EndpointsDto dto = new EndpointsDto();
            dto.NumeroMatricula = "A01424206";
            dto.ClaveCarrera = "ITC";
            dto.ClaveNivelAcademico = "05";
            dto.ClaveEjercicioAcademico = "202311";
            var list = new List<PeriodosEntity>() {
                new PeriodosEntity()
                {
                    Matricula = "A01424206",
                    PeriodoId = "202311",
                    Descripcion = "Enero - Junio 2023",
                    IsRegular = true,
                    TipoPeriodo = 1
                } ,
                new PeriodosEntity()
                {
                    Matricula = "A01424206",
                    PeriodoId = "202312",
                    Descripcion = "Verano 2023",
                    IsRegular = false,
                    TipoPeriodo = 1
                } ,
                new PeriodosEntity()
                {
                    Matricula = "A01424206",
                    PeriodoId = "202313",
                    Descripcion = "Agosto - Diciembre 2023",
                    IsRegular = true,
                    TipoPeriodo = 1
                } ,
                new PeriodosEntity()
                {
                    Matricula = "A01424206",
                    PeriodoId = "202410",
                    Descripcion = "Invierno 2024",
                    IsRegular = false,
                    TipoPeriodo = 1
                } ,
                new PeriodosEntity()
                {
                    Matricula = "A01424206",
                    PeriodoId = "202411",
                    Descripcion = "Enero - Junio 2024",
                    IsRegular = true,
                    TipoPeriodo = 1
                }
            };

            //Prueba
            periodoService.Setup(m => m.GetPeriodos(dto)).Returns(Task.FromResult(list));
            var resultado = await periodosController.GetPeriodos(dto);
            var actual = resultado.Result as ObjectResult;
            var response = (List<PeriodosDto>)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<PeriodosDto>>(actual.Value);
            Assert.True(response.Count == 5);
        }

        [Fact]
        public async Task GetPeriodos_Failure()
        {
            //Preparacion  
            EndpointsDto dto = new EndpointsDto();
            dto.NumeroMatricula = "A01424206";
            dto.ClaveCarrera = "ITC";
            dto.ClaveNivelAcademico = "05";
            dto.ClaveEjercicioAcademico = "202311";
            var list = new List<PeriodosEntity>();


            //Prueba
            periodoService.Setup(m => m.GetPeriodos(dto)).Returns(Task.FromResult(list));
            var resultado = await periodosController.GetPeriodos(dto);
            var actual = resultado.Result as ObjectResult;
            var response = (List<PeriodosDto>)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<PeriodosDto>>(actual.Value);
            Assert.True(response.Count == 0);
        }
        [Fact]
        public async Task GetPeriodoPronostico_Success()
        {
            //Preparacion 
            EndpointsDto dto = new EndpointsDto();
            dto.NumeroMatricula = "A01424206";
            dto.ClaveCarrera = "ITC";
            dto.ClaveNivelAcademico = "05";
            dto.ClaveEjercicioAcademico = "202311";
            var periodo = new PeriodosEntity
            {
                Matricula = "A01424206",
                PeriodoId = "202311",
                Descripcion = "Enero - Junio 2023",
                IsRegular = true,
                TipoPeriodo = 1
            };

            //Prueba
            periodoService.Setup(m => m.GetPeriodoPronostico(dto)).Returns(Task.FromResult(periodo));
            var resultado = await periodosController.GetPeriodoPronostisco(dto);
            var actual = resultado.Result as ObjectResult;
            var response = (PeriodosDto)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<PeriodosDto>(actual.Value);
            Assert.NotNull(response.PeriodoId);
        }

        [Fact]
        public async Task GetPeriodoPronostico_Failure()
        {
            //Preparacion
            EndpointsDto dto = new EndpointsDto();
            dto.NumeroMatricula = "A01424206";
            dto.ClaveCarrera = "ITC";
            dto.ClaveNivelAcademico = "05";
            dto.ClaveEjercicioAcademico = "202311";
            var periodo = new PeriodosEntity();

            //Prueba
            periodoService.Setup(m => m.GetPeriodoPronostico(dto)).Returns(Task.FromResult(periodo));
            var resultado = await periodosController.GetPeriodoPronostisco(dto);
            var actual = resultado.Result as ObjectResult;
            var response = (PeriodosDto)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<PeriodosDto>(actual.Value);
            Assert.Null(response.PeriodoId);
        }

        [Fact]
        public async Task GetPeriodoAlumno_Success()
        {
            //Preparacion    
            string matricula = "A01424206";
            var periodo = new PeriodosEntity
            {
                Matricula = "A01424206",
                PeriodoId = "202311",
                Descripcion = "Enero - Junio 2023",
                IsRegular = true,
                TipoPeriodo = 1
            };


            //Prueba
            periodoService.Setup(m => m.GetPeriodoAlumno(matricula)).Returns(Task.FromResult(periodo));
            var resultado = await periodosController.GetPeriodoAlumno(matricula);
            var actual = resultado.Result as ObjectResult;
            var response = (PeriodosDto)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<PeriodosDto>(actual.Value);
            Assert.NotNull(response.PeriodoId);
        }

        [Fact]
        public async Task GetPeriodoAlumno_Failure()
        {
            //Preparacion    
            string matricula = "A01424218";
            var periodo = new PeriodosEntity();

            //Prueba
            periodoService.Setup(m => m.GetPeriodoAlumno(matricula)).Returns(Task.FromResult(periodo));
            var resultado = await periodosController.GetPeriodoAlumno(matricula);
            var actual = resultado.Result as ObjectResult;
            var response = (PeriodosDto)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<PeriodosDto>(actual.Value);
            Assert.Null(response.PeriodoId);
        }

        [Fact]
        public async Task GuardarPeriodo_Success()
        {
            //Preparacion    
            var periodo = new PeriodosDto
            {
                Matricula = "A01424206",
                PeriodoId = "202311",
                PeriodoElegido = "202311",
                PeriodoCeremonia = "202311",
                PeriodoEstimado = "202311",
                MotivoCambioPeriodo = "Comence a trabajar",
                OrigenActualizacionPeriodoId = 2
            };

            //Prueba
            BaseOutDto res = new BaseOutDto { Result = true, ErrorMessage = string.Empty };
            periodoService.Setup(m => m.GuardarPeriodo(periodo)).Returns(Task.FromResult(res));

            var resultado = await periodosController.GuardarPeriodo(periodo);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;
            //Verificacion

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GuardarPeriodo_Failure()
        {
            //Preparacion    
            var periodo = new PeriodosDto
            {
                Matricula = "",
                PeriodoId = "202311",
                PeriodoElegido = "202311",
                PeriodoCeremonia = "202311",
                PeriodoEstimado = "202311",
                MotivoCambioPeriodo = "Comence a trabajar",
                OrigenActualizacionPeriodoId = 2
            };

            //Prueba
            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };
            periodoService.Setup(m => m.GuardarPeriodo(periodo)).Returns(Task.FromResult(res));

            var resultado = await periodosController.GuardarPeriodo(periodo);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.False(response.Result);
        }
    }
}
