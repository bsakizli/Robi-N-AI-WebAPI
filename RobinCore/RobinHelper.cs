using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using RobinCore.Models;
using System.Data;



namespace RobinCore
{
    public class RobinHelper
    {
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
                imageArray = System.IO.File.ReadAllBytes(String.Format(@"\\192.168.110.100\HR_Content\Persone_Resimler\{0}.jpg", fileName));
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
            List<startedList> _personelListesi = new List<startedList>();

            foreach (DataRow row in table.Rows)
            {
                string _fullname = String.Empty;

                if (!String.IsNullOrEmpty(row[2].ToString()))
                {
                    _fullname = row[1] + " " + row[2] + " " + row[3];
                }
                else
                {
                    _fullname = row[1] + " " + row[3];
                }
                string? _image = String.Empty;
                string? _registryNumber = row[0].ToString();
                string? _departmentName = row[6].ToString();
                string? _section = row[5].ToString();
                string? _previousJob = row[9].ToString();
                string? _education = row[10].ToString();


                var byteImage = getImagePath(_registryNumber);

                if (byteImage != null)
                {
                    _image = Convert.ToBase64String(byteImage);
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
                    image = "data:image/jpeg;base64," + _image
                };
                _personelListesi.Add(personel);

            }

            return _personelListesi;


        }
    }
}