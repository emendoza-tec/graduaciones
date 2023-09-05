using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Core.Token;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Serializers;
using System.Data;
using System.Text.Json;

namespace HabilitadorGraduaciones.Data
{
    public class ExamenConocimientosData : IExamenConocimientosRepository
    {
        private readonly string _connectionString;

        public ExamenConocimientosData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        private readonly ConfiguracionApis _configuracionApis = new ConfiguracionApis();

        public async Task<TipoExamenPorCarreraDto> GetTipoExamenPorCarrera(string claveCarrera)
        {
            var dto = new TipoExamenPorCarreraDto();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pClaveCarrera", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, claveCarrera)
            };

            using (IDataReader reader = await DataBase.GetReader("spCarrerasConRequisitoExamen_ObtenerPorClaveCarrera", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    dto.IdTipoExamen = ComprobarNulos.CheckIntNull(reader["IdTipoExamen"]);
                    dto.ClaveCarrera = ComprobarNulos.CheckStringNull(reader["ClaveCarrera"]);
                    dto.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);
                    dto.Titulo = ComprobarNulos.CheckStringNull(reader["Titulo"]);
                }
            }
            return dto;
        }

        public async Task<ExamenConocimientosDto> GetCeneval(EndpointsDto dto, Sesion sesion)
        {
            ExamenConocimientosDto ceneval = new();
            ceneval.Result = false;
            try
            {

                if (string.IsNullOrEmpty(dto.NumeroMatricula))
                {
                    return ceneval;
                }

                EndpointInfo endpointInfo = _configuracionApis.ObtenerEndpoint();

                string bodyApi = endpointInfo.BodyApiCeneval;
                endpointInfo.BodyApiCeneval = bodyApi.Replace("#Matricula#", dto.NumeroMatricula).
                    Replace("#Programa#", dto.ClaveProgramaAcademico).
                    Replace("#Carrera#", dto.ClaveCarrera);

                var apiCeneval = new RestClient();

                RestRequest request = new RestRequest(endpointInfo.RutaOperations, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + sesion.OAuthToken);
                request.AddHeader("X-Auth-JWT", sesion.JwtToken);
                request.AddHeader("Accept", "application/json");
                request.AddStringBody(endpointInfo.BodyApiCeneval, ContentType.Json);

                RestResponse respuestaApi = await apiCeneval.ExecuteAsync(request);

                if (Convert.ToString(respuestaApi.StatusCode) == "OK")
                {
                    JsonDocument apiResponse = JsonDocument.Parse(respuestaApi.Content);
                    foreach (JsonElement element in apiResponse.RootElement.EnumerateArray())
                    {
                        var result = element.GetProperty("result").ToString();
                        JsonDocument parsedObject = JsonDocument.Parse(result);
                        ceneval.CumpleRequisito = ComprobarNulos.CheckBooleanNull(
                            parsedObject.RootElement.GetProperty("cumpleRequisitoCeneval").ToString());
                        ceneval.EsRequisito = ceneval.CumpleRequisito;
                        if (ceneval.CumpleRequisito)
                        {
                            ceneval.FechaExamen = ComprobarNulos.CheckDateTimeNull(
                                parsedObject.RootElement.GetProperty("fechaExamen").ToString());
                            if (ceneval.FechaExamen >= DateTime.Now)
                            {
                                ceneval.CumpleRequisito = false;
                            }
                        }
                        else
                        {
                            ceneval.EsRequisito = true;
                            ceneval.CumpleRequisito = false;
                        }

                        ceneval.FechaRegistro = DateTime.Now;
                        ceneval.Result = true;
                    }
                }
                else
                {
                    throw new CustomException(respuestaApi.Content.ToString(), respuestaApi.StatusCode);
                }

                return ceneval;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método GetCeneval", ex);
            }
        }

        public async Task<ExamenConocimientosDto> GetExamenConocimientoPorTipo(string matricula, int tipoExamen)
        {
            var dto = new ExamenConocimientosDto();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pMatricula", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, matricula),
                DataBase.CreateParameter("@pIdTipoExamen", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, tipoExamen)
            };

            using (IDataReader reader = await DataBase.GetReader("spExamenConocimientoPorTipo", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    dto.Estatus = ComprobarNulos.CheckStringNull(reader["ESTATUS"]);
                    dto.FechaExamen = ComprobarNulos.CheckDateTimeNull(reader["FECHA_EXAMEN"]);
                    dto.FechaRegistro = ComprobarNulos.CheckDateTimeNull(reader["FECHA_REGISTRO"]);
                    dto.EsRequisito = ComprobarNulos.CheckBooleanNull(reader["APLICA"]);
                    dto.Result = true;
                }
            }
            return dto;
        }

        public async Task<TipoExamenConocimientosEntity> GetExamenConocimientoPorLenguaje(int tipoExamen, string lenguaje)
        {
            var dto = new TipoExamenConocimientosEntity();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pIdTipoExamen", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, tipoExamen),
                DataBase.CreateParameter("@pIdioma", DbType.String, 3, ParameterDirection.Input, false, null, DataRowVersion.Default, lenguaje)
            };

            using (IDataReader reader = await DataBase.GetReader("spTipoExamen_ObtenerMultilenguaje", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    dto.IdTipoExamen = ComprobarNulos.CheckIntNull(reader["IdTipoExamen"]);
                    dto.Descripcion = ComprobarNulos.CheckStringNull(reader["Descripcion"]);
                    dto.Titulo = ComprobarNulos.CheckStringNull(reader["Titulo"]);
                    dto.Nota = ComprobarNulos.CheckStringNull(reader["Nota"]);
                    dto.Link = ComprobarNulos.CheckStringNull(reader["Link"]);
                    dto.Result = true;
                }
            }
            return dto;
        }
    }
}
