using HabilitadorGraduaciones.Core.Token;
using Microsoft.Extensions.Configuration;

namespace HabilitadorGraduaciones.Data.Utils
{
    public class ConfiguracionApis
    {
        private readonly IConfiguration _configuration =
           new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        /// <summary>Método que obtiene los EndPoints del Archivo de Configuracion</summary>
        /// <returns>Endpoints</returns>
        public EndpointInfo ObtenerEndpoint()
        {
            EndpointInfo endpointInfo = new EndpointInfo();

            string clientId = Convert.ToString(_configuration["APIConfig:ClientID"]);
            string clientSecret = Convert.ToString(_configuration["APIConfig:Secret"]);

            string server = Convert.ToString(_configuration["Endpoints:Server"]);

            string rutaOAuth = Convert.ToString(_configuration["Endpoints:OAuth"]);
            string rutaJwt = Convert.ToString(_configuration["Endpoints:JWT"]);
            string rutaOperations = Convert.ToString(_configuration["Endpoints:OperationsNew"]);
            string rutaConcentraciones = Convert.ToString(_configuration["Endpoints:Concentraciones"]);

            string cenevalApi = Convert.ToString(_configuration["Endpoints:Ceneval"]);
            string bodyApiCeneval = _configuration["BodyApis:CenevalApi"];

            string semanasTecApi = Convert.ToString(_configuration["Endpoints:SemanasTec"]);
            string bodyApiSemanasTec = _configuration["BodyApis:SemanasTecApi"];

            string distincionesApi = Convert.ToString(_configuration["Endpoints:Distinciones"]);
            string bodyApiDistinciones = _configuration["BodyApis:DistincionesApi"];

            string ApiPlanDeEstudios = Convert.ToString(_configuration["Endpoints:PlanDeEstudios"]);
            string bodyApiPlanDeEstudios = _configuration["BodyApis:PlanDeEstudiosApi"];

            string programaBGBApi = Convert.ToString(_configuration["Endpoints:ProgramaBGB"]);
            string bodyApiBGB = _configuration["BodyApis:ProgramaBGBApi"];

            rutaOperations = rutaOperations.Replace("#Server#", server);
            rutaOAuth = rutaOAuth.Replace("#Server#", server);
            rutaJwt = rutaJwt.Replace("#Server#", server);
            rutaConcentraciones = rutaConcentraciones.Replace("#Server#", server);

            bodyApiCeneval = bodyApiCeneval.Replace("#Metodo#", cenevalApi);
            bodyApiDistinciones = bodyApiDistinciones.Replace("#Metodo#", distincionesApi);
            bodyApiPlanDeEstudios = bodyApiPlanDeEstudios.Replace("#Metodo#", ApiPlanDeEstudios);
            bodyApiBGB = bodyApiBGB.Replace("#Metodo#", programaBGBApi);
            bodyApiSemanasTec = bodyApiSemanasTec.Replace("#Metodo#", semanasTecApi);

            endpointInfo.ClientId = clientId;
            endpointInfo.ClientSecret = clientSecret;
            endpointInfo.RutaOAuth = rutaOAuth;
            endpointInfo.RutaJWT = rutaJwt;
            endpointInfo.RutaOperations = rutaOperations;
            endpointInfo.BodyApiCeneval = bodyApiCeneval;
            endpointInfo.BodyApiSemanasTec = bodyApiSemanasTec;
            endpointInfo.BodyApiDistinciones = bodyApiDistinciones;
            endpointInfo.RutaConcentraciones= rutaConcentraciones;
            endpointInfo.BodyApiPlanDeEstudios = bodyApiPlanDeEstudios;
            endpointInfo.BodyApiBGB = bodyApiBGB;
            endpointInfo.Host = server;

            return endpointInfo;
        }
    }
}
