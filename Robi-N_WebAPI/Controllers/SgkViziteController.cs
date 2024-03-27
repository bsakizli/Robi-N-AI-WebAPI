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
        private HR_IAppSettings _appConfig;


        public SgkViziteController(AIServiceDbContext db, IWebHostEnvironment appEnvironment, IConfiguration configuration, ILogger<SgkViziteController> logger, HR_IAppSettings appConfig)
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


       



    }
}
