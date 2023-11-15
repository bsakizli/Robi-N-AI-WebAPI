using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nancy.Json;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Model.Request;
using Robi_N_WebAPI.Model.Response;
using Robi_N_WebAPI.Utility;
using Robi_N_WebAPI.Utility.Tables;
using SimpleCrypto;
using static Robi_N_WebAPI.Model.Request.requestVoiceIVRApplication;
using static Robi_N_WebAPI.Model.Response.responseSMSTemplate;
using static Robi_N_WebAPI.Model.Response.responseVoiceIVRApplication;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class SMSTemplateController : ControllerBase
    {

        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;
        private readonly IConfiguration _configuration;

        PBKDF2 crypto = new PBKDF2();

        public SMSTemplateController(IConfiguration configuration, ILogger<IdentityCheckController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }


        [HttpGet("getSMSTemplateById")]
        public IActionResult getSMSTemplateById(int Id)
        {
            getSMSTemplate globalResponse;
            try
            {
                var _smsTemplate = _db.RBN_SMS_TEMPLATES.FirstOrDefault(x => x.Id == Id);

               if(_smsTemplate != null)
                {
                    globalResponse = new getSMSTemplate
                    {
                        statusCode = 200,
                        status = false,
                        message = "Message template information received.",
                        data = _smsTemplate
                    };
                } else
                {
                    globalResponse = new getSMSTemplate
                    {
                        statusCode = 201,
                        status = false,
                        message = "No such template definition was found.",
                        data = _smsTemplate
                    };
                }

                var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                return Ok(globalResponse);

            }
            catch (Exception ex)
            {
                globalResponse = new getSMSTemplate
                {
                    statusCode = 500,
                    status = false,
                    message = String.Format(@"System error please inform your administrator. - Message: {0}", ex.Message.ToString())

                };
                var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                return BadRequest(globalResponse);
            }
        }

        [HttpGet("getSMSTemplateList")]
        public IActionResult getSMSTemplateList()
        {
            getSMSTemplateList response;
            try
            {
                var _holidays = _db.RBN_SMS_TEMPLATES.ToList();
                response = new getSMSTemplateList
                {
                    statusCode = 200,
                    message = "The listing was done successfully.",
                    status = true,
                    data = _holidays

                };

                var textResult = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));

                return Ok(response);

            }
            catch (Exception e)
            {
                response = new getSMSTemplateList
                {
                    statusCode = 404,
                    message = String.Format("The listing was done successfully. - Exception: {0}", e.Message),
                    status = false,
                };
                var textResult = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));

                return BadRequest(response);
            }

        }

        [HttpPost("addSMSTemplate")]
        public IActionResult addSMSTemplate(requestSMSTemplate item)
        {
            getSMSTemplate response;

            try
            {
                var payLoadResult = new JavaScriptSerializer().Serialize(item);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - PayloadData: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), payLoadResult));


                var _record = new RBN_SMS_TEMPLATES()
                {
                    Message = item.messageText,
                    active = true,
                    addDate = DateTime.Now,

                };
                var lastRecord = _db.RBN_SMS_TEMPLATES.Add(_record);
                _db.SaveChanges();

                if (lastRecord != null)
                {
                    response = new getSMSTemplate
                    {
                        status = true,
                        statusCode = 200,
                        message = "Message template defined successfully.",
                        data = _record

                    };
                    var _responseText = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                    return Ok(response);
                }
                else
                {
                    response = new getSMSTemplate
                    {
                        status = false,
                        statusCode = 202,
                        message = "There was a problem defining the SMS Template."
                    };
                    var _responseText = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText)); return BadRequest(response);
                }


            }
            catch (Exception ex)
            {
                response = new getSMSTemplate
                {
                    status = false,
                    statusCode = 404,
                    message = String.Format("There was a problem with the definition. - Exception: {0}", ex.Message)
                };
                var _responseText = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText)); return BadRequest(response);
            }
        }

        [HttpPut("updateSMSTemplate/{id}")]
        public IActionResult updateSMSTemplate(int id, [FromBody] requestSMSTemplate item)
        {
            getSMSTemplate response;
            try
            {
                var payLoadResult = new JavaScriptSerializer().Serialize(item);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - PayloadData: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), payLoadResult));

                var _item = _db.RBN_SMS_TEMPLATES.First(x => x.Id == id);
                _item.Message = item.messageText;
                _item.updateDate = DateTime.Now;
                _item.active = item.active;

                if (_db.SaveChanges() == 1)
                {
                    response = new getSMSTemplate
                    {
                        status = true,
                        statusCode = 200,
                        message = "The update has been successfully implemented.",
                        data = _item
                    };
                    var _responseText = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                    return Ok(response);
                }
                else
                {
                    response = new getSMSTemplate
                    {
                        status = true,
                        statusCode = 201,
                        message = "No changes were detected.",
                        data = _item
                    };
                    var _responseText = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                response = new getSMSTemplate
                {
                    status = false,
                    statusCode = 404,
                    message = String.Format("A problem occurred during the update. - Exception: {0}", ex.Message)
                };
                var _responseText = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));

                return BadRequest(response);
            }

        }

        [HttpDelete("deleteSMSTemplate/{id}")]
        public IActionResult deleteSMSTemplate(int id)
        {
            getSMSTemplate response;

            try
            {
                var payLoadResult = new JavaScriptSerializer().Serialize(id);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - PayloadData: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), payLoadResult));

                var record = _db.RBN_SMS_TEMPLATES.Where(x => x.Id == id).FirstOrDefault();

                if (record != null)
                {
                    if (_db.RBN_SMS_TEMPLATES.Where(x => x.Id == id).ExecuteDelete() == 1)
                    {
                        response = new getSMSTemplate
                        {
                            status = true,
                            statusCode = 200,
                            message = "The definition of SMS template has been deleted.",
                            data = record
                        };
                        var _responseText = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                        return Ok(response);
                    }
                    else
                    {
                        response = new getSMSTemplate
                        {
                            status = false,
                            statusCode = 201,
                            message = "The SMS template could not be deleted. Please try again.",
                            data = record
                        };
                        var _responseText = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                        return BadRequest(response);
                    }
                }
                else
                {
                    response = new getSMSTemplate
                    {
                        status = false,
                        statusCode = 202,
                        message = "No SMS template found to delete."
                    };
                    var _responseText = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response = new getSMSTemplate
                {
                    status = false,
                    statusCode = 404,
                    message = String.Format("A system error occurred while deleting the holiday description. - Exception: {0}", ex.Message)
                };
                var _responseText = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                return BadRequest(response);
            }
        }


       

    }
}
