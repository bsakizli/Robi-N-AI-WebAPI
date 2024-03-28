using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System.Text;
using System.Text.Json;
using WhatsAppBusinessAPI.Model;
using WhatsAppBusinessAPI.WhatsAppBusinessModel;

namespace WhatsAppBusinessAPI
{
    public class WhatsAppBusinessClient
    {
        private static string _url = "https://gate.whapi.cloud";
        private static string _token = "DDcE9NCM24lBG4KwrJ6mbxYEy6WNfqO8";


        #region CheckPhones
        public async Task<dynamic> CheckPhones(CheckPhonesRequest checkPhonesRequest)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                client.DefaultRequestHeaders.Add("accept", "application/json");
                var json = JsonConvert.SerializeObject(checkPhonesRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_url}/contacts", data);
                var result = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);
                return jsonResponse;
                #region Exp
                //if (response.StatusCode  == System.Net.HttpStatusCode.OK)
                //{
                //    _CheckPhones = JsonConvert.DeserializeObject<CheckPhones>(response.Content);

                //} else
                //{
                //    _CheckPhones = new CheckPhones
                //    {

                //    };
                //}
                #endregion
            }
            catch (Exception ex)
            {
                return new CheckPhones();
            }
        }
        #endregion

        #region SendTextMessage
        public async Task<dynamic> SendTextMessage(SendTextMessageRequest sendTextMessageRequest)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                client.DefaultRequestHeaders.Add("accept", "application/json");
                var json = JsonConvert.SerializeObject(sendTextMessageRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_url}/messages/text", data);
                var result = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);
                return jsonResponse;
            }
            catch (Exception ex)
            {
                return new SendTextMessage.Root();
            }
        }
        #endregion


        #region SendLocationMessage
        public async Task<dynamic> SendLocationMessage(SendLocationMessageRequest sendLocationMessageRequest)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                client.DefaultRequestHeaders.Add("accept", "application/json");
                var json = JsonConvert.SerializeObject(sendLocationMessageRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_url}/messages/location", data);
                var result = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);
                return jsonResponse;
            }
            catch (Exception ex)
            {
                return new SendLocationMessage.Root();
            }
        }
        #endregion

        #region GetGroupById
        public async Task<bool> checkGroupsById(string groupId)
        {
            try
            {
                var options = new RestClientOptions($"{_url}/api/groups/{groupId}");
                var client = new RestClient(options);
                var request = new RestRequest();
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", $"Bearer {_token}");
                var response = await client.GetAsync(request);
                var _response = JsonConvert.DeserializeObject<dynamic>(response.Content);
                if (String.IsNullOrEmpty(_response.id))
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

    }




}