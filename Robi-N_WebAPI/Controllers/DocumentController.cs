using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robi_N_WebAPI.Model.Response;

namespace Robi_N_WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {


        [HttpGet("getpdfpassword")]
        public async Task<ActionResult> getpdfpassword(DateTime? datetime = null)
        {
            try
            {
                getPdfPasswordResponse _response;

                if (datetime != null)
                {
                    long _password = Convert.ToInt64(await Helper.Helper.PdfGenerateCustomPassword(datetime.Value));
                    if (_password != null && _password > 0)
                    {
                        _response = new getPdfPasswordResponse
                        {
                            status = true,
                            statusCode = 200,
                            displayMessage = "Şifre üretilmiştir.",
                            message = "Successfuly",
                            date = datetime.Value,
                            password = _password
                        };
                        return Ok(_response);

                    }
                    else
                    {
                        _response = new getPdfPasswordResponse
                        {
                            status = true,
                            statusCode = 404,
                            displayMessage = "Şifre hatası, lütfen değeleri kontrol edin ve tekrar deneyiniz.",
                            message = "Unsuccessfuly",
                            date = datetime.Value
                        };
                        return BadRequest(_response);
                    }
                }
                else
                {
                    long _password = Convert.ToInt64(await Helper.Helper.PdfGenerateCustomPassword(DateTime.Now));
                    if (_password != null && _password > 0)
                    {
                        _response = new getPdfPasswordResponse
                        {
                            status = true,
                            statusCode = 200,
                            displayMessage = "Şifre üretilmiştir.",
                            message = "Successfuly",
                            date = DateTime.Now.Date,
                            password = _password
                        };
                        return Ok(_response);

                    }
                    else
                    {
                        _response = new getPdfPasswordResponse
                        {
                            status = true,
                            statusCode = 404,
                            displayMessage = "Şifre hatası, lütfen değeleri kontrol edin ve tekrar deneyiniz.",
                            message = "Unsuccessfuly",
                            date = DateTime.Now.Date
                        };
                        return BadRequest(_response);
                    }
                }


            }
            catch
            {
                getPdfPasswordResponse _response;
                _response = new getPdfPasswordResponse
                {
                    status = true,
                    statusCode = 404,
                    displayMessage = "Şifre hatası, lütfen değeleri kontrol edin ve tekrar deneyiniz.",
                    message = "Unsuccessfuly",
                    date = DateTime.Now.Date
                };
                return BadRequest(_response);

            }

        }
    }
}
