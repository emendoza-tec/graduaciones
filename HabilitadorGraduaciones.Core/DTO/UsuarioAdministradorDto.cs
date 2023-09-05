using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class UsuarioAdministradorDto : BaseOutDto
    {
        public int IdUsuario { get; set; }
        public string Nomina { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int Estatus { get; set; }
        public string Rol { get; set; }
        public int CantidadRoles { get; set; }
        public string Campus { get; set; }
        public string Sede { get; set; }
        public string Nivel { get; set; }
        public string UsuarioModificacion { get; set; }
        public List<RolesEntity> Roles { get; set; }
        public List<Campus> ListCampus { get; set; }
        public List<Sede> Sedes { get; set; }
        public List<Nivel> Niveles { get; set; }
    }
    public class Campus
    {
        public string ClaveCampus { get; set; }
        public string Descripcion { get; set; }

    }
    public class Sede
    {
        public string ClaveCampus { get; set; }
        public string ClaveSede { get; set; }
        public string Descripcion { get; set; }
    }
    public class Nivel
    {
        public string ClaveNivel { get; set; }
        public string Descripcion { get; set; }
    }
}
