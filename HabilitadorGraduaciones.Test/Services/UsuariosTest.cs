using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using HabilitadorGraduaciones.Services.Interfaces;
using Moq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Xunit;

namespace HabilitadorGraduaciones.Test.Services
{
    public class UsuariosTest
    {
        Mock<IUsuarioRepository> _usuarioData;
        UsuarioService _usuarioService;
        IAvisosService avisosService;
        IRolesService roleService;

        public UsuariosTest()
        {
            _usuarioData = new Mock<IUsuarioRepository>();
            _usuarioService = new UsuarioService(_usuarioData.Object, avisosService, roleService);
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

            _usuarioData.Setup(m => m.ObtenerUsuario(matricula)).Returns(Task.FromResult(usuario));

            var actualData = await _usuarioService.ObtenerUsuario(matricula);
            Assert.Equal(usuario, actualData);

        }
        [Fact]
        public async Task GetUsuario_Failure()
        {
            //Preparacion
            string matricula = "A016534122";
            var usuario = new UsuarioDto();


            //Prueba
            _usuarioData.Setup(m => m.ObtenerUsuario(matricula)).Returns(Task.FromResult(usuario));

            var actualData = await _usuarioService.ObtenerUsuario(matricula);

            Assert.IsType<UsuarioDto>(actualData);
            Assert.False(actualData.Result);
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
            _usuarioData.Setup(m => m.ObtenerUsuarioPorMatriculaONombre(busqueda)).Returns(Task.FromResult(usuarios));

            var actualData = await _usuarioService.ObtenerUsuarioPorMatriculaONombre(busqueda);
            Assert.Equal(usuarios, actualData);

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
            _usuarioData.Setup(m => m.ObtenerUsuarioPorMatriculaONombre(busqueda)).Returns(Task.FromResult(usuarios));

            var actualData = await _usuarioService.ObtenerUsuarioPorMatriculaONombre(busqueda);

            Assert.IsType<List<UsuarioDto>>(actualData);
            Assert.False(actualData.Count > 0);
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

            //Prueba
            _usuarioData.Setup(m => m.ObtenerUsuarios()).Returns(Task.FromResult(usuario));

            var actualData = await _usuarioService.ObtenerUsuarios();
            Assert.Equal(usuario, actualData);

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

            //Prueba
            _usuarioData.Setup(m => m.ObtenerUsuarioAdminsitrador(IdUsuario)).Returns(Task.FromResult(usuario));

            var actualData = await _usuarioService.ObtenerUsuarioAdminsitrador(IdUsuario);
            Assert.Equal(usuario, actualData);

        }
        [Fact]
        public async Task GetUsuarioAdminsitrador_Failure()
        {
            //Preparacion    
            int IdUsuario = 0;
            var usuario = new UsuarioAdministradorDto();

            //Prueba
            _usuarioData.Setup(m => m.ObtenerUsuarioAdminsitrador(IdUsuario)).Returns(Task.FromResult(usuario));

            var actualData = await _usuarioService.ObtenerUsuarioAdminsitrador(IdUsuario);

            Assert.IsType<UsuarioAdministradorDto>(actualData);
            Assert.False(actualData.Result);
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
            _usuarioData.Setup(m => m.GuardarUsuario(usuario)).Returns(Task.FromResult(result));
            var actualData = await _usuarioService.GuardarUsuario(usuario);

            Assert.IsType<UsuarioAdministradorDto>(actualData);
            Assert.True(actualData.Result);

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
            _usuarioData.Setup(m => m.GuardarUsuario(usuario)).Returns(Task.FromResult(result));
            var actualData = await _usuarioService.GuardarUsuario(usuario);

            Assert.IsType<UsuarioAdministradorDto>(actualData);
            Assert.False(actualData.Result);
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
            _usuarioData.Setup(m => m.EliminarUsuario(usuario.IdUsuario, usuario.UsuarioModificacion)).Returns(Task.FromResult(result));
            var actualData = await _usuarioService.EliminarUsuario(usuario.IdUsuario, usuario.UsuarioModificacion);

            Assert.IsType<BaseOutDto>(actualData);
            Assert.True(actualData.Result);
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
            _usuarioData.Setup(m => m.EliminarUsuario(usuario.IdUsuario, usuario.UsuarioModificacion)).Returns(Task.FromResult(result));
            var actualData = await _usuarioService.EliminarUsuario(usuario.IdUsuario, usuario.UsuarioModificacion);

            Assert.IsType<BaseOutDto>(actualData);
            Assert.False(actualData.Result);
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
            _usuarioData.Setup(m => m.ObtenerUsuarioNombrePorNomina(nomina)).Returns(Task.FromResult(usuarios));

            var actualData = await _usuarioService.ObtenerUsuarioNombrePorNomina(nomina);
            Assert.Equal(usuarios, actualData);
        }
        [Fact]
        public async Task ObtenerUsuarioPorNomina_Failure()
        {
            //Preparacion    
            string nomina = "L0082891122";
            var usuarios = new List<UsuarioAdministradorDto>();

            //Prueba
            _usuarioData.Setup(m => m.ObtenerUsuarioNombrePorNomina(nomina)).Returns(Task.FromResult(usuarios));

            var actualData = await _usuarioService.ObtenerUsuarioNombrePorNomina(nomina);

            Assert.IsType<List<UsuarioAdministradorDto>>(actualData);
            Assert.False(actualData.Count > 0);
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
            _usuarioData.Setup(m => m.ObtenerHistorialUsuario(idUsuario)).Returns(Task.FromResult(usuarios));

            var actualData = await _usuarioService.ObtenerHistorialUsuario(idUsuario);
            Assert.Equal(usuarios, actualData);

        }
        [Fact]
        public async Task ObtenerHistorialUsuario_Failure()
        {

            //Preparacion    
            int idUsuario = 1000;
            var usuarios = new List<UsuarioAdministradorDto>();

            //Prueba
            _usuarioData.Setup(m => m.ObtenerHistorialUsuario(idUsuario)).Returns(Task.FromResult(usuarios));

            var actualData = await _usuarioService.ObtenerHistorialUsuario(idUsuario);

            Assert.IsType<List<UsuarioAdministradorDto>>(actualData);
            Assert.False(actualData.Count > 0);
        }
    }
}
