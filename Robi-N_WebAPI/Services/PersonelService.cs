using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Nancy.Diagnostics;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Robi_N_WebAPI.Utility;
using System;
using System.Dynamic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace Robi_N_WebAPI.Services
{
    public class PersonelService : ControllerBase
    {


        private string _url = "http://10.254.46.69/BackOfficeApi/Personal/SearchPersonalInfos";
        private string _apiKey = "f53ecc86-4b8d-4b6c-ac96-c4e139e8fd2b";

        public class SearchPersonalInfosRequest
        {
            public string? apiKey { get; set; }
            public string? TcKimlikNo { get; set; }
        }


        public bool CalisanSorgulama(long tcKimlikNo)
        {

            SearchPersonalInfosRequest _reqeust = new SearchPersonalInfosRequest
            {
                apiKey = _apiKey,
                TcKimlikNo = tcKimlikNo.ToString()
            };


            var myRequest = WebRequest.Create(_url);
            var request = (HttpWebRequest)myRequest;
            request.Method = "GET";
            request.ContentType = "application/json";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(_reqeust);
                        streamWriter.Write(json);
                        WebResponse _WebResponse = request.GetResponse();
                        var tt = _WebResponse.GetResponseStream();
                        StreamReader streamReader = new StreamReader(stream);
                        string jsonRaw = streamReader.ReadToEnd();
                        var yy = JsonConvert.DeserializeObject<dynamic>(jsonRaw);
                        return yy.Aktif;
                    }
                }
            }


            //WebResponse _WebResponse = request.GetResponse();
            //Stream stream = _WebResponse.GetResponseStream();
            //StreamReader streamReader = new StreamReader(stream);
            //string jsonRaw = streamReader.ReadToEnd();
            //var response = JsonConvert.DeserializeObject<dynamic>(jsonRaw);
            //return response.Aktif;

            //var client = new HttpClient();
            //client.BaseAddress = new Uri("https://baseApi/");
            //client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));

            //var query = new Dictionary<string, string>
            //{
            //    ["apiKey"] = _apiKey,
            //    ["TcKimlikNo"] = tcKimlikNo.ToString()
            //};

            //var response = await client.GetAsync(QueryHelpers.AddQueryString(_url, query));
            //var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            //dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);
            //return jsonResponse.Aktif;
        }


    }
}
