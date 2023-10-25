using Nancy.Json;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using Robi_N_WebAPI.Model.Request;
using Robi_N_WebAPI.Model.Response;
using Robi_N_WebAPI.Model.Service.Request;
using Robi_N_WebAPI.Model.Service.Response;
using System.Net;

namespace Robi_N_WebAPI.Services
{
    public class EmptorService
    {
        public responseEmptorServiceUserLogin emptorLoginUserCheck(string username, string password)
        {
            responseEmptorServiceUserLogin response;

            try
            {
                requestEmptorServiceUserLogin _request = new requestEmptorServiceUserLogin
                {
                    UserName = username,
                    Password = password,
                    ProcessCode = "EL-ML-LOGIN"
                };
                //Reqeust End

                var myUri = new Uri(String.Format("https://dynamicapi.bdh.com.tr/DynamicService/ProcessBasicAuth"));
                var myRequest = WebRequest.Create(myUri);
                var request = (HttpWebRequest)myRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Basic cm9iaW4tZWwtbWw6ckBiaW5NbEVs");


                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var _json = JsonConvert.SerializeObject(_request);
                    //string json = new JavaScriptSerializer().Serialize(_request);
                    streamWriter.Write(_json);
                }
                WebResponse _WebResponse = request.GetResponse();
                Stream stream = _WebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream);
                string jsonRaw = streamReader.ReadToEnd();
                response = JsonConvert.DeserializeObject<responseEmptorServiceUserLogin>(jsonRaw);

            } catch(Exception ex)
            {
                response = new responseEmptorServiceUserLogin
                {
                    ResultCode = "500",
                    ResultMessage = "Sistemlerde yaşanan teknik bir problem sebebi ile giriş yapılamıyor. Lütfen teknik birime bilgi veriniz."
                };
            }

            return response;
        }
    }
}
