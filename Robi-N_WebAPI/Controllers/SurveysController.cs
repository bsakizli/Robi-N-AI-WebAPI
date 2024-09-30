using MailEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetGsmAPI;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Services;
using Robi_N_WebAPI.Utility;
using SimpleCrypto;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,IVR Read Only Web Service,IVR Full Authorization")]
    [ApiController]
    public class SurveysController : ControllerBase
    {

        MobilDevService _mobilDevService = new MobilDevService();
        private readonly AIServiceDbContext _db;
        private readonly ILogger<IvrApiController> _logger;
        private readonly IConfiguration _configuration;
        NetGsmService _smsService = new NetGsmService();
        PBKDF2 crypto = new PBKDF2();
        MailService MailService = new MailService();

        public SurveysController(IConfiguration configuration, ILogger<IvrApiController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }

        [HttpPost("SurveyCollectionOfAnswers")]
        public async Task<IActionResult> SurveyCollectionOfAnswers([FromForm] string test)
        {
            return Ok();
        }

       
    }
}
