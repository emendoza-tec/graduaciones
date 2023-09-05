using HabilitadorGraduaciones.Core.DTO;

namespace HabilitadorGraduaciones.Data.Utils
{
    public class GeneraPermisos
    {
        public List<PermisosMenu> PermisosMenu(List<RolesNomina> roles)
        {
            List<PermisosMenu> permisosMenu = new List<PermisosMenu>();
            if (roles.Count > 1)
            {
                int idRol = ObtenerRolConMayorSecciones(roles);
                RolesNomina seccionesRol = roles.Find(r => r.IdRol == idRol);
                bool isTodos = VerificaTodosLosPermisos(seccionesRol.Permisos);
                if (!isTodos)
                {
                    List<PermisosRol> listaPermisos = ExtraerPermisosDeRoles(roles.Where(r => r.IdRol != idRol).ToList());
                    List<PermisosRol> listaCombinadaDePermisos = CombinarPermisosRoles(seccionesRol.Permisos.OrderBy(m => m.IdMenu).ToList(),
                                listaPermisos.OrderBy(m => m.IdMenu).ToList());
                    permisosMenu = Menu(listaCombinadaDePermisos);
                }
                else
                {
                    permisosMenu = Menu(seccionesRol.Permisos.OrderBy(m => m.IdMenu).ToList());
                }
            }
            else
            {
                if (roles.Count == 1)
                {
                    permisosMenu = Menu(roles[0].Permisos.OrderBy(m => m.IdMenu).ToList());
                }
            }
            return permisosMenu;
        }

        private static int ObtenerRolConMayorSecciones(List<RolesNomina> roles)
        {
            int idRolMayor = 0;
            int mayor = 0;
            foreach (RolesNomina rol in roles)
            {
                if (rol.Permisos.Count > mayor)
                {
                    mayor = rol.Permisos.Count;
                    idRolMayor = rol.IdRol;
                }
            }
            return idRolMayor;
        }

        private static List<PermisosRol> ExtraerPermisosDeRoles(List<RolesNomina> rolesNominas)
        {
            var listaPermisos = new List<PermisosRol>();
            foreach (var rol in rolesNominas)
            {
                foreach (var permisosRol in rol.Permisos)
                {
                    PermisosRol permiso = new()
                    {
                        IdPermiso = permisosRol.IdPermiso,
                        IdMenu = permisosRol.IdMenu,
                        NombreMenu = permisosRol.NombreMenu,
                        IdSubMenu = permisosRol.IdSubMenu,
                        NombreSubMenu = permisosRol.NombreSubMenu,
                        Ver = permisosRol.Ver,
                        Editar = permisosRol.Editar,
                        Activa = permisosRol.Activa
                    };
                    listaPermisos.Add(permiso);
                }
            }
            return listaPermisos;
        }

        private static bool VerificaTodosLosPermisos(List<PermisosRol> permisos)
        {
            bool isTodos = true;
            foreach (var permiso in permisos)
            {
                if (!permiso.Editar)
                {
                    isTodos = false;
                    return isTodos;
                }
            }

            return isTodos;
        }

        private static List<PermisosRol> CombinarPermisosRoles(List<PermisosRol> permisos, List<PermisosRol> listaPermisos)
        {
            foreach (var (permiso, permisoLista) in from permiso in permisos
                                                    from permisoLista in listaPermisos
                                                    where permiso.IdMenu == permisoLista.IdMenu && permiso.IdSubMenu == permisoLista.IdSubMenu
                                                    select (permiso, permisoLista))
            {
                if (permisoLista.Editar && !permiso.Editar)
                {
                    permiso.Editar = permisoLista.Editar;
                    permiso.Ver = permisoLista.Ver;
                }

                if (permisoLista.Ver && !permiso.Ver)
                {
                    permiso.Ver = permisoLista.Ver;
                }
            }

            return permisos;
        }

        private static List<PermisosMenu> Menu(List<PermisosRol> listaPermisos)
        {
            var menu = new List<PermisosMenu>();
            foreach (var permiso in listaPermisos)
            {
                PermisosMenu permisosMenu = new()
                {
                    IdPermiso = permiso.IdPermiso,
                    IdMenu = permiso.IdMenu,
                    NombreMenu = permiso.NombreMenu,
                    PathMenu = permiso.PathMenu,
                    IconoMenu = permiso.IconoMenu,
                    IdSubMenu = permiso.IdSubMenu,
                    NombreSubMenu = permiso.NombreSubMenu,
                    PathSubMenu = permiso.PathSubMenu,
                    IconoSubMenu = permiso.IconoSubMenu,
                    Seccion = permiso.Seccion,
                    Ver = permiso.Ver,
                    Editar = permiso.Editar,
                    Activa = permiso.Activa
                };
                menu.Add(permisosMenu);
            }
            return menu;
        }
    }
}
