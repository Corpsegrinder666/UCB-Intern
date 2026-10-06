using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CIBnext.LIB
{
    public static class ExcelReportHelper
    {
        public static void SetTitleRow(ExcelWorksheet ws, int row, string text, int endCol, int fontSize, bool bold)
        {
            ws.Cells[row, 1].Value = text;
            ws.Cells[row, 1, row, endCol].Merge = true;
            ws.Cells[row, 1, row, endCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[row, 1, row, endCol].Style.Font.Size = fontSize;
            ws.Cells[row, 1, row, endCol].Style.Font.Bold = bold;
        }

        public static void SetBorder(ExcelRange rng)
        {
            rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        }
    }
}
