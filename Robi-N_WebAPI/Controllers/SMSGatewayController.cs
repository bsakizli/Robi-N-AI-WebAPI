using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Office.Interop.Excel;
using Nancy.Json;
using NetGsmAPI;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Model.Request;
using Robi_N_WebAPI.Model.Response;
using Robi_N_WebAPI.Services;
using Robi_N_WebAPI.Utility;
using SimpleCrypto;

using System.Text.RegularExpressions;
using System.Xml.Serialization;
using static Robi_N_WebAPI.Model.Response.responseSMSGateway;


namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,IVR Read Only Web Service")]
    [ApiController]
    public class SMSGatewayController : ControllerBase
    {


        NetGsmService _smsService = new NetGsmService();
        MobilDevService _mobilDevService = new MobilDevService();
        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;
        private readonly IConfiguration _configuration;

      

        PBKDF2 crypto = new PBKDF2();

        public SMSGatewayController(IConfiguration configuration, ILogger<IdentityCheckController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _configuration = configuration;
            _logger = logger;
            _db = db;
          
        }


        [HttpPost("sendSMSById")]
        public async Task<IActionResult> sendSMSById([FromBody] requestSendSmsById _request)
        {
            responsesendSMSById response;
            try
            {
                string _log = String.Format(@"GSM:{0} - MessageId: {1} ", _request.gsmNumber, _request.messageId);
                var textResult = new JavaScriptSerializer().Serialize(_log);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));


                if (!String.IsNullOrEmpty(_request.gsmNumber) && _request!= null && _request.messageId > 0)
                {
                    var _getMessage = _db.RBN_SMS_TEMPLATES.FirstOrDefault(x => x.Id == _request.messageId);

                    if (_getMessage != null && !String.IsNullOrEmpty(_getMessage.Message))
                    {
                        Regex rgx = new Regex("#([A-Za-z0-9]+)#");
                        if (rgx.Matches(_getMessage.Message).Count == 0)
                        {
                            var _sendSMSResponse = await _mobilDevService.sendSms(_request.gsmNumber, _getMessage.Message);

                            if (_sendSMSResponse.status)
                            {
                                response = new responsesendSMSById
                                {
                                    status = true,
                                    statusCode = 200,
                                    message = "Message sent successfully.",
                                    data = _getMessage,
                                    displayMessage = "Mesaj başarıyla gönderilmiştir.",
                                    bulkid = _sendSMSResponse.bulkid
                                };
                                textResult = new JavaScriptSerializer().Serialize(response);
                                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));

                                return Ok(response);
                            }
                            else
                            {
                                response = new responsesendSMSById
                                {
                                    status = true,
                                    statusCode = 202,
                                    displayMessage = "Mesaj gönderilemedi",
                                    message = "The message could not be sent due to an ISP problem. Please provide information.",
                                    data = _getMessage
                                };
                              
                            }
                        }
                        else
                        {
                            response = new responsesendSMSById
                            {
                                status = false,
                                statusCode = 203,
                                message = "Unsuccessfully",
                                displayMessage = "SMS içeriğinde değişken tanımı var, bu method üzerinden sms gönderilemez."

                            };
                        }

                    }
                    else
                    {
                        response = new responsesendSMSById
                        {
                            status = false,
                            statusCode = 201,
                            message = "The message could not be sent because there is no template defined for the Id number entered."
                        };
                    }
                } else
                {
                    response = new responsesendSMSById
                    {
                        status = false,
                        statusCode = 201,
                        displayMessage = "Gönderilen parametre bilgisi hatalı lütfen kontrol edin.",
                        message = "Unsuccessful"
                    };
                }

                textResult = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));

                return BadRequest(response);
                //Mesaj tanımı yok

            }
            catch
            {
                response = new responsesendSMSById
                {
                    status = false,
                    statusCode = 500,
                    message = "System Error"
                };

                var textResult = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));

                return BadRequest(response);

            }


        }


        
        [HttpPost("sendSmsParameterById")]
        public async Task<IActionResult> sendSmsParameterById([FromBody] requestSendSmsParameterById _request)
        {
            responsesendSMSById response;
            try
            {
                string _log = String.Format(@"GSM:{0} - MessageId: {1} ", _request.gsmNumber, _request.messageId);
                var textResult = new JavaScriptSerializer().Serialize(_log);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));
                string _MessageBody = String.Empty;
                if (_request != null && _request.parameters != null)
                {

                    var _getMessage = _db.RBN_SMS_TEMPLATES.FirstOrDefault(x => x.Id == _request.messageId && x.active == true);

                    if (_getMessage != null && !String.IsNullOrEmpty(_getMessage.Message))
                    {
                        _MessageBody = _getMessage.Message;

                        List<string> _parameters = new List<string>();
                        List<string> _requestParameters = new List<string>();



                        Regex rgx = new Regex("#([A-Za-z0-9]+)#");

                        if (rgx.Matches(_getMessage.Message).Count > 0)
                        {
                            foreach (Match match in rgx.Matches(_getMessage.Message))
                            {
                                _parameters.RemoveAll(x => x == match.Value);
                                _parameters.Add(match.Value);
                            }

                            foreach (var item in _request.parameters)
                            {
                                if (!String.IsNullOrEmpty(item.key))
                                {
                                    _requestParameters.RemoveAll(x => x == item.key);
                                    _requestParameters.Add(item.key);
                                }
                            }

                            if (_parameters.All(_requestParameters.Contains))
                            {

                                foreach (var parameter in _request.parameters)
                                {
                                    if (!String.IsNullOrEmpty(parameter.value) && !String.IsNullOrEmpty(parameter.key))
                                    {
                                        _MessageBody = _MessageBody.Replace(parameter.key, parameter.value);
                                    }
                                    else
                                    {
                                        response = new responsesendSMSById
                                        {
                                            status = false,
                                            statusCode = 203,
                                            message = "Parameter information is incorrect or missing.",
                                            displayMessage = "Parametre bilgisi hatalı veya eksik."
                                        };
                                        textResult = new JavaScriptSerializer().Serialize(response);
                                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));
                                        return BadRequest(response);
                                    }
                                }

                                var _sendSMSResponse = await _mobilDevService.sendSms(_request.gsmNumber, _MessageBody);

                                if (_sendSMSResponse.status)
                                {
                                    _getMessage.Message = _MessageBody;

                                    response = new responsesendSMSById
                                    {
                                        status = true,
                                        statusCode = 200,
                                        message = "Message sent successfully.",
                                        displayMessage = "SMS Başarıyla Gönderilmiştir",
                                        data = _getMessage,
                                        bulkid = _sendSMSResponse.bulkid
                                    };
                                    textResult = new JavaScriptSerializer().Serialize(response);
                                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));

                                }
                                else
                                {
                                    response = new responsesendSMSById
                                    {
                                        status = true,
                                        statusCode = 202,
                                        message = "The message could not be sent due to an ISP problem. Please provide information.",
                                        data = _getMessage
                                    };
                                    textResult = new JavaScriptSerializer().Serialize(response);
                                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));
                                    return BadRequest(response);
                                }

                            }
                            else
                            {
                                response = new responsesendSMSById
                                {
                                    status = false,
                                    statusCode = 205,
                                    message = "Unsuccessful",
                                    displayMessage = "Şablon içinde bulunan parametre ile gönderilen parametre farklı olduğundan dolayı mesaj gönderilemez."
                                };
                                textResult = new JavaScriptSerializer().Serialize(response);
                                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));
                                return BadRequest(response);
                            }

                        } else
                        {
                            response = new responsesendSMSById
                            {
                                status = false,
                                statusCode = 210,
                                displayMessage = "Gönderilmek istenen mesajda parametre bilgisi yok tekli sms ile gönderim deneyiniz.",
                                message = "The message could not be sent because there is no template defined for the Id number entered."
                            };
                            return BadRequest(response);
                        }
                    }
                    else
                    {
                        response = new responsesendSMSById
                        {
                            status = false,
                            statusCode = 201,
                            message = "The message could not be sent because there is no template defined for the Id number entered."
                        };
                        return BadRequest(response);
                        //Mesaj tanımı yok
                    }
                } else
                {
                    response = new responsesendSMSById
                    {
                        status = false,
                        statusCode = 404,
                        message = "The message could not be sent because there is no template defined for the Id number entered."
                    };
                    return NotFound(response);
                }


                textResult = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));
                return Ok(response);

            }
            catch
            {
                response = new responsesendSMSById
                {
                    status = false,
                    statusCode = 500,
                    message = "System Error"
                };

                var textResult = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));

                return BadRequest(response);

            }


        }




        //[HttpPost("smsReport")]
        //public async Task<IActionResult> smsReport(long bulkId)
        //{
        //    responsesendSMSById response;

        //    try
        //    {
        //        string _log = String.Format(@"BulkId:{0}", bulkId);
        //        var textResult = new JavaScriptSerializer().Serialize(_log);
        //        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));


        //        if(bulkId > 0)
        //        {
        //            var tt = await _smsService.SmsReport(bulkId);

        //            return Ok("evet");
        //        } else
        //        {
        //            return Ok("hayır");
        //        }
        //    }
        //    catch
        //    {
        //        response = new responsesendSMSById
        //        {
        //            status = false,
        //            statusCode = 500,
        //            message = "System Error"
        //        };

        //        var textResult = new JavaScriptSerializer().Serialize(response);
        //        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));

        //        return BadRequest(response);

        //    }


        //}
    }
}
