using NetGsmAPI.Models.Response;
using NetGsmServiceReference;

namespace NetGsmAPI
{
    public class NetGsmService
    {
        private string _username = "5071310019";
        private string _password = "76D1_55";
        private string _header = "TOSPI BILG.";

        smsnnClient smsnnClient = new smsnnClient();

        public async Task<responseSendSms>  sendSms(long gsmNumber, string message)
        {
            responseSendSms response = new responseSendSms();
           
            try
            {
                string[] numbers = { Convert.ToString(gsmNumber) };
                smsGonder1NV3Request request = new smsGonder1NV3Request
                {
                    username = _username,
                    password = _password,
                    filter = 0,
                    gsm = numbers,
                    appkey = null,
                    bayikodu = null,
                    encoding = "TR",
                    header = _header,
                    msg = message,
                    startdate = null,
                    stopdate = null
                };

                var _response = await smsnnClient.smsGonder1NV3Async(
                    request.username,
                    request.password,
                    request.header,
                    request.msg,
                    request.gsm,
                    request.encoding,
                    request.startdate,
                    request.stopdate,
                    request.bayikodu,
                    request.filter,
                    request.appkey
                    );

                if (_response != null && Convert.ToUInt64(_response.@return) > 0)
                {
                    response = new responseSendSms
                    {
                        status = true,
                        bulkid = Convert.ToInt64(_response.@return)
                    };
                }
                else
                {
                    response = new responseSendSms
                    {
                        status = false
                    };
                }

            } catch
            {
                response = new responseSendSms
                {
                    status = false
                };
            }
            return response;
        }

        public async Task<bool> SmsReport(long bulkid)
        {
            try {

                raporV3Request _request = new raporV3Request
                {
                    username = _username,
                    password = _password,
                    bulkid = Convert.ToString(bulkid)
                };
                var tt = await smsnnClient.raporV3Async(
                    _request.username,
                    _request.password,
                    _request.bulkid,
                    _request.telno,
                    _request.header,
                    _request.startdate,
                    _request.stopdate,
                    _request.type,
                    _request.status,
                    _request.detail
                    );
                return true;
            } catch
            {
                return false;
            }
        }

    }
}