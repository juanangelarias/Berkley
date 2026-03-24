namespace James.Shared.Dto;

/// <summary>
/// Represents a lookup entity with an identifier, code, and name.
/// </summary>
/// <remarks>
/// This class provides a Label property which dynamically formats the
/// display text based on the values of Code and Name properties.
/// The Label follows the pattern "(Code) Name", unless the Code is empty
/// or already prefixed in the Name.
/// </remarks>
public class JamesLookup
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";

    public string Label => string.IsNullOrEmpty(Code)
        ? Name
        : Name.StartsWith($"({Code})")
            ? Name
            : $"({Code}) {Name}";
}