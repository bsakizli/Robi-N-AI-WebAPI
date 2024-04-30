using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.Excel;
using EmptorUtility;
using Hangfire.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Office.Interop.Excel;
using Nancy.Json;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Model.Response;
using Robi_N_WebAPI.Utility;
using Robi_N_WebAPI.Utility.Tables;
using static Robi_N_WebAPI.Model.Request.requestVoiceIVRApplication;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api")]
	[Authorize]
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
        public async Task<IActionResult> getAutomaticTicketClosingList()
        {
			_logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), null));

			responseAutomaticTicketClosingList _response;
            try
            {

                List<RBN_EMPTOR_AUTOTICKETCLOSEDScheduler> jobs = await _db.RBN_EMPTOR_AUTOTICKETCLOSEDScheduler.ToListAsync();
                if (jobs != null)
                {
                    _response = new responseAutomaticTicketClosingList
                    {
                        status = true,
                        statusCode = 200,
                        displayMessage = "Görevler listelenmiştir.",
                        message = "Successfully",
                        data = jobs
                    };
					var globalResponseResult = new JavaScriptSerializer().Serialize(_response);
					_logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
					return Ok(_response);
                } else
                {
                    _response = new responseAutomaticTicketClosingList
                    {
                        status = false,
                        statusCode = 400,
                        message = "Unsuccessfully",
                        displayMessage = "Tanımlı bir görev bulunamadı!"
					};
                    return BadRequest(_response);
                }

            } catch (Exception ex)
            {
                _response = new responseAutomaticTicketClosingList
                {
                    status = false,
                    displayMessage = ex.Message,
                    message = "Unsuccessfully",
                    statusCode = 500
                };
                return BadRequest(_response);
            }
        }

        [HttpGet("AutomaticTicketClosing/{id}")]
        public async Task<IActionResult> AutomaticTicketClosing(int id)
        {
            responseSingleAutomaticTicketClosingList _response;

            try
            {
				var job = await _db.RBN_EMPTOR_AUTOTICKETCLOSEDScheduler.Where(x => x.Id == id).FirstOrDefaultAsync();
                
                if (job !=null)
                {


                    _response = new responseSingleAutomaticTicketClosingList
                    {
						status = true,
						statusCode = 200,
						displayMessage = "Görevler listelenmiştir.",
						message = "Successfully",
						data = job
					};

                    return Ok(_response);

                } else
                {
                    _response = new responseSingleAutomaticTicketClosingList
                    {
                        status = false,
                        displayMessage = "Gönderilen JobId'ye göre bir görev bulunamadı.",
                        message = "Unsuccessfully",
                        statusCode = 404
                    };
                    return BadRequest(_response);
                }


			} catch (Exception ex)
            {
				_response = new responseSingleAutomaticTicketClosingList
				{
					status = false,
					displayMessage = ex.Message,
					message = "Unsuccessfully",
					statusCode = 500
				};
				return BadRequest(_response);
			}
        }

        [HttpGet("AutomaticTicketClosing/{id}/ticketList")]
        public async Task<IActionResult> AutomaticTicketClosingTicketList(int id)
        {
            responseAutomaticTicketList _response;
            try
            {
                var job = await _db.RBN_EMPTOR_AUTOTICKETCLOSEDScheduler.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (job != null)
                {
                    if(!String.IsNullOrEmpty(job.ticketQuery))
                    {
						EmptorDbAction emptorDbAction = new EmptorDbAction(_configuration);
						List<long> tickets = await emptorDbAction.getTicketClosedList(job.ticketQuery);
                        //Emptor
                        _response = new responseAutomaticTicketList
                        {
                            status = true,
                            statusCode = 200,
                            displayMessage = "Kapatılacak kayıtlar listelenmiştir.",
                            message = "Successfully",
                            data = tickets
						};
                        return Ok(_response);
					} else
                    {
						_response = new responseAutomaticTicketList
						{
							status = false,
							displayMessage = "Gönderilen JobId'ye emptor kuralı yok",
							message = "Unsuccessfully",
							statusCode = 202
						};
						return BadRequest(_response);
					}
				}
				else
                {
					_response = new responseAutomaticTicketList
					{
						status = false,
						displayMessage = "Gönderilen JobId'ye göre bir görev bulunamadı.",
						message = "Unsuccessfully",
						statusCode = 404
					};
					return BadRequest(_response);
				}

            } catch(Exception ex)
            {
				_response = new responseAutomaticTicketList
				{
					status = false,
					displayMessage = ex.Message,
					message = "Unsuccessfully",
					statusCode = 500
				};
				return BadRequest(_response);
			}
           
        }

		[HttpPost("AutomaticTicketClosing")]
		public async Task<IActionResult> addAutomaticTicketClosingList([FromBody] RBN_EMPTOR_AUTOTICKETCLOSEDScheduler data)
		{
			responseSingleAutomaticTicketClosingList _response;

			try
			{
				if (data != null)
				{
					var _record = new RBN_EMPTOR_AUTOTICKETCLOSEDScheduler()
					{
						active = true,
						cron = data.cron,
						lastStartDate = data.lastStartDate,
						name = data.name,
						process = 1,
						registerDate = DateTime.UtcNow,
						ticketQuery = data.ticketQuery
					};
					var lastRecord = _db.RBN_EMPTOR_AUTOTICKETCLOSEDScheduler.Add(_record);
		
					if(await _db.SaveChangesAsync() == 1)
					{
						
						_response = new responseSingleAutomaticTicketClosingList
						{
							data = _record,
							status = true,
							displayMessage = "Görev eklenmiştir.",
							message = "Successfully",
							statusCode = 200,
						};
						return Ok(_response);
					}
					else
					{
						_response = new responseSingleAutomaticTicketClosingList
						{
							status = false,
							displayMessage = "Görev eklenemedi!",
							message = "Unsuccessfully",
							statusCode = 500
						};
						return BadRequest(_response);
					}

				} else
				{
					_response = new responseSingleAutomaticTicketClosingList
					{
						status = false,
						displayMessage = "Payload Hatalı",
						message = "Unsuccessfully",
						statusCode = 404
					};
					return BadRequest(_response);
				};

				

			} catch(Exception ex)
			{
				_response = new responseSingleAutomaticTicketClosingList
				{
					status = false,
					displayMessage = ex.Message,
					message = "Unsuccessfully",
					statusCode = 500
				};
				return BadRequest(_response);
			}
		}

		[HttpPut("AutomaticTicketClosing/{id}")]
        public async Task<IActionResult> UpdateAutomaticTicketClosing(int id, [FromBody] RBN_EMPTOR_AUTOTICKETCLOSEDScheduler data)
        {
            responseSingleAutomaticTicketClosingList _response;
            try
            {
                var job = await _db.RBN_EMPTOR_AUTOTICKETCLOSEDScheduler.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (job != null)
                {
                    job = data;
                    if (await _db.SaveChangesAsync() == 1)
                    {
						_response = new responseSingleAutomaticTicketClosingList
						{
							status = false,
							displayMessage = "Görev güncellemesi sırasında bir sorun oluştu.",
							message = "Unsuccessfully",
							statusCode = 201,
                            data = job
						};
						return Ok(_response);

					} else
                    {
						_response = new responseSingleAutomaticTicketClosingList
						{
							status = false,
							displayMessage = "Görev güncellemesi sırasında bir sorun oluştu.",
							message = "Unsuccessfully",
							statusCode = 201
						};
						return BadRequest(_response);
					}

				} else
                {
					_response = new responseSingleAutomaticTicketClosingList
					{
						status = false,
						displayMessage = "Gönderilen JobId'ye emptor kuralı yok",
						message = "Unsuccessfully",
						statusCode = 202
					};
					return BadRequest(_response);
				}

            } catch (Exception ex)
            {
				_response = new responseSingleAutomaticTicketClosingList
				{
					status = false,
					displayMessage = ex.Message,
					message = "Unsuccessfully",
					statusCode = 500
				};
				return BadRequest(_response);
			}
        }

        [HttpDelete("AutomaticTicketClosing/{id}")]
        public async Task<IActionResult> DeleteAutomaticTicketClosing(int id)
		{
			responseSingleAutomaticTicketClosingList _response;

			try
            {
                var job = await _db.RBN_EMPTOR_AUTOTICKETCLOSEDScheduler.Where(x => x.Id == id).FirstOrDefaultAsync();
				if (job != null)
				{
					if (await _db.RBN_EMPTOR_AUTOTICKETCLOSEDScheduler.Where(x => x.Id == id).ExecuteDeleteAsync() == 1)
                    {
						_response = new responseSingleAutomaticTicketClosingList
						{
							status = false,
							displayMessage = "Görev Silindi!",
							message = "Unsuccessfully",
							statusCode = 200,
							data = job
						};
						return Ok(_response);

					} else
                    {
						_response = new responseSingleAutomaticTicketClosingList
						{
							status = false,
							displayMessage = "Görev silinemedi!",
							message = "Unsuccessfully",
							statusCode = 404
						};
						return BadRequest(_response);
					}

				} else
                {
					_response = new responseSingleAutomaticTicketClosingList
					{
						status = false,
						displayMessage = "Gönderilen ID'ye göre bir görev bulunamadı!",
						message = "Unsuccessfully",
						statusCode = 201
					};
					return BadRequest(_response);
				}
            } catch(Exception ex)
            {
				_response = new responseSingleAutomaticTicketClosingList
				{
					status = false,
					displayMessage = ex.Message,
					message = "Unsuccessfully",
					statusCode = 500
				};
				return BadRequest(_response);
			}
        }
    }
}
