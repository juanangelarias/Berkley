using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace James.Shared.Model;
/// <summary>
/// Contains search results and the functionality to add them safely from background threads
/// </summary>
public class JamesSearchResults 
{
    private List<JamesSearchResult> _results = [];
    private string _rootSearchTerm = "";
    private string _searchTerm = "";

    /// <summary>
    /// Prevents performance problems from too many results.  If exceeded, the lowest confidence results will be dropped.
    /// </summary>
    public const int MaximumSearchResults = 30;

    public List<JamesSearchResult> ResultItems
    {
        get => _results;
        set => _results = value;
    }

    [JsonRequired]
    public string RootSearchTerm
    {
        get => _rootSearchTerm;
        set
        {
            _rootSearchTerm = value;
            if (null! == _searchTerm || !_searchTerm.Contains(_rootSearchTerm, StringComparison.CurrentCultureIgnoreCase))
                _searchTerm = _rootSearchTerm;
        }
    }

    [JsonIgnore]
    public string SearchTerm
    {
        get => _searchTerm;
        set
        {
            if (!_searchTerm.Contains(_rootSearchTerm, StringComparison.CurrentCultureIgnoreCase))
                throw new ArgumentException("SearchTerm must contain RootSearchTerm.");
            _searchTerm = value;
        }
    }

    public IEnumerable<JamesSearchResult> Results => SearchTerm == RootSearchTerm
        ? _results
        : _results.Where(sr => sr.SearchString.Contains(SearchTerm, StringComparison.CurrentCultureIgnoreCase));

    public IEnumerable<JamesSearchResult> OrderedResults =>
        Results.OrderBy(r => r.Type).ThenBy(r => r.Name).ThenByDescending(r => r.Confidence);

    public void AddRange(IEnumerable<JamesSearchResult> newResults)
    {
        foreach (var jamesSearchResult in newResults)
            AddWithoutSorting(jamesSearchResult);
        _results.Sort(SearchResultComparer.Instance);
        LimitResult();
    }

    public JamesSearchResult Add(JamesSearchResult newResult)
    {
        var result = AddWithoutSorting(newResult);
        _results.Sort(SearchResultComparer.Instance);
        LimitResult();
        return result;
    }

    private void LimitResult()
    {
        while (_results.Count > MaximumSearchResults)
            _results.RemoveRange(MaximumSearchResults, _results.Count - MaximumSearchResults);
    }
    private JamesSearchResult AddWithoutSorting(JamesSearchResult newResult)
    {
        lock (this)
        {
            var existing = _results.FirstOrDefault(newResult.Equals);
            if (existing == null)
            {
                //TODO: Insert in order to obviate sorting
                _results.Add(newResult);
                return newResult;
            }

            //Merge any changed data into the existing result
            Debug.Assert(existing != null, nameof(existing) + " != null");
            if (existing.Confidence < newResult.Confidence)
            {
                existing.Entity = newResult.Entity;
                existing.AccountNum = newResult.AccountNum;
                existing.AgencyNumber = newResult.AgencyNumber;
            }
            //If the same item is found twice, just keep the higher confidence
            existing.Confidence = Math.Max(existing.Confidence, newResult.Confidence);
            //If there are matching FromDescription = true and FromDescription = false records, only use the FromDescription = false
            //This will prevent duplicates if something has its own name in its own description.
            existing.FromDescription = existing.FromDescription && newResult.FromDescription;
            //Use most up-to-date BondList
            if ((existing.BondList?.Count ?? 0) < (newResult.BondList?.Count ?? 0))
                existing.BondList = newResult.BondList;
            return existing;
        }
    }

    public int Count => _results.Count;
}

public class SearchResultComparer : IComparer<JamesSearchResult>
{
    private SearchResultComparer() { }

    private static SearchResultComparer? _instance;
    public static SearchResultComparer Instance => _instance ??= new SearchResultComparer();
    public int Compare(JamesSearchResult? x, JamesSearchResult? y)
    {
        if (null == x && null == y) return 0;
        if (null == x) return -1;
        if (null == y) return 1;
        if (x.Confidence.CompareTo(y.Confidence) != 0)
            return -x.Confidence.CompareTo(y.Confidence);
        if (x.Type.CompareTo(y.Type) != 0)
            return x.Type.CompareTo(y.Type);
        if (x.Name.CompareTo(y.Name) != 0)
            return x.Name.CompareTo(y.Name);
        return x.Entity.Id.CompareTo(y.Entity.Id);
    }
}

public struct SearchOptions
{
    public bool Account { get; set; }
    public bool ActiveOnly { get; set; }
    public bool Agency { get; set; }
    public bool Agent { get; set; }
    public bool Bond { get; set; }
    public bool Obligee { get; set; }
    public bool People { get; set; }
    public bool PersonalFinancials { get; set; }
    public bool VirtualFile { get; set; }
}
