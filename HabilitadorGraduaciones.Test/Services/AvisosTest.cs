using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Core.Entities.Bases;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class AvisosTest
    {
        readonly Mock<IAvisosRepository> _avisosData;
        readonly AvisosService _avisosService;

        public AvisosTest()
        {
            _avisosData = new Mock<IAvisosRepository>();
            _avisosService = new AvisosService(_avisosData.Object);
        }

        [Fact]
        public async Task Get3AvisosService_Success()
        {
            //Preparacion
            AvisosDto dto = new AvisosDto();
            dto.Result = true;
            dto.ErrorMessage = string.Empty;
            dto.lstAvisos = new List<Aviso>()
            {
                new Aviso
                {
                    Id = 1,
                    Titulo = "Titulo del aviso",
                    Texto = "Descipci&#243;n del aviso<p></p><ul><li>Opci&#243;n 1</li><li>Opci&#243;n 2</li></ul><p></p>",
                    Nivel = "05",
                    CampusId = "W",
                    SedeId = "CMP",
                    EscuelasId = "43",
                    ProgramaId = "ARQ19",
                    RequisitoId = "",
                    RequisitoEstatus = "",
                    Matricula = "",
                    Cc_rolesId = "",
                    Cc_camposId = "A",
                    Habilitador = true,
                    Correo = true,
                    UrlImage = null,
                    FechaCreacion = DateTime.Now
                },
                new Aviso
                {
                    Id = 2,
                    Titulo = "Titulo del aviso 2",
                    Texto = "Descipción del aviso",
                    Nivel = "05",
                    CampusId = "2",
                    SedeId = "CSF",
                    EscuelasId = "43",
                    ProgramaId = "ARQ19",
                    RequisitoId = "",
                    RequisitoEstatus = "",
                    Matricula = "",
                    Cc_rolesId = "",
                    Cc_camposId = "A",
                    Habilitador = true,
                    Correo = false,
                    UrlImage = null,
                    FechaCreacion = DateTime.Now
                },
                new Aviso
                {
                    Id = 3,
                    Titulo = "Titulo del aviso 2",
                    Texto = "Descipción del aviso",
                    Nivel = "05",
                    CampusId = "2",
                    SedeId = "CSF",
                    EscuelasId = "43",
                    ProgramaId = "ARQ19",
                    RequisitoId = "",
                    RequisitoEstatus = "",
                    Matricula = "",
                    Cc_rolesId = "",
                    Cc_camposId = "A",
                    Habilitador = true,
                    Correo = false,
                    UrlImage = null,
                    FechaCreacion = DateTime.Now
                }
            };

            _avisosData.Setup(m => m.Get3Avisos(It.IsAny<AvisosEntity>())).Returns(Task.FromResult(dto));

            var actualData = await _avisosService.Get3AvisosService(It.IsAny<AvisosEntity>());
            Assert.Equal(dto, actualData);
        }

        [Fact]
        public async Task Get3AvisosService_Failure()
        {
            //Preparacion
            AvisosDto expectedData = new AvisosDto();
            expectedData.Result = false;
            expectedData.ErrorMessage = string.Empty;
            expectedData.lstAvisos = new List<Aviso>();

            _avisosData.Setup(m => m.Get3Avisos(It.IsAny<AvisosEntity>())).Returns(Task.FromResult(expectedData));

            var actualData = await _avisosService.Get3AvisosService(It.IsAny<AvisosEntity>());
            Assert.IsType<AvisosDto>(actualData);
            Assert.False(expectedData.Result);
        }

        [Fact]
        public async Task SetAvisosService_Success()
        {
            //Preparacion
            var dto = new AvisoGuardar();
            var aviso = new Aviso()
            {
                Titulo = "Titulo del aviso",
                Texto = "Descipci&#243;n del aviso<p></p><ul><li>Opci&#243;n 1</li><li>Opci&#243;n 2</li></ul><p></p>",
                Nivel = "05",
                CampusId = "W",
                SedeId = "CMP",
                EscuelasId = "43",
                ProgramaId = "ARQ19",
                RequisitoId = "",
                RequisitoEstatus = "",
                Matricula = "",
                Cc_rolesId = "",
                Cc_camposId = "A",
                Habilitador = true,
                Correo = true,
                UrlImage = null,
                FechaCreacion = DateTime.Now
            };
            dto.Aviso = aviso;

            BaseOutDto res = new BaseOutDto { Result = true, ErrorMessage = string.Empty };

            _avisosData.Setup(m => m.SetAviso(dto)).Returns(Task.FromResult(res));

            var actualData = await _avisosService.SetAvisosService(dto);
            Assert.IsType<BaseOutDto>(actualData);
            Assert.True(actualData.Result);
        }

        [Fact]
        public async Task SetAvisosService_Failure()
        {
            AvisoGuardar dto = new AvisoGuardar();
            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };

            _avisosData.Setup(m => m.SetAviso(dto)).Returns(Task.FromResult(res));

            var actualData = await _avisosService.SetAvisosService(dto);
            Assert.IsType<BaseOutDto>(actualData);
            Assert.False(actualData.Result);
        }

        [Fact]
        public async Task GetAvisosService_Success()
        {
            //Preparacion
            AvisosDto dto = new AvisosDto();
            dto.Result = true;
            dto.ErrorMessage = string.Empty;
            dto.lstAvisos = new List<Aviso>()
            {
                new Aviso
                {
                    Id = 1,
                    Titulo = "Titulo del aviso",
                    Texto = "Descipci&#243;n del aviso<p></p><ul><li>Opci&#243;n 1</li><li>Opci&#243;n 2</li></ul><p></p>",
                    Nivel = "05",
                    CampusId = "W",
                    SedeId = "CMP",
                    EscuelasId = "43",
                    ProgramaId = "ARQ19",
                    RequisitoId = "",
                    RequisitoEstatus = "",
                    Matricula = "",
                    Cc_rolesId = "",
                    Cc_camposId = "A",
                    Habilitador = true,
                    Correo = true,
                    UrlImage = null,
                    FechaCreacion = DateTime.Now
                },
                new Aviso
                {
                    Id = 2,
                    Titulo = "Titulo del aviso 2",
                    Texto = "Descipción del aviso",
                    Nivel = "05",
                    CampusId = "2",
                    SedeId = "CSF",
                    EscuelasId = "43",
                    ProgramaId = "ARQ19",
                    RequisitoId = "",
                    RequisitoEstatus = "",
                    Matricula = "",
                    Cc_rolesId = "",
                    Cc_camposId = "A",
                    Habilitador = true,
                    Correo = false,
                    UrlImage = null,
                    FechaCreacion = DateTime.Now
                },
                new Aviso
                {
                    Id = 3,
                    Titulo = "Titulo del aviso 2",
                    Texto = "Descipción del aviso",
                    Nivel = "05",
                    CampusId = "2",
                    SedeId = "CSF",
                    EscuelasId = "43",
                    ProgramaId = "ARQ19",
                    RequisitoId = "",
                    RequisitoEstatus = "",
                    Matricula = "",
                    Cc_rolesId = "",
                    Cc_camposId = "A",
                    Habilitador = true,
                    Correo = false,
                    UrlImage = null,
                    FechaCreacion = DateTime.Now
                },
                new Aviso
                {
                    Id = 4,
                    Titulo = "Titulo del aviso 2",
                    Texto = "Descipción del aviso",
                    Nivel = "05",
                    CampusId = "2",
                    SedeId = "CSF",
                    EscuelasId = "43",
                    ProgramaId = "ARQ19",
                    RequisitoId = "",
                    RequisitoEstatus = "",
                    Matricula = "",
                    Cc_rolesId = "",
                    Cc_camposId = "A",
                    Habilitador = true,
                    Correo = false,
                    UrlImage = null,
                    FechaCreacion = DateTime.Now
                }
            };

            _avisosData.Setup(m => m.GetAvisos(It.IsAny<AvisosEntity>())).Returns(Task.FromResult(dto));

            var actualData = await _avisosService.GetAvisosService(It.IsAny<AvisosEntity>());
            Assert.Equal(dto, actualData);
        }

        [Fact]
        public async Task GetAvisosService_Failure()
        {
            //Preparacion
            AvisosDto expectedData = new AvisosDto();
            expectedData.Result = false;
            expectedData.ErrorMessage = string.Empty;
            expectedData.lstAvisos = new List<Aviso>();

            _avisosData.Setup(m => m.GetAvisos(It.IsAny<AvisosEntity>())).Returns(Task.FromResult(expectedData));

            var actualData =  await _avisosService.GetAvisosService(It.IsAny<AvisosEntity>());
            Assert.IsType<AvisosDto>(actualData);
            Assert.False(expectedData.Result);
        }

        [Fact]
        public async Task ObtenerCatalogo_Success()
        {
            //Preparacion
            List<CatalogoDto> catalogo = new List<CatalogoDto>()
            {
                new CatalogoDto
                {
                    Clave = "01",
                    Descripcion = "Preescolar"
                },
                new CatalogoDto
                {
                    Clave = "05",
                    Descripcion = "Profesional"
                }
            };

            _avisosData.Setup(m => m.GetCatalogo(It.IsAny<int>())).Returns(Task.FromResult(catalogo));

            var actualData = await _avisosService.ObtenerCatalogo(It.IsAny<int>());
            Assert.Equal(catalogo, actualData);
        }

        [Fact]
        public async Task ObtenerCatalogo_Failure()
        {
            //Preparacion
            List<CatalogoDto> expectedData = new List<CatalogoDto>();

            _avisosData.Setup(m => m.GetCatalogo(It.IsAny<int>())).Returns(Task.FromResult(expectedData));

            var actualData = await _avisosService.ObtenerCatalogo(It.IsAny<int>());
            Assert.IsType<List<CatalogoDto>>(actualData);
            Assert.False(expectedData.Count > 0);
        }

        [Fact]
        public async Task ObtenerCatalogoMatricula_Success()
        {
            //Preparacion
            List<CatalogoDto> expectedData = new List<CatalogoDto>()
            {
                new CatalogoDto
                {
                    Clave = "A01023670",
                    Descripcion = "A01023670"
                },
                new CatalogoDto
                {
                    Clave = "A01653412",
                    Descripcion = "A01653412"
                }
            };

            FiltrosMatriculaDto dto = new FiltrosMatriculaDto()
            {
                CampusId = "O",
                SedeId = "TOL",
                EscuelasId = "43",
                NivelId = "05",
                ProgramaId = "BGB19",
                IdUsuario = 1
            };

            _avisosData.Setup(m => m.GetCatalogoMatricula(dto)).Returns(Task.FromResult(expectedData));

            var actualData = await _avisosService.ObtenerCatalogoMatricula(dto);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task ObtenerCatalogoMatricula_Failure()
        {
            //Preparacion
            List<CatalogoDto> expectedData = new List<CatalogoDto>();

            FiltrosMatriculaDto dto = new FiltrosMatriculaDto()
            {
                CampusId = "O",
                SedeId = "TOL",
                EscuelasId = "43",
                NivelId = "05",
                ProgramaId = "BGB19",
                IdUsuario = 1
            };

            _avisosData.Setup(m => m.GetCatalogoMatricula(dto)).Returns(Task.FromResult(expectedData));

            var actualData = await _avisosService.ObtenerCatalogoMatricula(dto);
            Assert.IsType<List<CatalogoDto>>(actualData);
            Assert.False(expectedData.Count > 0);
        }
    }
}
