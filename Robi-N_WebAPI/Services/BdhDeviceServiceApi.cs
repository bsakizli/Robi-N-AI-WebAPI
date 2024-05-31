using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;

namespace Robi_N_WebAPI.Services
{
    public class BdhDeviceServiceApi
    {
        public string _serviceUrl = "https://api.bdh.com.tr/Device/GetCaseStatusForIVR?CaseId={0}";
        public string _bearerToken = "dm-2gr2zbuHrh1a3HG6fn8sJ7eeFC1lxAF2FOO_LLQ1ZN9FeXpVE2zPmGibrL5uFTj9sgmPs6V8rkRyruIUBRcQZYFQvGyLrEHiQAxZT2JhZqIlPjJQYTT8wLbmxIaRrFypblbM-U84bJapDcayz6ZufHMYtnGgs0wT9xcSOufepHKvrt9hT4v_AAI6Daxd1ou224wFnpK9cx4fxt3LJsiz_yYb7ou3hbmo1t_289j2p2M1UjCofKE4cIM7QAmjjprJrG1f9GcSAozpgT6UJPQ";
        public string _token = "5102eecbaafd4f138041c9de34199336";
        public string _username = "Ivr";
        public string _password = "RoUKK*+l!1jJ#";


        public async Task<dynamic> SamsungDeviceService(string service_number)
        {
           
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "dm-2gr2zbuHrh1a3HG6fn8sJ7eeFC1lxAF2FOO_LLQ1ZN9FeXpVE2zPmGibrL5uFTj9sgmPs6V8rkRyruIUBRcQZYFQvGyLrEHiQAxZT2JhZqIlPjJQYTT8wLbmxIaRrFypblbM-U84bJapDcayz6ZufHMYtnGgs0wT9xcSOufepHKvrt9hT4v_AAI6Daxd1ou224wFnpK9cx4fxt3LJsiz_yYb7ou3hbmo1t_289j2p2M1UjCofKE4cIM7QAmjjprJrG1f9GcSAozpgT6UJPQ");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            client.DefaultRequestHeaders.Add("Operation", "9");
            client.DefaultRequestHeaders.Add("Token", _token);
            client.DefaultRequestHeaders.Add("UserName", _username);
            client.DefaultRequestHeaders.Add("Password", _password);
            var response = await client.GetAsync(string.Format(_serviceUrl, service_number));
            //var result = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);
            return jsonResponse;
        }

        public async Task<dynamic> OtherTTDeviceService(string service_number)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            client.DefaultRequestHeaders.Add("Operation", "11");
            client.DefaultRequestHeaders.Add("Token", _token);
            client.DefaultRequestHeaders.Add("UserName", _username);
            client.DefaultRequestHeaders.Add("Password", _password);
            var response = await client.GetAsync(string.Format(_serviceUrl, service_number));
            //var result = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);
            return jsonResponse;
        }
    }
}
