using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Core.Entities.Bases;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using HabilitadorGraduaciones.Web.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class AvisosControllerTest
    {
        readonly Mock<IAvisosService> _avisosService;
        private readonly AvisosController _avisosController;
        readonly Mock<IArchivoStorage> _archivoStorage;

        public AvisosControllerTest()
        {
            _avisosService = new Mock<IAvisosService>();
            _archivoStorage = new Mock<IArchivoStorage>();
            _avisosController = new AvisosController(_avisosService.Object, _archivoStorage.Object);
        }

        [Fact]
        public async Task PostAvisos_Success()
        {
            //Preparacion
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

            BaseOutDto res = new BaseOutDto { Result = true, ErrorMessage = string.Empty };

            //Prueba
            _avisosService.Setup(m => m.SetAvisosService(It.IsAny<AvisoGuardar>())).Returns(Task.FromResult(res));
            var resultado = await _avisosController.PostAvisos(aviso);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task PostAvisos_Failure()
        {
            //Preparacion
            var aviso = new Aviso();

            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };

            //Prueba    
            _avisosService.Setup(m => m.SetAvisosService(It.IsAny<AvisoGuardar>())).Returns(Task.FromResult(res));
            var resultado = await _avisosController.PostAvisos(aviso);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task GetAvisos_Success()
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
            string matricula = "A01023670";

            //Prueba
            _avisosService.Setup(m => m.Get3AvisosService(It.IsAny<AvisosEntity>())).Returns(Task.FromResult(dto));
            var resultado = await _avisosController.GetAvisos(matricula);
            var actual = resultado.Result as ObjectResult;
            var response = (AvisosDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<AvisosDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GetAvisos_Failure()
        {
            //Preparacion
            AvisosDto dto = new AvisosDto();
            dto.Result = false;
            dto.ErrorMessage = string.Empty;
            dto.lstAvisos = new List<Aviso>();
            string matricula = "A01023670";

            //Prueba
            _avisosService.Setup(m => m.Get3AvisosService(It.IsAny<AvisosEntity>())).Returns(Task.FromResult(dto));
            var resultado = await _avisosController.GetAvisos(matricula);
            var actual = resultado.Result as ObjectResult;
            var response = (AvisosDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<AvisosDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task GetAvisosHistorial_Success()
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
            string matricula = "A01023670";

            //Prueba
            _avisosService.Setup(m => m.GetAvisosService(It.IsAny<AvisosEntity>())).Returns(Task.FromResult(dto));
            var resultado = await _avisosController.GetAvisosHistorial(matricula);
            var actual = resultado.Result as ObjectResult;
            var response = (AvisosDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<AvisosDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GetAvisosHistorial_Failure()
        {
            //Preparacion
            AvisosDto dto = new AvisosDto();
            dto.Result = false;
            dto.ErrorMessage = string.Empty;
            dto.lstAvisos = new List<Aviso>();
            string matricula = "A01023670";

            //Prueba
            _avisosService.Setup(m => m.GetAvisosService(It.IsAny<AvisosEntity>())).Returns(Task.FromResult(dto));
            var resultado = await _avisosController.GetAvisosHistorial(matricula);
            var actual = resultado.Result as ObjectResult;
            var response = (AvisosDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<AvisosDto>(actual.Value);
            Assert.False(response.Result);
        }

    }
}
