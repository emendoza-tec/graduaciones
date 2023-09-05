using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using Moq;
using Xunit;
namespace HabilitadorGraduaciones.Test
{
    public class NivelInglesTest
    {

        readonly Mock<INivelInglesRepository> _nivelInglesData;
        private readonly NivelInglesService _nivelInglesService;

        public NivelInglesTest()
        {

            _nivelInglesData = new Mock<INivelInglesRepository>();
            _nivelInglesService = new NivelInglesService(_nivelInglesData.Object);

        }

        [Fact]
        public async Task GetAlumnoNivelIngles_Success()
        {
            //Preparacion    
            var inglesDto = new NivelInglesDto

            {
                NivelIdiomaAlumno = "C1",
                RequisitoNvl = "C1",
                NivelIdiomaRequisito = "B2",
                FechaUltimaModificacion = Convert.ToDateTime("2022-04-24"),
                NivelCumple = true,
                Result = true

            };

            //Prueba
            _nivelInglesData.Setup(m => m.GetAlumnoNivelIngles(It.IsAny<NivelInglesEntity>())).Returns(Task.FromResult(inglesDto));

            // Assert
            var actualData = await _nivelInglesService.GetAlumnoNivelIngles(It.IsAny<NivelInglesEntity>());
            Assert.Equal(inglesDto, actualData);

        }


        [Fact]
        public async Task GetAlumnoNivelIngles_Failure()
        {
            //Preparacion    
            var inglesDto = new NivelInglesDto

            {
                NivelIdiomaAlumno = "C1",
                RequisitoNvl = "C1",
                NivelIdiomaRequisito = "B2",
                FechaUltimaModificacion = Convert.ToDateTime("2022-04-24"),
                NivelCumple = true,
                Result = false

            };

            //Prueba
            _nivelInglesData.Setup(m => m.GetAlumnoNivelIngles(It.IsAny<NivelInglesEntity>())).Returns(Task.FromResult(inglesDto));

            // Assert
            var actualData = await _nivelInglesService.GetAlumnoNivelIngles(It.IsAny<NivelInglesEntity>());

            Assert.IsType<NivelInglesDto>(actualData);
            Assert.False(inglesDto.Result);

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
            _nivelInglesData.Setup(m => m.GetProgramas(It.IsAny<ProgramaDto>())).Returns(Task.FromResult(dto));

            var actualData = await _nivelInglesService.GetProgramas(It.IsAny<ProgramaDto>());
            Assert.Equal(dto, actualData);

        }

        [Fact]
        public async Task GetProgramas_Failure()
        {
            //Preparacion    
            ProgramaDto dto = new ProgramaDto();
            dto.Result = false;
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
            _nivelInglesData.Setup(m => m.GetProgramas(It.IsAny<ProgramaDto>())).Returns(Task.FromResult(dto));

            var actualData = await _nivelInglesService.GetProgramas(It.IsAny<ProgramaDto>());

            Assert.IsType<ProgramaDto>(actualData);
            Assert.False(dto.Result);


        }

        [Fact]
        public async Task ModificarNivelIngles_Success()
        {
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
            _nivelInglesData.Setup(m => m.ModificarNivelIngles(configuracionIngles)).Returns(Task.FromResult(res));


            var actualData = await _nivelInglesService.GuardarConfiguracionNivelIngles(configuracionIngles);
            Assert.Equal(res, actualData);
        }

        [Fact]
        public async Task ModificarNivelIngles_Failure()
        {
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
            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };

            //Prueba
            _nivelInglesData.Setup(m => m.ModificarNivelIngles(configuracionIngles)).Returns(Task.FromResult(res));


            var actualData = await _nivelInglesService.GuardarConfiguracionNivelIngles(configuracionIngles);
            Assert.IsType<BaseOutDto>(actualData);
            Assert.False(res.Result);
        }

    }
}
