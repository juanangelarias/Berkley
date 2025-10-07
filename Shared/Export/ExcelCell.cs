namespace James.Shared.Export;

public class ExcelCell
{
    public ExcelColumn Column { get; set; } = null!;
    public int RowNumber { get; set; }
    public string Value { get; set; } = string.Empty;
    public object ValueObject =>
        Column.Type switch
        {
            ExcelColumnType.Date => Value,
            ExcelColumnType.Int => int.Parse(Value),
            ExcelColumnType.Double => double.Parse(Value),
            ExcelColumnType.Boolean => bool.Parse(Value),
            ExcelColumnType.String =>Value,
            _ => Value
        };
    
}