namespace James.Shared.Export;

public class ExcelColumn
{
    public int Number { get; set; }
    public string Title { get; set; } = string.Empty;
    public ExcelColumnType Type { get; set; }
}