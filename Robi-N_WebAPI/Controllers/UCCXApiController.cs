using MailEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetGsmAPI;
using Robi_N_WebAPI.Schedule;
using Robi_N_WebAPI.Utility;
using Robi_N_WebAPI.Utility.Tables;
using RobinCore;
using UCCXSoapService;
using WhatsAppBusinessAPI;
using WhatsAppBusinessAPI.Model;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UCCXApiController : ControllerBase
    {
        MailService mailService = new MailService();

    
        private readonly AIServiceDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<UCCXApiController> _logger;
        private HR_IAppSettings _appConfig;


        public UCCXApiController(AIServiceDbContext db, IWebHostEnvironment appEnvironment, IConfiguration configuration, ILogger<UCCXApiController> logger, HR_IAppSettings appConfig)
        {
            _appEnvironment = appEnvironment;
            _logger = logger;
            _db = db;
            _configuration = configuration;
            _appConfig = appConfig;
        }

        WhatsAppBusinessClient wpClient = new WhatsAppBusinessClient();
        NetGsmService _smsService = new NetGsmService();


        [HttpPost("MissedCallSendMessage")]
        public async Task<ActionResult> MissedCallSendMessage()
        {
            MissedCallsMessages missedCallsMessages = new MissedCallsMessages(_db);
            missedCallsMessages.MissedCallMessageService();


            return Ok(true);

        }
    }
}
