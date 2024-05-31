using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nancy.Json;
using Robi_N_WebAPI.Helper;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Model.Response;
using Robi_N_WebAPI.Services;
using Robi_N_WebAPI.Utility;
using SimpleCrypto;
using static Robi_N_WebAPI.Model.Response.responseSMSTemplate;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,IVR Read Only Web Service,IVR Full Authorization")]
    [ApiController]
    public class DefectiveDeviceInquiryController : ControllerBase
    {
        private readonly AIServiceDbContext _db;
        private readonly ILogger<DefectiveDeviceInquiryController> _logger;
        private readonly IConfiguration _configuration;

        PBKDF2 crypto = new PBKDF2();
        BdhDeviceServiceApi bdhDeviceServiceApi = new BdhDeviceServiceApi();
        NetGsmAPI.NetGsmService netGsmService = new NetGsmAPI.NetGsmService();
       
        public DefectiveDeviceInquiryController(IConfiguration configuration, ILogger<DefectiveDeviceInquiryController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }


        [HttpGet("inquiry")]
        public async Task<IActionResult> inquiry(string service_number, ulong phoneNumber = 0)
        {
            DeviceInquiryResponse response;
            try
            {
                var _serviceResult = await bdhDeviceServiceApi.SamsungDeviceService(service_number);
                int _statusCode = 0;
                bool _IsStatus = false;
                string _displayMessage = String.Empty;
                string _Message = String.Empty;
                bool _IsSmsSend = false;
                long _BulkdId = 0;
                string _MessageTemplate = String.Empty;
                ulong _CargoTrackingNumber = 0;
                ulong _PhoneNo = 0;
                decimal _OfferPrice = 0;
                string _CargoCompany = String.Empty;
                string _ServiceName = String.Empty;
                string _Description = String.Empty;


                #region Diğer Operasyon Onarım Sorgulama
                if (_serviceResult.Data.Ref == 0)
                {
                    _serviceResult = await bdhDeviceServiceApi.OtherTTDeviceService(service_number);
                }
                #endregion



                #region Mapping Adımları
                if (!String.IsNullOrEmpty(Convert.ToString(_serviceResult.Data.CargoTrackingNumber))) {
                     _CargoTrackingNumber = (ulong)Convert.ToUInt64(Convert.ToString(_serviceResult.Data.CargoTrackingNumber));
                }

                if (!String.IsNullOrEmpty(Convert.ToString(_serviceResult.Data.PhoneNo)))
                {
                    _PhoneNo = (ulong)Convert.ToUInt64(Helper.Helper.FormatPhoneNumber(Convert.ToString(_serviceResult.Data.PhoneNo)));
                }

                if (!String.IsNullOrEmpty(Convert.ToString(_serviceResult.Data.OfferPrice)))
                {
                    _OfferPrice = (decimal)Convert.ToDecimal(Convert.ToString(_serviceResult.Data.OfferPrice));
                }

                if (!String.IsNullOrEmpty(Convert.ToString(_serviceResult.Data.CargoCompany)))
                {
                    _CargoCompany = Convert.ToString(_serviceResult.Data.CargoCompany);
                }

                if (!String.IsNullOrEmpty(Convert.ToString(_serviceResult.Data.ServiceName)))
                {
                    _ServiceName = Convert.ToString(_serviceResult.Data.ServiceName);
                }

                if (!String.IsNullOrEmpty(Convert.ToString(_serviceResult.Data.Description)))
                {
                    _Description = Convert.ToString(_serviceResult.Data.Description);
                }
                #endregion



                #region Operasyonel Süreçler ve Kargo Adımları & SMS Gönderimi

                if (phoneNumber > 0)
                {
                    _PhoneNo = phoneNumber;
                }

                if (Convert.ToInt32(Convert.ToString(_serviceResult.Data.Ref)) == 11)
                {
                    var _message = await _db.RBN_SMS_TEMPLATES.Where(x => x.MessageCode == 102).FirstOrDefaultAsync();
                    _MessageTemplate = _message.Message.Replace("{tutar}", _OfferPrice.ToString()).Replace("{CaseId}", service_number);

                } else if (Convert.ToInt32(Convert.ToString(_serviceResult.Data.Ref)) == 5)
                {
                    var _message = await _db.RBN_SMS_TEMPLATES.Where(x => x.MessageCode == 103).FirstOrDefaultAsync();
                    _MessageTemplate = _message.Message.Replace("{CaseId}", service_number);

                } else if (Convert.ToInt32(Convert.ToString(_serviceResult.Data.Ref)) == 4)
                {
                    var _message = await _db.RBN_SMS_TEMPLATES.Where(x => x.MessageCode == 104).FirstOrDefaultAsync();
                    _MessageTemplate = _message.Message.Replace("{CaseId}", service_number);

                }  else if (Convert.ToInt32(Convert.ToString(_serviceResult.Data.Ref)) == 9)
                {
                    var _message = await _db.RBN_SMS_TEMPLATES.Where(x => x.MessageCode == 105).FirstOrDefaultAsync();
                    var _cargoList = await _db.RBN_CARGO_COMPANY_LIST.Where(x => x.cargoName == _CargoCompany).FirstOrDefaultAsync();
                    string _trackLink = _cargoList.trackingUrl.Replace("{tracking_no}", _CargoTrackingNumber.ToString());
                    _MessageTemplate = _message.Message.Replace("{CaseId}", service_number).Replace("{CargoCompany}", _CargoCompany).Replace("{CargoTrackingNumber}", _CargoTrackingNumber.ToString()).Replace("{CargoTrackingUrl}", _trackLink);
                }


                //SMS Step Action
                if (!String.IsNullOrEmpty(_MessageTemplate) && _PhoneNo > 0)
                {
                    var _sendSms = await netGsmService.sendSms(_PhoneNo.ToString(), _MessageTemplate);
                    if (_sendSms.status)
                    {
                        _IsSmsSend = true;
                        _BulkdId = _sendSms.bulkid;
                    }
                    else
                    {
                        _IsSmsSend = false;
                    }
                }
                #endregion


                if (Convert.ToInt32(Convert.ToString(_serviceResult.Data.Ref)) != 0)
                {
                    _statusCode = 200;
                    _IsStatus = true;
                    _displayMessage = "Cihaz Sorgulaması Tamamlanmıştır";
                    _Message = "Successful";
                } else
                {
                    _statusCode = 201;
                    _IsStatus = false;
                    _displayMessage = "Cihaz kaydı bulunamadı.";
                    _Message = "Unsuccessful";
                }

                response = new DeviceInquiryResponse
                {
                    status = _IsStatus,
                    statusCode = _statusCode,
                    displayMessage = _displayMessage,
                    message = _Message,
                    Ref = _serviceResult.Data.Ref,
                    Description = _serviceResult.Data.Description,
                    CargoCompany = _CargoCompany,
                    CargoTrackingNumber = _CargoTrackingNumber,
                    PhoneNo = _PhoneNo,
                    OfferPrice = _OfferPrice,
                    ServiceName = _ServiceName,
                    BulkId = _BulkdId,
                    IsSendSms = _IsSmsSend
                };

                var globalResponseResult = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                return Ok(response);

            }
            catch (Exception ex)
            {
                response = new DeviceInquiryResponse
                {
                    statusCode = 500,
                    status = false,
                    message = String.Format(@"System error please inform your administrator. - Message: {0}", ex.Message.ToString())

                };
                var globalResponseResult = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                return BadRequest(response);
            }
        }

    }
}
