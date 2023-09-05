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
    public class ProgramaBgbData : IProgramaBgbRepository
    {
        private readonly ConfiguracionApis _configuracionApis = new ConfiguracionApis();
        public async Task<ProgramaBgbDto> ProgramaBGBApi(EndpointsDto dto, Sesion sesion)
        {
            ProgramaBgbDto programaBGB = new();
            programaBGB.Result = false;
            try
            {
                if (string.IsNullOrEmpty(dto.NumeroMatricula) || !dto.ClaveProgramaAcademico.Contains("BGB"))
                {
                    return programaBGB;
                }

                EndpointInfo endpointInfo = _configuracionApis.ObtenerEndpoint();

                string bodyApi = endpointInfo.BodyApiBGB;
                endpointInfo.BodyApiBGB = bodyApi.Replace("#Matricula#", dto.NumeroMatricula).
                    Replace("#Programa#", dto.ClaveProgramaAcademico).
                    Replace("#Campus#", dto.ClaveCampus).
                    Replace("#Nivel#", dto.ClaveNivelAcademico);

                var apiBGB = new RestClient();

                RestRequest request = new RestRequest(endpointInfo.RutaOperations, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + sesion.OAuthToken);
                request.AddHeader("X-Auth-JWT", sesion.JwtToken);
                request.AddHeader("Accept", "application/json");
                request.AddStringBody(endpointInfo.BodyApiBGB, ContentType.Json);

                RestResponse respuestaApi = await apiBGB.ExecuteAsync(request);

                if (Convert.ToString(respuestaApi.StatusCode) == "OK")
                {
                    JsonDocument apiResponse = JsonDocument.Parse(respuestaApi.Content);
                    foreach (JsonElement element in apiResponse.RootElement.EnumerateArray())
                    {
                        var result = element.GetProperty("result").ToString();
                        JsonDocument parsedObject = JsonDocument.Parse(result);
                        programaBGB.CumpleRequisitoInglesCuarto = ComprobarNulos.CheckBooleanNull(
                            parsedObject.RootElement.GetProperty("cumpleRequisitoInglesCuartoSemestre").ToString());
                        programaBGB.CumpleRequisitoInglesQuinto = ComprobarNulos.CheckBooleanNull(
                           parsedObject.RootElement.GetProperty("cumpleRequisitoInglesQuintoSemestre").ToString());
                        programaBGB.CumpleRequisitoInglesOctavo = ComprobarNulos.CheckBooleanNull(
                           parsedObject.RootElement.GetProperty("cumpleRequisitoInglesOctavoSemestre").ToString());
                        programaBGB.CumpleProgramaInternacional = ComprobarNulos.CheckBooleanNull(
                           parsedObject.RootElement.GetProperty("cumpleProgramaInternacional").ToString());
                        programaBGB.CumpleRequisitosProgramaEspecial = ComprobarNulos.CheckBooleanNull(
                           parsedObject.RootElement.GetProperty("cumpleRequisitosProgramaEspecial").ToString());
                        programaBGB.CreditosDeInglesAprobadosCuartoSemestre = ComprobarNulos.CheckIntNull(
                           parsedObject.RootElement.GetProperty("creditosDeInglesAprobadosCuartoSemestre").ToString());
                        programaBGB.CreditosDeInglesAprobadosQuintoSemestre = ComprobarNulos.CheckIntNull(
                          parsedObject.RootElement.GetProperty("creditosDeInglesAprobadosQuintoSemestre").ToString());
                        programaBGB.CreditosDeInglesAprobadosOctavoSemestre = ComprobarNulos.CheckIntNull(
                          parsedObject.RootElement.GetProperty("creditosDeInglesAprobadosOctavoSemestre").ToString());
                        programaBGB.CreditosAprobadosProgramaInternacional = ComprobarNulos.CheckIntNull(
                          parsedObject.RootElement.GetProperty("creditosAprobadosProgramaInternacional").ToString());
                        programaBGB.UltimaActualizacion = DateTime.UtcNow;
                        programaBGB.Result = true;
                    }
                }
                else
                {
                    throw new CustomException(respuestaApi.Content.ToString(), respuestaApi.StatusCode);
                }

                return programaBGB;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método ProgramaBGBApi", ex);
            }

            
        }
    }
}
