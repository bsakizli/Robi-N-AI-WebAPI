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
using Microsoft.IdentityModel.Tokens;
using ExtendedXmlSerializer.ExtensionModel.Types.Sources;
using Robi_N_WebAPI.Helper.Models;
using Org.BouncyCastle.Utilities;

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
        public async Task<IActionResult> addlog([FromBody] requestAddLog _request)
        {
            GlobalResponse response;

            try
            {

                if (_request != null)
                {
                    if (_request.logs != null && _request.logkey > 0 && !String.IsNullOrEmpty(_request.uniqId)) {

                        if(_request.logs.Count > 0)
                        {
                            List<addLogTemplate> logs = new List<addLogTemplate>();
                            foreach (var item in _request.logs)
                            {
                                if (!String.IsNullOrEmpty(item.key))
                                {
                                    logs.Add(new addLogTemplate { logName = item.key, content = item.value });
                                }
                            }

                            //var list = new List<KeyValuePair<string, string>>();
                            //list.Add(new KeyValuePair<string, string>("Cat", "1"));
                            //list.Add(new KeyValuePair<string, string>("Dog", "2"));
                            //list.Add(new KeyValuePair<string, string>("Dog", "3"));


                           

                            //var newEntry = new KeyValuePair<TKey, TValue>(oldEntry.Key, newValue);

                            string json = new JavaScriptSerializer().Serialize(logs);
                            var _record = new RBN_IVR_LOGS()
                            {
                                active = true,
                                log = json,
                                logKey = _request.logkey,
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

                            } else
                            {
                                response = new GlobalResponse
                                {
                                    status = true,
                                    statusCode = 201,
                                    displayMessage = "Log kayıt edilemedi.",
                                    message = "Unsuccessful"
                                };
                            }

                        }
                        else
                        {
                            response = new GlobalResponse
                            {
                                status = true,
                                statusCode = 201,
                                displayMessage = "Gönderilen bir log bilgisi bulunamadı",
                                message = "Unsuccessful"
                            };
                        }
                    
                    } else
                    {
                        response = new GlobalResponse
                        {
                            status = true,
                            statusCode = 201,
                            displayMessage = "Eksik veya hatalı parametre",
                            message = "Unsuccessful"
                        };
                    }
                } else
                {
                    response = new GlobalResponse
                    {
                        status = true,
                        statusCode = 201,
                        displayMessage = "Hatalı log isteği",
                        message = "Unsuccessful"
                    };
                }
                return BadRequest(response);

            }catch (Exception ex)
            {
                response = new GlobalResponse
                {
                    status = true,
                    statusCode = 500,
                    displayMessage = "Server Hatası: " +ex.Message,
                    message = "Unsuccessful"
                };
                return BadRequest(response);
            }
        }
    }
}
