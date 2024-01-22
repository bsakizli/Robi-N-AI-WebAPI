using Newtonsoft.Json;
using System.Net;
using Robi_N_WebAPI.Model.Response;
using Nancy.Json;
using Robi_N_WebAPI.Model;
using DocumentFormat.OpenXml.Bibliography;
using System.Data;
using System.Windows.Forms;

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


        public static DataSet ToDataSet<T>(this IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                t.Columns.Add(propInfo.Name, propInfo.PropertyType);
            }

            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                DataRow row = t.NewRow();
                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null);
                }
            }

            return ds;
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



        public static string tcToMask(string textvalue, bool check)
        {
            if (check)
            {
                var first = textvalue.Substring(0, 2);
                var last = textvalue.Substring(textvalue.Length - 2, 2);
                var mask = new string('*', textvalue.Length - first.Length - last.Length);
                var masked = string.Concat(first, mask, last);
                return masked.ToString();
            }
            else
            {
                return textvalue;
            }

        }



        public async static Task<string> PdfPasswordGenerator()
        {
            // Şifre oluşturulacak tarih
            DateTime tarih = DateTime.Now;

            // Tarih bilgilerini al
            int yil = tarih.Year;
            int ay = tarih.Month;
            int gun = tarih.Day;

            // Tarihi şifreye dönüştür
            string sifre = GeneratePassword(yil, ay, gun);
            // Oluşturulan şifreyi ekrana yazdır
            return sifre;


        }

        static string GeneratePassword(int year, int month, int day)
        {
            // Yıl, ay ve gün bilgilerini kullanarak şifre oluştur
            string sifre = $"{year % 254:00}{month:00}{day:00}";

            return sifre;
        }






    }
}
