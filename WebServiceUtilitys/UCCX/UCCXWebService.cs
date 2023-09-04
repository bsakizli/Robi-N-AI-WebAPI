using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace WebServiceUtilitys.UCCX
{
    public class UCCXWebService
    {
        public void getCSQList()
        {
            var myUri = new Uri(String.Format("http://10.254.51.145/adminapi/csq"));
            var myRequest = WebRequest.Create(myUri);
            var request = (HttpWebRequest)myRequest;
            var username = "crsadmin";
            var password = "netas2013ccx";
            string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                           .GetBytes(username + ":" + password));

            request.Headers.Add("Authorization", "Basic " + encoded);
            request.Method = "GET";
            request.ContentType = "text/xml";

            //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            //{
            //    string json = new JavaScriptSerializer().Serialize(requestData);
            //    streamWriter.Write(json);
            //}


            WebResponse _WebResponse = request.GetResponse();
            Stream stream = _WebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            string jsonRaw = streamReader.ReadToEnd();

            StringReader stringReader = new StringReader(jsonRaw);


            XmlSerializer serializer = new XmlSerializer(typeof(responseCSQList.Csqs), new XmlRootAttribute("csqs"));
            var tt = (responseCSQList.Csqs)serializer.Deserialize(stringReader);
        }
    }
}
