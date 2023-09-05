using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
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
    public class SemanasTecData : ISemanasTecRepository
    {
        private readonly string _connectionString;
        private readonly ConfiguracionApis _configuracionApis = new ConfiguracionApis();

        public SemanasTecData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<SemanasTecDto> GetSemanasTec(EndpointsDto dtosemTec, Sesion sesion)
        {
            dtosemTec.Result = false;
            SemanasTecDto semanasTec = new();
            semanasTec.Result = false;
            await GetProgramaNivel(dtosemTec);

            try
            {

                if (string.IsNullOrEmpty(dtosemTec.NumeroMatricula))
                {
                    return semanasTec;
                }
                EndpointInfo endpointInfo = _configuracionApis.ObtenerEndpoint();

                string bodyApi = endpointInfo.BodyApiSemanasTec;
                endpointInfo.BodyApiSemanasTec = bodyApi.Replace("#Matricula#", dtosemTec.NumeroMatricula).
                    Replace("#Programa#", dtosemTec.ClaveProgramaAcademico).
                    Replace("#Nivel#", dtosemTec.ClaveNivelAcademico);


                var apiSemanasTec = new RestClient();

                RestRequest request = new RestRequest(endpointInfo.RutaOperations, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + sesion.OAuthToken);
                request.AddHeader("X-Auth-JWT", sesion.JwtToken);
                request.AddHeader("Accept", "application/json");
                request.AddStringBody(endpointInfo.BodyApiSemanasTec, ContentType.Json);

                RestResponse respuestaApi = await apiSemanasTec.ExecuteAsync(request);

                if (Convert.ToString(respuestaApi.StatusCode) == "OK")
                {
                    JsonDocument apiResponse = JsonDocument.Parse(respuestaApi.Content);
                    int semanasMaximas18 = 0;
                    int semanasObtenidas18 = 0;
                    foreach (JsonElement element in apiResponse.RootElement.EnumerateArray())
                    {
                        var result = element.GetProperty("result").ToString();
                        JsonDocument parsedObject = JsonDocument.Parse(result);

                        semanasMaximas18 = ComprobarNulos.CheckIntNull(parsedObject.RootElement.GetProperty("totalSemanas18").ToString());
                        semanasObtenidas18 = ComprobarNulos.CheckIntNull(parsedObject.RootElement.GetProperty("totalSemanas18Acreditadas").ToString());
                        semanasTec.SemanasMaximas = ComprobarNulos.CheckIntNull(parsedObject.RootElement.GetProperty("totalSemanasTec").ToString()) + semanasMaximas18;
                        semanasTec.SemanasObtenidas = ComprobarNulos.CheckIntNull(parsedObject.RootElement.GetProperty("totalSemanasTecAcreditadas").ToString()) + semanasObtenidas18;
                        semanasTec.UltimaActualizacion = ComprobarNulos.CheckDateTimeNull(parsedObject.RootElement.GetProperty("fechaAuditoria").ToString());
                        semanasTec.Result = true;
                    }
                }
                else
                {
                    throw new CustomException(respuestaApi.Content.ToString(), respuestaApi.StatusCode);
                }
                return semanasTec;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método GetSemanasTec()", ex);
            }
        }
        public async Task<SemanasTecDto> GetProgramaNivel(EndpointsDto dtosemTec)

        {
            try
            {
                SemanasTecDto result = new SemanasTecDto();

                IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Matricula", DbType.String, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, dtosemTec.NumeroMatricula)
            };

                using IDataReader reader = await DataBase.GetReader("spSemanasTec_ObtenerClaveYNivel", CommandType.StoredProcedure, list, _connectionString);
                while (reader.Read())
                {
                    dtosemTec.ClaveProgramaAcademico = ComprobarNulos.CheckNull<string>(reader["programa"]);
                    dtosemTec.ClaveNivelAcademico = ComprobarNulos.CheckNull<string>(reader["nivel"]);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método GetProgramaNivel()", ex);
            }
        }

    }
}