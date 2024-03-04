using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
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
        public async Task<CheckPhones> CheckPhones(CheckPhonesRequest checkPhonesRequest)
        {
            CheckPhones _CheckPhones;
            try
            {
                var options = new RestClientOptions($"{_url}/contacts");
                var client = new RestClient(options, configureSerialization: s => s.UseNewtonsoftJson());
                var request = new RestRequest();

                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", $"Bearer {_token}");
                //request.AddJsonBody("{\"blocking\":\"wait\",\"force_check\":false,\"contacts\":[\"905074441444\"]}", false);
                request.AddBody(checkPhonesRequest);
                var response = await client.PostAsync(request);
                _CheckPhones = JsonConvert.DeserializeObject<CheckPhones>(response.Content);
                return _CheckPhones;
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
        public async Task<SendTextMessage.Root> SendTextMessage(SendTextMessageRequest sendTextMessageRequest)
        {
            SendTextMessage.Root _SendTextMessage;
            try
            {
                var options = new RestClientOptions($"{_url}/messages/text");
                var client = new RestClient(options);
                var request = new RestRequest();
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", $"Bearer {_token}");
                //request.AddJsonBody("{\"typing_time\":0,\"to\":\"1234567891@s.whatsapp.net\",\"quoted\":\"yqKWpvMRwhI-wGXSunQ0ww\",\"ephemeral\":5,\"edit\":\"yqKWpvMRwhI-wGXSunQ0ww\",\"body\":\"string\",\"no_link_preview\":true,\"mentions\":[\"905071310019\"],\"view_once\":true}", false);
                request.AddBody(sendTextMessageRequest);
                var response = await client.PostAsync(request);
                _SendTextMessage = JsonConvert.DeserializeObject<SendTextMessage.Root>(response.Content);
                return _SendTextMessage;
            }
            catch (Exception ex)
            {
                return new SendTextMessage.Root();
            }
        }
        #endregion


        #region SendLocationMessage
        public async Task<SendLocationMessage.Root> SendLocationMessage(SendLocationMessageRequest sendLocationMessageRequest)
        {
            SendLocationMessage.Root _SendTextMessage;
            try
            {
                var options = new RestClientOptions($"{_url}/messages/location");
                var client = new RestClient(options);
                var request = new RestRequest();
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", $"Bearer {_token}");
                request.AddBody(sendLocationMessageRequest);
                var response = await client.PostAsync(request);
                _SendTextMessage = JsonConvert.DeserializeObject<SendLocationMessage.Root>(response.Content);
                return _SendTextMessage;
            }
            catch (Exception ex)
            {
                return new SendLocationMessage.Root();
            }
        }
        #endregion


    }




}