namespace James.Shared.Data;

public class LoadItem
{
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
    /// Argumentless lambda expression or function that returns the result variable.
    /// </summary>
    /// <remarks>This must be a function because the result variable will not be set until the AsyncLoadTask has run.
    /// </remarks>
    /// <returns>Returns a reference to the IDataAccessResult base class, ISaveDataResult, that is non-generic and only cares about success and errors</returns>
    public required Func<IDataAccessResult> ResultVariable { get; init; }

    /// <summary>
    /// Time until cached result is considered stale
    /// </summary>
    /// <remarks>Everything should be cached, at minimum, for a minute to protect from multiple queries.
    /// When subscriptions are implemented, the cache can be held much longer.</remarks>
    public TimeSpan CacheDuration { get; set; }= TimeSpan.FromHours(1);

    public event EventHandler Loaded;

    internal void LoadErrorsEncountered(string[] errors, bool fatal) =>
        LoadError?.Invoke(this, new LoadErrorEventArgs(){Errors = errors, Fatal = fatal});

    internal void FireLoaded()
    {
        Loaded?.Invoke(this, EventArgs.Empty);
    }
    
    public event EventHandler<LoadErrorEventArgs> LoadError;
}

public class CachedResult
{
    private object? _data;
    public object? Data
    {
        get => _data;
        set
        {
            _data = value;
            CachedTime = DateTime.Now;
        }
    }
    public DateTime CachedTime { get; private set; } = DateTime.Now;
}

public class LoadErrorEventArgs : EventArgs
{
    public required string[] Errors { get; set; }
    public bool Fatal { get; set; }
}