namespace HabilitadorGraduaciones.Core.Entities
{
    public class RolesEntity
    {
        public int IdRol { get; set; }
        public string Descripcion { get; set; }
        public bool Estatus { get; set; }
        public int TotalUsuarios { get; set; }
        public List<UsuariosRol> Usuarios { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioModifico { get; set; }
        public DateTime FechaModificacion { get; set; }
        public bool Activo { get; set; }
        public List<Permisos> Permisos { get; set; }
        public bool Result { get; set; }
        public string Error { get; set; }
    }

    public class Permisos
    {
        public int IdPermiso { get; set; }
        public int IdMenu { get; set; }
        public string NombreMenu { get; set; }
        public int IdSubMenu { get; set; }
        public string NombreSubMenu { get; set; }
        public bool Ver { get; set; }
        public bool Editar { get; set; }
        public bool Activa { get; set; }
        public bool Ok { get; set; }
        public string Error { get; set; }
    }

    public class UsuariosRol
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string NumeroNomina { get; set; }
        public string Nombre { get; set; }
        public int Roles { get; set; }
    }
}
