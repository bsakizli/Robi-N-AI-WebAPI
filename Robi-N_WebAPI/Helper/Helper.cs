using Newtonsoft.Json;
using System.Net;
using Robi_N_WebAPI.Model.Response;
using Nancy.Json;
using Robi_N_WebAPI.Model;
using DocumentFormat.OpenXml.Bibliography;

namespace Robi_N_WebAPI.Helper
{
    public static class Helper
    {
        public static  async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }


        public static async Task<responseVoiceIVRApplication.GoogleCalender.Root>? getGoogleTurkeyHolidays()
        {


            Uri myUri = new Uri("https://www.googleapis.com/calendar/v3/calendars/turkish__tr%40holiday.calendar.google.com/events?key=AIzaSyBsYRMzm0k_h7l1frdKi7kXpr-af11_nPQ");
            var myWebRequest = WebRequest.Create(myUri);
            var request = (HttpWebRequest)myWebRequest;

            //WebProxy myProxy = new WebProxy();
            //Uri newUri = new Uri(ProxyUrl);
            //myProxy.Address = newUri;
            //myProxy.Credentials = new NetworkCredential(ProxyUsername, ProxyPassword);
            //request.Proxy = myProxy;

            request.Method = "GET";

            WebResponse response = await request.GetResponseAsync();
            Stream result = response.GetResponseStream();
            StreamReader reader = new StreamReader(result);
            string cikti = reader.ReadToEnd();

           responseVoiceIVRApplication.GoogleCalender.Root? _response = JsonConvert.DeserializeObject<responseVoiceIVRApplication.GoogleCalender.Root>(cikti);
            return _response;
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}



        



	}
}
