using System.Collections;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace James.Shared.Model;
/// <summary>
/// Contains search results and the functionality to add them safely from background threads
/// </summary>
public class JamesSearchResults : IEnumerable<JamesSearchResult>
{
    [JsonInclude]
    private List<JamesSearchResult> _results = new();
    private string _rootSearchTerm;
    private string _searchTerm;

    public IEnumerator<JamesSearchResult> GetEnumerator()
    {
        return _results.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _results.GetEnumerator();
    }

    private JamesSearchResult this[int i] => _results[i];

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
            Add(jamesSearchResult);
    }
    public JamesSearchResult Add(JamesSearchResult newResult)
    {
        lock (this)
        {
            var existing = _results.FirstOrDefault(newResult.Equals);
            if (existing == null)
            {
                _results.Add(newResult);
                return newResult;
            }

            //Merge any changed data into the existing result
            Debug.Assert(existing != null, nameof(existing) + " != null");
            //If the same item is found twice, just keep the higher confidence
            existing.Confidence = Math.Max(existing.Confidence, newResult.Confidence);
            //If there are matching FromDescription = true and FromDescription = false records, only use the FromDescription = false
            //This will prevent duplicates if something has its own name in its own description.
            existing.FromDescription = existing.FromDescription && newResult.FromDescription;
            //Use most up-to-date BondList
            existing.BondList = newResult.BondList;
            return existing;
        }
    }

    public int Count => _results.Count;
}