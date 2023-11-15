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
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Nancy.Json;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer;
using Robi_N_WebAPI.Model.Xml.Response;
using Nancy.Diagnostics;
using Newtonsoft.Json;
using Robi_N_WebAPI.Model.Service.Response;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using Robi_N_WebAPI.Services;
using Microsoft.Win32;
using Sprache;
using Org.BouncyCastle.Math.EC;
using System.Net.Mime;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class VoiceIVRApplicationController : ControllerBase
    {
        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;
        private readonly IConfiguration _configuration;

        PBKDF2 crypto = new PBKDF2();

        public VoiceIVRApplicationController(IConfiguration configuration,ILogger<IdentityCheckController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }

        [HttpGet("getHolidayDayCheckNowCsq")]
        public async Task<IActionResult> getHolidayDayCheckNowCsq(string csq)
        {
            GlobalResponse globalResponse;
            try
            {

                var _holidays = await _db.RBN_IVR_HOLIDAY_DAYS.FirstOrDefaultAsync(x => x.startDate.Date == DateTime.Now.Date && x.csq == csq);

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

        [HttpGet("getHolidayDayCheckDateCsq")]
        public async Task<IActionResult> getHolidayDayCheckDateCsq(DateTime dateTime, string csq)
        {
            GlobalResponse globalResponse;

            try
            {
                var _holidays = await _db.RBN_IVR_HOLIDAY_DAYS.FirstOrDefaultAsync(x => x.startDate.Date == dateTime.Date && x.csq == csq);

                //Holiday Start Time Control
                var _date = dateTime.Date;
                var _time = dateTime.TimeOfDay.Ticks;

                if (_holidays != null)
                {
                    if (_time != 0)
                    {
                        if (dateTime > _holidays.holidayDate)
                        {
                            globalResponse = new GlobalResponse
                            {
                                statusCode = 201,
                                status = true,
                                message = String.Format("Today is a holiday. - {0} - {1}", _holidays.displayName, _holidays.description)
                            };
                            var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                            _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                            return BadRequest(globalResponse);
                        }
                        else
                        {
                            globalResponse = new GlobalResponse
                            {
                                statusCode = 200,
                                status = false,
                                message = "You are in working hours."
                            };
                            var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                            _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                            return Ok(globalResponse);
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
                        var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                        return BadRequest(globalResponse);
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
                    var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                    _logger.LogInformation(String.Format(@"{0} - Response: {1}", this.ControllerContext.RouteData.Values["controller"].ToString(), globalResponseResult));
                    return Ok(globalResponse);
                }

            }
            catch
            {
                globalResponse = new GlobalResponse
                {
                    statusCode = 500,
                    status = false,
                    message = "System error please inform your administrator."
                };
                var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                _logger.LogInformation(String.Format(@"{0} - Response: {1}", this.ControllerContext.RouteData.Values["controller"].ToString(), globalResponseResult));
                return BadRequest(globalResponse);
            }
        }
        
        [HttpGet("getCSQList")]
        public async Task<IActionResult> listCSQ()
        {
            responseListCSQ response = new responseListCSQ();

            try
            {
                UCCXWebService webService = new UCCXWebService(_configuration);
                
                //var _csqs = await Task.Run(() => webService.getCSQList().Result);

                var _csqs = await webService.getCSQList();
                if (_csqs.Csq.Count > 0)
                {
                    List<responseListCSQ.csq> _csqList = new List<responseListCSQ.csq>();
                    foreach (var item in _csqs.Csq)
                    {
                        responseListCSQ.csq csq = new responseListCSQ.csq
                        {
                            name = item.Name
                        };
                        _csqList.Add(csq);
                    }

                    response = new responseListCSQ
                    {
                        status = true,
                        statusCode = 200,
                        message = "CSQ List achievement.",
                        csqs = _csqList
                    };


                    var _responseText = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                    return Ok(response);
                }
                else
                {
                    response = new responseListCSQ
                    {
                        status = false,
                        statusCode = 201,
                        message = "CSQ Defined"
                    };
                    var _responseText = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                response = new responseListCSQ
                {
                    status = false,
                    statusCode = 500,
                    message = String.Format(@"CSQ Listing system error. - Exception: {0}", ex.Message.ToString())
                };
                var _responseText = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                return BadRequest(response);
            }
        }


        [HttpGet("getHolidayDayCheckDate")]
        public async Task<IActionResult> getHolidayDayCheckDate(DateTime dateTime)
        {
            GlobalResponse globalResponse;

            try
            {
                var _holidays = await _db.RBN_IVR_HOLIDAY_DAYS.FirstOrDefaultAsync(x => x.startDate.Date == dateTime.Date);

                //Holiday Start Time Control
                var _date = dateTime.Date;
                var _time = dateTime.TimeOfDay.Ticks;

                if (_holidays != null)
                {
                    if (_time != 0)
                    {
                        if (dateTime > _holidays.holidayDate)
                        {
                            globalResponse = new GlobalResponse
                            {
                                statusCode = 201,
                                status = true,
                                message = String.Format("Today is a holiday. - {0} - {1}", _holidays.displayName, _holidays.description)
                            };
                            var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                            _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                            return BadRequest(globalResponse);
                        }
                        else
                        {
                            globalResponse = new GlobalResponse
                            {
                                statusCode = 200,
                                status = false,
                                message = "You are in working hours."
                            };
                            var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                            _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                            return Ok(globalResponse);
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
                        var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                        return BadRequest(globalResponse);
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
                    var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                    _logger.LogInformation(String.Format(@"{0} - Response: {1}", this.ControllerContext.RouteData.Values["controller"].ToString(), globalResponseResult));
                    return BadRequest(globalResponse);
                }

            } catch
            {
                globalResponse = new GlobalResponse
                {
                    statusCode = 500,
                    status = false,
                    message = "System error please inform your administrator."
                };
                var globalResponseResult = new JavaScriptSerializer().Serialize(globalResponse);
                _logger.LogInformation(String.Format(@"{0} - Response: {1}", this.ControllerContext.RouteData.Values["controller"].ToString(), globalResponseResult));
                return BadRequest(globalResponse);
            }
        }


        [HttpGet("getHolidayDayCheckNow")]
        public async Task<IActionResult> getHolidayDayCheckNow()
        {
            GlobalResponse globalResponse;
            try
            {

                var _holidays = await _db.RBN_IVR_HOLIDAY_DAYS.FirstOrDefaultAsync(x => x.startDate.Date == DateTime.Now.Date);

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

            } catch(Exception ex)
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


        [HttpGet("getholidayDayList")]
        public async Task<IActionResult> getholidayDayList()
        {
            getholidayDayList response;
            try
            {
                var _holidays = await _db.RBN_IVR_HOLIDAY_DAYS.ToListAsync();
                response = new responseVoiceIVRApplication.getholidayDayList
                {
                    statusCode = 200,
                    message = "The listing was done successfully.",
                    status = true,
                    data = _holidays

                };

                var textResult = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));

                return Ok(response);

            } catch(Exception e)
            {
                response = new responseVoiceIVRApplication.getholidayDayList
                {
                    statusCode = 404,
                    message = String.Format("The listing was done successfully. - Exception: {0}", e.Message),
                    status = false,
                };
                var textResult = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), textResult));

                return BadRequest(response);
            }
            
        }

        [HttpPost("getGoogleHolidayDaysUpdate")]
        public async Task<IActionResult> getGoogleHolidayDaysUpdate(string csq)
        {
            Root _getGoogleHolidays = await Helper.Helper.getGoogleTurkeyHolidays();

            foreach (var item in _getGoogleHolidays.items)
            {
                var _getHoliday = await _db.RBN_IVR_HOLIDAY_DAYS.Where(x => x.startDate == Convert.ToDateTime(item.start.date) && x.csq == csq).FirstOrDefaultAsync();

                if (_getHoliday == null)
                {

                    DateTime _startDate = Convert.ToDateTime(item.start.date);

                    var Record = _db.Set<RBN_IVR_HOLIDAY_DAYS>();
                    //_db.RBN_VOICE_SOUNDS.AddObject(Record);
                    Record.Add(new RBN_IVR_HOLIDAY_DAYS
                    {
                        csq = csq,
                        active = true,
                        addDate = DateTime.Now,
                        description = item.description,
                        holidayDate = Convert.ToDateTime(item.start.date),
                        displayName = item.summary,
                        endDate = Convert.ToDateTime(item.end.date),
                        holidayName = item.summary,
                        years = Convert.ToInt32(_startDate.Year),
                        startDate = Convert.ToDateTime(item.start.date)
                    });
                    await _db.SaveChangesAsync();
                }

            }

            return Ok("tamam");
        }

        [HttpPost("addHolidayDay")]
        public async Task<IActionResult> addHolidayDay(newHolidayDay item)
        {
            getholidayDay response;

            try
            {
                var payLoadResult = new JavaScriptSerializer().Serialize(item);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - PayloadData: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), payLoadResult));

                var _holiday = await _db.RBN_IVR_HOLIDAY_DAYS.Where(x => x.holidayDate == item.startDate).FirstOrDefaultAsync();
                if (_holiday == null)
                {


                    var _record = new RBN_IVR_HOLIDAY_DAYS()
                    {
                        active = true,
                        csq = item.csq,
                        addDate = DateTime.Now,
                        description = item.description,
                        holidayDate = Convert.ToDateTime(item.startDate),
                        displayName = item.displayName,
                        endDate = Convert.ToDateTime(item.endDate),
                        holidayName = item.displayName,
                        startDate = Convert.ToDateTime(item.startDate)
                    };
                    var lastRecord = _db.RBN_IVR_HOLIDAY_DAYS.Add(_record);
                    await _db.SaveChangesAsync();

                    if (lastRecord != null)
                    {
                        response = new getholidayDay
                        {
                            status = true,
                            statusCode = 200,
                            message = "The definition of holiday has been successfully made.",
                            data = _record

                        };
                        var _responseText = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                        return Ok(response);
                    }
                    else
                    {
                        response = new getholidayDay
                        {
                            status = false,
                            statusCode = 202,
                            message = "There was a problem with the definition."
                        };
                        var _responseText = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText)); return BadRequest(response);
                    }

                }
                else
                {
                    response = new getholidayDay
                    {
                        status = false,
                        statusCode = 201,
                        message = "Such a holiday has been defined."
                    };
                    var _responseText = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText)); return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response = new getholidayDay
                {
                    status = false,
                    statusCode = 404,
                    message = String.Format("There was a problem with the definition. - Exception: {0}", ex.Message)
                };
                var _responseText = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText)); return BadRequest(response);
            }
        }


        [HttpPut("updateHolidayDay/{id}")]
        //[FromBody]
        //newHolidayDay item
        public async Task<IActionResult> updateHolidayDay(int id, [FromBody] newHolidayDay item)
        {
            getholidayDay response;
            try
            {
                var payLoadResult = new JavaScriptSerializer().Serialize(item);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - PayloadData: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), payLoadResult));

                //var _item = await _db.RBN_IVR_HOLIDAY_DAYS.Where(x=>x.Id == id && x.Id == null).FirstOrDefaultAsync();

                //RBN_IVR_HOLIDAY_DAYS data = new RBN_IVR_HOLIDAY_DAYS
                //{
                //    Id = id,
                //    description = item.description,
                //    displayName = item.displayName,
                //    active = item.active,
                //    endDate = item.endDate,
                //    startDate = item.startDate,
                //    updateDate = DateTime.Now,
                //    holidayDate = item.startDate,
                //    years = item.years,
                //};
                //_db.Update(data);

                var _item = await _db.Set<RBN_IVR_HOLIDAY_DAYS>().FirstOrDefaultAsync(x => x.Id == id);


                if (_item != null) {

                    _item.description = item.description;
                    _item.holidayName = item.holidayName;
                    _item.displayName = item.displayName;
                    _item.active = item.active;
                    _item.endDate = item.endDate;
                    _item.startDate = item.startDate;
                    _item.holidayDate = item.startDate;
                    _item.updateDate = DateTime.Now;
                    _item.years = item.years;

                    if (await _db.SaveChangesAsync() == 1)
                    {
                        response = new getholidayDay
                        {
                            status = true,
                            statusCode = 200,
                            message = "The update has been successfully implemented.",
                            data = _item
                        };
                        var _responseText = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));

                        return Ok(response);
                    }
                    else
                    {
                        response = new getholidayDay
                        {
                            status = true,
                            statusCode = 201,
                            message = "No changes were detected.",
                            data = _item
                        };
                        var _responseText = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                        return BadRequest(response);
                    }
                } else
                {
                    response = new getholidayDay
                    {
                        status = true,
                        statusCode = 199,
                        message = "No record",
                        data = _item
                    };
                    var _responseText = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
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
                var _responseText = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));

                return BadRequest(response);
            }

        }

        [HttpDelete("deleteHolidayDay/{id}")]
        public async Task<IActionResult> deleteHolidayDay(int id)
        {
            getholidayDay response;

            try
            {
                var payLoadResult = new JavaScriptSerializer().Serialize(id);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - PayloadData: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), payLoadResult));

                var record = await _db.RBN_IVR_HOLIDAY_DAYS.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (record != null)
                {
                    if (await _db.RBN_IVR_HOLIDAY_DAYS.Where(x => x.Id == id).ExecuteDeleteAsync() == 1)
                    {
                        response = new getholidayDay
                        {
                            status = true,
                            statusCode = 200,
                            message = "The definition of holiday has been deleted.",
                            data = record
                        };
                        var _responseText = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                       
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
                        var _responseText = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
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
                    var _responseText = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
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
                var _responseText = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
                return BadRequest(response);
            }
        }

        //[HttpPost("addlog")]
        //public async Task<IActionResult> addlog(requestAddLog _request)
        //{
        //    GlobalResponse response;

        //    try
        //    {
        //        var payLoadResult = new JavaScriptSerializer().Serialize(_request);
        //        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - PayloadData: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), payLoadResult));

        //        if (!String.IsNullOrEmpty(_request.uniqId) && !String.IsNullOrEmpty(_request.log))
        //        {

        //            var _record = new RBN_IVR_LOGS()
        //            {
        //                active = true,
        //                logKey = _request.logKey,
        //                log = _request.log,
        //                uniqId = _request.uniqId,
        //                addDate = DateTime.Now,
        //            };
        //            var lastRecord = _db.RBN_IVR_LOGS.Add(_record);
        //            await _db.SaveChangesAsync();
        //            if (lastRecord != null)
        //            {
        //                response = new GlobalResponse
        //                {
        //                    status = true,
        //                    statusCode = 200,
        //                    displayMessage = "Log kaydı yapılmıştır.",
        //                    message = "Successful"
        //                };
        //                return Ok(response);
        //            }
        //            else
        //            {
        //                response = new GlobalResponse
        //                {
        //                    status = false,
        //                    statusCode = 201,
        //                    displayMessage = "Log kaydı yapılamadı.",
        //                    message = "Unsuccessful"
        //                };
        //            }
        //        }
        //        else
        //        {
        //            response = new GlobalResponse
        //            {
        //                status = false,
        //                statusCode = 202,
        //                displayMessage = "Log parametrelerini kontrol ediniz.",
        //                message = "Unsuccessful"
        //            };
        //        }

        //        var _responseText = new JavaScriptSerializer().Serialize(response);
        //        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
        //        return BadRequest(response);

        //    }
        //    catch (Exception ex)
        //    {
        //        response = new GlobalResponse
        //        {
        //            status = false,
        //            statusCode = 202,
        //            displayMessage = $"Log parametrelerini kontrol ediniz. - Error {ex.Message}",
        //            message = "Unsuccessful"
        //        };
        //        var _responseText = new JavaScriptSerializer().Serialize(response);
        //        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), _responseText));
        //        return BadRequest(response);
        //    }
        //}

    }
}
