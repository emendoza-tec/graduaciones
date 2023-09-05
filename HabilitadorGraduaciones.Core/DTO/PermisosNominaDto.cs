using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class PermisosNominaDto : BaseOutDto
    {
        public string Nomina { get; set; }
        public int IdUsuario { get; set; }
        public string Ambiente { get; set; }
        public bool Acceso { get; set; }
        public List<RolesNomina> Roles { get; set; }
        public List<NivelesNomina> Niveles { get; set; }
        public List<CampusNomina> Campus { get; set; }
        public List<PermisosMenu> Menu { get; set; }
    }

    public class RolesNomina
    {
        public int IdRol { get; set; }
        public string Descripcion { get; set; }
        public List<PermisosRol> Permisos { get; set; }
        public bool Result { get; set; }
    }

    public class PermisosRol
    {
        public int IdPermiso { get; set; }
        public int IdMenu { get; set; }
        public string NombreMenu { get; set; }
        public string PathMenu { get; set; }
        public string IconoMenu { get; set; }
        public int IdSubMenu { get; set; }
        public string NombreSubMenu { get; set; }
        public string PathSubMenu { get; set; }
        public string IconoSubMenu { get; set; }
        public bool Seccion { get; set; }
        public bool Ver { get; set; }
        public bool Editar { get; set; }
        public bool Activa { get; set; }
    }

    public class NivelesNomina
    {
        public string ClaveNivel { get; set; }
        public string Descripcion { get; set; }
        public bool Result { get; set; }
    }

    public class CampusNomina
    {
        public string ClaveCampus { get; set; }
        public string Descripcion { get; set; }
        public List<Sedes> Sedes { get; set; }
        public bool Result { get; set; }
    }

    public class Sedes
    {
        public string ClaveSede { get; set; }
        public string Descripcion { get; set; }
    }

    public class PermisosMenu
    {
        public int IdPermiso { get; set; }
        public int IdMenu { get; set; }
        public string NombreMenu { get; set; }
        public string PathMenu { get; set; }
        public string IconoMenu { get; set; }
        public int IdSubMenu { get; set; }
        public string NombreSubMenu { get; set; }
        public string PathSubMenu { get; set; }
        public string IconoSubMenu { get; set; }
        public bool Seccion { get; set; }
        public bool Ver { get; set; }
        public bool Editar { get; set; }
        public bool Activa { get; set; }
    }
}
