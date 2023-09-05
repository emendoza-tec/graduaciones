using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IUsuarioService
    {
        public Task<UsuarioDto> ObtenerUsuario(string matricula);
        public Task<List<UsuarioDto>> ObtenerUsuarioPorMatriculaONombre(BusquedaAlumno busqueda);
        public Task<List<UsuarioAdministradorDto>> ObtenerUsuarios();
        public Task<UsuarioAdministradorDto> ObtenerUsuarioAdminsitrador(int IdUsuario);
        public Task<UsuarioAdministradorDto> ObtenerUsuarioAdminsitradorPorNomina(string nomina);
        public Task<UsuarioAdministradorDto> GuardarUsuario(UsuarioAdministradorDto usuario);
        public Task<BaseOutDto> EliminarUsuario(int IdUsuario, string UsuarioElimino);
        public Task<List<UsuarioAdministradorDto>> ObtenerUsuarioNombrePorNomina(string nomina);
        public Task<List<Campus>> ObtenerCampus();
        public Task<List<Sede>> ObtenerSedes();
        public Task<List<UsuarioAdministradorDto>> ObtenerHistorialUsuario(int IdUsuario);
        public Task<HttpResponseMessage> GuardarCargaArchivo(ArchivoEntity archivo);
    }
}
