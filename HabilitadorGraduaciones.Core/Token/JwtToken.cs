using Newtonsoft.Json;

namespace HabilitadorGraduaciones.Core.Token
{
    public class JwtToken
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
        [JsonProperty("jsonapi")]
        public Jsonapi Jsonapi { get; set; }
        [JsonProperty("links")]
        public Links Links { get; set; }
    }

    public class Jsonapi
    {
        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public class Links
    {
        [JsonProperty("self")]
        public string Self { get; set; }
    }

    public class Meta
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
