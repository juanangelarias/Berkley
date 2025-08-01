using System.Text.Json;
using System.Text.Json.Serialization;

namespace James.Shared.Data;

public class LoadItem : IDisposable
{
    /// <summary>
    /// Unique key that describes the dataset to load
    /// </summary>
    /// <remarks>For caching to work across pages, the same key must be uses across all pages.</remarks>
    public required string Key { get; set; }

    /// <summary>
    /// Argumentless lambda expression or function that sets external IDataAccessResult variables
    /// </summary>
    public required Func<Task> AsyncLoadTask { get; init; }

    /// <summary>
    /// Action to take on the loaded data when loaded from Cache
    /// </summary>
    /// <remarks>Data is retrieved as an object and needs to be copied to the result variable as a strongly typed object.</remarks>
    public required Action<object?> CacheLoadTask { get; init; }

    /// <summary>
    /// Action to take on the loaded data when loaded from Cache
    /// </summary>
    /// <remarks>Data is retrieved as an object and needs to be copied to the result variable as a strongly typed object.</remarks>
    //[Obsolete]
    public Func<Task<object?>>? LocalStorageCacheLoadTask { get; init; }

    /// <summary>
    /// Set as false to prevent any caching in browser local storage
    /// </summary>
    public bool UseBrowserStorageIfAvailable { get; init; } = true;

    /// <summary>
    /// Argumentless lambda expression or function that returns the result variable.
    /// </summary>
    /// <remarks>This must be a function because the result variable will not be set until the AsyncLoadTask has run.
    /// </remarks>
    /// <returns>Returns a reference to the IDataAccessResult base class that is non-generic</returns>
    public required Func<IDataAccessResult> ResultVariable { get; init; }

    /// <summary>
    /// Optional reference to a subscription that keeps the datastore updated
    /// </summary>
    /// <remarks>should only be set in the AddSubscriptionTask</remarks>
    public IDisposable? Subscription { get; set; }
    public Action<LoadItem>? AddSubscriptionTask { get; set; }

    /// <summary>
    /// Time until cached result is considered stale
    /// </summary>
    /// <remarks>Everything should be cached, at minimum, for a minute to protect from multiple queries.
    /// When subscriptions are implemented, the cache can be held much longer.</remarks>
    public TimeSpan CacheDuration { get; set; } = TimeSpan.FromHours(1);

    /// <summary>
    /// Action taken after the data has loaded either from the source or from the cache
    /// </summary>
    public Action? AfterLoad { get; init; }

    public event EventHandler Loaded;//TODO:Review if this is needed, or is the after load Action all that is needed

    internal void FireLoaded()
    {
        Loaded?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<LoadErrorEventArgs> LoadError;

    internal void LoadErrorsEncountered(string[] errors, bool fatal) =>
        LoadError?.Invoke(this, new LoadErrorEventArgs() { Errors = errors, Fatal = fatal });

    public void Dispose()
    {
        Subscription?.Dispose();
    }
}

public class CachedResult
{
    public object? DataObject { get; set; }

    public required DateTime CacheUntil { get; init; } = DateTime.Now;
}

public class CachedResult<T> : CachedResult
{
    private static JsonSerializerOptions ignoreCycles = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false
    };
    public T Data
    {
        get
        {
            //When a cached value is pulled from browser local storage, the DataObject 
            //  does not always get properly converted to the proper type.
            if (DataObject is JsonElement je)
                try
                {
                    //DataObject = JsonSerializer.Deserialize<T>(je.ToString());
                    DataObject = je.Deserialize<T>(ignoreCycles);
                }
                catch (Exception ex)
                {
                    var msg = $"Exception deserializing cache from local storage.  Type: {typeof(T).Name}";
                    throw new Exception(msg, ex);
                }
            return (T)DataObject!;
        }

        set => DataObject = value;
    }
}

public class LoadErrorEventArgs : EventArgs
{
    public required string[] Errors { get; set; }
    public bool Fatal { get; set; }
}