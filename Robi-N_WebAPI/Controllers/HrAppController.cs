using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Utility;
using System.Data;
using RobinCore;




namespace Robi_N_WebAPI.Controllers
{
    public class HrAppController : Controller
    {
        private readonly AIServiceDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
     

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewlyHiredEmployees()
        {
            RobinHelper robin = new RobinHelper();
            return View(robin.GetExcelFile());
        }
    }
}
