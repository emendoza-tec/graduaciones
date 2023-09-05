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
    public class UsuarioControllerTest
    {
        Mock<IUsuarioService> usuarioService;
        private UsuarioController usuariosController;

        public UsuarioControllerTest()
        {
            usuarioService = new Mock<IUsuarioService>();
            usuariosController = new UsuarioController(usuarioService.Object);
        }
        [Fact]
        public async Task GetUsuario_Success()
        {
            //Preparacion    
            string matricula = "A01653412";
            var usuario = new UsuarioDto
            {
                Matricula = "A01653412",
                Nombre = "Alan",
                ApeidoPaterno = "Saucedo",
                ApeidoMaterno = "Robles",
                Correo = "alsaro24212040@tec.mx",
                ProgramaAcademico = "Ingeniero en Electrónica",
                Mentor = "Laura Estela Gómez Melo",
                DirectorPrograma = "Alfredo Victor Mantilla Caeiros",
                CarreraId = "IE",
                Result = true
            };


            //Prueba
            usuarioService.Setup(m => m.ObtenerUsuario(matricula)).Returns(Task.FromResult(usuario));
            var resultado = await usuariosController.ObtenerUsuario(matricula);
            var actual = resultado.Result as ObjectResult;
            var response = (UsuarioDto)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<UsuarioDto>(actual.Value);
            Assert.True(response.Result);
        }
        [Fact]
        public async Task GetUsuario_Failure()
        {
            //Preparacion
            string matricula = "A016534122";
            var usuario = new UsuarioDto();


            //Prueba
            usuarioService.Setup(m => m.ObtenerUsuario(matricula)).Returns(Task.FromResult(usuario));
            var resultado = await usuariosController.ObtenerUsuario(matricula);
            var actual = resultado.Result as ObjectResult;
            var response = (UsuarioDto)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<UsuarioDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task ObtenerUsuarioPorMatriculaOnombre_Success()
        {
            //Preparacion    
            BusquedaAlumno busqueda = new BusquedaAlumno();
            busqueda.isMatricula = true;
            busqueda.Busqueda = "A01653412";
            busqueda.IdUsuario = 10;
            var usuarios = new List<UsuarioDto>()
            {
                 new UsuarioDto
                 {
                     Matricula = "A01653412",
                     Nombre = "Alan",
                     Carrera = "Ing. en Electrónica"
                 }
            };


            //Prueba
            usuarioService.Setup(m => m.ObtenerUsuarioPorMatriculaONombre(busqueda)).Returns(Task.FromResult(usuarios));
            var resultado = await usuariosController.ObtenerUsuarioPorMatriculaOnombre(busqueda);
            var actual = resultado.Result as ObjectResult;
            var response = (List<UsuarioDto>)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<UsuarioDto>>(actual.Value);
            Assert.True(response.Count > 0);
        }
        [Fact]
        public async Task ObtenerUsuarioPorMatriculaOnombre_Failure()
        {
            //Preparacion
            BusquedaAlumno busqueda = new BusquedaAlumno();
            busqueda.isMatricula = true;
            busqueda.Busqueda = "A016534122";
            busqueda.IdUsuario = 5;
            var usuarios = new List<UsuarioDto>();


            //Prueba
            usuarioService.Setup(m => m.ObtenerUsuarioPorMatriculaONombre(busqueda)).Returns(Task.FromResult(usuarios));
            var resultado = await usuariosController.ObtenerUsuarioPorMatriculaOnombre(busqueda);
            var actual = resultado.Result as ObjectResult;
            var response = (List<UsuarioDto>)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<UsuarioDto>>(actual.Value);
            Assert.False(response.Count > 0);
        }
        [Fact]
        public async Task GetUsuarios_Success()
        {
            //Preparacion    
            var usuario = new List<UsuarioAdministradorDto>{
                new UsuarioAdministradorDto
                {
                    Nomina = "L00828911",
                    IdUsuario = 1,
                    Nombre = "Alejandro Tamez Galindo",
                    Correo = "altaga39332045@tec.mx",
                    Estatus = 1,
                    Campus = "A",
                    Rol = "Adminsitrador"
                }
            };

            usuarioService.Setup(m => m.ObtenerUsuarios()).Returns(Task.FromResult(usuario));
            var resultado = await usuariosController.ObtenerUsuarios();
            var actual = resultado.Result as ObjectResult;
            var response = (List<UsuarioAdministradorDto>)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<UsuarioAdministradorDto>>(actual.Value);
            Assert.True(response.Count > 0);
        }
        [Fact]
        public async Task GetUsuarioAdminsitrador_Success()
        {
            //Preparacion    
            int IdUsuario = 1;
            var usuario = new UsuarioAdministradorDto
            {
                Nomina = "L00828911",
                IdUsuario = 1,
                Nombre = "Alejandro Tamez Galindo",
                Correo = "altaga39332045@tec.mx",
                Estatus = 1,
                ListCampus = new List<Campus> { new Campus() { ClaveCampus = "A", Descripcion = "A" } },
                Sedes = new List<Sede> { new Sede() { ClaveSede = "AGS", Descripcion = "AGS" } },
                Niveles = new List<Nivel> { new Nivel() { ClaveNivel = "05", Descripcion = "Profesional" } },
                Result = true
            };

            usuarioService.Setup(m => m.ObtenerUsuarioAdminsitrador(IdUsuario)).Returns(Task.FromResult(usuario));
            var resultado = await usuariosController.ObtenerUsuarioAdminsitrador(IdUsuario);
            var actual = resultado.Result as ObjectResult;
            var response = (UsuarioAdministradorDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<UsuarioAdministradorDto>(actual.Value);
            Assert.True(response.Result);
        }
        [Fact]
        public async Task GetUsuarioAdminsitrador_Failure()
        {
            //Preparacion    
            int IdUsuario = 0;
            var usuario = new UsuarioAdministradorDto();

            usuarioService.Setup(m => m.ObtenerUsuarioAdminsitrador(IdUsuario)).Returns(Task.FromResult(usuario));
            var resultado = await usuariosController.ObtenerUsuarioAdminsitrador(IdUsuario);
            var actual = resultado.Result as ObjectResult;
            var response = (UsuarioAdministradorDto)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<UsuarioAdministradorDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task GuardarUsuario_Success()
        {
            //Preparacion    
            var usuario = new UsuarioAdministradorDto
            {
                Nomina = "L00828911",
                Nombre = "Alejandro Tamez Galindo",
                Correo = "altaga39332045@tec.mx",
                ListCampus = new List<Campus> { new Campus() { ClaveCampus = "A", Descripcion = "A" } },
                Sedes = new List<Sede> { new Sede() { ClaveSede = "AGS", Descripcion = "" } },
                Niveles = new List<Nivel> { new Nivel() { ClaveNivel = "05", Descripcion = "" } },
                Roles = new List<RolesEntity> { new RolesEntity() { IdRol = 1, Descripcion = "" } }
            };
            var result = new UsuarioAdministradorDto { IdUsuario = 1, Result = true, ErrorMessage = "" };


            //Prueba
            usuarioService.Setup(m => m.GuardarUsuario(usuario)).Returns(Task.FromResult(result));
            var resultado = await usuariosController.GuardarUsuario(usuario);
            var actual = resultado.Result as ObjectResult;
            var response = (UsuarioAdministradorDto)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<UsuarioAdministradorDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GuardarUsuario_Failure()
        {
            //Preparacion    
            var usuario = new UsuarioAdministradorDto
            {
                Nomina = "L00828911",
                Nombre = "Alejandro Tamez Galindo",
                Correo = "altaga39332045@tec.mx",
                ListCampus = new List<Campus> { new Campus() { ClaveCampus = "A", Descripcion = "A" } },
                Sedes = new List<Sede> { new Sede() { ClaveSede = "AGS", Descripcion = "" } },
                Niveles = new List<Nivel>(),
                Roles = new List<RolesEntity>()
            };
            var result = new UsuarioAdministradorDto { IdUsuario = 1, Result = false, ErrorMessage = "" };


            //Prueba
            usuarioService.Setup(m => m.GuardarUsuario(usuario)).Returns(Task.FromResult(result));
            var resultado = await usuariosController.GuardarUsuario(usuario);
            var actual = resultado.Result as ObjectResult;
            var response = (UsuarioAdministradorDto)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<UsuarioAdministradorDto>(actual.Value);
            Assert.False(response.Result);
        }


        [Fact]
        public async Task EliminarUsuario_Success()
        {
            //Preparacion    
            var usuario = new UsuarioAdministradorDto
            {
                IdUsuario = 1,
                UsuarioModificacion = "L00628545"
            };
            var result = new BaseOutDto { Result = true, ErrorMessage = "" };


            //Prueba
            usuarioService.Setup(m => m.EliminarUsuario(usuario.IdUsuario, usuario.UsuarioModificacion)).Returns(Task.FromResult(result));
            var resultado = await usuariosController.EliminarUsuario(usuario);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task EliminarUsuario_Failure()
        {
            //Preparacion    
            var usuario = new UsuarioAdministradorDto
            {
                IdUsuario = 0,
                UsuarioModificacion = "L00628545"
            };
            var result = new BaseOutDto { Result = false, ErrorMessage = "" };


            //Prueba
            usuarioService.Setup(m => m.EliminarUsuario(usuario.IdUsuario, usuario.UsuarioModificacion)).Returns(Task.FromResult(result));
            var resultado = await usuariosController.EliminarUsuario(usuario);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.False(response.Result);
        }
        [Fact]
        public async Task ObtenerUsuarioPorNomina_Success()
        {
            //Preparacion    
            string nomina = "L00828911";
            var usuarios = new List<UsuarioAdministradorDto>()
            {
                 new UsuarioAdministradorDto
                 {
                     Nomina = "L00828911",
                     Nombre = "Alejandro Tamez Galindo",
                     Correo = "altaga39332045@tec.mx"
                 }
            };


            //Prueba
            usuarioService.Setup(m => m.ObtenerUsuarioNombrePorNomina(nomina)).Returns(Task.FromResult(usuarios));
            var resultado = await usuariosController.ObtenerUsuarioNombrePorNomina(nomina);
            var actual = resultado.Result as ObjectResult;
            var response = (List<UsuarioAdministradorDto>)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<UsuarioAdministradorDto>>(actual.Value);
            Assert.True(response.Count > 0);
        }
        [Fact]
        public async Task ObtenerUsuarioPorNomina_Failure()
        {
            //Preparacion    
            string nomina = "L0082891122";
            var usuarios = new List<UsuarioAdministradorDto>();


            //Prueba
            usuarioService.Setup(m => m.ObtenerUsuarioNombrePorNomina(nomina)).Returns(Task.FromResult(usuarios));
            var resultado = await usuariosController.ObtenerUsuarioNombrePorNomina(nomina);
            var actual = resultado.Result as ObjectResult;
            var response = (List<UsuarioAdministradorDto>)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<UsuarioAdministradorDto>>(actual.Value);
            Assert.False(response.Count > 0);
        }

        [Fact]
        public async Task ObtenerHistorialUsuario_Success()
        {
            //Preparacion    
            int idUsuario = 1;
            var usuarios = new List<UsuarioAdministradorDto>()
            {
                 new UsuarioAdministradorDto
                 {
                     IdUsuario = 1,
                     Campus = "Campus San Luis Potosí, Campus Santa Fe, Campus Toluca",
                     Sede = " Campus CSF sede Santa Fe, Campus SLP sede S Luis Potosí, Campus TOL sede Toluca, CEP Toluca",
                     Nivel = " Profesional",
                     Rol = " Prueba API, Calendario",
                     FechaModificacion = new DateTime(2023,06,01),
                     UsuarioModificacion = ""
                 },
                 new UsuarioAdministradorDto
                 {
                     IdUsuario = 1,
                     Campus = "Campus Santa Fe, Campus Toluca",
                     Sede = " Campus CSF sede Santa Fe, Campus TOL sede Toluca, CEP Toluca",
                     Nivel = " Profesional",
                     Rol = " Prueba API",
                     FechaModificacion = new DateTime(2023,05,29),
                     UsuarioModificacion = ""
                 },
                 new UsuarioAdministradorDto
                 {
                     IdUsuario = 1,
                     Campus = " Campus Aguascalientes, Campus Santa Fe, Campus Toluca",
                     Sede = " Campus AGS Sede Aguascalientes, Campus CSF sede Santa Fe, Campus TOL sede Toluca, CEP Toluca",
                     Nivel = " Profesional",
                     Rol = " Prueba API",
                     FechaModificacion = new DateTime(2023,05,24),
                     UsuarioModificacion = ""
                 }
            };


            //Prueba
            usuarioService.Setup(m => m.ObtenerHistorialUsuario(idUsuario)).Returns(Task.FromResult(usuarios));
            var resultado = await usuariosController.ObtenerHistorialUsuario(idUsuario);
            var actual = resultado.Result as ObjectResult;
            var response = (List<UsuarioAdministradorDto>)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<UsuarioAdministradorDto>>(actual.Value);
            Assert.True(response.Count > 0);
        }
        [Fact]
        public async Task ObtenerHistorialUsuario_Failure()
        {

            //Preparacion    
            int idUsuario = 1000;
            var usuarios = new List<UsuarioAdministradorDto>();


            //Prueba
            usuarioService.Setup(m => m.ObtenerHistorialUsuario(idUsuario)).Returns(Task.FromResult(usuarios));
            var resultado = await usuariosController.ObtenerHistorialUsuario(idUsuario);
            var actual = resultado.Result as ObjectResult;
            var response = (List<UsuarioAdministradorDto>)actual?.Value;

            //Verificacion
            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<UsuarioAdministradorDto>>(actual.Value);
            Assert.False(response.Count > 0);
        }
    }
}
