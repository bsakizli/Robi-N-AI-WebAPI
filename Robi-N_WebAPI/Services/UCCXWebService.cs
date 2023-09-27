using Robi_N_WebAPI.Model.Xml.Response;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace Robi_N_WebAPI.Services
{
    public class UCCXWebService
    {

        private readonly IConfiguration _configuration;


        public UCCXWebService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<responseCSQList.Csqs> getCSQList()
        {
            responseCSQList.Csqs response = new responseCSQList.Csqs();

            try
            {

                var _host = _configuration.GetValue<string>("UCCX:host");
                var _username = _configuration.GetValue<string>("UCCX:username");
                var _password = _configuration.GetValue<string>("UCCX:password");

                var myUri = new Uri(String.Format("http://{0}/adminapi/csq", _host));
                var myRequest = WebRequest.Create(myUri);
                var request = (HttpWebRequest)myRequest;

                string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                               .GetBytes(_username + ":" + _password));

                request.Headers.Add("Authorization", "Basic " + encoded);
                request.Method = "GET";
                request.ContentType = "text/xml";

                //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                //{
                //    string json = new JavaScriptSerializer().Serialize(requestData);
                //    streamWriter.Write(json);
                //}

                WebResponse _WebResponse = await request.GetResponseAsync();
                Stream stream = _WebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream);
                string jsonRaw = streamReader.ReadToEnd();

                StringReader stringReader = new StringReader(jsonRaw);


                XmlSerializer serializer = new XmlSerializer(typeof(responseCSQList.Csqs), new XmlRootAttribute("csqs"));
                return (responseCSQList.Csqs)serializer.Deserialize(stringReader);

                

            } catch (Exception ex) {
                response = new responseCSQList.Csqs
                {
                    Csq = null
                };
               return response;
            }
            
        }
    }
}
