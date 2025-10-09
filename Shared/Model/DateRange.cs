namespace James.Shared.Model;

public class DateRange
{
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string Label =>
        (End == null
            ? Start?.ToString("MM/dd/yyyy")
            : $"{Start?.ToString("MM/dd/yyyy")} - {End?.ToString("MM/dd/yyyy")}")
        ?? "";
}