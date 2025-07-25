using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace BaiTapFPT.Helpers
{
    public static class ExcelDataHelper
    {
        public static IEnumerable<object[]> ReadExcelData(string filePath, string sheetName)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var workbook = new XSSFWorkbook(fs);
                var sheet = workbook.GetSheet(sheetName);
                int rowCount = sheet.LastRowNum;

                for (int i = 1; i <= rowCount; i++) 
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;

                    string username = row.GetCell(0)?.ToString();
                    string password = row.GetCell(1)?.ToString();

                    yield return new object[] { username, password };
                }
            }
        }
    }
}
