using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Services.ProcesaExcel;

namespace HabilitadorGraduaciones.Services
{
    public class UsuarioService : IUsuarioService
    {
        public readonly IUsuarioRepository usuarioRepository;
        public readonly IAvisosService avisosService;
        public readonly IRolesService rolesService;
        public UsuarioService(IUsuarioRepository _usuarioRepository, IAvisosService _avisosService, IRolesService _rolesService)
        {
            usuarioRepository = _usuarioRepository;
            avisosService = _avisosService;
            rolesService = _rolesService;
        }
        /// <summary>Obtener información del usuario por matrícula.</summary>
        /// <param name="matricula">Matrícula del alumno.</param>
        /// <returns>Objeto con inforamción del usuario.</returns>
        public async Task<UsuarioDto> ObtenerUsuario(string matricula)
        {
            UsuarioDto usuarioDto = new UsuarioDto();
            try
            {
                usuarioDto =await usuarioRepository.ObtenerUsuario(matricula);
            }
            catch (Exception ex)
            {
                usuarioDto.Result = false;
                usuarioDto.ErrorMessage = ex.Message;
                throw new CustomException("Error en el método ObtenerUsuario", ex);
            }
            return usuarioDto;
        }
        public async Task<List<UsuarioDto>> ObtenerUsuarioPorMatriculaONombre(BusquedaAlumno busqueda)
        {
            var lsUsuarioDto = await usuarioRepository.ObtenerUsuarioPorMatriculaONombre(busqueda);
            return lsUsuarioDto;
        }

        public async Task<List<UsuarioAdministradorDto>> ObtenerUsuarios()
        {
            var result = await usuarioRepository.ObtenerUsuarios();

            return result;
        }

        public async Task<UsuarioAdministradorDto> ObtenerUsuarioAdminsitrador(int IdUsuario)
        {
            var result = await usuarioRepository.ObtenerUsuarioAdminsitrador(IdUsuario);

            return result;
        }
        public async Task<UsuarioAdministradorDto> ObtenerUsuarioAdminsitradorPorNomina(string nomina)
        {
            var result = await usuarioRepository.ObtenerUsuarioAdminsitradorPorNomina(nomina);

            return result;
        }

        public async Task<UsuarioAdministradorDto> GuardarUsuario(UsuarioAdministradorDto usuario)
        {
            var result = await usuarioRepository.GuardarUsuario(usuario);

            return result;
        }
        public async Task<List<UsuarioAdministradorDto>> ObtenerUsuarioNombrePorNomina(string nomina)
        {
            var result = await usuarioRepository.ObtenerUsuarioNombrePorNomina(nomina);

            return result;
        }
        public async Task<BaseOutDto> EliminarUsuario(int IdUsuario, string UsuarioElimino)
        {
            var result = await usuarioRepository.EliminarUsuario(IdUsuario, UsuarioElimino);

            return result;
        }

        public async Task<List<Campus>> ObtenerCampus()
        {
            var result = await usuarioRepository.ObtenerCampus();

            return result;
        }

        public async Task<List<Sede>> ObtenerSedes()
        {   
            var result = await usuarioRepository.ObtenerSedes();

            return result;
        }

        public async Task<List<UsuarioAdministradorDto>> ObtenerHistorialUsuario(int IdUsuario)
        {
            var result = await usuarioRepository.ObtenerHistorialUsuario(IdUsuario);
            return result;
        }
        public async Task<HttpResponseMessage> GuardarCargaArchivo(ArchivoEntity archivo)
        {
            var response = new HttpResponseMessage();
            var usuariosExcel = await ProcesaUsuario.ObtenerUsuariosFromExcel(archivo.ArchivoRecibido);
            var listNiveles = await avisosService.ObtenerCatalogo(1);
            var rolesList = await rolesService.ObtenerDescripcionRoles();
            foreach (var usuario in usuariosExcel)
            {
                usuario.Nivel = listNiveles.Where(x => x.Descripcion.ToUpper() == usuario.Nivel.ToUpper()).Select(x => x.Clave).First();
                usuario.Rol = Convert.ToString(rolesList.Where(x => x.Descripcion.ToUpper() == usuario.Rol.ToUpper()).Select(x => x.IdRol).First());
            }

            var usuarios = usuariosExcel.GroupBy(x => new { x.Nomina })
                .Select(x => new UsuarioAdministradorDto
                {
                    Nomina = x.First().Nomina,
                    Correo = x.First().Correo,
                    ListCampus = x.Select(c => new Campus { ClaveCampus = c.Campus }).DistinctBy(c => c.ClaveCampus).ToList(),
                    Sedes = x.Select(s => new Sede { ClaveCampus = s.Campus, ClaveSede = s.Sede }).DistinctBy(c => c.ClaveSede).ToList(),
                    Niveles = x.Select(n => new Nivel { ClaveNivel = n.Nivel }).DistinctBy(c => c.ClaveNivel).ToList(),
                    Roles = x.Select(r => new RolesEntity { IdRol = Convert.ToInt32(r.Rol) }).DistinctBy(c => c.IdRol).ToList(),

                }).ToList();

            foreach (var usuario in usuarios)
            {
                usuario.UsuarioModificacion = archivo.UsuarioApplicacion;
                var usuarioExistente = await ObtenerUsuarioAdminsitradorPorNomina(usuario.Nomina);
                
                if (usuarioExistente.IdUsuario > 0)
                {
                    usuario.IdUsuario = usuarioExistente.IdUsuario;
                }

                var result = await GuardarUsuario(usuario);
                if (!result.Result)
                {
                    response.StatusCode = System.Net.HttpStatusCode.NotModified;
                    throw new CustomException(response.Content.ToString(), response.StatusCode);
                }

            }

            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
            throw new CustomException(response.Content.ToString(), response.StatusCode);
        }
    }
}