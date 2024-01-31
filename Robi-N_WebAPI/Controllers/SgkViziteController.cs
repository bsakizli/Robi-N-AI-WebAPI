using DocumentFormat.OpenXml.Bibliography;
using FastReport;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using MailEntity;
using MailEntity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Robi_N_WebAPI.Model.Response;
using Robi_N_WebAPI.Services;
using Robi_N_WebAPI.Shecles;
using Robi_N_WebAPI.Utility;
using RobinCore;
using System.Data;

namespace Robi_N_WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SgkViziteController : ControllerBase
    {
        MailService mailService = new MailService();

        private readonly AIServiceDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<SgkViziteController> _logger;
        private IAppSettings _appConfig;


        public SgkViziteController(AIServiceDbContext db, IWebHostEnvironment appEnvironment, IConfiguration configuration, ILogger<SgkViziteController> logger, IAppSettings appConfig)
        {
            _appEnvironment = appEnvironment;
            _logger = logger;
            _db = db;
            _configuration = configuration;
            _appConfig = appConfig;
        }


        [HttpPost("confirmReports")]
        public async Task<ActionResult> confirmReports()
        {

            ViziteService viziteService = new ViziteService(_appEnvironment, _db, _appConfig);
            await viziteService.RaporSorgulaOnay();
            return Ok(true);

        }


        [HttpGet("getsgk")]
        public async Task<ActionResult> getpdf()
        {
            string webRootPath = _appEnvironment.WebRootPath; // Get the path to the wwwroot folder
            WebReport webReport = new WebReport(); // Create a Web Report Object
            webReport.Report.Load(webRootPath + "/reports/SgkViziteOnayFormu.frx"); // Load the report into the WebReport object
            var dataSet = new DataSet(); // Create a data source
            //dataSet.ReadXml(webRootPath + "/reports/nwind.xml"); // Open the xml database
            var data = await _db.RBN_SGK_HealthReports.Where(x => x.process == 0 && x.BildirimId != null).ToArrayAsync();

            webReport.Report.RegisterData(data, "Reports"); // Register the data source in the report
            webReport.Report.GetDataSource("Reports").Enabled = true;

            webReport.Report.Prepare();

            Stream stream = new MemoryStream();
            webReport.Report.Export(new PDFSimpleExport(), stream);
            stream.Position = 0;

            List<EmailReports> emailReports = new List<EmailReports>();
            foreach (var item in data)
            {
                EmailReports _mailReport = new EmailReports
                {
                    AdSoyad = item.AD + " " + item.SOYAD,
                    KimlikNumarasi = item.TCKIMLIKNO.ToString(),
                    MedulaRaporId = item.MEDULARAPORID,
                    RaporTakipNumarasi = item.RAPORTAKIPNO,
                    OnayReferansId = item.BildirimId,
                    RaporBaslamaTarihi = item.ABASTAR,
                    RaporBirisTarihi = item.RAPORBITTAR
                };
                emailReports.Add(_mailReport);
            }

            var tt = mailService.SGKOnayMailGonder(stream, emailReports);

            return File(stream, "application/pdf", Path.GetFileNameWithoutExtension("SGK-Onay-Bildirgesi") + ".pdf");
        }


        [HttpGet("getpdfpassword")]
        public async Task<ActionResult> getpdfpassword(DateTime? datetime = null)
        {
            try
            {
                getPdfPasswordResponse _response;

                if(datetime != null)
                {
                    long _password = Convert.ToInt64(await Helper.Helper.PdfGenerateCustomPassword(datetime.Value));
                    if (_password != null && _password > 0)
                    {
                        _response = new getPdfPasswordResponse
                        {
                            status = true,
                            statusCode = 200,
                            displayMessage = "Şifre üretilmiştir.",
                            message = "Successfuly",
                            date = datetime.Value,
                            password = _password
                        };
                        return Ok(_response);

                    }
                    else
                    {
                        _response = new getPdfPasswordResponse
                        {
                            status = true,
                            statusCode = 404,
                            displayMessage = "Şifre hatası, lütfen değeleri kontrol edin ve tekrar deneyiniz.",
                            message = "Unsuccessfuly",
                            date = datetime.Value
                        };
                        return BadRequest(_response);
                    }
                } else
                {
                    long _password = Convert.ToInt64(await Helper.Helper.PdfGenerateCustomPassword(DateTime.Now));
                    if (_password != null && _password > 0)
                    {
                        _response = new getPdfPasswordResponse
                        {
                            status = true,
                            statusCode = 200,
                            displayMessage = "Şifre üretilmiştir.",
                            message = "Successfuly",
                            date = DateTime.Now.Date,
                            password = _password
                        };
                        return Ok(_response);

                    }
                    else
                    {
                        _response = new getPdfPasswordResponse
                        {
                            status = true,
                            statusCode = 404,
                            displayMessage = "Şifre hatası, lütfen değeleri kontrol edin ve tekrar deneyiniz.",
                            message = "Unsuccessfuly",
                            date = DateTime.Now.Date
                        };
                        return BadRequest(_response);
                    }
                }

                
            } catch
            {
                getPdfPasswordResponse _response;
                _response = new getPdfPasswordResponse
                {
                    status = true,
                    statusCode = 404,
                    displayMessage = "Şifre hatası, lütfen değeleri kontrol edin ve tekrar deneyiniz.",
                    message = "Unsuccessfuly",
                    date = DateTime.Now.Date
                };
                return BadRequest(_response);

            }
           
        }


    }
}
