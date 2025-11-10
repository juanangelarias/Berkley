namespace JamesWebUI.Client.Model;

/// <summary>
/// Represents a lookup object used in the application.
/// </summary>
/// <remarks>
/// The JamesLookup class provides a structure to hold identification and descriptive
/// information, along with a computed label for display purposes.
///
/// Used by AccountAgency.razor and AccountBasicInfo.razor
/// </remarks>
public class JamesLookup
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";

    public string Label => string.IsNullOrEmpty(Code)
        ? Name
        : $"({Code}) {Name}";
}