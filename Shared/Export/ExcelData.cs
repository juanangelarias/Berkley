namespace James.Shared.Export;

public class ExcelData
{
    public List<ExcelColumn> Columns { get; set; } = [];
    public List<ExcelRow> Rows{ get; set; } = [];
}