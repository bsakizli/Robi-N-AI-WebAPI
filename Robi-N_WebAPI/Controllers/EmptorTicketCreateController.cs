using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Robi_N_WebAPI.Model.Request;
using Robi_N_WebAPI.Model.Response;
using Robi_N_WebAPI.Services;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmptorTicketCreateController : ControllerBase
    {
       
        CallService callService = new CallService();

        // GET: api/<EmptorTicketCreateController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<EmptorTicketCreateController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        public static string ClearTurkishCharacter(string _dirtyText)
        {
            var text = _dirtyText;
            var unaccentedText = String.Join("", text.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark));
            return unaccentedText.Replace("ı", "i");
        }

        
        //// POST api/<EmptorTicketCreateController>
        //[HttpPost]
        //public IActionResult Post(RequestEmptorTicketCreate req)
        //{
        //    ResponseEmptorTicketCreate response = new ResponseEmptorTicketCreate();
        //    try
        //    {
        //        Random random = new Random();
        //        string TicketId = "25200" + random.Next(1111111, 9999999).ToString();

        //        if (!String.IsNullOrEmpty(TicketId))
        //        {
        //            //İlgili Kişi Mesaj Hazırla
        //            string customerMessage = ClearTurkishCharacter(String.Format(@"Sayin {0} {1}, Bilisim Destek Hizmetleri sistemlerinde {3} lokasyonu için {2} numarali talebiniz olusturulmus olup ilgili birime yonlendirilmistir. Kaydinizin durumunu https://bdh.link/durum adresinden sorgulayabilirsiniz. Bizlere 7/24 saat 0216 500 17 17 telefon numarasindan ulasabilirsiniz.", req.fistName.ToUpper(), req.lastName.ToUpper(), TicketId, req.companyName.ToUpper()));

        //            var _smsResult = smsApi.SendSMS(req.gsmPhone,customerMessage);

        //            //bool _smsStatus = _smsResult.Contains("errno=0");

        //            if(_smsResult)
        //            {
        //                RequestRegistrationInformationCallSingleNumber reqCall = new RequestRegistrationInformationCallSingleNumber
        //                {
        //                    phone = "5324107091"
        //                    //phone = "5071310019"
        //                };
        //                var _callResult = callService.AutoCallServiceSinglePhone(reqCall);
        //                if (_callResult.header.code == 200)
        //                {
                            
        //                    response = new ResponseEmptorTicketCreate
        //                    {
        //                        code = 200,
        //                        message = "Kayıt oluşturuldu.",
        //                        status = true,
        //                        ticket = new TicketDetails
        //                        {
        //                            id = TicketId
        //                        }
        //                    };

        //                    return Ok(response);
        //                } else
        //                {
        //                    response = new ResponseEmptorTicketCreate
        //                    {
        //                        code = 404,
        //                        message = "Arama servislerinde sorun var.",
        //                        status= false
        //                    };
        //                    return BadRequest(response);
        //                }

                       
        //            } else
        //            {
        //                response = new ResponseEmptorTicketCreate
        //                {
        //                    code = 404,
        //                    message = "SMS gönderilemedi",
        //                    status = false
        //                };
        //                return BadRequest(response);
        //            }
        //        } else
        //        {
        //            response = new ResponseEmptorTicketCreate
        //            {
        //                code = 404,
        //                message = "Ticket Oluşturulamadı",
        //                status = false
        //            };
        //            return BadRequest(response);
        //        }

        //    } catch
        //    {
        //        response = new ResponseEmptorTicketCreate
        //        {
        //            code = 500,
        //            message = "Genel Sistem Hatası",
        //            status = false
        //        };
        //        return BadRequest(response);
        //    }
        //}

        // PUT api/<EmptorTicketCreateController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<EmptorTicketCreateController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
