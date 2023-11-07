using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetGSMService.NetGsmSmsService;

namespace NetGSMService
{
    public class NetGsmSmsWebService
    {
        
        public bool sendSms()
        {
            string[] numbers = { "05071310019" };

            smsGonder1NV3Request smsGonderNNV3Request = new smsGonder1NV3Request();
            smsGonderNNV3Request.username = "5071310019";
            smsGonderNNV3Request.password = "76D1_55";
            smsGonderNNV3Request.filter = 0;
            smsGonderNNV3Request.encoding = "TR";
            smsGonderNNV3Request.header = "TOSPI BILG.";
            smsGonderNNV3Request.msg = "asdasdasdasd";
            smsGonderNNV3Request.gsm = numbers;




            smsnnClient smsnnClient = new smsnnClient();
            var tt = smsnnClient.smsGonder1NV3(
                smsGonderNNV3Request.username,
                smsGonderNNV3Request.password,
                smsGonderNNV3Request.header,
                smsGonderNNV3Request.msg,
                smsGonderNNV3Request.gsm,
                smsGonderNNV3Request.encoding,
                smsGonderNNV3Request.startdate,
                smsGonderNNV3Request.stopdate,
                smsGonderNNV3Request.bayikodu,
                smsGonderNNV3Request.filter,
                smsGonderNNV3Request.appkey
                );

            return true;
        }
    }
}
