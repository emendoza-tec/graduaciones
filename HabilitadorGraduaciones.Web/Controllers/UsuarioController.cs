using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;

        public UsuarioController(IUsuarioService _usuarioService)
        {
            usuarioService = _usuarioService;
        }

        /// <summary>Obtener información del usuario por matrícula.</summary>
        /// <param name="id">Matrícula del alumno.</param>
        /// <returns>Objeto con inforamción del usuario.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> ObtenerUsuario(string id) => Ok(await usuarioService.ObtenerUsuario(id));

        [HttpPost("busqueda")]
        public async Task<ActionResult<List<UsuarioDto>>> ObtenerUsuarioPorMatriculaOnombre(BusquedaAlumno busqueda) => Ok(await usuarioService.ObtenerUsuarioPorMatriculaONombre(busqueda));

        /// <summary>Obtener lista de usuarios administradores.</summary>
        /// <returns>Lista de objetos con inforamción del usuario administrador.</returns>
        [HttpGet("ObtenerUsuarios")]
        public async Task<ActionResult<List<UsuarioAdministradorDto>>> ObtenerUsuarios() => Ok(await usuarioService.ObtenerUsuarios());


        /// <summary>Obtener información de usuario administrador por Id.</summary>
        /// <param name="IdUsuario">Id del usuario adminsitrador</param>
        /// <returns>Objeto tipo UsuarioAdministradorDto con inforamción del usuario administrador.</returns>
        [HttpGet("ObtenerUsuarioAdminsitrador/{IdUsuario}")]
        public async Task<ActionResult<UsuarioAdministradorDto>> ObtenerUsuarioAdminsitrador(int IdUsuario) => Ok(await usuarioService.ObtenerUsuarioAdminsitrador(IdUsuario));

            
        /// <summary>Guarda y actualiza usuario administrador.</summary>
        /// <param name="usuario">Objeto con información del usuario</param>
        /// <returns>Objeto tipo UsuarioAdministradorDto con inforamción del usuario administrador.</returns>
        [HttpPost("GuardarUsuario/")]
        public async Task<ActionResult<UsuarioAdministradorDto>> GuardarUsuario(UsuarioAdministradorDto usuario) => Ok(await usuarioService.GuardarUsuario(usuario));


        /// <summary>Elimina usuario administrador por Id.</summary>
        /// <param name="/IdUsuario">Id del usuario</param>
        /// <returns>Objeto tipo BaseOutDto con response de operacion correcta.</returns>
        [HttpPost("EliminarUsuario/")]
        public async Task<ActionResult<BaseOutDto>> EliminarUsuario(UsuarioAdministradorDto usuario) => Ok(await usuarioService.EliminarUsuario(usuario.IdUsuario, usuario.UsuarioModificacion));


        /// <summary>Obtener información de usuario administrador por nomina.</summary>
        /// <param name="nomina">Nomina del usuario adminsitrador</param>
        /// <returns>Objeto tipo UsuarioAdministradorDto con el nombre y correo de usaurio por nomina</returns>
        [HttpGet("ObtenerUsuarioNombrePorNomina/{nomina}")]
        public async Task<ActionResult<List<UsuarioAdministradorDto>>> ObtenerUsuarioNombrePorNomina(string nomina) => Ok(await usuarioService.ObtenerUsuarioNombrePorNomina(nomina));

        /// <summary>Obtener campus para llenar combo</summary>
        /// <returns>Lista de campus </returns>
        [HttpGet("ObtenerCampus/")]
        public async Task<ActionResult<List<Campus>>> ObtenerCampus() => Ok(await usuarioService.ObtenerCampus());

        /// <summary>Obtener campus para llenar combo</summary>
        /// <returns>Lista de campus </returns>
        [HttpGet("ObtenerSedes/")]
        public async Task<ActionResult<List<Sede>>> ObtenerSedes() => Ok(await usuarioService.ObtenerSedes());


        /// <summary>Obtener historial de usuario.</summary>
        /// <param name="IdUsuario">Id del usuario adminsitrador</param>
        /// <returns>Lista de tipo UsuarioAdministradorDto con inforamción del usuario administrador.</returns>
        [HttpGet("ObtenerHistorialUsuario/{IdUsuario}")]
        public async Task<ActionResult<List<UsuarioAdministradorDto>>> ObtenerHistorialUsuario(int IdUsuario) => Ok(await usuarioService.ObtenerHistorialUsuario(IdUsuario));


        /// <summary>Guardar usuarios de forma masiva por archivo de excel.</summary>
        /// <param name="archivo">Objeto tipo ArchivoEntity</param>
        /// <returns>Resultado del result del objeto ArchivoEntity.</returns>
        [HttpPost("GuardarCargaArchivo")]
        public async Task<IActionResult> GuardarCargaArchivo([FromForm] ArchivoEntity archivo)
        {
            if (archivo == null)
            {
                return BadRequest(new UsuarioAdministradorDto
                {
                    ErrorMessage = "Archivo vacío"
                });
            }

            var guardarCargaArchivo = await usuarioService.GuardarCargaArchivo(archivo);
            if (guardarCargaArchivo.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
            {
                return BadRequest();
            }

            archivo.Result = "success";
            return Ok(archivo);
        }
    }
}