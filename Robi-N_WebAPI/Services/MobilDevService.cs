using Nancy;
using Nancy.Json;
using NetGsmAPI.Models.Response;
using Newtonsoft.Json;
using Robi_N_WebAPI.Model.Request;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Robi_N_WebAPI.Services
{
    public class MobilDevService
    {
        private string _username = "1749577979";
        private string _password = "670698db1a2549c8abdf55ac1c09e8c3";
        private string _originator = "BDH BILISIM";
        public async Task<responseSendSms> sendSms(string gsmNumber, string MessageText)
        {
            responseSendSms _response;
            try
            {
                requestMobildevSmsSend.Root _request = new requestMobildevSmsSend.Root
                {
                    UserName = _username,
                    PassWord = _password,
                    AccountId = "-1",
                    Originator = _originator,
                    Action = 0,
                    Numbers = new List<string>
                {
                    gsmNumber.Replace(" ","")
                },
                    Mesgbody = MessageText,
                    Encoding = 0,
                    MessageType = "N"
                };

                var json = JsonConvert.SerializeObject(_request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync("https://xmlapi.mobildev.com/", data);
                var result = await response.Content.ReadAsStringAsync();

                Regex rgx = new Regex("ID: \\d+");
                _response = new responseSendSms
                {
                    status = rgx.IsMatch(result),
                    bulkid = 1
                };


            } catch
            {
                _response = new responseSendSms
                {
                    status = false
                };

            }
            return _response;
        }
    }
}
