using IronBarCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Drawing;
using System;
using Robi_N_WebAPI.Helper;
using Robi_N_WebAPI.Model.Response;
using System.IO.Pipelines;
using SixLabors.ImageSharp;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;
using Robi_N_WebAPI.Utility;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "MIS User")]
    [ApiController]
    public class AIServiceFormController : ControllerBase
    {
        private readonly AIServiceDbContext _db;
        private readonly IConfiguration _configuration;

        public AIServiceFormController(AIServiceDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }   


        [HttpPost]
        public async Task<ActionResult> FormQRCodeReading(IFormFile ServiceFormPicture)
        {
            ResponseFormQRCodeReading response;
            try
            {
                IronBarCode.License.LicenseKey = _configuration.GetValue<string>("IronBarCode.LicenseKey");

                IList<string> fileExtensionsToAllowed = new List<string> { ".jpg", ".png", ".jpeg" };
                var uploadedFileExtension = Path.GetExtension(ServiceFormPicture.FileName).ToLower();
                if (!fileExtensionsToAllowed.Contains(uploadedFileExtension))
                {
                    response = new ResponseFormQRCodeReading
                    {
                        status = true,
                        message = String.Format(@"Service form image extension is not valid. Accepted Extensions: {0}", String.Join(", ", fileExtensionsToAllowed.ToArray())),
                        statusCode = 201
                    };
                    return BadRequest(response);
                } else
                {
                    var bytes = await Helper.Helper.GetBytes(ServiceFormPicture);
                    //var PhotoResult = BarcodeReader.ReadASingleBarcode(@"C:\Users\baris.sakizli\Desktop\New folder (5)\IMG_5490.jpeg", BarcodeEncoding.QRCode, BarcodeReader.BarcodeRotationCorrection.Medium, BarcodeReader.BarcodeImageCorrection.LightlyCleanPixels);
                    //var resultFromFile = BarcodeReader.Read(@"C:\Users\baris.sakizli\Desktop\New folder (5)\IMG_5490.jpeg"); // From a file

                    BarcodeReaderOptions myOptions = new BarcodeReaderOptions()
                    {
                        
                        // Choose a speed from: Faster, Balanced, Detailed, ExtremeDetail
                        // There is a tradeoff in performance as more Detail is set
                        Speed = ReadingSpeed.ExtremeDetail,

                        // Reader will stop scanning once a barcode is found, unless set to true
                        ExpectMultipleBarcodes = true,

                        // By default, all barcode formats are scanned for.
                        // Specifying one or more, performance will increase.
                        ExpectBarcodeTypes = BarcodeEncoding.QRCode,
                        
                        // Utilizes multiple threads to reads barcodes from multiple images in parallel.
                        Multithreaded = true,

                        // Maximum threads for parallel. Default is 4
                        MaxParallelThreads = 4,

                        // The area of each image frame in which to scan for barcodes.
                        // Will improve perfornace significantly and avoid unwanted results and avoid noisy parts of the image.
                        CropArea = new System.Drawing.Rectangle(),

                        // Special Setting for Code39 Barcodes.
                        // If a Code39 barcode is detected. Try to use extended mode for the full ASCII Character Set
                        UseCode39ExtendedMode = true
                    };


                    var resultFromStream = BarcodeReader.Read(bytes, myOptions);

                    var results  = resultFromStream.ToList();
                    if (results.Count > 0)
                    {
                        var _readResult = results.ToList()[0];

                        response = new ResponseFormQRCodeReading
                        {
                            status = true,
                            message = "QR Reading Completed",
                            statusCode = 200,
                            result = _readResult.ToString()
                        };
                    } else
                    {
                        response = new ResponseFormQRCodeReading
                        {
                            status = true,
                            message = "The QR Code on the service form could not be read, the quality of the service form is not good enough or the QR Code was not found. Provide the checks and try again.",
                            statusCode = 202
                        };
                        return BadRequest(response);
                    }
                }
                
            } catch(Exception ex)
            {
                response = new ResponseFormQRCodeReading
                {
                    status = false,
                    statusCode= 500,
                    message = ex.Message
                };
            }
            return Ok(response);
        }
       


    }
}
