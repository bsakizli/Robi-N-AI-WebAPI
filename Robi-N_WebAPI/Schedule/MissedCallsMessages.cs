using Hangfire;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.EntityFrameworkCore;
using NetGsmAPI;
using Robi_N_WebAPI.Services;
using Robi_N_WebAPI.Utility;
using Robi_N_WebAPI.Utility.Tables;
using System.ServiceModel;
using UCCXSoapService;
using WhatsAppBusinessAPI;
using WhatsAppBusinessAPI.Model;

namespace Robi_N_WebAPI.Schedule
{
    public class MissedCallsMessages
    {

        private readonly IConfiguration _configuration;
        private readonly AIServiceDbContext _db;
   
        public MissedCallsMessages(AIServiceDbContext db)
        {
            _db = db;
        }


        WhatsAppBusinessClient wpClient = new WhatsAppBusinessClient();
        NetGsmService _smsService = new NetGsmService();


        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task MissedCallMessageService()
         {
            uccxSoapClient uCCXWebService = new uccxSoapClient();
            try
            {

                bool MultipleCallSendMessage = false;
                var missedCalsList = await uCCXWebService.MissedCallListServiceAsync();
                if (missedCalsList.Body.MissedCallListServiceResult.count > 0)
                {
                    var _SmsMessage = await _db.RBN_SMS_TEMPLATES.Where(x => x.MessageCode == 101 && x.active == true).FirstOrDefaultAsync();
                    if (_SmsMessage != null && !String.IsNullOrEmpty(_SmsMessage.Message))
                    {
                        var _wpMessage = await _db.RBN_WhatsAppMessageTemplate.Where(x => x.active == true && x.MessageCode == _SmsMessage.MessageCode && x.active == _SmsMessage.whatsappSend).FirstOrDefaultAsync();

                        foreach (var item in missedCalsList.Body.MissedCallListServiceResult.calls)
                        {
                            string formattedNumber = Helper.Helper.FormatPhoneNumber(item.phonenumber);
                            if (true)  //item.phonenumber == "05071310019" || item.phonenumber == "05324107091" || item.phonenumber == "05347002291" || item.phonenumber == "05337420911" || item.phonenumber == "05370376793"
                            {

                                RBN_UnansweredCalls _numberCheck;
                                if(MultipleCallSendMessage)
                                {
                                     _numberCheck = await _db.RBN_UnansweredCalls.Where(x => x.contactid == item.contactid && x.phonenumber == item.phonenumber && x.startdatetime.Value.Date == DateTime.Now.Date).FirstOrDefaultAsync();
                                } else
                                {
                                     _numberCheck = await _db.RBN_UnansweredCalls.Where(x => x.phonenumber == item.phonenumber && x.startdatetime.Value.Date == DateTime.Now.Date).FirstOrDefaultAsync();
                                }
                                
                                if (_numberCheck == null)
                                {
                                    var _record = new RBN_UnansweredCalls()
                                    {
                                        active = true,
                                        agentid = item.agentid,
                                        calltype = item.calltype,
                                        contactid = item.contactid,
                                        csqname = item.csqname,
                                        disposition = item.disposition,
                                        phonenumber = item.phonenumber,
                                        record_date = DateTime.Now,
                                        startdatetime = item.startdatetime,
                                        enddatetime = item.enddatetime,
                                        smsSendStatus = false,
                                        process = 0
                                    };
                                    var lastRecord = _db.RBN_UnansweredCalls.Add(_record);
                                    await _db.SaveChangesAsync();
                                }
                            }
                        }


                        var _sendMessageList = await _db.RBN_UnansweredCalls.Where(x => x.active == true && x.smsSendStatus == false && x.process == 0).ToListAsync();
                        foreach (var item in _sendMessageList)
                        {
                            item.process = 2;
                            await _db.SaveChangesAsync();
                            string formattedNumber = Helper.Helper.FormatPhoneNumber(Convert.ToString(item.phonenumber));
                            string _hours = DateTime.Now.ToLocalTime().ToString("HH:mm");
                            
                            var _sendSmsStatus = await _smsService.sendSms(formattedNumber, _SmsMessage.Message.Replace("{HOURS}", _hours));
                            
                            if (_wpMessage != null && !String.IsNullOrEmpty(_wpMessage.MessageBody))
                            {
                                //Whatssap Mesaj Gönderme
                                SendTextMessageRequest _rq = new SendTextMessageRequest
                                {
                                    body = _wpMessage.MessageBody.Replace("{HOURS}", _hours),
                                    typing_time = 0,
                                    to = formattedNumber
                                };
                                var _wpMessageSendCheck = await wpClient.SendTextMessage(_rq);
                            }

                            if (_sendSmsStatus.status)
                            {
                                item.process = 1;
                                item.smsSendId = _sendSmsStatus.bulkid;
                                item.smsSendStatus = true;
                                item.smsSendDate = DateTime.Now;
                                await _db.SaveChangesAsync();
                            }
                        }

                    }
                }

            }
            catch
            {

            }
        }

    }
}
