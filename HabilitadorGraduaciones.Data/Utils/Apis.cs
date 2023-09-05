using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Serializers;
using System.Text;
using System.Text.Json;

namespace HabilitadorGraduaciones.Data.Utils
{
    public class Apis
    {

        public IConfiguration _configuration { get; }

        public Apis(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetURlParams(List<Param> urlParams)
        {
            StringBuilder queryParams;
            queryParams = new StringBuilder();
            queryParams.Append("?");
            bool first = true;
            foreach (Param param in urlParams)
            {
                if (!first)
                {
                    queryParams.Append("&");
                }
                first = false;
                queryParams.Append(param.Name + "=" + param.Value);
            }
            return queryParams.ToString();
        }
        public async Task<JsonDocument> GetApi(string url, List<Param> headers, List<Param> body)
        {
            return await GetApi(url, headers, null, body, string.Empty);
        }
        public async Task<JsonDocument> GetApi(string url, List<Param> headers)
        {
            return await GetApi(url, headers, null, null, string.Empty);
        }
        public async Task<JsonDocument> GetApiGet(string url, List<Param> headers, List<Param> parameter, List<Param> body, string bodyRowData)
        {
            var client = new RestClient();
            var request = new RestRequest(url, Method.Get);

            if (headers != null) fillHeader(request, headers);

            if (parameter != null) fillParameters(request, parameter);

            if (body != null) fillBody(request, body);

            if (bodyRowData != string.Empty) fillBodyRow(request, bodyRowData);

            RestResponse response = await client.ExecuteAsync(request);
            var parsedObject = JsonDocument.Parse(response.Content);
            return parsedObject;

        }
        public async Task<JsonDocument> GetApi(string url, List<Param> headers, List<Param> parameter, List<Param> body, string bodyRowData)
        {
            var client = new RestClient();
            var request = new RestRequest(url, Method.Post);

            if (headers != null) fillHeader(request, headers);

            if (parameter != null) fillParameters(request, parameter);

            if (body != null) fillBody(request, body);

            if (bodyRowData != string.Empty) fillBodyRow(request, bodyRowData);

            RestResponse response = await client.ExecuteAsync(request);
            var parsedObject = JsonDocument.Parse(response.Content);
            return parsedObject;

        }
        private void fillBodyRow(RestRequest request, string bodyRowData)
        {
            List<string> optionList = new List<string>();
            optionList.Add(bodyRowData);
            request.AddStringBody(bodyRowData, ContentType.Json);
        }
        private void fillBody(RestRequest request, List<Param> bodys)
        {
            StringBuilder bld = new StringBuilder();
            string paramsBody = string.Empty;
            string paramsBodyClear = string.Empty;
            foreach (var body in bodys)
            {
                bld.Append("&" + body.Name + "=" + body.Value);
            }
            paramsBody = bld.ToString();
            paramsBodyClear = paramsBody.Remove(0, 1);

            request.AddParameter("application/x-www-form-urlencoded", paramsBodyClear, ParameterType.RequestBody);

        }
        private void fillParameters(RestRequest request, List<Param> parameters)
        {
            foreach (var param in parameters)
            {
                request.AddParameter(param.Name, param.Value);
            }
        }
        private void fillHeader(RestRequest request, List<Param> headers)
        {
            foreach (var header in headers)
            {
                request.AddHeader(header.Name, header.Value);
            }
        }
        public class Param
        {
            public Param(string Name, string Value)
            {
                this.Name = Name;
                this.Value = Value;
            }
            public string Name { get; set; }
            public string Value { get; set; }
        }
        public async Task<string> getToken()
        {
            string urlSemanasTecGetToken = _configuration["Endpoints:Server"] + _configuration["Endpoints:OAuth"];
            string clientId = _configuration["APIConfig:ClientID"];
            string clientSecret = _configuration["APIConfig:Secret"];

            List<Param> bodys = new List<Param>();
            bodys.Add(new Param("client_id", clientId));
            bodys.Add(new Param("client_secret", clientSecret));
            bodys.Add(new Param("grant_type", _configuration["APIConfig:GrantType"]));
            bodys.Add(new Param("scope", _configuration["APIConfig:Scope"]));

            List<Param> headers = new List<Param>();

            headers.Add(new Param("Content-Type", "application/x-www-form-urlencoded"));
            JsonDocument apiAnswer = await this.GetApi(urlSemanasTecGetToken, headers, bodys);

            string token = apiAnswer.RootElement.GetProperty("access_token").ToString();
            return token;
        }
        public async Task<string> getJwtToken(string token)
        {
            string urlSemanasTecJwtToken = _configuration["Endpoints:Server"] + _configuration["Endpoints:JWT"];
            string jwtToken = string.Empty;

            List<Param> headers = new List<Param>();
            headers.Add(new Param("grant_type", _configuration["APIConfig:GrantType"]));
            headers.Add(new Param("scope", _configuration["APIConfig:Scope"]));
            headers.Add(new Param("Accept", "application/vnd.api+json"));
            headers.Add(new Param("aud-claim", "aud"));
            headers.Add(new Param("Authorization", "Bearer " + token));
            headers.Add(new Param("iss-claim", "A00344770"));
            headers.Add(new Param("sub-claim", "sub"));

            JsonDocument apiAnswer = await this.GetApi(urlSemanasTecJwtToken, headers);
            var meta = apiAnswer.RootElement.GetProperty("meta").ToString();
            JsonDocument parsedObject = JsonDocument.Parse(meta);
            jwtToken = parsedObject.RootElement.GetProperty("token").ToString();
            return jwtToken;
        }
    }
}