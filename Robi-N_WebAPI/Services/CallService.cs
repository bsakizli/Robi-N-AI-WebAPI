using Nancy.Json;
using Newtonsoft.Json;
using Robi_N_WebAPI.Model.Request;
using Robi_N_WebAPI.Model.Service.Request;
using Robi_N_WebAPI.Model.Service.Response;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using static Robi_N_WebAPI.Model.Service.Request.ServiceRequestAddAutoCall;

namespace Robi_N_WebAPI.Services;

public class CallService
{
    public static string _apiServerUrl = "https://api.netgsm.com.tr/autocallservice";
    public static string _username = "8503088638";
    public static string _password = "76D1_55";

    public ServiceResponseAddAutoCall.Root AutoCallServiceSinglePhone(RequestRegistrationInformationCallSingleNumber req)
    {
        ServiceResponseAddAutoCall.Root response = new ServiceResponseAddAutoCall.Root();
        
        try
        {
            Random random = new Random();
            //Request Start


            List<Request.Number> numbers = new List<Request.Number>();
            Request.Number number = new Request.Number();
            number.name = "Teknisyen";
            number.number = req.phone;
            numbers.Add(number);

            List<string> groups = new List<string>();

            string _date = DateTime.Now.ToString("yyyy-MM-dd");

            ServiceRequestAddAutoCall.Request.Root requestData = new ServiceRequestAddAutoCall.Request.Root
            {
               header = new ServiceRequestAddAutoCall.Request.Header
               {
                   username = _username,
                   password = _password,
               },
               body = new ServiceRequestAddAutoCall.Request.Body
               {
                   @event = "addautocall",
                   data = new ServiceRequestAddAutoCall.Request.Data
                   {
                       list_name = String.Format("AutoCallRegister#{0}",random.Next()),
                       list_prefix = "",
                       liste_type = 2,
                       list_startdate = _date,
                       list_stopdate = _date,
                       list_starttime = "00:00",
                       list_stoptime = "23:59",
                       retry_count = 0,
                       try_time = 0,
                       queue_limit_type = 3,
                       department = "Operator",
                       trunk = "8503088638",
                       callstop_type = 0,
                       destination_type = "ivr",
                       destination_name = "Bilgilendirme",
                       queue_limit = 5,
                       groups = groups,
                       numbers = numbers
                   }
               }
            };
            //Reqeust End



            var myUri = new Uri(String.Format(_apiServerUrl));
            var myRequest = WebRequest.Create(myUri);
            var request = (HttpWebRequest)myRequest;
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(requestData);
                streamWriter.Write(json);
            }
            WebResponse _WebResponse = request.GetResponse();
            Stream stream = _WebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            string jsonRaw = streamReader.ReadToEnd();
            response = JsonConvert.DeserializeObject<ServiceResponseAddAutoCall.Root>(jsonRaw);
            return response;

        }
        catch (Exception ex) {

            response.header.error = true;
            response.header.code = 500;
            response.header.message = ex.Message;
            return response;
           
        }
    }
   
}
