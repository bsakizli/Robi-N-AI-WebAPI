using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robi_N_WebAPI.Services;
using Robi_N_WebAPI.Shecles;
using Robi_N_WebAPI.Utility;

namespace Robi_N_WebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SgkViziteController : ControllerBase
    {

        private readonly AIServiceDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SgkViziteController> _logger;



        public SgkViziteController(AIServiceDbContext db, IConfiguration configuration, ILogger<SgkViziteController> logger)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
        }


        [HttpPost("confirmReports")]
        public async Task<ActionResult> confirmReports()
        {

            ViziteService viziteService = new ViziteService(_db);
            await viziteService.RaporSorgulaOnay();

            return Ok(true);
        }
    }
}
