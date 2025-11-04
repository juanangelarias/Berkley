namespace JamesWebUI.Client.Model;

public class JamesLookup
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";

    public string Label => string.IsNullOrEmpty(Code)
        ? Name
        : $"({Code}) {Name}";
}