using AutoMapper;
using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.Token;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using HabilitadorGraduaciones.Services.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace HabilitadorGraduaciones.Services
{
    public class ApiService : IApiService
    {
        private readonly IMapper _mapper;
        private readonly ISesionRepository _sessionData;
        private readonly ConfiguracionApis _configuracionApis = new ConfiguracionApis();

        public ApiService(IMapper mapper, ISesionRepository sessionData)
        {
            _mapper = mapper;
            _sessionData = sessionData;
        }

        /// <summary>Verifica la Creacion de OAuth y JWT Tokens<summary>
        /// <param name="matricula"></param>
        public async Task<Sesion> VerificaTokenUsuario(string matricula)
        {
            Sesion data = await _sessionData.GetSesion(matricula);
            if (data != null)
            {
                if (data.JwtToken == null)
                {
                    ApiToken apiToken = await GeneraTokens(matricula);
                    data = _mapper.Map<Sesion>(apiToken);
                    await _sessionData.GuardaSesion(data);
                }
                else if (data.FechaExpiracion <= DateTime.Now)
                {
                    ApiToken apiToken = await GeneraTokens(matricula);
                    data = _mapper.Map<Sesion>(apiToken);
                    await _sessionData.ModificaSesion(data);
                }
                return data;
            }
            else
            {
                throw new CustomException("Error en el Método GetSesion()", System.Net.HttpStatusCode.NoContent);
            }

        }
        /// <summary>Método que genera el OAuth y JWT Tokens </summary>
        /// <param name="matricula"></param>
        /// <returns>regresa OAuth y JWT GeneraTokens</returns>
        public async Task<ApiToken> GeneraTokens(string matricula)
        {
            ApiToken apiToken = new ApiToken();
            apiToken.Matricula = matricula;

            EndpointInfo endpointInfo = _configuracionApis.ObtenerEndpoint();

            if (string.IsNullOrEmpty(endpointInfo.RutaOAuth) || string.IsNullOrEmpty(endpointInfo.RutaJWT))
            {
                apiToken.Mensaje = "Endpoints no configurados";
                throw new CustomException("Endpoints no configurados", System.Net.HttpStatusCode.NoContent);
            }
            else
            {
                try
                {
                    RestClient oAuthCliente = new RestClient(endpointInfo.RutaOAuth);

                    RestRequest requestOAuth = new RestRequest(endpointInfo.RutaOAuth, Method.Post);
                    requestOAuth.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    requestOAuth.AddParameter("client_id", endpointInfo.ClientId);
                    requestOAuth.AddParameter("client_secret", endpointInfo.ClientSecret);
                    requestOAuth.AddParameter("grant_type", "client_credentials");
                    requestOAuth.AddParameter("scope", "default");

                    RestResponse responseOAuth = await oAuthCliente.ExecuteAsync(requestOAuth);

                    if (Convert.ToString(responseOAuth.StatusCode) == "OK")
                    {
                        var responseOAuthJson = JsonConvert.DeserializeObject<OAuthToken>(responseOAuth.Content);

                        RestClient jwtCliente = new RestClient(endpointInfo.RutaJWT);

                        RestRequest requestJwt = new RestRequest(endpointInfo.RutaJWT, Method.Post);
                        requestJwt.AddHeader("Accept", "application/vnd.api+json");
                        requestJwt.AddHeader("aud-claim", "aud");
                        requestJwt.AddHeader("Authorization", "Bearer " + responseOAuthJson.AccessToken);
                        requestJwt.AddHeader("iss-claim", matricula);
                        requestJwt.AddHeader("sub-claim", "sub");
                        RestResponse responseJwt = await jwtCliente.ExecuteAsync(requestJwt);

                        if (Convert.ToString(responseJwt.StatusCode) == "OK")
                        {
                            var responseJwtJson = JsonConvert.DeserializeObject<JwtToken>(responseJwt.Content);

                            apiToken.OAuthToken = responseOAuthJson.AccessToken;
                            apiToken.JwtToken = responseJwtJson.Meta.Token;
                            apiToken.FechaCreacion = DateTime.Now;
                            apiToken.FechaExpiracion = apiToken.FechaCreacion.AddSeconds(responseOAuthJson.ExpiresIn - 300);

                        }
                        else
                        {
                            throw new CustomException("Error al generar el JWT TOKEN", responseJwt.StatusCode);
                        }
                    }
                    else
                    {
                        throw new CustomException("Error al generar el OAuth TOKEN", responseOAuth.StatusCode);

                    }
                    return apiToken;
                }
                catch (Exception ex)
                {
                    throw new CustomException("Error en el Método GeneraTokens", ex);
                }

            }
        }
    }
}
