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
    public class DistincionesData : IDistincionesRepository
    {
        private readonly ConfiguracionApis _configuracionApis = new ConfiguracionApis();
       
        public async Task<DistincionesDto> GetDistinciones(EndpointsDto dto, Sesion sesion)
        {
            try
            {
                DistincionesDto result = new DistincionesDto();
                result.LstConcentracion = new List<string>();
                await GetDiploma(result, dto, sesion);
                await GetConcentraciones(result, dto, sesion);
                await GetMinors(result, dto, sesion);
                if (result.HasUlead)
                {
                    result.Ulead = string.Empty;
                    result.UleadOk = true;
                }
                if (result.HasDiploma)
                {
                    result.Diploma = string.Empty;
                    result.DiplomaOk = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método GetDistinciones", ex);
            }
        }


        public async Task GetDiploma(DistincionesDto distincionesDiploma, EndpointsDto dto, Sesion sesion)
        {
            distincionesDiploma.Result = false;

            try
            {
                if (string.IsNullOrEmpty(dto.NumeroMatricula))
                {
                    return;
                }

                EndpointInfo endpointInfo = _configuracionApis.ObtenerEndpoint();

                string bodyApi = endpointInfo.BodyApiDistinciones;
                endpointInfo.BodyApiDistinciones = bodyApi.Replace("#Matricula#", dto.NumeroMatricula).
                    Replace("#Ejercicio#", dto.ClaveEjercicioAcademico).
                    Replace("#Nivel#", dto.ClaveNivelAcademico);

                var apiDistinciones = new RestClient();

                RestRequest request = new RestRequest(endpointInfo.RutaOperations, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + sesion.OAuthToken);
                request.AddHeader("X-Auth-JWT", sesion.JwtToken);
                request.AddHeader("Accept", "application/json");
                request.AddStringBody(endpointInfo.BodyApiDistinciones, ContentType.Json);

                RestResponse respuestaApi = await apiDistinciones.ExecuteAsync(request);

                if (Convert.ToString(respuestaApi.StatusCode) == "OK")
                {
                    JsonDocument apiResponse = JsonDocument.Parse(respuestaApi.Content);
                    foreach (JsonElement element in apiResponse.RootElement.EnumerateArray())
                    {
                        var result = element.GetProperty("result").ToString();
                        JsonDocument parsedObject = JsonDocument.Parse(result);
                        distincionesDiploma.HasUlead = ComprobarNulos.CheckBooleanNull(parsedObject.RootElement.GetProperty("diplomaULAD").ToString());
                        distincionesDiploma.HasDiploma = ComprobarNulos.CheckBooleanNull(parsedObject.RootElement.GetProperty("diplomaInternacional").ToString());                        
                    }
                    distincionesDiploma.Result = true;
                }
                else
                {
                    distincionesDiploma.ErrorMessage = respuestaApi.Content.ToString();
                }

            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método GetDiploma", ex);
            }

        }


        public async Task GetConcentraciones(DistincionesDto distinciones, EndpointsDto dto, Sesion sesion)
        {
            distinciones.Result = false;

            try
            {
                if (string.IsNullOrEmpty(dto.NumeroMatricula))
                {
                    return;
                }
                EndpointInfo endpointInfo = _configuracionApis.ObtenerEndpoint();

                string rutaConcentraciones = endpointInfo.RutaConcentraciones;
                endpointInfo.RutaConcentraciones = rutaConcentraciones.Replace("#Matricula#", dto.NumeroMatricula).
                    Replace("#Tipo#", "concentraciones");

                var apiConcentraciones = new RestClient();

                RestRequest request = new RestRequest(endpointInfo.RutaConcentraciones, Method.Get);
                request.AddQueryParameter("ejercicioAcademico", dto.ClaveEjercicioAcademico);
                request.AddQueryParameter("claveNivelAcademico", dto.ClaveNivelAcademico);

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + sesion.OAuthToken);
                request.AddHeader("X-Auth-JWT", sesion.JwtToken);
                request.AddHeader("Accept", "application/vnd.api+json");

                RestResponse respuestaApi = await apiConcentraciones.ExecuteAsync(request);

                if (Convert.ToString(respuestaApi.StatusCode) == "OK")
                {
                    JsonDocument apiResponse = JsonDocument.Parse(respuestaApi.Content);
                    var data = apiResponse.RootElement.GetProperty("data");
                    GetListaConcentraciones(data, distinciones);                    
                    distinciones.Result = true;
                }
                else
                {
                    distinciones.ErrorMessage = respuestaApi.Content.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método GetConcentraciones", ex);
            }
        }

        public async Task GetMinors(DistincionesDto distinciones, EndpointsDto dto, Sesion sesion)
        {
            distinciones.Result = false;

            try
            {
                if (string.IsNullOrEmpty(dto.NumeroMatricula))
                {
                    return;
                }
                EndpointInfo endpointInfo = _configuracionApis.ObtenerEndpoint();

                string rutaConcentraciones = endpointInfo.RutaConcentraciones;
                endpointInfo.RutaConcentraciones = rutaConcentraciones.Replace("#Matricula#", dto.NumeroMatricula).
                    Replace("#Tipo#", "modalidades");

                var apiConcentraciones = new RestClient();

                RestRequest request = new RestRequest(endpointInfo.RutaConcentraciones, Method.Get);
                request.AddQueryParameter("ejercicioAcademico", dto.ClaveEjercicioAcademico);
                request.AddQueryParameter("claveNivelAcademico", dto.ClaveNivelAcademico);

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + sesion.OAuthToken);
                request.AddHeader("X-Auth-JWT", sesion.JwtToken);
                request.AddHeader("Accept", "application/vnd.api+json");

                RestResponse respuestaApi = await apiConcentraciones.ExecuteAsync(request);

                if (Convert.ToString(respuestaApi.StatusCode) == "OK")
                {
                    JsonDocument apiResponse = JsonDocument.Parse(respuestaApi.Content);
                    var data = apiResponse.RootElement.GetProperty("data");
                    GetListaMinors(data, distinciones);
                    distinciones.Result = true;
                }
                else
                {
                    distinciones.ErrorMessage = respuestaApi.Content.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método GetMinors", ex);
            }
        }

        public void GetListaConcentraciones(JsonElement data, DistincionesDto distinciones)
        {
            try
            {
                foreach (var param in data.EnumerateArray())
                {
                    if (param.GetProperty("id").ToString() != "CMEL" && param.GetProperty("id").ToString() != "CPXX")
                    {
                        string concentracion = ComprobarNulos.CheckStringNull(
                            param.GetProperty("attributes").GetProperty("descripcionConcentracion").ToString());
                        if (!concentracion.Equals("Conc o Acen no elegida"))
                        {
                            distinciones.LstConcentracion.Add(concentracion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método GetListaConcentraciones()", ex);
            }
        }

        public void GetListaMinors(JsonElement data, DistincionesDto distinciones)
        {
            try
            {
                foreach (var param in data.EnumerateArray())
                {
                    if (param.GetProperty("id").ToString() != "CMEL" && param.GetProperty("id").ToString() != "CPXX")
                    {
                        string concentracion = ComprobarNulos.CheckStringNull(
                            param.GetProperty("attributes").GetProperty("descripcionModalidades").ToString());
                        if (!concentracion.Equals("Modalidad no elegida"))
                        {
                            distinciones.LstConcentracion.Add(concentracion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método GetListaMinors()", ex);
            }
        }
    }
}