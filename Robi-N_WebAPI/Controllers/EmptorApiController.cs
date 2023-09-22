using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Utility;
using EmptorUtility;
using RobinCore;
using Microsoft.AspNetCore.Authorization;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EmptorApiController : ControllerBase
    {

        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;



        public EmptorApiController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration configuration, ILogger<IdentityCheckController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }

        [HttpGet("setStandbyRequest")]
        public IActionResult setStandbyRequest(int Id)
        {
            EmptorDbAction db = new EmptorDbAction(_configuration);

            return Ok(db.getEmptorTicketId(Id));
        }
    }
}
