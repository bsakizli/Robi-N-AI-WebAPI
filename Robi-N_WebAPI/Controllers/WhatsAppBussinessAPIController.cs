using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetGsmAPI;
using Newtonsoft.Json;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Model.Response.WhatsApp;
using Robi_N_WebAPI.Services;
using Robi_N_WebAPI.Utility;
using System.Text.Json.Nodes;
using WhatsAppBusinessAPI;
using WhatsAppBusinessAPI.Model;
using WhatsAppBusinessAPI.WhatsAppBusinessModel;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class WhatsAppBussinessAPIController : ControllerBase
    {
        NetGsmService _smsService = new NetGsmService();
        MobilDevService _mobilDevService = new MobilDevService();
        private readonly AIServiceDbContext _db;
        private readonly ILogger<WhatsAppBussinessAPIController> _logger;
        private readonly IConfiguration _configuration;


        public WhatsAppBussinessAPIController(IConfiguration configuration, ILogger<WhatsAppBussinessAPIController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _configuration = configuration;
            _logger = logger;
            _db = db;

        }

        WhatsAppBusinessClient wpClient = new WhatsAppBusinessClient();

        [HttpPost("checkPhones")]
        public async Task<IActionResult> checkPhones(long phone)
        {
            checkPhones checkPhonesResponse;
            try
            {
                if (Convert.ToString(phone).Length == 12)
                {
                    CheckPhonesRequest checkPhonesRequest = new CheckPhonesRequest
                    {
                        blocking = "no_wait",
                        force_check = false,
                        contacts = new List<string> { Convert.ToString(phone) }
                    };

                    checkPhonesResponse = new checkPhones
                    {
                        status = true,
                        statusCode = 200,
                        displayMessage = "İşlem tamamlandı",
                        message = "Successful",
                        result = await wpClient.CheckPhones(checkPhonesRequest)
                    };

                }
                else
                {
                    checkPhonesResponse = new checkPhones
                    {
                        status = false,
                        displayMessage = "Telefon numarası 10 haneli olmak zorunda",
                        message = "Unsuccessful",
                        statusCode = 404
                    };
                    return BadRequest(checkPhonesResponse);
                }

                return Ok(checkPhonesResponse);

            }
            catch (Exception ex)
            {

                checkPhonesResponse = new checkPhones
                {
                    status = false,
                    displayMessage = ex.Message,
                    message = "Unsuccessful",
                    statusCode = 500,
                    result = null
                };
                return BadRequest(checkPhonesResponse);
            }

        }

        [HttpPost("sendTextMessage")]
        public async Task<IActionResult> sendTextMessage([FromBody] SendTextMessageRequest sendTextMessageRequest)
        {
            sendTextMessageResponse response;
            try
            {
                if (Convert.ToString(sendTextMessageRequest.to).Length == 12)
                {
                    CheckPhonesRequest checkPhonesRequest = new CheckPhonesRequest
                    {
                        blocking = "no_wait",
                        force_check = false,
                        contacts = new List<string> { Convert.ToString(sendTextMessageRequest.to) }
                    };

                    var checkPhone = await wpClient.CheckPhones(checkPhonesRequest);

                    IEnumerable<dynamic> items = @checkPhone.contacts;

                    if (checkPhone != null && items.Count() > 0)
                    {
                        if (checkPhone.contacts[0].status.ToString() == "valid")
                        {
                            sendTextMessageRequest.to = checkPhone.contacts[0].wa_id;
                            var tt = await wpClient.SendTextMessage(sendTextMessageRequest);

                            if (Convert.ToBoolean(tt.sent.ToString())) {
                                response = new sendTextMessageResponse
                                {
                                    status = true,
                                    displayMessage = "Mesaj gönderildi!",
                                    statusCode = 200,
                                    message = "Successful",
                                    result = null
                                };
                                return Ok(response);
                            } else
                            {
                                response = new sendTextMessageResponse
                                {
                                    status = false,
                                    displayMessage = "Mesaj gönderilmedi!",
                                    statusCode = 400,
                                    message = "Unsuccessful",
                                    result = null
                                };
                                return BadRequest(response);
                            }
                        }
                        else
                        {
                            response = new sendTextMessageResponse
                            {
                                status = false,
                                displayMessage = "Mesaj göndermek istediğiniz numarada WhatsApp hesabı tanımlı değil",
                                message = "Unsuccessful",
                                statusCode = 201,
                                result = null
                            };
                            return BadRequest(response);
                            //Bu numarada Wp yok
                        }
                    }
                    else
                    {
                        response = new sendTextMessageResponse
                        {
                            status = false,
                            displayMessage = "Telefon numarası kontrol edilemedi, lütfen daha sonra tekrar deneyin.",
                            message = "",
                            statusCode = 201,
                            result = null
                        };
                        return BadRequest(response);
                        //Telefon Kontrolü Yapılmadı
                    }
                }
                else
                {
                    response = new sendTextMessageResponse
                    {
                        status = false,
                        displayMessage = "Telefon numarası 10 haneli olmak zorunda",
                        message = "Unsuccessful",
                        statusCode = 404
                    };
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                response = new sendTextMessageResponse
                {
                    status = false,
                    displayMessage = ex.Message,
                    message = "Unsuccessful",
                    statusCode = 500,
                    result = null
                };
                return BadRequest(response);
            }

        }


        [HttpPost("sendTextGroupMessage")]
        public async Task<IActionResult> sendTextGroupMessage([FromBody] SendTextMessageRequest sendTextMessageRequest)
        {
            sendTextMessageResponse response;
            try
            {
                var tt = await wpClient.SendTextMessage(sendTextMessageRequest);

                if (Convert.ToBoolean(tt.sent.ToString()))
                {
                    response = new sendTextMessageResponse
                    {
                        status = true,
                        displayMessage = "Mesaj gönderildi!",
                        statusCode = 200,
                        message = "Successful",
                        result = null
                    };
                    return Ok(response);
                }
                else
                {
                    response = new sendTextMessageResponse
                    {
                        status = false,
                        displayMessage = "Mesaj gönderilmedi!",
                        statusCode = 400,
                        message = "Unsuccessful",
                        result = null
                    };
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response = new sendTextMessageResponse
                {
                    status = false,
                    displayMessage = ex.Message,
                    message = "Unsuccessful",
                    statusCode = 500,
                    result = null
                };
                return BadRequest(response);
            }

        }

        [HttpPost("sendLocationMessage")]
        public async Task<IActionResult> sendLocationMessage([FromBody] SendLocationMessageRequest sendLocationMessageRequest)
        {
            SendLocationMessageResponse response;
            try
            {
                if (Convert.ToString(sendLocationMessageRequest.to).Length == 12)
                {
                    CheckPhonesRequest checkPhonesRequest = new CheckPhonesRequest
                    {
                        blocking = "no_wait",
                        force_check = false,
                        contacts = new List<string> { Convert.ToString(sendLocationMessageRequest.to) }
                    };

                    var checkPhone = await wpClient.CheckPhones(checkPhonesRequest);
                    if (checkPhone != null && checkPhone.contacts.Count() > 0)
                    {
                        if (checkPhone.contacts[0].status == "valid")
                        {
                            sendLocationMessageRequest.to = checkPhone.contacts[0].wa_id;

                            response = new SendLocationMessageResponse
                            {
                                status = true,
                                displayMessage = "Konum gönderimi kuyruğa alınmıştır.",
                                statusCode = 200,
                                message = "Successful",
                                result = await wpClient.SendLocationMessage(sendLocationMessageRequest)
                            };
                            return Ok(response);
                        }
                        else
                        {
                            response = new SendLocationMessageResponse
                            {
                                status = false,
                                displayMessage = "Mesaj göndermek istediğiniz numarada WhatsApp hesabı tanımlı değil",
                                message = "Unsuccessful",
                                statusCode = 201,
                                result = null
                            };
                            return BadRequest(response);
                            //Bu numarada Wp yok
                        }
                    }
                    else
                    {
                        response = new SendLocationMessageResponse
                        {
                            status = false,
                            displayMessage = "Telefon numarası kontrol edilemedi, lütfen daha sonra tekrar deneyin.",
                            message = "",
                            statusCode = 201,
                            result = null
                        };
                        return BadRequest(response);
                        //Telefon Kontrolü Yapılmadı
                    }
                }
                else
                {
                    response = new SendLocationMessageResponse
                    {
                        status = false,
                        displayMessage = "Telefon numarası 10 haneli olmak zorunda",
                        message = "Unsuccessful",
                        statusCode = 404
                    };
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                response = new SendLocationMessageResponse
                {
                    status = false,
                    displayMessage = ex.Message,
                    message = "Unsuccessful",
                    statusCode = 500,
                    result = null
                };
                return BadRequest(response);
            }

        }

    }
}
