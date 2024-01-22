
using FastReport.Export.PdfSimple;
using FastReport.Web;
using Google.Api.Gax.Grpc.Rest;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Utility;
using System.Data;
using System.Diagnostics;
using Image = Google.Cloud.Vision.V1.Image;

namespace Robi_N_WebAPI.Controllers
{

    public class HomeModel
    {
        public WebReport WebReport { get; set; }
        public string[] ReportsList { get; set; }
    }



    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly AIServiceDbContext _db;
        public HomeController(AIServiceDbContext db, IWebHostEnvironment appEnvironment, ILogger<HomeController> logger)
        {
            _db = db;
            _appEnvironment = appEnvironment;
            _logger = logger;
        }



        //public async Task<IActionResult> Index()
        //{

        //    WebReport WebReport = new WebReport(); // Create a Web Report Object
          
        //    WebReport.Width = "1000"; // Set the width of the report
        //    WebReport.Height = "1000"; // Set the height of the report
        //    string webRootPath = _appEnvironment.WebRootPath; // Get the path to wwwroot folder
        //                                                          //string contentRootPath = _hostingEnvironment.ContentRootPath;

        //    WebReport.Report.Load(webRootPath + "/reports/Simple List.frx"); // Load the report into a WebReport object
        //    System.Data.DataSet dataSet = new System.Data.DataSet(); // Create a data source
        //    dataSet.ReadXml(webRootPath + "/reports/nwind.xml"); // Open the xml database

        //    var _reports = await _db.RBN_SGK_HealthReports.Where(x => x.BildirimId != null).ToListAsync();

        //    WebReport.Report.RegisterData(_reports, "NorthWind"); //Register the data source in the report

        //    WebReport.Mode = WebReportMode.Designer; // Set the mode of the web report object - display of the designer
        //    WebReport.DesignerPath = "/WebReportDesigner/index.html"; // Specify the URL of the online designer
        //    WebReport.DesignerSaveCallBack = "/Home/SaveDesignedReport"; // Set the view URL for the report save method

        //    ViewBag.WebReport = WebReport; // pass report to View
        //    return View();

        //}

        //[HttpPost]
        //public ActionResult SaveDesignedReport(string reportID, string reportUUID)
        //{
        //    string webRootPath = _appEnvironment.WebRootPath; // Get the path to wwwroot folder ViewBag.Message = String.Format("Confirmed {0} {1}", reportID, reportUUID); // Set a message for view

        //    Stream reportForSave = Request.Body; // Write the result of the Post query to the stream
        //    string pathToSave = webRootPath + "/DesignedReports/TestReport.frx"; // get the path to save the file
        //    using (FileStream file = new FileStream(pathToSave, FileMode.Create)) // Create a file stream 
        //    {
        //        reportForSave.CopyToAsync(file).Wait(); // Save the result of the query to a file
        //    }
        //    return View();
        //}



        //public IActionResult getPDF()
        //{
        //    string webRootPath = _appEnvironment.WebRootPath; // Get the path to the wwwroot folder
        //    WebReport webReport = new WebReport(); // Create a Web Report Object
        //    webReport.Report.Load(webRootPath + "/reports/Master-Detail.frx"); // Load the report into the WebReport object
        //    var dataSet = new DataSet(); // Create a data source
        //    dataSet.ReadXml(webRootPath + "/reports/nwind.xml"); // Open the xml database
        //    webReport.Report.RegisterData(dataSet, "NorthWind"); // Register the data source in the report


        //    webReport.Report.Prepare();

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        PDFSimpleExport pdfExport = new PDFSimpleExport();
        //        pdfExport.Export(webReport.Report, ms);
        //        ms.Flush();
        //        return File(ms.ToArray(), "application/pdf", Path.GetFileNameWithoutExtension("Master-Detail") + ".pdf");
        //    }
        //}

       
       

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
