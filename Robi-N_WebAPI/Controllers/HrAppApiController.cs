


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

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class HrAppApiController : ControllerBase
    {

        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
     

        public HrAppApiController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration configuration, ILogger<IdentityCheckController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _logger = logger;
            _db = db;
         
        }

        RobinHelper _robin = new RobinHelper();
      
        [HttpGet("hrHiredEmployeesSendMail")]
        public async Task<IActionResult> hrHiredEmployeesSendMail()
        {
            var baseUri = $"{Request.Scheme}://{Request.Host}";
            try
            {
                GlobalResponse _response = new GlobalResponse();
                if (await _robin.getMailTemplate(baseUri))
                {
                    _response = new GlobalResponse
                    {
                        status = true,
                        statusCode = 200,
                        message = "Mail has been sent."
                    };

                    return Ok(_response);
                }
                else
                {
                    _response = new GlobalResponse
                    {
                        status = false,
                        statusCode = 201,
                        message = "Failed to send mail."
                    };

                    return BadRequest(_response);
                }


            } catch(Exception ex)
            {
                GlobalResponse _response = new GlobalResponse();
                _response = new GlobalResponse
                {
                    status = false,
                    statusCode = 500,
                    message = String.Format(@"Server error please inform the administrator. - Message: {0}", ex.Message.ToString())
                };

                return BadRequest(_response);
            }
        }

        [HttpGet("personelTest")]
        public async Task<IActionResult> personelTest(long tc)
        {
            PersonelService personelService = new PersonelService();

            bool test = personelService.CalisanSorgulama(tc);

            return Ok(test);
        }

    }
}
