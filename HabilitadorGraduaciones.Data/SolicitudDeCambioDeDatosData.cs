using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using HabilitadorGraduaciones.Data.Utils.Enums;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace HabilitadorGraduaciones.Data
{
    public class SolicitudDeCambioDeDatosData : ISolicitudDeCambioDeDatosRepository
    {
        private readonly string _connectionString;
        private readonly IEmailModuleRepository _emailData;
        private readonly IConfiguration _configuration;

        public SolicitudDeCambioDeDatosData(IConfiguration configuration, IEmailModuleRepository emailData)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _emailData = emailData;
            _configuration = configuration;
        }

        public async Task<List<EstatusSolicitudDatosPersonalesEntity>> GetEstatusSolicitudes()
        {
            var listEntity = new List<EstatusSolicitudDatosPersonalesEntity>();

            using (IDataReader reader = await DataBase.GetReader("spEstatusSolicitudDatosPersonales_ObtenerEstatus", CommandType.StoredProcedure, _connectionString))
            {
                while (reader.Read())
                {
                    var entity = new EstatusSolicitudDatosPersonalesEntity();

                    entity.IdEstatusSolicitud = ComprobarNulos.CheckIntNull(reader["IdEstatusSolicitud"]);
                    entity.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);

                    listEntity.Add(entity);
                }
            }
            return listEntity;
        }

        public async Task<List<SolicitudDeCambioDeDatosEntity>> GetPendientes(int idUsuario)
        {
            var listEntity = new List<SolicitudDeCambioDeDatosEntity>();

            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pIdUsuario", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idUsuario)
            };
            using (IDataReader reader = await DataBase.GetReader("spSolicitudesDatosPersonales_ObtenerPendientes", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var entity = new SolicitudDeCambioDeDatosEntity();

                    entity.IdSolicitud = ComprobarNulos.CheckIntNull(reader["IdSolicitud"]);
                    entity.NumeroSolicitud = ComprobarNulos.CheckIntNull(reader["NumeroSolicitud"]);
                    entity.Matricula = ComprobarNulos.CheckStringNull(reader["Matricula"]);
                    entity.PeriodoGraduacion = ComprobarNulos.CheckStringNull(reader["PeriodoGraduacion"]);
                    entity.IdDatosPersonales = ComprobarNulos.CheckIntNull(reader["IdDatosPersonales"]);
                    entity.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);
                    entity.FechaSolicitud = ComprobarNulos.CheckDateTimeNull(reader["FechaSolicitud"]);
                    entity.UltimaActualizacion = ComprobarNulos.CheckDateTimeNull(reader["UltimaActualizacion"]);
                    entity.IdEstatusSolicitud = ComprobarNulos.CheckIntNull(reader["IdEstatusSolicitud"]);
                    entity.Estatus = ComprobarNulos.CheckStringNull(reader["Estatus"]);

                    listEntity.Add(entity);
                }
            }
            return listEntity;
        }

        public async Task<List<SolicitudDeCambioDeDatosEntity>> Get(int idEstatusSolicitud, int idUsuario)
        {
            var listEntity = new List<SolicitudDeCambioDeDatosEntity>();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pIdEstatusSolicitud", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idEstatusSolicitud),
                DataBase.CreateParameter("@pIdUsuario", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idUsuario)
            };
            using (IDataReader reader = await DataBase.GetReader("spSolicitudesDatosPersonales_Obtener", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var entity = new SolicitudDeCambioDeDatosEntity();

                    entity.IdSolicitud = ComprobarNulos.CheckIntNull(reader["IdSolicitud"]);
                    entity.NumeroSolicitud = ComprobarNulos.CheckIntNull(reader["NumeroSolicitud"]);
                    entity.Matricula = ComprobarNulos.CheckStringNull(reader["Matricula"]);
                    entity.PeriodoGraduacion = ComprobarNulos.CheckStringNull(reader["PeriodoGraduacion"]);
                    entity.IdDatosPersonales = ComprobarNulos.CheckIntNull(reader["IdDatosPersonales"]);
                    entity.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);
                    entity.FechaSolicitud = ComprobarNulos.CheckDateTimeNull(reader["FechaSolicitud"]);
                    entity.UltimaActualizacion = ComprobarNulos.CheckDateTimeNull(reader["UltimaActualizacion"]);
                    entity.IdEstatusSolicitud = ComprobarNulos.CheckIntNull(reader["IdEstatusSolicitud"]);
                    entity.Estatus = ComprobarNulos.CheckStringNull(reader["Estatus"]);

                    listEntity.Add(entity);
                }
            }

            return listEntity;
        }

        public async Task<List<DetalleSolicitudDeCambioDeDatosEntity>> GetDetalle(int idSolicitud)
        {
            var listEntity = new List<DetalleSolicitudDeCambioDeDatosEntity>();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pIdSolicitud", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idSolicitud)
            };

            using (IDataReader reader = await DataBase.GetReader("spDetalleSolicitudDatosPersonales_ObtenerDatos", CommandType.StoredProcedure, list, _connectionString))
            {
                int row = 0;
                while (reader.Read())
                {
                    var entity = new DetalleSolicitudDeCambioDeDatosEntity();

                    entity.IdSolicitud = ComprobarNulos.CheckIntNull(reader["IdSolicitud"]);
                    entity.IdDetalleSolicitud = ComprobarNulos.CheckIntNull(reader["IdDetalleSolicitud"]);
                    entity.IdDatosPersonales = ComprobarNulos.CheckIntNull(reader["IdDatosPersonales"]);
                    entity.FechaSolicitud = ComprobarNulos.CheckDateTimeNull(reader["FechaSolicitud"]);
                    if (row == 0)
                    {
                        entity.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);
                        entity.DatoIncorrecto = ComprobarNulos.CheckStringNull(reader["DatoIncorrecto"]);
                        entity.DatoCorrecto = ComprobarNulos.CheckStringNull(reader["DatoCorrecto"]);
                    }
                    entity.IdEstatusSolicitud = ComprobarNulos.CheckIntNull(reader["IdEstatusSolicitud"]);
                    entity.Estatus = ComprobarNulos.CheckStringNull(reader["Estatus"]);
                    entity.Documento = ComprobarNulos.CheckStringNull(reader["Documento"]);
                    entity.Extension = ComprobarNulos.CheckStringNull(reader["Extension"]);
                    entity.AzureStorage = ComprobarNulos.CheckStringNull(reader["AzureStorage"]);

                    listEntity.Add(entity);
                    row++;
                }
            }
            return listEntity;
        }

        public async Task<BaseOutDto> GuardaSolicitudes(List<SolicitudDeCambioDeDatosDto> solicitudes)
        {
            BaseOutDto insert = new BaseOutDto();
            int idCorreo = 0;
            try
            {
                if (solicitudes != null)
                {
                    foreach (var solicitud in solicitudes)
                    {
                        solicitud.IdCorreo = idCorreo;
                        IList<Parameter> list = new List<Parameter>
                        {
                            DataBase.CreateParameter("@OK", DbType.Boolean, 1, ParameterDirection.Output, false, null, DataRowVersion.Default, solicitud.OK ),
                            DataBase.CreateParameter("@Error", DbType.AnsiString, -1, ParameterDirection.Output, false, null, DataRowVersion.Default, solicitud.Error ),
                            DataBase.CreateParameter("@pIdSolicitud", DbType.Int32, 10, ParameterDirection.Output, false, null, DataRowVersion.Default, solicitud.IdSolicitud),
                            DataBase.CreateParameter("@pIdCorreo", DbType.Int32, 10, ParameterDirection.InputOutput, false, null, DataRowVersion.Default, solicitud.IdCorreo),
                            DataBase.CreateParameter("@pNumeroSolicitud", DbType.Int32, 10, ParameterDirection.Output, false, null, DataRowVersion.Default, solicitud.NumeroSolicitud),
                            DataBase.CreateParameter("@pMatricula", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitud.Matricula),
                            DataBase.CreateParameter("@pPeriodoGraduacion", DbType.AnsiString, 6, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitud.PeriodoGraduacion),
                            DataBase.CreateParameter("@pIdDatosPersonales", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitud.IdDatosPersonales),
                            DataBase.CreateParameter("@pDatoIncorrecto", DbType.AnsiString, 250, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitud.DatoIncorrecto),
                            DataBase.CreateParameter("@pDatoCorrecto", DbType.AnsiString, 250, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitud.DatoCorrecto)
                        };

                        SqlCommand paramsOut = await DataBase.InsertOut("spSolicitudesDatosPersonales_Insertar", CommandType.StoredProcedure, list, _connectionString);
                        solicitud.OK = Convert.ToBoolean(paramsOut.Parameters["@OK"].Value);
                        solicitud.Error = Convert.ToString(paramsOut.Parameters["@Error"].Value);
                        solicitud.IdSolicitud = Convert.ToInt32(paramsOut.Parameters["@pIdSolicitud"].Value);
                        solicitud.NumeroSolicitud = Convert.ToInt32(paramsOut.Parameters["@pNumeroSolicitud"].Value);
                        idCorreo = Convert.ToInt32(paramsOut.Parameters["@pIdCorreo"].Value);

                        if (solicitud.OK && solicitud.Detalle != null)
                            await GuardaArchivosSolicitud(solicitud.Detalle, solicitud.NumeroSolicitud);

                        insert.Result = solicitud.Result;
                    }

                    var correoDto = await GetCorreo(idCorreo);
                    string cuerpo = BodySolicitudHabilitador(solicitudes, correoDto.Nombre).ToString();
                    if (correoDto != null)
                        await _emailData.EnviarCorreo(correoDto.Destinatario, "Cambios solicitados en el habilitador", cuerpo, "", "", (int)TipoCorreo.SolicitudCambiosHab);
                    
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
                throw new CustomException("Error al guardar las Solicitudes", ex);
            }
            return insert;
        }

        public async Task GuardaArchivosSolicitud(List<DetalleSolicitudDeCambioDeDatosDto> archivos, int numeroSolicitud)
        {
            foreach (var archivo in archivos)
            {
                IList<Parameter> list = new List<Parameter>
                {
                    DataBase.CreateParameter("@OK", DbType.Boolean, 1, ParameterDirection.Output, false, null, DataRowVersion.Default, archivo.OK ),
                    DataBase.CreateParameter("@Error", DbType.AnsiString, -1, ParameterDirection.Output, false, null, DataRowVersion.Default, archivo.Error ),
                    DataBase.CreateParameter("@pIdDetalleSolicitud", DbType.Int32, 10, ParameterDirection.Output, false, null, DataRowVersion.Default, archivo.IdDetalleSolicitud),
                    DataBase.CreateParameter("@pIdSolicitud", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, numeroSolicitud),
                    DataBase.CreateParameter("@pDocumento", DbType.AnsiString, -1, ParameterDirection.Input, false, null, DataRowVersion.Default, archivo.Documento),
                    DataBase.CreateParameter("@pExtension", DbType.AnsiString, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, archivo.Extension),
                    DataBase.CreateParameter("@pAzureStorage", DbType.AnsiString, -1, ParameterDirection.Input, false, null, DataRowVersion.Default, archivo.AzureStorage)
                };

                SqlCommand paramsOut = await DataBase.InsertOut("spDetalleSolicitudDatosPersonales_Insertar", CommandType.StoredProcedure, list, _connectionString);
                archivo.OK = Convert.ToBoolean(paramsOut.Parameters["@OK"].Value);
                archivo.Error = Convert.ToString(paramsOut.Parameters["@Error"].Value);
                archivo.IdDetalleSolicitud = Convert.ToInt32(paramsOut.Parameters["@pIdDetalleSolicitud"].Value);
            }
        }

        public async Task<BaseOutDto> ModificaSolicitud(ModificarEstatusSolicitudDto solicitud)
        {
            BaseOutDto update = new BaseOutDto();
            try
            {
                if (solicitud != null)
                {
                    IList<Parameter> list = new List<Parameter>
                    {
                        DataBase.CreateParameter("@OK", DbType.Boolean, 1, ParameterDirection.Output, false, null, DataRowVersion.Default, solicitud.OK ),
                        DataBase.CreateParameter("@Error", DbType.AnsiString, -1, ParameterDirection.Output, false, null, DataRowVersion.Default, solicitud.Error ),
                        DataBase.CreateParameter("@pIdCorreo", DbType.Int32, 10, ParameterDirection.Output, false, null, DataRowVersion.Default, solicitud.IdCorreo),
                        DataBase.CreateParameter("@pIdSolicitud", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitud.IdSolicitud),
                        DataBase.CreateParameter("@pMatricula", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitud.Matricula),
                        DataBase.CreateParameter("@pIdEstatusSolicitud", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitud.IdEstatusSolicitud),
                        DataBase.CreateParameter("@pUsarioRegistro", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitud.UsarioRegistro),
                        DataBase.CreateParameter("@pComentarios", DbType.AnsiString, 250, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitud.Comentarios)
                    };

                    SqlCommand paramsOut = await DataBase.UpdateOut("spSolicitudCambioDatosPersonales_ActualizarEstatus", CommandType.StoredProcedure, list, _connectionString);
                    solicitud.OK = Convert.ToBoolean(paramsOut.Parameters["@OK"].Value);
                    solicitud.Error = Convert.ToString(paramsOut.Parameters["@Error"].Value);
                    solicitud.IdCorreo = Convert.ToInt32(paramsOut.Parameters["@pIdCorreo"].Value);

                    update.Result = solicitud.OK;

                    if (update.Result)
                    {
                       var correoDto = await GetCorreo(solicitud.IdCorreo);
                        var detalleSolicitud = await GetDetalle(solicitud.NumeroSolicitud);
                        string cuerpo = BodyEstatusSolicitud(detalleSolicitud, correoDto.Nombre, correoDto.Comentarios).ToString();
                        if (correoDto != null)
                            await _emailData.EnviarCorreo(correoDto.Destinatario, "Validación de datos por el colaborador", cuerpo, "", "", (int)TipoCorreo.SolicitudCambiosHab);
                    }
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
                throw new CustomException("Error al modificar el estatus de la solicitud", ex);
            }
            return update;
        }

        public async Task<TotalSolicitudesDto> GetConteoPendientes(int idUsuario)
        {
            TotalSolicitudesDto dto = new TotalSolicitudesDto();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pTotalPendientes", DbType.Int32, 10, ParameterDirection.Output, false, null, DataRowVersion.Default, dto.Total),
                DataBase.CreateParameter("@pIdUsuario", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idUsuario)
            };
            SqlCommand paramsOut = await DataBase.ExecuteOut("spSolicitudCambioDatosPersonales_ObtenerPendientes", CommandType.StoredProcedure, list, _connectionString);
            dto.Total = Convert.ToInt32(paramsOut.Parameters["@pTotalPendientes"].Value);

            return dto;
        }

        public async Task<CorreoSolicitudDatosPersonalesDto> GetCorreo(int idCorreo)
        {
            var dto = new CorreoSolicitudDatosPersonalesDto();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pIdCorreo", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idCorreo),
            };
            using (IDataReader reader = await DataBase.GetReader("spCorreoSolicitudDatosPersonales_ObtenerCorreoSolicitudDatosPersonales", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    dto.IdCorreo = ComprobarNulos.CheckIntNull(reader["IdCorreo"]);
                    dto.IdSolicitud = ComprobarNulos.CheckIntNull(reader["IdSolicitud"]);
                    dto.Destinatario = ComprobarNulos.CheckStringNull(reader["Destinatario"]);
                    dto.Comentarios = ComprobarNulos.CheckStringNull(reader["Comentarios"]);
                    dto.Enviado = ComprobarNulos.CheckBooleanNull(reader["Enviado"]);
                    dto.Nombre = ComprobarNulos.CheckStringNull(reader["Nombre"]);
                }
            }

            return dto;
        }

        public StringBuilder BodySolicitudHabilitador(List<SolicitudDeCambioDeDatosDto> solicitudes, string nombre)
        {
            StringBuilder textoCorreo;
            try
            {
                StringBuilder template = HtmlTemplate.GetTemplateCambiosSolicitadosHabilitador();
                template.Replace("HomePage", _configuration["ConfiguracionLinks:Home"]);
                template.Replace("#Usuario#", nombre);

                StringBuilder actual = new StringBuilder();
                StringBuilder cambio = new StringBuilder();
                foreach (var solicutud in solicitudes)
                {
                    switch (solicutud.IdDatosPersonales)
                    {
                        case 1:
                            actual.Append(@"
                            <tr>
                                <td width='100%' style='padding-bottom: 0;'>
                                    <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>Nombre:</p>
                                    <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>" + solicutud.DatoIncorrecto + @"</p>
                                </td>
                            </tr>");
                            cambio.Append(@"
                            <tr>
                                <td width='100%' style='padding-bottom: 0;'>
                                    <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>Nombre:</p>
                                    <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>" + solicutud.DatoCorrecto + @"</p>
                                </td>
                            </tr>");
                            break;
                        case 2:
                            actual.Append(@"
                            <tr>
                                <td width='100%' style='padding-bottom: 0;'>
                                    <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>CURP:</p>
                                    <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>" + solicutud.DatoIncorrecto + @"</p>
                                </td>
                            </tr>");
                            cambio.Append(@"
                            <tr>
                                <td width='100%' style='padding-bottom: 0;'>
                                    <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>CURP:</p>
                                    <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>" + solicutud.DatoCorrecto + @"</p>
                                </td>
                            </tr>");
                            break;
                        case 3:
                            actual.Append(@"
                            <tr>
                                <td width='100%' style='padding-bottom: 0;'>
                                    <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>Programa Académico:</p>
                                    <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>" + solicutud.DatoIncorrecto + @"</p>
                                </td>
                            </tr>");
                            cambio.Append(@"
                            <tr>
                                <td width='100%' style='padding-bottom: 0;'>
                                    <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>Programa Académico:</p>
                                    <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>" + solicutud.DatoCorrecto + @"</p>
                                </td>
                            </tr>");
                            break;
                        case 4:
                            actual.Append(@"
                            <tr>
                                <td width='100%' style='padding-bottom: 0;'>
                                    <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>Concentración:</p>
                                    <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>" + solicutud.DatoIncorrecto + @"</p>
                                </td>
                            </tr>");
                            cambio.Append(@"
                            <tr>
                                <td width='100%' style='padding-bottom: 0;'>
                                    <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>Concentración:</p>
                                    <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>" + solicutud.DatoCorrecto + @"</p>
                                </td>
                            </tr>");
                            break;
                        case 5:
                            actual.Append(@"
                            <tr>
                                <td width='100%' style='padding-bottom: 0;'>
                                    <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>Diploma:</p>
                                    <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>" + solicutud.DatoIncorrecto + @"</p>
                                </td>
                            </tr>");
                            cambio.Append(@"
                            <tr>
                                <td width='100%' style='padding-bottom: 0;'>
                                    <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>Diploma:</p>
                                    <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>" + solicutud.DatoCorrecto + @"</p>
                                </td>
                            </tr>");
                            break;
                        default:
                            break;
                    }
                }

                textoCorreo = template.Replace("#Actual#", actual.ToString());
                textoCorreo = template.Replace("#Cambio#", cambio.ToString());

                return textoCorreo;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método BodySolicitudHabilitador", ex);
            }
        }

        public StringBuilder BodyEstatusSolicitud(List<DetalleSolicitudDeCambioDeDatosEntity> solicitudes, string nombre, string comentarios)
        {
            StringBuilder template;
            try
            {
                var solicitud = solicitudes.FirstOrDefault();
                if (solicitud == null)
                    return null;

                switch (solicitud.IdEstatusSolicitud)
                {
                    case 2:
                        template = HtmlTemplate.GetTemplateColaboradorRevision();
                        break;
                    case 3:
                        template = HtmlTemplate.GetTemplateColaboradorAprobado();
                        break;
                    case 4:
                        template = HtmlTemplate.GetTemplateColaboradorRechazada();
                        break;
                    default:
                        return new StringBuilder();
                }

                template.Replace("HomePage", _configuration["ConfiguracionLinks:Home"]);
                template.Replace("#Usuario#", nombre);


                template.Replace("#Comentario#", comentarios);
                template.Replace("#Tipo#", solicitud.Descripcion);
                template.Replace("#Actual#", solicitud.DatoIncorrecto);
                template.Replace("#Cambio#", solicitud.DatoCorrecto);
                return template;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método BodyEstatusSolicitud", ex);
            }
            
        }
    }
}
