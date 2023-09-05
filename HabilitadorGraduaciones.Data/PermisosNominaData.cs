using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class PermisosNominaData : IPermisosNominaRepository
    {
        private readonly string _connectionString;
        private readonly GeneraPermisos _generaPermisos = new GeneraPermisos();

        public PermisosNominaData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<PermisosNominaDto> ObtenerPermisosPorNomina(string nomina)
        {
            var dto = new PermisosNominaDto();

            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Matricula", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, nomina)
            };

            using (IDataReader reader = await DataBase.GetReader("spAccesoNomina_ObtenerNomina", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    dto.Nomina = nomina;
                    dto.IdUsuario = ComprobarNulos.CheckIntNull(reader["IdUsuario"]);
                    dto.Ambiente = ComprobarNulos.CheckStringNull(reader["Ambiente"]);
                    dto.Acceso = ComprobarNulos.CheckBooleanNull(reader["Acceso"]);
                    dto.IdUsuario = ComprobarNulos.CheckIntNull(reader["IdUsuario"]);
                    dto.Roles = await ObtenerRolesPorNomina(dto.Nomina);
                    dto.Niveles = await ObtenerNivelesPorNomina(dto.Nomina);
                    dto.Campus = await ObtenerCampusPorNomina(dto.Nomina);
                    dto.Result = true;
                }
            }
            if (dto.Roles != null)
            {
                dto.Menu = _generaPermisos.PermisosMenu(dto.Roles);
            }

            return dto;
        }

        private async Task<List<RolesNomina>> ObtenerRolesPorNomina(string nomina)
        {
            var dtoRoles = new List<RolesNomina>();
            IList<Parameter> list = new List<Parameter>
             {
                  DataBase.CreateParameter("@pNumeroNomina", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, nomina)
             };
            using (IDataReader reader = await DataBase.GetReader("spRoles_ObtenerRolesPorNomina", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var rol = new RolesNomina();
                    rol.IdRol = ComprobarNulos.CheckIntNull(reader["IdRol"]);
                    rol.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);
                    rol.Permisos = await ObtenerPermisosPorIdRol(rol.IdRol);
                    rol.Result = true;
                    dtoRoles.Add(rol);
                }
            }
            return dtoRoles;
        }

        private async Task<List<PermisosRol>> ObtenerPermisosPorIdRol(int idRol)
        {
            var dtoPermisos = new List<PermisosRol>();
            IList<Parameter> list = new List<Parameter>
             {
                  DataBase.CreateParameter("@pIdRol", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idRol)
             };
            using (IDataReader reader = await DataBase.GetReader("spSecciones_ObtenerSeccionesPorRol", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var permisos = new PermisosRol();
                    permisos.IdPermiso = ComprobarNulos.CheckIntNull(reader["IdPermiso"]);
                    permisos.IdMenu = ComprobarNulos.CheckIntNull(reader["IdMenu"]);
                    permisos.NombreMenu = ComprobarNulos.CheckStringNull(reader["NombreMenu"]);
                    permisos.PathMenu = ComprobarNulos.CheckStringNull(reader["PathMenu"]);
                    permisos.IconoMenu = ComprobarNulos.CheckStringNull(reader["IconoMenu"]);
                    permisos.IdSubMenu = ComprobarNulos.CheckIntNull(reader["IdSubMenu"]);
                    permisos.NombreSubMenu = ComprobarNulos.CheckStringNull(reader["NombreSubMenu"]);
                    permisos.PathSubMenu = ComprobarNulos.CheckStringNull(reader["PathSubMenu"]);
                    permisos.IconoSubMenu = ComprobarNulos.CheckStringNull(reader["IconoSubMenu"]);
                    permisos.Seccion = ComprobarNulos.CheckBooleanNull(reader["Seccion"]);
                    permisos.Ver = ComprobarNulos.CheckBooleanNull(reader["Ver"]);
                    permisos.Editar = ComprobarNulos.CheckBooleanNull(reader["Editar"]);
                    permisos.Activa = ComprobarNulos.CheckBooleanNull(reader["Activa"]);
                    dtoPermisos.Add(permisos);
                }
            }
            return dtoPermisos;
        }

        private async Task<List<NivelesNomina>> ObtenerNivelesPorNomina(string nomina)
        {
            var dtoNiveles = new List<NivelesNomina>();
            IList<Parameter> list = new List<Parameter>
             {
                  DataBase.CreateParameter("@pNumeroNomina", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, nomina)
             };
            using (IDataReader reader = await DataBase.GetReader("spNivel_ObtenerNivelesPorNomina", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var nivel = new NivelesNomina();
                    nivel.ClaveNivel = ComprobarNulos.CheckStringNull(reader["ClaveNivel"]);
                    nivel.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);
                    nivel.Result = true;
                    dtoNiveles.Add(nivel);
                }
            }
            return dtoNiveles;
        }

        private async Task<List<CampusNomina>> ObtenerCampusPorNomina(string nomina)
        {
            var dtoRoles = new List<CampusNomina>();
            IList<Parameter> list = new List<Parameter>
             {
                  DataBase.CreateParameter("@pNumeroNomina", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, nomina)
             };
            using (IDataReader reader = await DataBase.GetReader("spCampus_ObtenerCampusPorNomina", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var campus = new CampusNomina();
                    campus.ClaveCampus = ComprobarNulos.CheckStringNull(reader["ClaveCampus"]);
                    campus.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);
                    campus.Sedes = await ObtenerSedesPorCampusNomina(nomina, campus.ClaveCampus);
                    campus.Result = true;
                    dtoRoles.Add(campus);
                }
            }
            return dtoRoles;
        }

        private async Task<List<Sedes>> ObtenerSedesPorCampusNomina(string nomina, string claveCampus)
        {
            var dtoSedes = new List<Sedes>();
            IList<Parameter> list = new List<Parameter>
             {
                  DataBase.CreateParameter("@pNumeroNomina", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, nomina),
                  DataBase.CreateParameter("@pClaveCampus", DbType.AnsiString, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, claveCampus)
             };
            using (IDataReader reader = await DataBase.GetReader("spSedes_ObtenerSedesPorCampusNomina", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var sede = new Sedes();
                    sede.ClaveSede = ComprobarNulos.CheckStringNull(reader["ClaveSede"]);
                    sede.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);
                    dtoSedes.Add(sede);
                }
            }
            return dtoSedes;
        }
    }
}
