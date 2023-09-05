using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Core.Entities.Bases;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using HabilitadorGraduaciones.Data.Utils.Enums;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace HabilitadorGraduaciones.Data
{
    public class AvisosData : IAvisosRepository
    {
        private readonly string _connectionString;
        public IConfiguration _configuration { get; }
        private readonly IEmailModuleRepository _email;
        public AvisosData(IConfiguration configuration, IEmailModuleRepository email)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
            _email = email;
        }
        public async Task<BaseOutDto> SetAviso(AvisoGuardar entity)
        {
            BaseOutDto result = new BaseOutDto();
            int idAviso;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    DataBase.CreateParameter("@titulo", DbType.AnsiString, 255, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.Titulo ),
                    DataBase.CreateParameter("@texto", DbType.AnsiString, -1, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.Texto ),
                    DataBase.CreateParameter("@urlImagen", DbType.AnsiString, 255, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.UrlImage ),
                    DataBase.CreateParameter("@fechaCreacion", DbType.DateTime, 8, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.FechaCreacion ),
                    DataBase.CreateParameter("@matricula", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.Matricula ),
                    DataBase.CreateParameter("@campusId", DbType.AnsiString, 3, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.CampusId ),
                    DataBase.CreateParameter("@sedeId", DbType.AnsiString, 3, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.SedeId ),
                    DataBase.CreateParameter("@requisitoId", DbType.AnsiString, 255, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.RequisitoId ),
                    DataBase.CreateParameter("@programaId", DbType.AnsiString, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.ProgramaId ),
                    DataBase.CreateParameter("@escuelasId", DbType.AnsiString, 2, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.EscuelasId ),
                    DataBase.CreateParameter("@cc_rolesId", DbType.AnsiString, 255, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.Cc_rolesId ),
                    DataBase.CreateParameter("@cc_camposId", DbType.AnsiString, 3, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.Cc_camposId ),
                    DataBase.CreateParameter("@nivel", DbType.AnsiString, 2, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.Nivel ),
                    DataBase.CreateParameter("@correo", DbType.Boolean, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.Correo ),
                    DataBase.CreateParameter("@habilitador", DbType.Boolean, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.Habilitador ),
                    DataBase.CreateParameter("@cumple", DbType.Int32, 4, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.Cumple ),
                    DataBase.CreateParameter("@IdUsuario", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Aviso.IdUsuario ),
                    DataBase.CreateParameter("@id", DbType.Int32, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null )
                };

                SqlCommand paramsOut = await DataBase.InsertOut("spAvisos_InsertarAviso", CommandType.StoredProcedure, list, _connectionString);
                idAviso = Convert.ToInt32(paramsOut.Parameters["@id"].Value);
                entity.Result = true;
                SendEmails(idAviso, entity);
                result.Result = true;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrio un error en el método SetAviso", ex);
            }
            
            return result;
        }
        public async void SendEmails(int idAviso, AvisoGuardar entity)
        {
            Aviso aviso = entity.Aviso;
            List<CorreosEnviadosEntity> lstCorreos;
            StringBuilder template = HtmlTemplate.GetTemplateAvisos();
            template.Replace("Firma de título", aviso.Titulo);
            template.Replace("Recuerda que tu firma de título será el día 24 de junio en Aula Magna a las 11:00 a.m. ¡Te esperamos!", aviso.Texto);
            template.Replace("10 / 11 / 2022", aviso.FechaCreacion.ToString("dd / MM / yyyy"));
            template.Replace("HomePage", _configuration["ConfiguracionLinks:Home"]);
            if (string.IsNullOrEmpty(aviso.UrlImage))
            {
                template.Replace("cid:imgAdjunta", "");
            }
            StringBuilder listId;
            bool finish = false;
            
            while (!finish)
            {
                lstCorreos = await GetCorreoLote(idAviso);
                if (lstCorreos.Count > 0)
                {
                    listId = new();
                    foreach (CorreosEnviadosEntity correo in lstCorreos)
                    {
                        StringBuilder textoCorreo = template.Replace("Usuario", correo.Nombre);
                        BaseOutDto enviado = await _email.EnviarCorreo(correo.Correo, aviso.Titulo, textoCorreo.ToString(), aviso.UrlImage, "", (int)TipoCorreo.Avisos);

                        if (enviado.Result)
                        {
                            listId.Append(correo.Id);
                            listId.Append(',');
                        }
                    }
                    if (listId.Length > 0)
                    {
                        listId.Remove(listId.Length - 1, 1);
                        await SetCorreoEnviado(listId);
                    }
                }
                else
                {
                    finish = true;
                }
            }
        }
        public async Task<bool> SetCorreoEnviado(StringBuilder listId)
        {
            bool result = false;
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@user_correo_list", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, listId.ToString())
            };
            await DataBase.GetReader("spCorreos_Enviados_update", CommandType.StoredProcedure, list, _connectionString);
            return result;
        }
        public async Task<List<CorreosEnviadosEntity>> GetCorreoLote(int idAviso)
        {
            List<CorreosEnviadosEntity> lstCorreos = new();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@idAviso", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, idAviso)
            };
            using (IDataReader reader = await DataBase.GetReader("spCorreos_Enviados_Get", CommandType.StoredProcedure, list, _connectionString))
            {

                while (reader.Read())
                {
                    CorreosEnviadosEntity correo = new()
                    {
                        Id = ComprobarNulos.CheckNull<int>(reader["CorreoId"]),
                        Correo = ComprobarNulos.CheckNull<string>(reader["Correo"]),
                        Nombre = ComprobarNulos.CheckNull<string>(reader["Nombre"])
                    };

                    lstCorreos.Add(correo);
                }
            }
            return lstCorreos;
        }
        public async Task<AvisosDto> Get3Avisos(AvisosEntity entity)
        {
            AvisosDto avisos = new AvisosDto();
            avisos.Result = false;
            List<Aviso> lstAvisos = new();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Matricula", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Matricula)
            };
            using IDataReader reader = await DataBase.GetReader("spAvisos_Obtener3Avisos", CommandType.StoredProcedure, list, _connectionString);
            while (reader.Read())
            {
                Aviso aviso = new()
                {
                    Id = ComprobarNulos.CheckNull<int>(reader["AvisoId"]),
                    Titulo = ComprobarNulos.CheckNull<string>(reader["Titulo"]),
                    Texto = ComprobarNulos.CheckNull<string>(reader["Texto"]),
                    CampusId = ComprobarNulos.CheckNull<string>(reader["CampusId"]),
                    SedeId = ComprobarNulos.CheckNull<string>(reader["SedeId"]),
                    RequisitoId = ComprobarNulos.CheckNull<string>(reader["RequisitoId"]),
                    ProgramaId = ComprobarNulos.CheckNull<string>(reader["ProgramaId"]),
                    EscuelasId = ComprobarNulos.CheckNull<string>(reader["EscuelasId"]),
                    Cc_rolesId = ComprobarNulos.CheckNull<string>(reader["CcRolesId"]),
                    Cc_camposId = ComprobarNulos.CheckNull<string>(reader["CcCampusId"]),
                    Nivel = ComprobarNulos.CheckNull<string>(reader["Nivel"]),
                    FechaCreacion = ComprobarNulos.CheckNull<DateTime>(reader["FechaCreacion"]),
                    Correo = ComprobarNulos.CheckNull<Boolean>(reader["Correo"]),
                    Habilitador = ComprobarNulos.CheckNull<Boolean>(reader["Habilitador"]),
                    UrlImage = ComprobarNulos.CheckNull<string>(reader["UrlImagen"])
                };

                lstAvisos.Add(aviso);
            }
            avisos.lstAvisos = new List<Aviso>();
            avisos.lstAvisos = lstAvisos;
            avisos.Result = true;
            return avisos;
        }
        public async Task<AvisosDto> GetAvisos(AvisosEntity entity)
        {
            AvisosDto avisos = new AvisosDto();
            avisos.Result = false;
            List<Aviso> lstAvisos = new();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Matricula", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Matricula)
            };
            using IDataReader reader = await DataBase.GetReader("spAvisos_ObtenerAvisos", CommandType.StoredProcedure, list, _connectionString);

            while (reader.Read())
            {
                Aviso aviso = new()
                {
                    Id = ComprobarNulos.CheckNull<int>(reader["AvisoId"]),
                    Titulo = ComprobarNulos.CheckNull<string>(reader["Titulo"]),
                    Texto = ComprobarNulos.CheckNull<string>(reader["Texto"]),
                    CampusId = ComprobarNulos.CheckNull<string>(reader["CampusId"]),
                    SedeId = ComprobarNulos.CheckNull<string>(reader["SedeId"]),
                    RequisitoId = ComprobarNulos.CheckNull<string>(reader["RequisitoId"]),
                    ProgramaId = ComprobarNulos.CheckNull<string>(reader["ProgramaId"]),
                    EscuelasId = ComprobarNulos.CheckNull<string>(reader["EscuelasId"]),
                    Cc_rolesId = ComprobarNulos.CheckNull<string>(reader["CcRolesId"]),
                    Cc_camposId = ComprobarNulos.CheckNull<string>(reader["CcCampusId"]),
                    Nivel = ComprobarNulos.CheckNull<string>(reader["Nivel"]),
                    FechaCreacion = ComprobarNulos.CheckNull<DateTime>(reader["FechaCreacion"]),
                    Correo = ComprobarNulos.CheckNull<Boolean>(reader["Correo"]),
                    Habilitador = ComprobarNulos.CheckNull<Boolean>(reader["Habilitador"]),
                    UrlImage = ComprobarNulos.CheckNull<string>(reader["UrlImagen"])
                };

                lstAvisos.Add(aviso);
            }
            avisos.lstAvisos = new List<Aviso>();
            avisos.lstAvisos = lstAvisos;
            avisos.Result = true;
            return avisos;
        }
        public async Task<List<CatalogoDto>> GetCatalogo(int opcion)
        {
            List<CatalogoDto> lstCatalogo = new();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Opcion", DbType.Int32, 2, ParameterDirection.Input, false, null, DataRowVersion.Default, opcion)
            };

            using (IDataReader reader = await DataBase.GetReader("spAvisos_ObtenerCatalogo", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    CatalogoDto catalogo = new()
                    {
                        Clave = ComprobarNulos.CheckNull<string>(reader["CLAVE"]),
                        Descripcion = ComprobarNulos.CheckNull<string>(reader["DESCRIPCION"])
                    };

                    lstCatalogo.Add(catalogo);
                }
            }
            return lstCatalogo;
        }
        public async Task<List<CatalogoDto>> GetCatalogoMatricula(FiltrosMatriculaDto filtros)
        {
            List<CatalogoDto> lstCatalogo = new();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Nivel", DbType.String, 2, ParameterDirection.Input, false, null, DataRowVersion.Default, filtros.NivelId),
                DataBase.CreateParameter("@Campus", DbType.String, 3, ParameterDirection.Input, false, null, DataRowVersion.Default, filtros.CampusId),
                DataBase.CreateParameter("@Sedes", DbType.String, 3, ParameterDirection.Input, false, null, DataRowVersion.Default, filtros.SedeId),
                DataBase.CreateParameter("@Escuela", DbType.String, 2, ParameterDirection.Input, false, null, DataRowVersion.Default, filtros.EscuelasId),
                DataBase.CreateParameter("@Programa", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, filtros.ProgramaId),
                DataBase.CreateParameter("@IdUsuario", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, filtros.IdUsuario)
            };

            using (IDataReader reader = await DataBase.GetReader("spAvisos_ObtenerCatalogoMatriculas", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    CatalogoDto catalogo = new()
                    {
                        Clave = ComprobarNulos.CheckNull<string>(reader["CLAVE"]),
                        Descripcion = ComprobarNulos.CheckNull<string>(reader["DESCRIPCION"])
                    };

                    lstCatalogo.Add(catalogo);
                }
            }
            return lstCatalogo;
        }
    }
}