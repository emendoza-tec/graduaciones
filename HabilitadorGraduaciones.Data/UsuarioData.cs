using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class UsuarioData : IUsuarioRepository
    {
        public const string ConnectionStrings = "ConnectionStrings:DefaultConnection";
        public IConfiguration Configuration { get; }

        public UsuarioData(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<UsuarioDto> ObtenerUsuario(string matricula)
        {
            UsuarioDto usuario = new UsuarioDto();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Matricula", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, matricula)
            };

            using (IDataReader reader = await DataBase.GetReader("spUsuarios_ObtenerUsuario", CommandType.StoredProcedure, list, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    usuario.Matricula = ComprobarNulos.CheckNull<string>(reader["MATRICULA"]);
                    usuario.Nombre = ComprobarNulos.CheckStringNull(reader["NOMBRE"]);  // para pruebas
                    usuario.ApeidoPaterno = ComprobarNulos.CheckStringNull(reader["APELLIDO_PATERNO"]);
                    usuario.ApeidoMaterno = ComprobarNulos.CheckStringNull(reader["APELLIDO_MATERNO"]);
                    usuario.Correo = ComprobarNulos.CheckStringNull(reader["CORREO"]);
                    usuario.Curp = ComprobarNulos.CheckStringNull(reader["CURP"]); // para pruebas
                    usuario.CarreraId = ComprobarNulos.CheckStringNull(reader["CARRERAID"]);
                    usuario.Carrera = ComprobarNulos.CheckStringNull(reader["CARRERA"]);
                    usuario.ProgramaAcademico = ComprobarNulos.CheckStringNull(reader["PROGRAMAACADEMICO"]);
                    usuario.Mentor = ComprobarNulos.CheckStringNull(reader["MENTOR"]);
                    usuario.DirectorPrograma = ComprobarNulos.CheckStringNull(reader["DIRECTOR"]);
                    usuario.CorreoDirector = ComprobarNulos.CheckStringNull(reader["CORREODIRECTOR"]);
                    usuario.CorreoMentor = ComprobarNulos.CheckStringNull(reader["CORREOMENTOR"]);
                    usuario.PeriodoActual = ComprobarNulos.CheckStringNull(reader["LAST_TERM_CURSADO"]);
                    usuario.NivelAcademico = ComprobarNulos.CheckStringNull(reader["NIVEL_ACADEMICO"]);
                    usuario.ClaveProgramaAcademico = ComprobarNulos.CheckStringNull(reader["CLAVE_PROGRAMA_ACADEMICO"]);
                    usuario.ClaveCampus = ComprobarNulos.CheckStringNull(reader["CLAVE_CAMPUS"]);
                    usuario.PeriodoGraduacion = ComprobarNulos.CheckStringNull(reader["PERIODO_GRADUACION"]);
                    usuario.PeriodoTranscurridoActual = ComprobarNulos.CheckStringNull(reader["PERIODO_ACTUAL"]);
                    usuario.ClaveEstatusGraduacion = ComprobarNulos.CheckStringNull(reader["CLAVE_ESTATUS_GRADUACION"]);
                    usuario.Result = true;
                }
            }
            return usuario;
        }

        public async Task<List<UsuarioDto>> ObtenerUsuarioPorMatriculaONombre(BusquedaAlumno busqueda)
        {
            var ListUsuarios = new List<UsuarioDto>();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@IsMatricula", DbType.Boolean, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, busqueda.isMatricula),
                DataBase.CreateParameter("@Busqueda", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, busqueda.Busqueda),
                DataBase.CreateParameter("@pIdUsuario", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, busqueda.IdUsuario)
            };

            using (IDataReader reader = await DataBase.GetReader("spUsuarios_ObtenerUsuarioPorMatriculaONombre", CommandType.StoredProcedure, list, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    UsuarioDto usuario = new UsuarioDto();
                    usuario.Matricula = ComprobarNulos.CheckStringNull(reader["MATRICULA"]);
                    usuario.Nombre = ComprobarNulos.CheckStringNull(reader["NOMBRE"]);
                    usuario.Carrera = ComprobarNulos.CheckStringNull(reader["CARRERA"]);

                    ListUsuarios.Add(usuario);
                    usuario.Result = true;
                }
            }
            return ListUsuarios;
        }


        public async Task<List<UsuarioAdministradorDto>> ObtenerUsuarios()
        {
            var ListUsuarios = new List<UsuarioAdministradorDto>();
            using (IDataReader reader = await DataBase.GetReader("spUsuarios_ObtenerUsuarios", CommandType.StoredProcedure, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    UsuarioAdministradorDto usuario = new UsuarioAdministradorDto();
                    usuario.IdUsuario = ComprobarNulos.CheckIntNull(reader["IdUsuario"]);
                    usuario.Nomina = ComprobarNulos.CheckNull<string>(reader["NumeroNomina"]);
                    usuario.Nombre = ComprobarNulos.CheckStringNull(reader["Nombre"]);
                    usuario.Correo = ComprobarNulos.CheckStringNull(reader["Correo"]);
                    usuario.Campus = ComprobarNulos.CheckStringNull(reader["Campus"]);
                    usuario.Rol = ComprobarNulos.CheckStringNull(reader["Roles"]);
                    usuario.CantidadRoles = ComprobarNulos.CheckIntNull(reader["CantidadRoles"]);

                    ListUsuarios.Add(usuario);
                    usuario.Result = true;
                }
            }
            return ListUsuarios;
        }

        public async Task<UsuarioAdministradorDto> ObtenerUsuarioAdminsitrador(int IdUsuario)
        {
            UsuarioAdministradorDto usuario = new UsuarioAdministradorDto();
            List<Campus> ListCampus = new List<Campus>();
            List<Sede> ListSedes = new List<Sede>();
            List<RolesEntity> ListRoles = new List<RolesEntity>();
            List<Nivel> ListNiveles = new List<Nivel>();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@IdUsuario", DbType.Int32, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, IdUsuario)
            };
            try
            {
                using (IDataReader reader = await DataBase.GetReader("spUsuarios_ObtenerUsuarioAdministrador", CommandType.StoredProcedure, list, Configuration[ConnectionStrings]))
                {
                    while (reader.Read())
                    {
                        usuario.IdUsuario = ComprobarNulos.CheckIntNull(reader["IdUsuario"]);
                        usuario.Nomina = ComprobarNulos.CheckNull<string>(reader["NumeroNomina"]);
                        usuario.Nombre = ComprobarNulos.CheckStringNull(reader["Nombre"]);
                        usuario.Correo = ComprobarNulos.CheckStringNull(reader["Correo"]);
                        usuario.Estatus = ComprobarNulos.CheckIntNull(reader["Estatus"]);
                        usuario.FechaModificacion = ComprobarNulos.CheckDateTimeNull(reader["FechaModificacion"]);

                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        ListCampus.Add(new Campus
                        {
                            ClaveCampus = ComprobarNulos.CheckStringNull(reader["ClaveCampus"]),
                            Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"])
                        });
                    }
                    usuario.ListCampus = ListCampus;
                    reader.NextResult();
                    while (reader.Read())
                    {
                        ListSedes.Add(new Sede
                        {
                            ClaveSede = ComprobarNulos.CheckStringNull(reader["ClaveSede"]),
                            ClaveCampus = ComprobarNulos.CheckStringNull(reader["ClaveCampus"]),
                            Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"])
                        });
                    }
                    usuario.Sedes = ListSedes;
                    reader.NextResult();
                    while (reader.Read())
                    {
                        ListNiveles.Add(new Nivel
                        {
                            ClaveNivel = ComprobarNulos.CheckStringNull(reader["ClaveNivel"]),
                            Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"])
                        });
                    }
                    usuario.Niveles = ListNiveles;

                    reader.NextResult();

                    while (reader.Read())
                    {
                        ListRoles.Add(new RolesEntity
                        {
                            IdRol = ComprobarNulos.CheckIntNull(reader["IdRol"]),
                            Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"])
                        });
                    }
                    usuario.Roles = ListRoles;
                    usuario.Result = true;
                }
            }
            catch (Exception ex)
            {

                usuario.Result = false;
                usuario.ErrorMessage = "Ocurrio un Error al insertar en el método" + ex.Message;
                throw new CustomException("Ocurrió un error al obtener al usuario administrador en el método ObtenerUsuarioAdminsitrador", ex);
            }
            return usuario;
        }
        public async Task<UsuarioAdministradorDto> ObtenerUsuarioAdminsitradorPorNomina(string nomina)
        {
            UsuarioAdministradorDto usuario = new UsuarioAdministradorDto();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Nomina", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, nomina)
            };
            try
            {
                using (IDataReader reader = await DataBase.GetReader("spUsuarios_ObtenerUsuarioAdministradorPorNomina", CommandType.StoredProcedure, list, Configuration[ConnectionStrings]))
                {
                    while (reader.Read())
                    {
                        usuario.IdUsuario = ComprobarNulos.CheckIntNull(reader["IdUsuario"]);
                        usuario.Nomina = ComprobarNulos.CheckNull<string>(reader["NumeroNomina"]);
                        usuario.Nombre = ComprobarNulos.CheckStringNull(reader["Nombre"]);
                        usuario.Correo = ComprobarNulos.CheckStringNull(reader["Correo"]);
                        usuario.Estatus = ComprobarNulos.CheckIntNull(reader["Estatus"]);
                        usuario.FechaModificacion = ComprobarNulos.CheckDateTimeNull(reader["FechaModificacion"]);
                    }

                    usuario.Result = true;
                }
            }
            catch (Exception ex)
            {

                usuario.Result = false;
                usuario.ErrorMessage = "Ocurrio un Error al consultar en el método" + ex.Message; 
                throw new CustomException("Ocurrió un error al obtener al usuario administrador por nomina en el método ObtenerUsuarioAdminsitradorPorNomina", ex);

            }
            return usuario;
        }

        public async Task<UsuarioAdministradorDto> GuardarUsuario(UsuarioAdministradorDto usuario)
        {
            UsuarioAdministradorDto response = new UsuarioAdministradorDto();

            try
            {
                DataTable dtCampusSede = GetDataTableCampus(usuario);
                DataTable dtNivel = GetDataTableNivel(usuario);
                DataTable dtRoles = GetDataTableRoles(usuario);
                IList<ParameterSQl> list = new List<ParameterSQl>
                {
                    DataBase.CreateParameterSql("@IdUsuario", SqlDbType.Int, 11, ParameterDirection.Input, false, null, DataRowVersion.Default, usuario.IdUsuario),
                    DataBase.CreateParameterSql("@Nomina", SqlDbType.VarChar, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, usuario.Nomina),
                    DataBase.CreateParameterSql("@Nombre", SqlDbType.VarChar, 250, ParameterDirection.Input, false, null, DataRowVersion.Default, usuario.Nombre),
                    DataBase.CreateParameterSql("@Correo", SqlDbType.VarChar, 250, ParameterDirection.Input, false, null, DataRowVersion.Default, usuario.Correo),
                    DataBase.CreateParameterSql("@UsuarioModificacion", SqlDbType.VarChar, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, usuario.UsuarioModificacion),
                    DataBase.CreateParameterSql("@UsuarioCampusSede", SqlDbType.Structured, int.MaxValue, ParameterDirection.Input,false,null,DataRowVersion.Default, dtCampusSede),
                    DataBase.CreateParameterSql("@UsuarioNivel", SqlDbType.Structured, int.MaxValue, ParameterDirection.Input,false,null,DataRowVersion.Default, dtNivel),
                    DataBase.CreateParameterSql("@UsuarioRol", SqlDbType.Structured, int.MaxValue, ParameterDirection.Input,false,null,DataRowVersion.Default, dtRoles)
                };

                using (IDataReader reader = await DataBase.GetReaderSql("spUsuarios_InsertarUsuario", CommandType.StoredProcedure, list, Configuration[ConnectionStrings]))
                {
                    while (reader.Read())
                    {
                        response.Result = ComprobarNulos.CheckBooleanNull(reader["Result"]);
                        response.ErrorMessage = ComprobarNulos.CheckStringNull(reader["Mensaje"]);
                        response.IdUsuario = ComprobarNulos.CheckIntNull(reader["IdUsuario"]);
                    }
                }
            }
            catch (Exception ex)
            {

                response.Result = false;
                response.ErrorMessage = "Ocurrio un Error al insertar en el método" + ex.Message;
                throw new CustomException("Error al Guardar Usuario", ex);
            }
            return response;
        }

        public async Task<BaseOutDto> EliminarUsuario(int IdUsuario, string UsuarioElimino)
        {
            BaseOutDto response = new BaseOutDto();

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    DataBase.CreateParameter("@IdUsuario", DbType.Int32, 11, ParameterDirection.Input, false, null, DataRowVersion.Default, IdUsuario),
                    DataBase.CreateParameter("@UsuarioElimino", DbType.String, 11, ParameterDirection.Input, false, null, DataRowVersion.Default, UsuarioElimino)
                };
                using (IDataReader reader = await DataBase.GetReader("spUsuarios_EliminarUsuario", CommandType.StoredProcedure, list, Configuration[ConnectionStrings]))
                {
                    while (reader.Read())
                    {
                        response.Result = ComprobarNulos.CheckBooleanNull(reader["Result"]);
                        response.ErrorMessage = ComprobarNulos.CheckStringNull(reader["Mensaje"]);
                    }
                }
            }
            catch (Exception ex)
            {

                response.Result = false;
                response.ErrorMessage = "Ocurrio un Error al insertar en el método" + ex.Message;
                throw new CustomException("Error al Eliminar el Usuario", ex);
            }
            return response;
        }

        public async Task<List<UsuarioAdministradorDto>> ObtenerUsuarioNombrePorNomina(string nomina)
        {
            List<UsuarioAdministradorDto> listUsuarios = new List<UsuarioAdministradorDto>();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Nomina", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, nomina)
            };

            using (IDataReader reader = await DataBase.GetReader("spUsuarios_ObtenerNombrePorNomina", CommandType.StoredProcedure, list, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    UsuarioAdministradorDto usuario = new UsuarioAdministradorDto();
                    usuario.Nomina = ComprobarNulos.CheckNull<string>(reader["ID_AFILIACION"]);
                    usuario.Nombre = ComprobarNulos.CheckStringNull(reader["NOMBRE"]);
                    usuario.Correo = ComprobarNulos.CheckStringNull(reader["CORREO_ELECTRONICO"]);
                    usuario.Result = true;
                    listUsuarios.Add(usuario);
                }
            }
            return listUsuarios;
        }

        public async Task<List<Campus>> ObtenerCampus()
        {
            List<Campus> list = new List<Campus>();
            using (IDataReader reader = await DataBase.GetReader("spUsuarios_ObtenerCampus", CommandType.StoredProcedure, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    Campus campus = new Campus();
                    campus.ClaveCampus = ComprobarNulos.CheckStringNull(reader["ClaveCampus"]);
                    campus.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);

                    list.Add(campus);
                }
            }
            return list;
        }

        public async Task<List<Sede>> ObtenerSedes()
        {
            List<Sede> Sedes = new List<Sede>();
            using (IDataReader reader = await DataBase.GetReader("spUsuarios_ObtenerSede", CommandType.StoredProcedure, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    Sede campus = new Sede();
                    campus.ClaveCampus = ComprobarNulos.CheckStringNull(reader["ClaveCampus"]);
                    campus.ClaveSede = ComprobarNulos.CheckStringNull(reader["ClaveSede"]);
                    campus.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);

                    Sedes.Add(campus);
                }
            }
            return Sedes;
        }

        public async Task<List<UsuarioAdministradorDto>> ObtenerHistorialUsuario(int IdUsuario)
        {
            var ListUsuarios = new List<UsuarioAdministradorDto>();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@IdUsuario", DbType.Int32, 11, ParameterDirection.Input, false, null, DataRowVersion.Default, IdUsuario)
            };
            using (IDataReader reader = await DataBase.GetReader("spUsuarios_ObtenerHistorial", CommandType.StoredProcedure, list, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    UsuarioAdministradorDto usuario = new UsuarioAdministradorDto();
                    usuario.IdUsuario = ComprobarNulos.CheckIntNull(reader["IdUsuario"]);
                    usuario.Nombre = ComprobarNulos.CheckStringNull(reader["Nombre"]);
                    usuario.Campus = ComprobarNulos.CheckStringNull(reader["Campus"]);
                    usuario.Rol = ComprobarNulos.CheckStringNull(reader["Roles"]);
                    usuario.Sede = ComprobarNulos.CheckStringNull(reader["Sedes"]);
                    usuario.Nivel = ComprobarNulos.CheckStringNull(reader["Niveles"]);
                    usuario.FechaModificacion = ComprobarNulos.CheckDateTimeNull(reader["FechaModificacion"]);
                    usuario.UsuarioModificacion = ComprobarNulos.CheckStringNull(reader["UsuarioModifico"]);

                    ListUsuarios.Add(usuario);
                    usuario.Result = true;
                }
            }
            return ListUsuarios;
        }
        private DataTable GetDataTableCampus(UsuarioAdministradorDto usuario)
        {
            DataRow row;
            DataTable dtCampusSede = new DataTable("UsuarioCampusSedeType");

            DataColumn column = new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "IdUsuario",
                ReadOnly = true
            };
            dtCampusSede.Columns.Add(column);
            column = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "ClaveCampus",
                ReadOnly = true
            };
            dtCampusSede.Columns.Add(column);
            column = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "ClaveSede",
                ReadOnly = false
            };
            dtCampusSede.Columns.Add(column);
            foreach (var sede in usuario.Sedes)
            {
                row = dtCampusSede.NewRow();
                row["IdUsuario"] = usuario.IdUsuario;
                row["ClaveCampus"] = sede.ClaveCampus;
                row["ClaveSede"] = sede.ClaveSede;
                dtCampusSede.Rows.Add(row);
            }
            return dtCampusSede;
        }

        private DataTable GetDataTableNivel(UsuarioAdministradorDto usuario)
        {
            DataRow row;
            DataTable dtNivel = new DataTable("UsuarioNivelType");

            DataColumn column = new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "IdUsuario",
                ReadOnly = true
            };
            dtNivel.Columns.Add(column);
            column = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "ClaveNivel",
                ReadOnly = true
            };
            dtNivel.Columns.Add(column);
            foreach (var nivel in usuario.Niveles)
            {
                row = dtNivel.NewRow();
                row["IdUsuario"] = usuario.IdUsuario;
                row["ClaveNivel"] = nivel.ClaveNivel;
                dtNivel.Rows.Add(row);
            }
            return dtNivel;
        }
        private DataTable GetDataTableRoles(UsuarioAdministradorDto usuario)
        {
            DataRow row;
            DataTable dtRol = new DataTable("UsuarioRolType");

            DataColumn column = new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "IdUsuario",
                ReadOnly = true
            };
            dtRol.Columns.Add(column);
            column = new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "IdRol",
                ReadOnly = true
            };
            dtRol.Columns.Add(column);
            foreach (var rol in usuario.Roles)
            {
                if (rol.IdRol > 0)
                {
                    row = dtRol.NewRow();
                    row["IdUsuario"] = usuario.IdUsuario;
                    row["IdRol"] = rol.IdRol;
                    dtRol.Rows.Add(row);
                }
            }
            return dtRol;
        }

    }
}