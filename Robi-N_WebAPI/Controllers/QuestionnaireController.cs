using Robi_N_WebAPI.Utility;
using Microsoft.Extensions.Options;
using Robi_N_WebAPI.Model;
using DocumentFormat.OpenXml.Packaging;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using RobinCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Robi_N_WebAPI.Services;
using DocumentFormat.OpenXml.Wordprocessing;
using NetGsmAPI;
using SimpleCrypto;
using MailEntity;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,IVR Read Only Web Service,IVR Full Authorization")]
    [ApiController]
    public class QuestionnaireController : ControllerBase
    {
        MobilDevService _mobilDevService = new MobilDevService();
        private readonly AIServiceDbContext _db;
        private readonly ILogger<IvrApiController> _logger;
        private readonly IConfiguration _configuration;
        NetGsmService _smsService = new NetGsmService();
        PBKDF2 crypto = new PBKDF2();
        MailService MailService = new MailService();

        public QuestionnaireController(IConfiguration configuration, ILogger<IvrApiController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }

        [HttpPost("cevap")]
        public async Task<IActionResult> Index()
        {
            return Ok();
        }
    }
}
