
using AutoMapper;
using HabilitadorGraduaciones.Core.Automapper;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class PeriodoTest
    {
        Mock<IPeriodosRepository> periodoData;
        readonly PeriodosService periodoService;
        public IConfiguration configuration { get; }
        public ISesionRepository _sessionData { get; }
        public IPlanDeEstudiosService _planDeEstudiosService { get; }

        public PeriodoTest()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper();
            periodoData = new Mock<IPeriodosRepository>();
            periodoService = new PeriodosService(mapper, _sessionData, periodoData.Object, _planDeEstudiosService);
        }
        [Fact]
        public async Task GetPeriodos_Success()
        {
            string matricula = "A01424206";
            PeriodosEntity entity = new PeriodosEntity();
            entity.Matricula = matricula;
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

            periodoData.Setup(m => m.GetPeriodos(entity)).Returns(Task.FromResult(list));

            var actualData = await periodoService.GetPeriodos(entity);

            // Assert
            Assert.IsType<List<PeriodosEntity>>(actualData);
            Assert.True(actualData.Count == 5);
        }

        [Fact]
        public async Task GetPeriodos_Failure()
        {
            string matricula = "A0142420611";
            PeriodosEntity entity = new PeriodosEntity();
            entity.Matricula = matricula;
            var list = new List<PeriodosEntity>();

            periodoData.Setup(m => m.GetPeriodos(entity)).Returns(Task.FromResult(list));

            var actualData = await periodoService.GetPeriodos(entity);

            // Assert
            Assert.IsType<List<PeriodosEntity>>(actualData);
            Assert.True(actualData.Count == 0);
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

            BaseOutDto res = new BaseOutDto { Result = true, ErrorMessage = string.Empty };
            //Prueba
            periodoData.Setup(m => m.GuardarPeriodo(periodo)).Returns(Task.FromResult(res));

            var actualData = await periodoService.GuardarPeriodo(periodo);
            //Verificacion
            Assert.IsType<BaseOutDto>(actualData);
            Assert.True(actualData.Result);
        }
        [Fact]
        public async Task GuardarPeriodo_Failure()
        {
            //Preparacion    
            string matricula = "A01424206";
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

            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };
            //Prueba
            periodoData.Setup(m => m.GuardarPeriodo(periodo)).Returns(Task.FromResult(res));

            var actualData = await periodoService.GuardarPeriodo(periodo);
            //Verificacion
            Assert.IsType<BaseOutDto>(actualData);
            Assert.False(actualData.Result);
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
            periodoData.Setup(m => m.GetPeriodoAlumno(matricula)).Returns(Task.FromResult(periodo));

            var actualData = await periodoService.GetPeriodoAlumno(matricula);

            //Verificacion

            Assert.IsType<PeriodosEntity>(actualData);
            Assert.NotNull(actualData.PeriodoId);
        }
        [Fact]
        public async Task GetPeriodoAlumno_Failure()
        {
            //Preparacion    
            string matricula = "A0142420611";
            var periodo = new PeriodosEntity();


            //Prueba
            periodoData.Setup(m => m.GetPeriodoAlumno(matricula)).Returns(Task.FromResult(periodo));

            var actualData = await periodoService.GetPeriodoAlumno(matricula);

            //Verificacion

            Assert.IsType<PeriodosEntity>(actualData);
            Assert.Null(actualData.PeriodoId);
        }
    }
}
