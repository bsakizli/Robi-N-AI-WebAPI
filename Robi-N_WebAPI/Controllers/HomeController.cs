using Google.Api.Gax.Grpc.Rest;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Mvc;
using Robi_N_WebAPI.Model;
using System.Diagnostics;
using Image = Google.Cloud.Vision.V1.Image;

namespace Robi_N_WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            //var client = new ImageAnnotatorClientBuilder
            //{
            //    GrpcAdapter = RestGrpcAdapter.Default
            //}.Build();

           

            return View();
        }

        public IActionResult Privacy()
        {
           


            //var httpContext = context.GetHttpContext();

            //// Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return httpContext.User.Identity?.IsAuthenticated ?? false;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult baris()
        {
            return View();
        }
    }
}
