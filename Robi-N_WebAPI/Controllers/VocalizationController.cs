using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GoogleCloudUtility;
using System.Drawing;
using Robi_N_WebAPI.Utility;
using Microsoft.Extensions.Options;
using Robi_N_WebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Robi_N_WebAPI.Utility.Tables;
using static Robi_N_WebAPI.Model.Response.responseVoiceIVRApplication.GoogleCalender;
using Robi_N_WebAPI.Model.Response;
using Nancy.Json;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Google.Apis.Auth.OAuth2;
using Swashbuckle.AspNetCore.Annotations;


namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,IVR Read Only Web Service")]
    [ApiController]
    

    public class VocalizationController : ControllerBase
    {

        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;


        public VocalizationController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration configuration, ILogger<IdentityCheckController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }

        [HttpPost("customVocalization")]
        [SwaggerOperation("SSML supported voice-over.", "Link: https://cloud.google.com/text-to-speech/docs/ssml", OperationId = "customVocalization")]

        public IActionResult customVocalization(string ssml)
        {
            responseGetGoogleTextToSpech response;

            try
            {
                var globalResponseResult = new JavaScriptSerializer().Serialize(ssml);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));

                GoogleCloud _googleService = new GoogleCloud();

                var _googleServiceResponse = _googleService.textToSpechSsml(ssml);

                if (!_googleServiceResponse.AudioContent.IsEmpty)
                {

                    RBN_VOICE_SOUNDS item = new RBN_VOICE_SOUNDS
                    {
                        active = true,
                        addDate = DateTime.Now,
                        platform = "Google",
                        fileName = Guid.NewGuid().ToString(),
                        text = ssml,
                        soundContent = _googleServiceResponse.AudioContent.ToByteArray()
                    };
                    _db.RBN_VOICE_SOUNDS.Add(item);
                    if (_db.SaveChanges() == 1)
                    {
                        response = new responseGetGoogleTextToSpech
                        {
                            status = true,
                            statusCode = 200,
                            message = "Voiceover Successfully Identified",
                            soundId = item.Id,
                            base64SoundContent = _googleServiceResponse.AudioContent.ToBase64(),
                            soundUrl = String.Format(@"{0}/soundfiles/{1}.wav", "https://callcdn.bdh.com.tr", item.fileName),
                        };
                        var _responseText = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                        return Ok(response);
                        //Kayıt Eklendi
                    }
                    else
                    {
                        response = new responseGetGoogleTextToSpech
                        {
                            status = false,
                            message = "The audio file could not be saved to the database.",
                            statusCode = 201
                        };
                        var _responseText = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                        return BadRequest(response);
                        //Kayıt Yapılamadı
                    }

                }
                else
                {
                    response = new responseGetGoogleTextToSpech
                    {
                        status = false,
                        message = "The audio file could not be voiced by Google Cloud.",
                        statusCode = 203
                    };
                    var _responseText = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                    return BadRequest(response);
                }

            }
            catch
            {
                response = new responseGetGoogleTextToSpech
                {
                    status = false,
                    message = "System Error",
                    statusCode = 500
                };
                var _responseText = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                return BadRequest(response);
            }
        }


        [HttpPost("WarrantyTransactionFeeVocalization")]
        //[Consumes("application/xml")]
        public async Task<IActionResult> WarrantyTransactionFeeVocalization(string price = null)
        {
            var xmlDoc = new XmlDocument();
            var response = new responseWarrantyTransactionFeeVocalization();
            try
            {
                var globalResponseResult = new JavaScriptSerializer().Serialize(price);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));


                if (!String.IsNullOrEmpty(price))
                {
                    GoogleCloud _googleService = new GoogleCloud();

                    string _ssmlText = String.Format(@"<speak>
                 Cihazınız servisimizde <emphasis level='strong'>garanti</emphasis> dışı işlem görmektedir. <break time='500ms'/> Garanti dışı işlem ücreti vergiler dahil <break time='300ms'/> <emphasis level='strong'><say-as interpret-as='currency' language='tr-TR'>₺{0}</say-as></emphasis> ücreti bulunmaktadır.
                </speak>", price);

                    var _googleServiceResponse = _googleService.textToSpechSsml(_ssmlText);

                    if (!_googleServiceResponse.AudioContent.IsEmpty)
                    {

                        RBN_VOICE_SOUNDS item = new RBN_VOICE_SOUNDS
                        {
                            active = true,
                            addDate = DateTime.Now,
                            platform = "Google",
                            fileName = Guid.NewGuid().ToString(),
                            text = _ssmlText,
                            soundContent = _googleServiceResponse.AudioContent.ToByteArray()
                        };
                        _db.RBN_VOICE_SOUNDS.Add(item);
                        if (await _db.SaveChangesAsync() == 1)
                        {
                            //var _url = String.Format(@"http://callcdn.bdh.com.tr/Kayit_Altina_Alinacaktir.wav");
                            var _url = String.Format(@"{0}/soundfiles/{1}.wav", _configuration.GetValue<string>("SoundServerHost"), item.fileName);
                            xmlDoc.LoadXml(String.Format(@"<vxml xmlns='http://www.w3.org/2001/vxml' version='2.0'><form><block><prompt bargein='true'><audio src='{0}'/></prompt></block></form></vxml>", _url));


                            response = new responseWarrantyTransactionFeeVocalization
                            {
                                status = true,
                                message = "Successful voice-over",
                                displayMessage = "Seslendirme başarılı",
                                statusCode = 200,
                                url = _url
                            };
                            //return new ContentResult
                            //{
                            //    Content = xmlDoc.DocumentElement.OuterXml.ToString(),
                            //    ContentType = "application/xml",
                            //    StatusCode = 200
                            //};

                            return Ok(response);


                            //return Ok(xmlDoc.DocumentElement.OuterXml.ToString());
                        }
                        else
                        {

                            var _url = String.Format(@"{0}/soundfiles/{1}.wav", _configuration.GetValue<string>("SoundServerHost"), "503ebb18-ce0e-4439-bb41-9f7d61c530f2");
                            xmlDoc.LoadXml(String.Format(@"<vxml xmlns='http://www.w3.org/2001/vxml' version='2.0'><form><block><prompt bargein='true'><audio src='{0}'/></prompt></block></form></vxml>", _url));

                            response = new responseWarrantyTransactionFeeVocalization
                            {
                                status = true,
                                message = "Unavailable for Voice Over",
                                displayMessage = "Seslendirme servisi hatası",
                                statusCode = 404,
                                url = _url
                            };
                            return BadRequest(response);

                            //return new ContentResult
                            //{
                            //    Content = xmlDoc.DocumentElement.OuterXml.ToString(),
                            //    ContentType = "application/xml",
                            //    StatusCode = 404
                            //};
                        }

                    }
                    else
                    {
                        var _url = String.Format(@"{0}/soundfiles/{1}.wav", _configuration.GetValue<string>("SoundServerHost"), "503ebb18-ce0e-4439-bb41-9f7d61c530f2");
                        xmlDoc.LoadXml(String.Format(@"<vxml xmlns='http://www.w3.org/2001/vxml' version='2.0'><form><block><prompt bargein='true'><audio src='{0}'/></prompt></block></form></vxml>", _url));

                        response = new responseWarrantyTransactionFeeVocalization
                        {
                            status = true,
                            message = "Unavailable for Voice Over",
                            displayMessage = "Seslendirme servisi hatası",
                            statusCode = 404,
                            url = _url
                        };
                        return BadRequest(response);
                    }

                } else
                {
                    var _url = String.Format(@"{0}/soundfiles/{1}.wav", _configuration.GetValue<string>("SoundServerHost"), "503ebb18-ce0e-4439-bb41-9f7d61c530f2");
                    xmlDoc.LoadXml(String.Format(@"<vxml xmlns='http://www.w3.org/2001/vxml' version='2.0'><form><block><prompt bargein='true'><audio src='{0}'/></prompt></block></form></vxml>", _url));

                    //return new ContentResult
                    //{
                    //    Content = xmlDoc.DocumentElement.OuterXml.ToString(),
                    //    ContentType = "application/xml",
                    //    StatusCode = 404
                    //};

                    response = new responseWarrantyTransactionFeeVocalization
                    {
                        status = true,
                        message = "Unavailable for Voice Over",
                        displayMessage = "Seslendirme servisi hatası",
                        statusCode = 404,
                        url = _url
                    };
                    return BadRequest(response);
                }

            }
            catch (Exception ?ex)
            {
                var _url = String.Format(@"{0}/soundfiles/{1}.wav", _configuration.GetValue<string>("SoundServerHost"), "503ebb18-ce0e-4439-bb41-9f7d61c530f2");
                xmlDoc.LoadXml(String.Format(@"<vxml xmlns='http://www.w3.org/2001/vxml' version='2.0'><form><block><prompt bargein='true'><audio src='{0}'/></prompt></block></form></vxml>", _url));

                //return new ContentResult
                //{
                //    Content = xmlDoc.DocumentElement.OuterXml.ToString(),
                //    ContentType = "application/xml",
                //    StatusCode = 404
                //};

                response = new responseWarrantyTransactionFeeVocalization
                {
                    status = true,
                    message = "Unavailable for Voice Over",
                    displayMessage = "Seslendirme servisi hatası",
                    statusCode = 404,
                    url = _url
                };
                return BadRequest(response);
            }
            
        }

    }
}
