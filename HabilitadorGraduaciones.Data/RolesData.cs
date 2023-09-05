using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HabilitadorGraduaciones.Data
{
    public class RolesData : IRolesRepository
    {
        private readonly string _connectionString;

        public RolesData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<RolesEntity>> ObtenRoles()
        {
            var roles = new List<RolesEntity>();

            using (IDataReader reader = await DataBase.GetReader("spRoles_ObtenerRoles", CommandType.StoredProcedure, _connectionString))
            {
                while (reader.Read())
                {
                    var rol = new RolesEntity();
                    rol.IdRol = ComprobarNulos.CheckIntNull(reader["IdRol"]);
                    rol.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);
                    rol.Estatus = ComprobarNulos.CheckBooleanNull(reader["Estatus"]);
                    rol.TotalUsuarios = ComprobarNulos.CheckIntNull(reader["Usuarios"]);
                    rol.UsuarioRegistro = ComprobarNulos.CheckStringNull(reader["UsuarioRegistro"]);
                    rol.FechaRegistro = ComprobarNulos.CheckDateTimeNull(reader["FechaRegistro"]);
                    rol.UsuarioModifico = ComprobarNulos.CheckStringNull(reader["UsuarioModifico"]);
                    rol.FechaModificacion = ComprobarNulos.CheckDateTimeNull(reader["FechaModificacion"]);
                    rol.Activo = ComprobarNulos.CheckBooleanNull(reader["Activo"]);
                    rol.Usuarios = await ObtenerUsuariosPorIdRol(rol.IdRol);
                    rol.Result = true;
                    roles.Add(rol);
                }
            }
            return roles;
        }

        public async Task<List<UsuariosRol>> ObtenerUsuariosPorIdRol(int idRol)
        {
            var listUsuarios = new List<UsuariosRol>();
            IList<Parameter> list = new List<Parameter>
             {
                  DataBase.CreateParameter("@pIdRol", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idRol)
             };
            using (IDataReader reader = await DataBase.GetReader("spRoles_ObtenerUsuariosPorRol", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var usuario = new UsuariosRol();
                    usuario.Id = ComprobarNulos.CheckIntNull(reader["Id"]);
                    usuario.IdUsuario = ComprobarNulos.CheckIntNull(reader["IdUsuario"]);
                    usuario.IdRol = ComprobarNulos.CheckIntNull(reader["IdRol"]);
                    usuario.NumeroNomina = ComprobarNulos.CheckStringNull(reader["NumeroNomina"]);
                    usuario.Nombre = ComprobarNulos.CheckStringNull(reader["Nombre"]);
                    usuario.Roles = ComprobarNulos.CheckIntNull(reader["Roles"]);
                    listUsuarios.Add(usuario);
                }
            }
            return listUsuarios;
        }

        public async Task<RolesEntity> ObtenerRolesPorId(int idRol)
        {
            var entity = new RolesEntity();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pIdRol", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idRol)
            };

            using (IDataReader reader = await DataBase.GetReader("spRoles_ObtenerRolPorIdRol", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    entity.IdRol = ComprobarNulos.CheckIntNull(reader["IdRol"]);
                    entity.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);
                    entity.Estatus = ComprobarNulos.CheckBooleanNull(reader["Estatus"]);
                    entity.Permisos = await ObtenerPermisosPorIdRol(entity.IdRol);
                    entity.Result = true;
                }
            }
            return entity;
        }

        public async Task<List<Permisos>> ObtenerPermisosPorIdRol(int idRol)
        {
            var dtoPermisos = new List<Permisos>();
            IList<Parameter> list = new List<Parameter>
             {
                  DataBase.CreateParameter("@pIdRol", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idRol)
             };
            using (IDataReader reader = await DataBase.GetReader("spSecciones_ObtenerSeccionesPorRol", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var permisos = new Permisos();
                    permisos.IdPermiso = ComprobarNulos.CheckIntNull(reader["IdPermiso"]);
                    permisos.IdMenu = ComprobarNulos.CheckIntNull(reader["IdMenu"]);
                    permisos.NombreMenu = ComprobarNulos.CheckStringNull(reader["NombreMenu"]);
                    permisos.IdSubMenu = ComprobarNulos.CheckIntNull(reader["IdSubMenu"]);
                    permisos.NombreSubMenu = ComprobarNulos.CheckStringNull(reader["NombreSubMenu"]);
                    permisos.Ver = ComprobarNulos.CheckBooleanNull(reader["Ver"]);
                    permisos.Editar = ComprobarNulos.CheckBooleanNull(reader["Editar"]);
                    permisos.Activa = ComprobarNulos.CheckBooleanNull(reader["Activa"]);
                    dtoPermisos.Add(permisos);
                }
            }
            return dtoPermisos;
        }

        public async Task<BaseOutDto> GuardaRol(RolesEntity rol)
        {
            BaseOutDto insert = new BaseOutDto();
            try
            {
                if (rol != null)
                {
                    IList<Parameter> list = new List<Parameter>
                    {
                        DataBase.CreateParameter("@OK", DbType.Boolean, 1, ParameterDirection.Output, false, null, DataRowVersion.Default, rol.Result ),
                        DataBase.CreateParameter("@Error", DbType.AnsiString, -1, ParameterDirection.Output, false, null, DataRowVersion.Default, rol.Error ),
                        DataBase.CreateParameter("@pIdRol", DbType.Int32, 10, ParameterDirection.Output, false, null, DataRowVersion.Default, rol.IdRol),
                        DataBase.CreateParameter("@pDescripcion", DbType.AnsiString, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.Descripcion),
                        DataBase.CreateParameter("@pEstatus", DbType.Boolean, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.Estatus),
                        DataBase.CreateParameter("@pUsarioRegistro", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.UsuarioRegistro)
                    };

                    SqlCommand paramsOut = await DataBase.InsertOut("spRoles_InsertaRol", CommandType.StoredProcedure, list, _connectionString);
                    rol.Result = Convert.ToBoolean(paramsOut.Parameters["@OK"].Value);
                    rol.Error = Convert.ToString(paramsOut.Parameters["@Error"].Value);
                    rol.IdRol = Convert.ToInt32(paramsOut.Parameters["@pIdRol"].Value);

                    if (rol.Result)
                        await GuardaoModificaPermisos(rol.Permisos, rol.IdRol);

                    insert.Result = rol.Result;
                }
                else
                {
                    insert.Result = false;
                }
            }
            catch (Exception ex)
            {
                insert.Result = false;
                insert.ErrorMessage = ex.Message;
                throw new CustomException("Error al guardar el rol", ex);
            }
            return insert;
        }

        private async Task GuardaoModificaPermisos(List<Permisos> permisos, int idRol)
        {
            foreach (var permiso in permisos)
            {
                IList<Parameter> list = new List<Parameter>
                {
                    DataBase.CreateParameter("@OK", DbType.Boolean, 1, ParameterDirection.Output, false, null, DataRowVersion.Default, permiso.Ok ),
                    DataBase.CreateParameter("@Error", DbType.AnsiString, -1, ParameterDirection.Output, false, null, DataRowVersion.Default, permiso.Error ),
                    DataBase.CreateParameter("@pIdRol", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idRol),
                    DataBase.CreateParameter("@pIdMenu", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, permiso.IdMenu),
                    DataBase.CreateParameter("@IdSubMenu", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, permiso.IdSubMenu),
                    DataBase.CreateParameter("@pVer", DbType.Boolean, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, permiso.Ver),
                    DataBase.CreateParameter("@pEditar", DbType.Boolean, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, permiso.Editar),
                    DataBase.CreateParameter("@pActiva", DbType.Boolean, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, permiso.Activa)
                };

                SqlCommand paramsOut = await DataBase.InsertOut("spSecciones_InsertarActualizarPermisos", CommandType.StoredProcedure, list, _connectionString);
                permiso.Ok = Convert.ToBoolean(paramsOut.Parameters["@OK"].Value);
                permiso.Error = Convert.ToString(paramsOut.Parameters["@Error"].Value);
            }
        }

        public async Task<BaseOutDto> ModificaRol(RolesEntity rol)
        {
            BaseOutDto update = new BaseOutDto();
            try
            {
                if (rol != null)
                {
                    IList<Parameter> list = new List<Parameter>
                    {
                        DataBase.CreateParameter("@OK", DbType.Boolean, 1, ParameterDirection.Output, false, null, DataRowVersion.Default, rol.Result ),
                        DataBase.CreateParameter("@Error", DbType.AnsiString, -1, ParameterDirection.Output, false, null, DataRowVersion.Default, rol.Error ),
                        DataBase.CreateParameter("@pIdRol", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.IdRol),
                        DataBase.CreateParameter("@pDescripcion", DbType.AnsiString, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.Descripcion),
                        DataBase.CreateParameter("@pEstatus", DbType.Boolean, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.Estatus),
                        DataBase.CreateParameter("@pUsuarioModifico", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.UsuarioModifico)
                    };

                    SqlCommand paramsOut = await DataBase.UpdateOut("spRoles_ModificaRol", CommandType.StoredProcedure, list, _connectionString);
                    rol.Result = Convert.ToBoolean(paramsOut.Parameters["@OK"].Value);
                    rol.Error = Convert.ToString(paramsOut.Parameters["@Error"].Value);

                    if (rol.Result)
                        await GuardaoModificaPermisos(rol.Permisos, rol.IdRol);

                    update.Result = rol.Result;
                }
                else
                {
                    update.Result = false;
                }
            }
            catch (Exception ex)
            {
                update.Result = false;
                update.ErrorMessage = ex.Message;
                throw new CustomException("Error al modificar el rol", ex);
            }
            return update;
        }

        public async Task<BaseOutDto> CambiaEstatusRol(RolesEntity rol)
        {
            BaseOutDto update = new BaseOutDto();
            try
            {
                if (rol != null)
                {
                    IList<Parameter> list = new List<Parameter>
                    {
                        DataBase.CreateParameter("@OK", DbType.Boolean, 1, ParameterDirection.Output, false, null, DataRowVersion.Default, rol.Result ),
                        DataBase.CreateParameter("@Error", DbType.AnsiString, -1, ParameterDirection.Output, false, null, DataRowVersion.Default, rol.Error ),
                        DataBase.CreateParameter("@pIdRol", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.IdRol),
                        DataBase.CreateParameter("@pDescripcion", DbType.AnsiString, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.Descripcion),
                        DataBase.CreateParameter("@pEstatus", DbType.Boolean, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.Estatus),
                        DataBase.CreateParameter("@pUsuarioModifico", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.UsuarioModifico)
                    };

                    SqlCommand paramsOut = await DataBase.UpdateOut("spRoles_ModificaEstatusRol", CommandType.StoredProcedure, list, _connectionString);
                    rol.Result = Convert.ToBoolean(paramsOut.Parameters["@OK"].Value);
                    rol.Error = Convert.ToString(paramsOut.Parameters["@Error"].Value);
                    update.Result = rol.Result;
                }
                else
                {
                    update.Result = false;
                }

            }
            catch (Exception ex)
            {
                update.Result = false;
                update.ErrorMessage = ex.Message;
                throw new CustomException("Error al cambiar de estatus el rol", ex);
            }
            return update;
        }

        public async Task<BaseOutDto> EliminaRol(RolesEntity rol)
        {
            BaseOutDto delete = new BaseOutDto();
            try
            {
                if (rol != null)
                {
                    IList<Parameter> list = new List<Parameter>
                    {
                        DataBase.CreateParameter("@OK", DbType.Boolean, 1, ParameterDirection.Output, false, null, DataRowVersion.Default, rol.Result ),
                        DataBase.CreateParameter("@Error", DbType.AnsiString, -1, ParameterDirection.Output, false, null, DataRowVersion.Default, rol.Error ),
                        DataBase.CreateParameter("@pIdRol", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.IdRol),
                        DataBase.CreateParameter("@pDescripcion", DbType.AnsiString, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.Descripcion),
                        DataBase.CreateParameter("@pEstatus", DbType.Boolean, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.Estatus),
                        DataBase.CreateParameter("@pUsuarioModifico", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, rol.UsuarioModifico)
                    };

                    SqlCommand paramsOut = await DataBase.UpdateOut("spRoles_EliminarRol", CommandType.StoredProcedure, list, _connectionString);
                    rol.Result = Convert.ToBoolean(paramsOut.Parameters["@OK"].Value);
                    rol.Error = Convert.ToString(paramsOut.Parameters["@Error"].Value);
                    delete.Result = rol.Result;
                }
                else
                {
                    delete.Result = false;
                }
            }
            catch (Exception ex)
            {
                delete.Result = false;
                delete.ErrorMessage = ex.Message;
                throw new CustomException("Error al eliminar el rol", ex);
            }
            return delete;
        }

        public async Task<List<SeccionesPermisosDto>> ObtenerSecciones()
        {
            var secciones = new List<SeccionesPermisosDto>();

            using (IDataReader reader = await DataBase.GetReader("spSecciones_ObtenerSecciones", CommandType.StoredProcedure, _connectionString))
            {
                while (reader.Read())
                {
                    var seccion = new SeccionesPermisosDto();
                    seccion.IdMenu = ComprobarNulos.CheckIntNull(reader["IdMenu"]);
                    seccion.NombreMenu = ComprobarNulos.CheckStringNull(reader["NombreMenu"]);
                    seccion.IdSubMenu = ComprobarNulos.CheckIntNull(reader["IdSubMenu"]);
                    seccion.NombreSubMenu = ComprobarNulos.CheckStringNull(reader["NombreSubMenu"]);
                    seccion.Ver = false;
                    seccion.Editar = false;
                    seccion.Activa = true;
                    seccion.Result = true;
                    secciones.Add(seccion);
                }
            }
            return secciones;
        }

        public async Task<List<RolesDto>> ObtenerDescripcionRoles()
        {
            var roles = new List<RolesDto>();

            using (IDataReader reader = await DataBase.GetReader("spRoles_ObtenerDescripcionRoles", CommandType.StoredProcedure, _connectionString))
            {
                while (reader.Read())
                {
                    var rol = new RolesDto();
                    rol.IdRol = ComprobarNulos.CheckIntNull(reader["IdRol"]);
                    rol.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);
                    rol.Result = true;
                    roles.Add(rol);
                }
            }
            return roles;
        }
    }
}
