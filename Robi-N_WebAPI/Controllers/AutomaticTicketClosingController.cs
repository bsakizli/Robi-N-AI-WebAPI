using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Utility;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api")]
    public class AutomaticTicketClosingController :ControllerBase
    {
        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;




        public AutomaticTicketClosingController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration configuration, ILogger<IdentityCheckController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }


        [HttpGet("AutomaticTicketClosingList")]
        public async Task<IActionResult> AutomaticTicketClosingList()
        {

         

            return Ok("");
        }

        [HttpPost("AutomaticTicketClosing/{jobId}")]
        public async Task<IActionResult> AutomaticTicketClosing(int jobId)
        {
            return Ok("");
        }

        [HttpGet("AutomaticTicketClosing/{jobId}/ticketList")]
        public async Task<IActionResult> AutomaticTicketClosingTicketList(int jobId)
        {
            return Ok("");
        }

        [HttpPut("AutomaticTicketClosing/{jobId}")]
        public async Task<IActionResult> UpdateAutomaticTicketClosing(int jobId)
        {
            return Ok("");
        }

        [HttpDelete("AutomaticTicketClosing/{jobId}")]
        public async Task<IActionResult> DeleteAutomaticTicketClosing(int jobId)
        {
            return Ok("");
        }
    }
}
