﻿using Nancy.Diagnostics;
using Nancy.Json;
using Nancy;
using Newtonsoft.Json;
using Robi_N_WebAPI.Model.Service.Response;
using System.Net;
using Robi_N_WebAPI.Model.Request;

namespace Robi_N_WebAPI.Services
{

    //public class SMSService
    //{
    //    //http://www.postaguvercini.com/api_http/sendsms.asp?user=aybarsyalcinotp&password=Qwe123**&gsm=905071310019&text=123123
    //    public static string _url = "www.postaguvercini.com";

    //    public bool SendSMS(long gsm, string text)
    //    {
    //        bool _status = false;
    //        var myUri = new Uri(String.Format("http://{0}/api_http/sendsms.asp?user=aybarsyalcinotp&password=Qwe123**&gsm={1}&text={2}", _url, gsm, text));
    //        var myRequest = WebRequest.Create(myUri);
    //        var request = (HttpWebRequest)myRequest;
    //        request.Method = "GET";
    //        //request.ContentType = "application/json";

    //        //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
    //        //{
    //        //    string json = new JavaScriptSerializer().Serialize(requestData);
    //        //    streamWriter.Write(json);
    //        //}
    //        WebResponse _WebResponse = request.GetResponse();
    //        Stream stream = _WebResponse.GetResponseStream();
    //        StreamReader streamReader = new StreamReader(stream);
    //        string jsonRaw = streamReader.ReadToEnd();
    //        if (jsonRaw.Contains("errno=0"))
    //        {
    //            _status = true;
    //        }
    //        else {
    //            _status = false;
    //        }

    //        return _status;
           
    //    }
    //}





}
