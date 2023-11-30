using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobinCore._3rdService.dub.Models.request;
using RobinCore._3rdService.dub.Models.response;
using System.Net.Http.Headers;

namespace RobinCore._3rdService.dub
{
    public class dubService
    {
        private static string projectSlug = "bdh";
        private static string apiUrl = "api.dub.co";
        private static string _https = "https";
        private static string _domain = "bdh.ist";

        public async Task<customSetLinkResponse> setLink(requestSetLink.Root _request)
        {
            customSetLinkResponse _response;
            try
            {
                string _url = String.Empty;
                _request.domain = _domain;
                var json = JsonConvert.SerializeObject(_request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YoaMi04J3CWvM6TMRJvHIFeI");
                var response = await client.PostAsync($"https://{apiUrl}/links?projectSlug={projectSlug}", data);

                if(response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (!String.IsNullOrEmpty(result))
                    {
                        responseSetLink.Root serviceResponse = JsonConvert.DeserializeObject<responseSetLink.Root>(result);
                        if (serviceResponse != null)
                        {
                             _url = String.Format($"https://{serviceResponse.domain}/{serviceResponse.key}");
                                _response = new customSetLinkResponse
                                {
                                    status = true,
                                    url = _url
                                };
                        } else
                        {
                            _response = new customSetLinkResponse
                            {
                                status = false
                            };
                        }
                    } else
                    {
                        _response = new customSetLinkResponse
                        {
                            status = false
                        };
                    }
                } else
                {
                    _response = new customSetLinkResponse
                    {
                        status = false
                    };
                }
                return _response;
            } catch
            {
                _response = new customSetLinkResponse
                {
                    status = false
                };
                return _response;
            }
         
        }
    }
}
