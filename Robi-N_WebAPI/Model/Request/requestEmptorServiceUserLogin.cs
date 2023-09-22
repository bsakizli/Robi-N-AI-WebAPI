using Newtonsoft.Json;

namespace Robi_N_WebAPI.Model.Request
{
    public class requestEmptorServiceUserLogin
    {
        [JsonProperty(PropertyName = "ProcessCode")]
        public string? ProcessCode { get; set; }
        [JsonProperty(PropertyName = "UserName")]
        public string? UserName { get; set; }
        [JsonProperty(PropertyName = "Password")]
        public string? Password { get; set; }
    }
}
