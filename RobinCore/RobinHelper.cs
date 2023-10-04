using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using HtmlAgilityPack;
using RobinCore.Models;
using System.Data;
using System.Net;
using System.Text;
using MailEntity;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;

namespace RobinCore
{
    public class RobinHelper
    {
        MailService mailService = new MailService();


        public static bool GetHtmlText()
        {

            WebRequest req = WebRequest.Create("http://localhost:5027/HrApp/NewlyHiredEmployees");
            WebResponse res = req.GetResponse();
            StreamReader reader = new StreamReader(res.GetResponseStream());
            string result = reader.ReadToEnd();
            reader.Close();
            res.Close();

            return true;
        }
        public string? BaseUrl(HttpRequest req)
        {
            if (req == null) return null;
            var uriBuilder = new UriBuilder(req.Scheme, req.Host.Host, req.Host.Port ?? -1);
            if (uriBuilder.Uri.IsDefaultPort)
            {
                uriBuilder.Port = -1;
            }

            return uriBuilder.Uri.AbsoluteUri;
        }

        public async Task<Boolean> getMailTemplate(string _url)
        {
            try
            {
                Uri url = new Uri(String.Format(@"{0}/HrApp/NewlyHiredEmployees",_url)); //Uri tipinde değişeken linkimizi veriyoruz.

                WebClient client = new WebClient(); // webclient nesnesini kullanıyoruz bağlanmak için.
                client.Encoding = Encoding.UTF8; //türkçe karakter sorunu yapmaması için encoding utf8 yapıyoruz.

                string html = await client.DownloadStringTaskAsync(url);


                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                var divs = document.DocumentNode.SelectNodes(@"//div");
                if (divs != null)
                {
                    foreach (var tag in divs)
                    {
                        if (tag.Attributes["class"] != null && string.Compare(tag.Attributes["class"].Value, "robin-action-button", StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            tag.Remove();
                        }
                        else if (tag.Attributes["id"] != null && string.Compare(tag.Attributes["id"].Value, "robin-action-button", StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            tag.Remove();
                        }
                    }
                }
                string _result = document.DocumentNode.OuterHtml;

                System.Globalization.CultureInfo usEnglish = new System.Globalization.CultureInfo("tr-TR");
                System.Globalization.DateTimeFormatInfo englishInfo = usEnglish.DateTimeFormat;
                string monthName = englishInfo.MonthNames[DateTime.Now.Month - 1];

                string MonthName = DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR"));

                string _subject = String.Format("Aramıza Yeni Katılan Çalışma Arkadaşlarımız.. - {0}", MonthName + " " + DateTime.Now.Year);

                bool status = mailService.SendMailHtml(_result, _subject);

                if(status)
                {
                    return true;
                } else
                {
                    return false;
                }
            } catch
            {
                return false;

            }

            

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


        private byte[] getImagePath(string fileName)
        {
            byte[] imageArray = null;
            try
            {
                imageArray = System.IO.File.ReadAllBytes(String.Format(@"\\192.168.110.100\HR_Content\Personel_Resimler\{0}.jpg", fileName));
                return imageArray;
            }
            catch
            {
                return imageArray;
            }
        }

        public List<startedList> GetExcelFile()
        {
            var table = new DataTable();
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(@"\\192.168.110.100\HR_Content\ExcelDosyalari\Aramiza_Katilanlar.xlsx", false))
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
            List<startedList> _personelListesi = new List<startedList>();

            foreach (DataRow row in table.Rows)
            {
                //string _fullname = String.Empty;

                //if (!String.IsNullOrEmpty(row[2].ToString()))
                //{
                //    _fullname = row[1] + " " + row[2] + " " + row[3];
                //}
                //else
                //{
                //    _fullname = row[1] + " " + row[3];
                //}
                
                string? _image = String.Empty;
                string? _registryNumber = row[0].ToString();
                string? _fullname = row[1].ToString();
                string? _departmentName = row[3].ToString();
                string? _section = row[2].ToString();
                string? _previousJob = row[4].ToString();
                string? _education = row[5].ToString();


                var byteImage = getImagePath(_registryNumber);

                if (byteImage != null)
                {
                    _image = "data:image/jpeg;base64," +Convert.ToBase64String(byteImage);
                }
                else
                {
                    _image = null;
                }

                startedList personel = new startedList
                {
                    fullname = _fullname,
                    education = _education,
                    departmentName = _departmentName,
                    previousJob = _previousJob,
                    section = _section,
                    image =  _image
                };
                _personelListesi.Add(personel);

            }

            return _personelListesi;


        }
    }
}