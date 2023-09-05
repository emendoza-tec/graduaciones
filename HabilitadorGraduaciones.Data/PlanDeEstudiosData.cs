using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Token;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using RestSharp;
using RestSharp.Serializers;
using System.Text.Json;

namespace HabilitadorGraduaciones.Data
{
    public class PlanDeEstudiosData : IPlanDeEstudiosRepository
    {
        private readonly ConfiguracionApis configuracionApis = new ConfiguracionApis();

        #region Método para consumir la API 
        public async Task<PlanDeEstudiosDto> ConsultarApiPlanDeEstudios(EndpointsDto dtoPE, Sesion sesion)
        {
            PlanDeEstudiosDto _PlanDeEstudios = new PlanDeEstudiosDto();
            _PlanDeEstudios.Result = false;
            try
            {
                if (string.IsNullOrEmpty(dtoPE.NumeroMatricula))
                {
                    return _PlanDeEstudios;
                }
                EndpointInfo endpointInfo = configuracionApis.ObtenerEndpoint();

                string bodyApi = endpointInfo.BodyApiPlanDeEstudios;
                endpointInfo.BodyApiPlanDeEstudios = bodyApi.Replace("#Matricula#", dtoPE.NumeroMatricula).
                      Replace("#Ejercicio#", dtoPE.ClaveEjercicioAcademico).
                    Replace("#Nivel#", dtoPE.ClaveNivelAcademico);

                var apiPlanDeEstudios = new RestClient();

                string token = sesion.OAuthToken;
                string jwtToken = sesion.JwtToken;

                RestRequest restRequestPE = new RestRequest(endpointInfo.RutaOperations, Method.Post);
                restRequestPE.AddHeader("Content-Type", "application/json");
                restRequestPE.AddHeader("Authorization", "Bearer " + token);
                restRequestPE.AddHeader("X-Auth-JWT", jwtToken);
                restRequestPE.AddHeader("Accept", "application/json");
                restRequestPE.AddStringBody(endpointInfo.BodyApiPlanDeEstudios, ContentType.Json);

                RestResponse restApiPE = await apiPlanDeEstudios.ExecuteAsync(restRequestPE);

                if (Convert.ToString(restApiPE.StatusCode) == "OK")
                {
                    JsonDocument apiResponsePE = JsonDocument.Parse(restApiPE.Content);

                    foreach (JsonElement element in apiResponsePE.RootElement.EnumerateArray())
                    {
                        var result = element.GetProperty("result").ToString();
                        JsonDocument parsedObject = JsonDocument.Parse(result);
                        var list = new List<CreditosPorCampus>();
                        var json = JsonDocument.Parse(parsedObject.RootElement.GetProperty("creditos-por-campus").ToString());

                        foreach (JsonElement item in json.RootElement.EnumerateArray())
                        {
                            list.Add(new CreditosPorCampus
                            {
                                ClaveCampus = item.GetProperty("claveCampus").ToString(),
                                CreditosCampus = ComprobarNulos.CheckJsonPropertyDecimalNull(item, "creditosCampus")
                            });
                        }
                        _PlanDeEstudios.CreditosRequisito = ComprobarNulos.CheckJsonPropertyDecimalNull(parsedObject.RootElement, "creditosPlanEstudios");
                        _PlanDeEstudios.CreditosInscritos = ComprobarNulos.CheckJsonPropertyDecimalNull(parsedObject.RootElement, "creditosInscritos");
                        _PlanDeEstudios.CreditosFaltantes = ComprobarNulos.CheckJsonPropertyDecimalNull(parsedObject.RootElement, "creditosFaltantes");
                        _PlanDeEstudios.UltimaActualizacionPE = ComprobarNulos.CheckDateTimeNull(parsedObject.RootElement.GetProperty("fechaAuditoria").ToString());
                        _PlanDeEstudios.CreditosCursadosExtranjero = ComprobarNulos.CheckJsonPropertyDecimalNull(parsedObject.RootElement, "creditosCursadosExtranjero");

                        _PlanDeEstudios.CreditosPorCampus = list;
                        _PlanDeEstudios.TotalDeCampus = list.Count;
                    }
                    _PlanDeEstudios.Result = true;
                }
                else
                {
                    throw new CustomException(restApiPE.Content.ToString(), restApiPE.StatusCode);                   
                }
                return _PlanDeEstudios;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método ConsultarApiPlanDeEstudios", ex);
            }
        }
        #endregion
    }
}