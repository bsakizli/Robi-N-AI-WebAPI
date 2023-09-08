using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nancy.Json;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Model.Response;
using Robi_N_WebAPI.Services;
using Robi_N_WebAPI.Utility;
using SimpleCrypto;
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
        public IActionResult sendSMSById(string gsmNumber, int MessageId)
        {
            responsesendSMSById response;
            try
            {
                string _log = String.Format(@"GSM:{0} - MessageId: {1} ", gsmNumber, MessageId);
                var textResult = new JavaScriptSerializer().Serialize(_log);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));


                var _getMessage = _db.RBN_SMS_TEMPLATES.FirstOrDefault(x => x.Id == MessageId);

                if (_getMessage != null)
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
                    } else
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
                } else
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

            } catch
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
