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
using Robi_N_WebAPI.Services;
using Robi_N_WebAPI.Shecles;
using Robi_N_WebAPI.Utility;
using System.Data;

namespace Robi_N_WebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SgkViziteController : ControllerBase
    {
        MailService mailService = new MailService();

        private readonly AIServiceDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<SgkViziteController> _logger;



        public SgkViziteController(AIServiceDbContext db, IWebHostEnvironment appEnvironment, IConfiguration configuration, ILogger<SgkViziteController> logger)
        {
            _appEnvironment = appEnvironment;
            _logger = logger;
            _db = db;
            _configuration = configuration;
        }


        [HttpPost("confirmReports")]
        public async Task<ActionResult> confirmReports()
        {

            ViziteService viziteService = new ViziteService(_appEnvironment, _db);
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


    }
}
