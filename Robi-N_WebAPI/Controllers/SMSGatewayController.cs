using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Nancy.Json;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Model.Request;
using Robi_N_WebAPI.Model.Response;
using Robi_N_WebAPI.Services;
using Robi_N_WebAPI.Utility;
using SimpleCrypto;
using System.Text.RegularExpressions;
using static Robi_N_WebAPI.Model.Response.responseSMSGateway;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SMSGatewayController : ControllerBase
    {

        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;
        private readonly IConfiguration _configuration;
        private readonly SMSService _smsService;

        PBKDF2 crypto = new PBKDF2();

        public SMSGatewayController(IConfiguration configuration, ILogger<IdentityCheckController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _configuration = configuration;
            _logger = logger;
            _db = db;
            _smsService = new SMSService();
        }

        [HttpPost("sendSMSById")]
        public IActionResult sendSMSById(long gsmNumber, int MessageId)
        {
            responsesendSMSById response;
            try
            {
                string _log = String.Format(@"GSM:{0} - MessageId: {1} ", gsmNumber, MessageId);
                var textResult = new JavaScriptSerializer().Serialize(_log);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));


                var _getMessage = _db.RBN_SMS_TEMPLATES.FirstOrDefault(x => x.Id == MessageId);

                if (_getMessage != null && !String.IsNullOrEmpty(_getMessage.Message))
                {
                    var _sendSMSResponse = _smsService.SendSMS(gsmNumber, _getMessage.Message);

                    if (_sendSMSResponse)
                    {
                        response = new responsesendSMSById
                        {
                            status = true,
                            statusCode = 200,
                            message = "Message sent successfully.",
                            data = _getMessage
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
                        statusCode = 201,
                        message = "The message could not be sent because there is no template defined for the Id number entered."
                    };

                    textResult = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));

                    return BadRequest(response);
                    //Mesaj tanımı yok
                }

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
        public IActionResult sendSmsParameterById(requestSendSmsParameterById _request)
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
                        foreach (Match match in rgx.Matches(_getMessage.Message))
                        {
                            _parameters.RemoveAll(x => x == match.Value);
                            _parameters.Add(match.Value);
                        }

                        foreach (var item in _request.parameters)
                        {
                            if (!String.IsNullOrEmpty(item.key)) {
                                _requestParameters.RemoveAll(x => x == item.key);
                                _requestParameters.Add(item.key);
                            }
                        }

                        if (_parameters.SequenceEqual(_requestParameters))
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

                            var _sendSMSResponse = _smsService.SendSMS(_request.gsmNumber, _getMessage.Message);

                            if (_sendSMSResponse)
                            {
                                _getMessage.Message = _MessageBody;

                                response = new responsesendSMSById
                                {
                                    status = true,
                                    statusCode = 200,
                                    message = "Message sent successfully.",
                                    displayMessage = "SMS Başarıyla Gönderilmiştir",
                                    data = _getMessage
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
                        {
                            response = new responsesendSMSById
                            {
                                status = false,
                                statusCode = 205,
                                message = "",
                                displayMessage = "Şablon içinde bulunan parametre ile gönderilen parametre farklı olduğundan dolayı mesaj gönderilemez."
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
    }
}
