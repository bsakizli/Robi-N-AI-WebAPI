
using Microsoft.AspNetCore.Mvc;

using Robi_N_WebAPI.Utility;
using Microsoft.Extensions.Options;
using Robi_N_WebAPI.Model;
using DocumentFormat.OpenXml.Packaging;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrAppApiController : ControllerBase
    {

        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;


        public HrAppApiController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration configuration, ILogger<IdentityCheckController> logger, AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }

        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string? value = cell.CellValue.InnerXml;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }

        [HttpGet("hrHiredEmployeesSendMail")]
        public IActionResult hrHiredEmployeesSendMail()
        {
            var table = new DataTable();
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(@"C:\Users\baris.sakizli\Desktop\2023.xlsx", false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();
                foreach (Cell cell in rows.ElementAt(0))
                {
                    table.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                }

                foreach (Row row in rows)
                {
                    DataRow tempRow = table.NewRow();
                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        tempRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                    }
                    table.Rows.Add(tempRow);
                }
            }
            table.Rows.RemoveAt(0);

            foreach (DataRow row in table.Rows)
            {
                string _fullname = String.Empty;

                if (!String.IsNullOrEmpty(row[2].ToString()))
                {
                    _fullname = row[1] +" " + row[2] +" " + row[3];
                } else
                {
                    _fullname = row[1] + " " + row[2];
                }

                string? _registryNumber = row[0].ToString();
                string? _departmentName = row[6].ToString();
                string? _previousJob = row[9].ToString();
                string? _education = row[10].ToString();
            }


            return Ok("asdasd");
        }

    }
}
