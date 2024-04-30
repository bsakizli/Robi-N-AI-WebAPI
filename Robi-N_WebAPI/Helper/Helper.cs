using Newtonsoft.Json;
using System.Net;
using Robi_N_WebAPI.Model.Response;
using Nancy.Json;
using Robi_N_WebAPI.Model;
using DocumentFormat.OpenXml.Bibliography;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;

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



        public static bool checkPhonesStatic(string phone)
        {
            try
            {
                if (Convert.ToString(phone).Length == 12)
                {
                    return true;                }
                else if (IsValidEmail(Convert.ToString(phone)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        public static bool IsValidEmail(string email)
        {
            // E-posta adresi için geçerli olan Regex deseni
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Regex desenini kullanarak e-posta adresini kontrol et
            return Regex.IsMatch(email, pattern);
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

        public async static Task<string> PdfGenerateCustomPassword(DateTime date)
        {
            // Tarih bilgisini kullanarak özel algoritmayla şifre oluşturun
            string dayPart = (date.Day * 2).ToString();
            string monthPart = (date.Month * 2).ToString();
            string yearPart = (date.Year - 2).ToString();

            string password = dayPart + monthPart + yearPart;

            return password;
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




        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Geçerli bir Türkiye telefon numarası regex deseni
            string pattern = @"^(5\d{9}|05\d{9}|905\d{10}||90\d{10})$";

            Regex regex = new Regex(pattern);
            return regex.IsMatch(phoneNumber);
        }

        public static string FormatPhoneNumber(string phoneNumber)
        {
            // Başına +90 veya 9 ekleyerek formatlama
            if (phoneNumber.StartsWith("5"))
            {
                return "90" + phoneNumber;
            }
            else if (phoneNumber.StartsWith("05"))
            {
                return "90" + phoneNumber.Substring(1);
            }
            else if (phoneNumber.StartsWith("905") || phoneNumber.StartsWith("90"))
            {
                return phoneNumber;
            }
            else
            {
                return "90" + phoneNumber;
            }
        }
    }

}

