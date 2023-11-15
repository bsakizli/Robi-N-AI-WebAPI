using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Model.Request;
using Robi_N_WebAPI.Utility;
using SimpleCrypto;
using Robi_N_WebAPI.Helper;
using System;
using Robi_N_WebAPI.Utility.Tables;
using Robi_N_WebAPI.Model.Response;
using static Robi_N_WebAPI.Model.Request.requestVoiceIVRApplication;
using static Robi_N_WebAPI.Model.Response.responseVoiceIVRApplication;
using static Robi_N_WebAPI.Model.Response.responseVoiceIVRApplication.GoogleCalender;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Nancy.Json;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer;
using Robi_N_WebAPI.Model.Xml.Response;
using Nancy.Diagnostics;
using Newtonsoft.Json;
using Robi_N_WebAPI.Model.Service.Response;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using Robi_N_WebAPI.Services;
using Microsoft.Win32;
using Sprache;
using Org.BouncyCastle.Math.EC;
using System.Net.Mime;
using System.Text.RegularExpressions;
using static Robi_N_WebAPI.Model.Response.responseSMSGateway;
using Nancy;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,IVR Read Only Web Service,IVR Full Authorization")]
    [ApiController]
    public class IvrApiController : ControllerBase
    {
        MobilDevService _mobilDevService = new MobilDevService();
        private readonly AIServiceDbContext _db;
        private readonly ILogger<IvrApiController> _logger;
        private readonly IConfiguration _configuration;

        PBKDF2 crypto = new PBKDF2();

        public IvrApiController(IConfiguration configuration, ILogger<IvrApiController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }


        [HttpPost("addlog")]
        public async Task<IActionResult> addlog([FromBody] string text)
        {
            GlobalResponse response;

            try
            {
                var _raw = text.Replace("'", "\"");
                Regex rgx = new Regex("({[^{}]*})");
                var tt = rgx.Match(_raw).Value;
                var _tempText = tt.Replace("\"","'");

                var _json = _raw.Replace(tt, _tempText);

                var _request = JsonConvert.DeserializeObject<requestAddLog>(_json);
                if (_request != null)
                {
                    var payLoadResult = new JavaScriptSerializer().Serialize(_request);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - PayloadData: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), payLoadResult));

                    if (_request != null)
                    {
                        if (!String.IsNullOrEmpty(_request.uniqId) && !String.IsNullOrEmpty(_request.log))
                        {

                            var _record = new RBN_IVR_LOGS()
                            {
                                active = true,
                                logKey = _request.logKey,
                                log = _request.log.Replace("'", "\""),
                                uniqId = _request.uniqId,
                                addDate = DateTime.Now,
                            };
                            var lastRecord = _db.RBN_IVR_LOGS.Add(_record);
                            await _db.SaveChangesAsync();
                            if (lastRecord != null)
                            {
                                response = new GlobalResponse
                                {
                                    status = true,
                                    statusCode = 200,
                                    displayMessage = "Log kaydı yapılmıştır.",
                                    message = "Successful"
                                };
                                return Ok(response);
                            }
                            else
                            {
                                response = new GlobalResponse
                                {
                                    status = false,
                                    statusCode = 201,
                                    displayMessage = "Log kaydı yapılamadı.",
                                    message = "Unsuccessful"
                                };
                            }
                        }
                        else
                        {
                            response = new GlobalResponse
                            {
                                status = false,
                                statusCode = 202,
                                displayMessage = "Log parametrelerini kontrol ediniz.",
                                message = "Unsuccessful"
                            };
                        }

                        var _responseText = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                        return BadRequest(response);

                    }
                    else
                    {
                        response = new GlobalResponse
                        {
                            status = false,
                            statusCode = 202,
                            displayMessage = $"Log parametresi hatalı, gönderdiğiniz içeriği kontrol ediniz.",
                            message = "Unsuccessful"
                        };
                        var _responseText = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                        return BadRequest(response);
                    }

                } else
                {
                    response = new GlobalResponse
                    {
                        status = false,
                        statusCode = 202,
                        displayMessage = $"Log parametresi hatalı, gönderdiğiniz içeriği kontrol ediniz.",
                        message = "Unsuccessful"
                    };
                    var _responseText = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                    return BadRequest(response);
                }

                

            }
            catch (Exception ex)
            {
                response = new GlobalResponse
                {
                    status = false,
                    statusCode = 202,
                    displayMessage = $"Log parametrelerini kontrol ediniz. - Error {ex.Message}",
                    message = "Unsuccessful"
                };
                var _responseText = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                return BadRequest(response);
            }
        }


        [HttpPost("sendSmsParameterById")]
        public async Task<IActionResult> sendSmsParameterById([FromBody] string text)
        {
            responsesendSMSById response;
           
            try
            {
                var _raw = text.Replace("\"", "'");
                 _raw = _raw.Replace("'", "\"");
                var _request = JsonConvert.DeserializeObject<requestSendSmsParameterById>(_raw);

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
                }
                else
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
