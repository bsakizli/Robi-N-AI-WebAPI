using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Model.Request;
using Robi_N_WebAPI.Utility;
using SimpleCrypto;
using Robi_N_WebAPI.Helper;
using System;
using Robi_N_WebAPI.Utility.Tables;
using Robi_N_WebAPI.Model.Response;
using static Robi_N_WebAPI.Model.Request.requestVoiceIVRApplication;
using static Robi_N_WebAPI.Model.Response.responseVoiceIVRApplication;
using static Robi_N_WebAPI.Model.Response.responseVoiceIVRApplication.GoogleCalender;
using Microsoft.EntityFrameworkCore;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoiceIVRApplicationController : ControllerBase
    {
        private readonly AIServiceDbContext _db;

        PBKDF2 crypto = new PBKDF2();

        public VoiceIVRApplicationController(AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _db = db;
        }

        [HttpPost("getGoogleHolidayDaysUpdate")]
        public IActionResult getGoogleHolidayDaysUpdate()
        {
            var _getGoogleHolidays = Helper.Helper.getGoogleTurkeyHolidays();

            foreach (var item in _getGoogleHolidays.items)
            {
                var _getHoliday = _db.RBN_IVR_HOLIDAY_DAYS.Where(x => x.startDate == Convert.ToDateTime(item.start.date)).FirstOrDefault();

                if (_getHoliday == null)
                {
                    var Record = _db.Set<RBN_IVR_HOLIDAY_DAYS>();
                    Record.Add(new RBN_IVR_HOLIDAY_DAYS
                    {
                        active = true,
                        addDate = DateTime.Now,
                        description = item.description,
                        holidayDate = Convert.ToDateTime(item.start.date),
                        displayName = item.summary,
                        endDate = Convert.ToDateTime(item.end.date),
                        holidayName = item.summary,
                        startDate = Convert.ToDateTime(item.start.date)

                    });
                    _db.SaveChanges();
                }
                
            }

            return Ok("tamam");
        }


        [HttpGet("getholidayDayCheckDate")]
        public IActionResult getholidayDayCheckDate(DateTime dateTime)
        {
            GlobalResponse globalResponse;
            var _holidays = _db.RBN_IVR_HOLIDAY_DAYS.FirstOrDefault(x => x.startDate.Date == dateTime.Date);

            //Holiday Start Time Control
            var _date = dateTime.Date;
            var _time = dateTime.TimeOfDay.Ticks;

            if(_time != 0)
            {
                if (dateTime > _holidays.holidayDate)
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
            } else
            {
                 globalResponse = new GlobalResponse
                {
                    statusCode = 201,
                    status = true,
                    message = String.Format("Today is a holiday. - {0} - {1}", _holidays.displayName, _holidays.description)
                };
            }
            return Ok(globalResponse);
        }


        [HttpGet("getholidayDayCheckNow")]
        public IActionResult getholidayDayCheckNow()
        {
            GlobalResponse globalResponse;
            var _holidays = _db.RBN_IVR_HOLIDAY_DAYS.FirstOrDefault(x => x.startDate.Date == DateTime.Now.Date);

            //Holiday Start Time Control
            var _date = DateTime.Now.Date;
            var _time = DateTime.Now.TimeOfDay.Ticks;

            if(_holidays != null)
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

            } else
            {
                globalResponse = new GlobalResponse
                {
                    statusCode = 200,
                    status = false,
                    message = "You are in working hours."
                };
            }
            
            
            return Ok(globalResponse);
        }


        [HttpGet("getholidayDayList")]
        public IActionResult getholidayDayList()
        {
            responseVoiceIVRApplication.getholidayDayList response;
            try
            {
                var _holidays = _db.RBN_IVR_HOLIDAY_DAYS.ToList();
                response = new responseVoiceIVRApplication.getholidayDayList
                {
                    statusCode = 200,
                    message = "The listing was done successfully.",
                    status = true,
                    data = _holidays

                };
                return Ok(response);

            } catch(Exception e)
            {
                response = new responseVoiceIVRApplication.getholidayDayList
                {
                    statusCode = 404,
                    message = String.Format("The listing was done successfully. - Exception: {0}", e.Message),
                    status = false,
                };
                return BadRequest(response);
            }
            
        }




        [HttpPut("updateHolidayDay/{id}")]
        public IActionResult updateHolidayDay(int id, [FromBody] newHolidayDay item)
        {
            getholidayDay response;

            try
            {
                var _item = _db.RBN_IVR_HOLIDAY_DAYS.First(x => x.Id == id);
                _item.description = item.description;
                _item.displayName = item.displayName;
                _item.endDate = item.endDate;
                _item.startDate = item.startDate;
                _item.updateDate = DateTime.Now;
                _item.holidayDate = item.startDate;
                if(_db.SaveChanges() == 1)
                {
                    response = new getholidayDay
                    {
                        status = true,
                        statusCode = 200,
                        message = "The update has been successfully implemented.",
                        data = _item
                    };
                    return Ok(response);
                } else
                {
                    response = new getholidayDay
                    {
                        status = true,
                        statusCode = 201,
                        message = "No changes were detected.",
                        data = _item
                    };
                    return BadRequest(response);
                }

            } catch(Exception ex)
            {
                response = new getholidayDay
                {
                    status = false,
                    statusCode = 404,
                    message = String.Format("A problem occurred during the update. - Exception: {0}", ex.Message)
                };
                return BadRequest(response);
            }
        }



        [HttpPost("addHolidayDay")]
        public IActionResult addHolidayDay(newHolidayDay item)
        {
            getholidayDay response;

            try
            {
                var _holiday = _db.RBN_IVR_HOLIDAY_DAYS.Where(x => x.holidayDate == item.startDate).FirstOrDefault();
                if (_holiday == null)
                {


                    var _record = new RBN_IVR_HOLIDAY_DAYS()
                    {
                        active = true,
                        addDate = DateTime.Now,
                        description = item.description,
                        holidayDate = Convert.ToDateTime(item.startDate),
                        displayName = item.displayName,
                        endDate = Convert.ToDateTime(item.endDate),
                        holidayName = item.displayName,
                        startDate = Convert.ToDateTime(item.startDate)
                    };
                    var lastRecord = _db.RBN_IVR_HOLIDAY_DAYS.Add(_record);
                    _db.SaveChanges(); 

                    if (lastRecord != null)
                    {
                        response = new getholidayDay
                        {
                            status = true,
                            statusCode = 200,
                            message = "The definition of holiday has been successfully made.",
                            data = _record

                        };
                        return Ok(response);
                    } else
                    {
                        response = new getholidayDay
                        {
                            status = false,
                            statusCode = 202,
                            message = "There was a problem with the definition."
                        };
                        return BadRequest(response);
                    }

                } else
                {
                    response = new getholidayDay
                    {
                        status = false,
                        statusCode = 201,
                        message = "Such a holiday has been defined."
                    };
                    return BadRequest(response);
                }
            }
            catch (Exception ex) {
                response = new getholidayDay
                {
                    status = false,
                    statusCode = 404,
                    message = String.Format("There was a problem with the definition. - Exception: {0}", ex.Message)
                };
                return BadRequest(response);
            }
        }


        [HttpDelete("deleteHolidayDay/{id}")]
        public IActionResult deleteHolidayDay(int id)
        {
            getholidayDay response;

            try
            {
                var record = _db.RBN_IVR_HOLIDAY_DAYS.Where(x => x.Id == id).FirstOrDefault();
             
                if(record != null)
                {
                    if (_db.RBN_IVR_HOLIDAY_DAYS.Where(x => x.Id == id).ExecuteDelete() == 1)
                    {
                        response = new getholidayDay
                        {
                            status = true,
                            statusCode = 200,
                            message = "The definition of holiday has been deleted.",
                            data = record
                        };
                        return Ok(response);
                    } else
                    {
                        response = new getholidayDay
                        {
                            status = false,
                            statusCode = 201,
                            message = "The holiday description could not be deleted, please try again.",
                            data = record
                        };
                        return BadRequest(response);
                    }
                } else
                {
                    response = new getholidayDay
                    {
                        status = false,
                        statusCode = 202,
                        message = "No holiday definition found to delete."
                    };
                    return BadRequest(response);
                }
            } 
            catch (Exception ex)
            {
                response = new getholidayDay
                {
                    status = false,
                    statusCode = 404,
                    message = String.Format("A system error occurred while deleting the holiday description. - Exception: {0}", ex.Message)
                };
                return BadRequest(response);
            }
        }
    }
}
