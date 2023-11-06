using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Utility;
using EmptorUtility;
using RobinCore;
using Microsoft.AspNetCore.Authorization;
using Robi_N_WebAPI.Model.Response;
using Microsoft.EntityFrameworkCore;
using Robi_N_WebAPI.Model.Request;
using Microsoft.Kiota.Abstractions;
using Robi_N_WebAPI.Utility.Tables;
using MailEntity;
using EmptorUtility.Models.Response;
using System.Net;
using DecaTec.WebDav;
using Google.Apis.Auth.OAuth2;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EmptorApiController : ControllerBase
    {

        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;




        public EmptorApiController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration configuration, ILogger<IdentityCheckController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }


        //[HttpGet("sqlquerytest")]
        //public async Task<IActionResult> getTest()
        //{
        //    var credentials = new NetworkCredential("admin", "Baris3857++");
        //    var webDavSession = new WebDavSession(@String.Format(@"https://drive.tospi.tech/remote.php/dav/files/{0}/",credentials.UserName), credentials);
        //    var items = await webDavSession.ListAsync(@"BDH/SQL_QUERY/");

        //    foreach (var item in items)
        //    {
        //        HttpClient client = new HttpClient();
        //        Uri baseUri = new Uri(item.Uri.ToString());
        //        client.BaseAddress = baseUri;
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.ConnectionClose = true;

        //        ////Post body content
        //        //var values = new List<KeyValuePair<string, string>>();
        //        //values.Add(new KeyValuePair<string, string>(credentials.UserName, credentials.Password));
        //        //var content = new FormUrlEncodedContent(values);

        //        var authenticationString = $"{credentials.UserName}:{credentials.Password}";
        //        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

        //        var requestMessage = new HttpRequestMessage(HttpMethod.Get, baseUri);
        //        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

        //        var response = await client.SendAsync(requestMessage);
        //        response.EnsureSuccessStatusCode();
        //        string responseBody = await response.Content.ReadAsStringAsync();



        //        var tt = item.ContentClass.ToString();
        //    }

        //    return Ok("Tamamdır");
        //}


        [HttpGet("getTicketById")]
        public IActionResult getTicketById(int Id)
        {
            EmptorDbAction db = new EmptorDbAction(_configuration);
            return Ok(db.getEmptorTicketId(Id));
        }


        [HttpGet("GetWaitReasonsListFromTicketId")]
        public async Task<IActionResult> GetWaitReasonsListFromTicketId(string TicketId)
        {
            responseGetWaitReasonsListFromTicketId _response;
            try
            {
                EmptorDbAction db = new EmptorDbAction(_configuration);

               var _check = await db.TicketWaitingPreCheck(TicketId);

                if (_check.StatusCode == 200)
                {
                    var _listReasons = await db.GetWaitReasonsListFromTicketId(TicketId);

                    if (_listReasons.Count > 0)
                    {
                        var _company = await db.getCompanyFullName(TicketId);

                        _response = new responseGetWaitReasonsListFromTicketId
                        {
                            status = true,
                            displayMessage = "Lütfen bir bekleme nedeni seçiniz.",
                            message = "Successfully",
                            statusCode = _check.StatusCode,
                            data = _listReasons,
                            company = _company
                        };
                        return Ok(_response);
                    }
                    else
                    {
                        _response = new responseGetWaitReasonsListFromTicketId
                        {
                            status = false,
                            displayMessage = "Böyle bir kayıt numarası mevcut değil, lütfen kayıt numarasını kontrol ediniz.",
                            message = "Successfully",
                            statusCode = 199,
                            data = await db.GetWaitReasonsListFromTicketId(TicketId)
                        };
                        return BadRequest(_response);
                    }

                }
                else
                {
                    _response = new responseGetWaitReasonsListFromTicketId
                    {
                        status = false,
                        displayMessage = _check.StatusMessage,
                        message = "Successfully",
                        statusCode = _check.StatusCode
                    };
                    return BadRequest(_response);
                }



            }
            catch
            {
                _response = new responseGetWaitReasonsListFromTicketId
                {
                    status = false,
                    displayMessage = "Sistem hatası lütfen daha sonra tekrar deneyiniz.",
                    message = "System Error",
                    statusCode = 500
                };
                return BadRequest(_response);
            }
        }


        [HttpPost("StandbyTicket")]
        public async Task<IActionResult> StandbyTicket([FromBody] requestStandbyTicket _request)
        {
            GlobalResponse _response = new GlobalResponse();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                string authHeader = Request.Headers["Authorization"];
                authHeader = authHeader.Replace("Bearer ", "");
                var jsonToken = handler.ReadToken(authHeader);
                var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                var UserId = tokenS.Claims.First(claim => claim.Type == "UserId").Value;
                

                EmptorDbAction db = new EmptorDbAction(_configuration);
                DateTime? _reasonDate;
                if (_request.TicketId != null && _request.ReasonId > 0 && Convert.ToInt32(UserId) > 0)
                {
                    var _check = await db.TicketWaitingPreCheck(_request.TicketId);
                    var _company = await db.getCompanyFullName(_request.TicketId);
                   
                    if (_check.StatusCode == 200 && _company.Name != null)
                    {
                        var item = await _db.RBN_WAITING_TIMES.Where(x => x.EmptorTicketWaitingReasonId == _request.ReasonId).FirstOrDefaultAsync();
                        if (item != null)
                        {
                            TimeSpan hours = new TimeSpan(10, 00, 0);

                            if (item.Overtime)
                            {
                                int _now = (int)DateTime.Now.DayOfWeek;

                                if (_now == 5)
                                {
                                    _reasonDate = DateTime.Now.AddDays(3).Date + hours;
                                }
                                else if (_now == 6)
                                {
                                    _reasonDate = DateTime.Now.AddDays(2).Date + hours;
                                }
                                else
                                {
                                    _reasonDate = DateTime.Now.AddDays(1).Date + hours;
                                }
                            }
                            else
                            {
                                _reasonDate = DateTime.UtcNow.AddDays(item.WaitingTimeDay).Date + hours;
                            }

                            var _wait = await db.TicketWaiting(_request.TicketId, (int)_request.ReasonId, (DateTime)_reasonDate);

                            if (_wait)
                            {
                                var _contactInformation = await db.getMainResponsibleInfo(Convert.ToInt32(UserId), _request.TicketId);
                                if (_contactInformation != null)
                                {
                                    if (Convert.ToBoolean(_contactInformation.status))
                                    {
                                        MailService mailService = new MailService();
                                        var _send = mailService.WaitingEmptorSendMail(_request.TicketId, _contactInformation, _company.Name, (DateTime)_reasonDate);
                                    }

                                    //Ticket Waiting Logs
                                    RBN_EMPTOR_WaitingTicketHistory _RBN_EMPTOR_WaitingTicketHistory = new RBN_EMPTOR_WaitingTicketHistory
                                    {
                                        TicketDesc = _request.TicketId,
                                        Active = true,
                                        CompanyId = _company.Id,
                                        CompanyName = _company.Name,
                                        MainUserFullName = _contactInformation.MainResponsibleFullName,
                                        SubUserFullName = _contactInformation.SubResponsibleFullName,
                                        WaitingDate = _reasonDate,
                                        WaitingReason = _request.ReasonId,
                                        TransactionDate = DateTime.Now
                                    };
                                    var lastRecord = _db.RBN_EMPTOR_WaitingTicketHistory.Add(_RBN_EMPTOR_WaitingTicketHistory);
                                    await _db.SaveChangesAsync();
                                    if (lastRecord != null)
                                    {
                                        _response = new GlobalResponse
                                        {
                                            status = true,
                                            statusCode = 200,
                                            message = "Successfuly",
                                            displayMessage = "Kayıt beklemeye alınmıştır."
                                        };
                                        return Ok(_response);
                                    } else {


                                        _response = new GlobalResponse
                                        {
                                            status = false,
                                            statusCode = 199,
                                            message = "Successfuly",
                                            displayMessage = "Kayıt beklemeye alınamadı!"
                                        };
                                        return BadRequest(_response);
                                    }

                                }
                            }
                            _response = new GlobalResponse
                            {
                                status = false,
                                statusCode = 199,
                                message = "Successfuly",
                                displayMessage = "Kayıt beklemeye alınamadı!"
                            };
                            return BadRequest(_response);
                        }
                        else
                        {
                            //Bekleme Nedeni ile Süresi Tanımsız
                            _response = new GlobalResponse
                            {
                                status = false,
                                statusCode = 404,
                                message = "Failed",
                                displayMessage = "Bekleme nedeni tanımsız olması sebebi ile kayıt beklemye alınamadı."
                            };
                            return BadRequest(_response);

                        }
                    }
                    else
                    {
                        //Beklemeye Alınamadı Sebebleri
                        _response = new GlobalResponse
                        {
                            status = false,
                            statusCode = _check.StatusCode,
                            message = "Failed",
                            displayMessage = _check.StatusMessage
                        };
                        return BadRequest(_response);

                    }
                }
                else
                {
                    _response = new GlobalResponse
                    {
                        status = false,
                        statusCode = 404,
                        message = "Failed",
                        displayMessage = "Kayıt numarası gereklidir. Lütfen kayıt numarası giriniz."
                    };
                    return BadRequest(_response);
                    //TİcketId Boş
                }

            }
            catch
            {
                _response = new GlobalResponse
                {
                    status = false,
                    statusCode = 500,
                    message = "System Error",
                    displayMessage = "Sistemlerde oluşan bir hata sebebi ile işlem gerçekleştirilmedi. Lütfen yöneticiye bilgi veriniz."
                };
                return BadRequest(_response);
            }


        }


    }
}
