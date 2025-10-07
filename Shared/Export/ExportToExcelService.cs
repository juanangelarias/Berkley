using ClosedXML.Excel;

namespace James.Shared.Export;

public interface IExportToExcelService
{
    MemoryStream ExportToExcel(ExcelData data, string fileName);
}

public class ExportToExcelService : IExportToExcelService
{
    public MemoryStream ExportToExcel(ExcelData data, string fileName = "export.xlsx")
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Related Parties");
        
        foreach (var col in data.Columns)
        {
            ws.Cell(1,col.Number).Value= col.Title;            
        }

        var r = 1;
        foreach (var row in data.Rows)
        {
            r += 1;
            foreach (var cell in row.Cells)
            {
                ws.Cell(r, cell.Column.Number).Value = cell.Column.Type switch
                {
                    ExcelColumnType.Date => (DateTime)cell.ValueObject,
                    ExcelColumnType.Boolean => (bool)cell.ValueObject,
                    ExcelColumnType.Double => (double)cell.ValueObject,
                    ExcelColumnType.Int => (int)cell.ValueObject,
                    ExcelColumnType.String => (string)cell.ValueObject,
                    _ => cell.ValueObject.ToString()
                };
            }
        }

        var stream = new MemoryStream();
        wb.SaveAs(stream);
        stream.Position = 0;
        
        return stream;
    }
}