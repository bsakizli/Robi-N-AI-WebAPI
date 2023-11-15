using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nancy.Json;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Utility;


namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,IVR Read Only Web Service,IVR Full Authorization")]
    [ApiController]
    public class CustomerIVRAppController : ControllerBase
    {
        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;
        private readonly IConfiguration _configuration;

        public CustomerIVRAppController(IConfiguration configuration, ILogger<IdentityCheckController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }


        [HttpGet("getHolidayDayCheckNow")]
        public IActionResult getHolidayDayCheckNow()
        {
            GlobalResponse globalResponse;
            try
            {

                var _holidays = _db.RBN_IVR_HOLIDAY_DAYS.FirstOrDefault(x => x.startDate.Date == DateTime.Now.Date);

                //Holiday Start Time Control
                var _date = DateTime.Now.Date;
                var _time = DateTime.Now.TimeOfDay.Ticks;

                if (_holidays != null)
                {
                    if (_time != 0)
                    {
                        if (DateTime.Now > _holidays.holidayDate)
                        {
                            globalResponse = new GlobalResponse
                            {
                                statusCode = 201,
                                status = true,
                                message = String.Format("Today is a holiday. - {0} - {1}", _holidays.displayName, _holidays.description)
                            };
                        }
                        else
                        {
                            globalResponse = new GlobalResponse
                            {
                                statusCode = 200,
                                status = false,
                                message = "You are in working hours."
                            };
                        }
                    }
                    else
                    {
                        globalResponse = new GlobalResponse
                        {
                            statusCode = 201,
                            status = true,
                            message = String.Format("Today is a holiday. - {0} - {1}", _holidays.displayName, _holidays.description)
                        };
                    }

                }
                else
                {
                    globalResponse = new GlobalResponse
                    {
                        statusCode = 200,
                        status = false,
                        message = "You are in working hours."
                    };
                }
                var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                return Ok(globalResponse);

            }
            catch (Exception ex)
            {
                globalResponse = new GlobalResponse
                {
                    statusCode = 500,
                    status = false,
                    message = "System error please inform your administrator."
                };
                var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                return BadRequest(globalResponse);
            }
        }
    }
}
