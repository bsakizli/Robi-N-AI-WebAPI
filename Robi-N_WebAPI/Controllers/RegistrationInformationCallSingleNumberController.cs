using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Robi_N_WebAPI.Model.Request;
using Robi_N_WebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationInformationCallSingleNumber : ControllerBase
    {
        CallService CallApi = new CallService();

        // GET: api/<RegistrationInformationCallSingleNumberControllerv>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RegistrationInformationCallSingleNumberController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RegistrationInformationCallSingleNumberController>
        [HttpPost]
        public IActionResult Post([FromBody] RequestRegistrationInformationCallSingleNumber request)
        {
            try
            {
                var _response = CallApi.AutoCallServiceSinglePhone(request);
                if (_response.header.code == 200)
                {
                    return Ok(_response);
                } else
                {
                    return BadRequest(_response);
                }
            } catch
            {
                return BadRequest("Sistem Hatası");
            }
            
        }

        //// PUT api/<RegistrationInformationCallSingleNumberController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<RegistrationInformationCallSingleNumberController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
