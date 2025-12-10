using James.Shared.Model;
using Radzen;

namespace JamesWebUI.Client.Model;

public class GridSettings
{
    public ExportFormat DefaultExportFormat { get; set; }
    public DataGridSettings? Settings { get; set; } = new();
}