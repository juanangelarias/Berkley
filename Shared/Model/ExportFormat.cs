using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace James.Shared.Model;

public enum ExportFormat
{
    Unknown = 0,
    CSV = 1,
    Excel = 2
}

/// <summary>
/// Class used to make tweaks to the property source and title for exported columns
/// </summary>
public class ExportColumnSubstitution
{
    /// <summary>
    /// Obscure Unicode character that can be used in Titles and then converted back into a space when exporting the file
    /// </summary>
    public const char SpaceSubstitution = '_';
    public ExportColumnSubstitution()
    {
        Original = "";
    }

    public ExportColumnSubstitution(string original, string? property = null, string? title = null)
    {
        Original = original;
        Property = property ?? "";
        Title = title ?? "";
    }

    private readonly string? _title;
    private readonly string? _property;

    /// <summary>
    /// User-friendly title given to the column in the exported file.
    /// </summary>
    /// <remarks>Limited to characters that can be used as variable names in .NET, and the space character</remarks>
    public string Title
    {
        get => _title ?? (string.IsNullOrWhiteSpace(Original) ? Property : Original);
        init => _title = value;
    }

    /// <summary>
    /// Property to use instead of what is in the RadzenDataGridColumn "Property" property
    /// </summary>
    public string Property
    {
        get => _property ?? Original;
        init => _property = value;
    }

    /// <summary>
    /// Either matches what is in the RadzenDataGridColumn "Property" property, or is blank to signify an added column not in the Grid.  The second scenario is primarily for adding data from nested grids.
    /// </summary>
    public string Original { get; init; }
}

public class ExportColumnSubstitutions : IDictionary<string, ExportColumnSubstitution>
{
    private readonly Dictionary<string, ExportColumnSubstitution> _substitutions = new();

    private static int _additionalColumns;
    private string AdditionalColumnId()
    {
        Interlocked.Increment(ref _additionalColumns);
        return $"Addition{ExportColumnSubstitution.SpaceSubstitution}{_additionalColumns:D5}";
    }

    public void AddSubstitution(ExportColumnSubstitution substitution)
    {
        var key = substitution.Original == string.Empty ? AdditionalColumnId() : substitution.Original;
        _substitutions[key] = substitution;
    }
    public void AddSubstitution(string original, string? property = null, string? title = null)
    {
        AddSubstitution(new ExportColumnSubstitution { Original = original, Property = property ?? "", Title = title });
    }

    public void Add(string key, ExportColumnSubstitution value)
    {
        if (key != value.Original) throw new ArgumentException("key must be equal to value.Original");
        _substitutions.Add(key, value);
    }

    public bool ContainsKey(string orignal) => _substitutions.ContainsKey(orignal);
    public bool Remove(string key)
    {
        return _substitutions.Remove(key);
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out ExportColumnSubstitution value)
    {
        return _substitutions.TryGetValue(key, out value);
    }

    public ExportColumnSubstitution this[string original]
    {
        get => _substitutions.GetValueOrDefault(original) ?? new ExportColumnSubstitution(original);
        set
        {
            if (value == null || original != value.Original)
                throw new ArgumentException("key must be equal to value.Original");
            _substitutions[original] = value;
        }
    }

    public ICollection<string> Keys => _substitutions.Keys;
    public ICollection<ExportColumnSubstitution> Values => _substitutions.Values;

    IEnumerator<KeyValuePair<string, ExportColumnSubstitution>> IEnumerable<KeyValuePair<string, ExportColumnSubstitution>>.GetEnumerator() => _substitutions.GetEnumerator();

    public IEnumerator<ExportColumnSubstitution> GetEnumerator() => _substitutions.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(KeyValuePair<string, ExportColumnSubstitution> item) => Add(item.Key, item.Value);

    public void Clear() => _substitutions.Clear();

    public bool Contains(KeyValuePair<string, ExportColumnSubstitution> item) => _substitutions.Contains(item);

    public void CopyTo(KeyValuePair<string, ExportColumnSubstitution>[] array, int arrayIndex) =>
        ((IDictionary<string, ExportColumnSubstitution>)_substitutions).CopyTo(array, arrayIndex);

    public bool Remove(KeyValuePair<string, ExportColumnSubstitution> item) => _substitutions.Remove(item.Key);

    public int Count => _substitutions.Count;
    public bool IsReadOnly => false;
    //public E
}