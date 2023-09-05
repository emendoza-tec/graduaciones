using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test
{
    public class NivelInglesControllerTest
    {
        readonly Mock<INivelInglesService> _nivelInglesService;
        private readonly NivelInglesController _nivelInglesController;


        public NivelInglesControllerTest()
        {
            _nivelInglesService = new Mock<INivelInglesService>();
            _nivelInglesController = new NivelInglesController(_nivelInglesService.Object);

        }

        [Fact]
        public async Task GetAlumnoNivelIngles_Success()
        {
            //Preparacion    
            var inglesDto = new NivelInglesDto

            {
                NivelIdiomaAlumno = "B2",
                RequisitoNvl = "B2",
                NivelIdiomaRequisito = "B2",
                FechaUltimaModificacion = Convert.ToDateTime("2023-05-24"),
                NivelCumple = true,
                Result = true

            };

            //Prueba
            _nivelInglesService.Setup(m => m.GetAlumnoNivelIngles(It.IsAny<NivelInglesEntity>())).Returns(Task.FromResult(inglesDto));

            var resultado = await _nivelInglesController.GetAlumnoNivelIngles(It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (NivelInglesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NivelInglesDto>(actual.Value);
            Assert.True(response.Result);

        }

        [Fact]
        public async Task GetAlumnoNivelIngles_Failure()
        {
            //Preparacion  
            var dto = new NivelInglesDto
            {
                ErrorMessage = string.Empty,
                Result = false
            };

            //Prueba            
            _nivelInglesService.Setup(m => m.GetAlumnoNivelIngles(It.IsAny<NivelInglesEntity>())).Returns(Task.FromResult(dto));
            var resultado = await _nivelInglesController.GetAlumnoNivelIngles(It.IsAny<string>());
            var actual = resultado.Result as ObjectResult;
            var response = (NivelInglesDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<NivelInglesDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task GetProgramas_Success()
        {
            //Preparacion    
            ProgramaDto dto = new ProgramaDto();
            dto.Result = true;
            dto.ErrorMessage = string.Empty;
            dto.Programa = new List<Programa>()
            {
               new Programa
               {
                   NombrePrograma = "ABC",
                   NivelIngles = "B2"
               },
                 new Programa
               {
                   NombrePrograma = "CBA",
                   NivelIngles = "C1"
               },
                   new Programa
               {
                   NombrePrograma = "BCA",
                   NivelIngles = "B2"
               }

            };

            //Prueba
            _nivelInglesService.Setup(m => m.GetProgramas(It.IsAny<ProgramaDto>())).Returns(Task.FromResult(dto));

            var resultado = await _nivelInglesController.GetProgramas();
            var actual = resultado.Result as ObjectResult;
            var response = (ProgramaDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<ProgramaDto>(actual.Value);
            Assert.True(response.Result);

        }

        [Fact]
        public async Task GetProgramas_Failure()
        {
            //Preparacion 
            ProgramaDto dto = new ProgramaDto();
            dto.Result = false;
            dto.ErrorMessage = string.Empty;

            //Prueba            
            _nivelInglesService.Setup(m => m.GetProgramas(It.IsAny<ProgramaDto>())).Returns(Task.FromResult(dto));
            var resultado = await _nivelInglesController.GetProgramas();
            var actual = resultado.Result as ObjectResult;
            var response = (ProgramaDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<ProgramaDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task ModificarNivelIngles_Success()
        {
            //Preparacion 
            List<ConfiguracionNivelInglesEntity> configuracionIngles = new List<ConfiguracionNivelInglesEntity>()
            {
                new ConfiguracionNivelInglesEntity()
                {
                    IdNivelIngles = "4",
                    ClaveProgramaAcademico = "ABC",
                    IdUsuario = "2235"
                },
                 new ConfiguracionNivelInglesEntity()
                {
                    IdNivelIngles = "5",
                    ClaveProgramaAcademico = "CAB",
                    IdUsuario = "4585"
                },
                    new ConfiguracionNivelInglesEntity()
                {
                    IdNivelIngles = "4",
                    ClaveProgramaAcademico = "BCA",
                    IdUsuario = "8546"
                },
            };
            BaseOutDto res = new BaseOutDto { Result = true, ErrorMessage = string.Empty };

            //Prueba            
            _nivelInglesService.Setup(m => m.GuardarConfiguracionNivelIngles(configuracionIngles)).Returns(Task.FromResult(res));
            var resultado = await _nivelInglesController.ModificarNivelIngles(configuracionIngles);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task ModificarNivelIngles_Failure()
        {
            //Preparacion 
            List<ConfiguracionNivelInglesEntity> configuracionIngles = new List<ConfiguracionNivelInglesEntity>();

            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };

            //Prueba            
            _nivelInglesService.Setup(m => m.GuardarConfiguracionNivelIngles(configuracionIngles)).Returns(Task.FromResult(res));
            var resultado = await _nivelInglesController.ModificarNivelIngles(configuracionIngles);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.False(response.Result);
        }

    }

}
