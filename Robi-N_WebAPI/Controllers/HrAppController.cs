using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Utility;
using System.Data;
using RobinCore;
using System.Net;
using Microsoft.AspNetCore.Http;
using Robi_N_WebAPI.Services;

namespace Robi_N_WebAPI.Controllers
{
    public class HrAppController : Controller
    {

        private readonly AIServiceDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;


        RobinHelper _robin =  new RobinHelper();


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult newlyHiredEmployees()
        {
           
            return View(_robin.GetExcelFile());
        }



        public static string? BaseUrl(HttpRequest req)
        {
            if (req == null) return null;
            var uriBuilder = new UriBuilder(req.Scheme, req.Host.Host, req.Host.Port ?? -1);
            if (uriBuilder.Uri.IsDefaultPort)
            {
                uriBuilder.Port = -1;
            }

            return uriBuilder.Uri.AbsoluteUri;
        }


        public async Task<IActionResult> hrSendMail()
        {

            var baseUri = $"{Request.Scheme}://{Request.Host}";


            Boolean status = await _robin.getMailTemplate(baseUri);

            return RedirectToAction("newlyHiredEmployees", "HrApp");


        }

       

    }
}
