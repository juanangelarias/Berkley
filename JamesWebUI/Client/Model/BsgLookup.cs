namespace JamesWebUI.Client.Model;

public class BsgLookup
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Label => $"({Code}) {Name}";
}