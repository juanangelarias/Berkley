using System;
using James.Shared.Server;

namespace James.Data.Server;

public class ServerSideSubscription<T> : IObservable<T>, IDisposable
{
    public ServerSideSubscription(IAsyncEnumerable<T> source, CancellationToken cancellationToken)
    {
        _source = source;
        if (null != cancellationToken)
            source.WithCancellation(cancellationToken);
        _publishTask = WaitAndPublish();
    }

    public static ServerLoggingService LoggingService { get; set; }
    private readonly Task _publishTask;
    private readonly IAsyncEnumerable<T> _source;

    private async Task WaitAndPublish()
    {
        await foreach (T message in _source)
        {
            try
            {
                foreach (var observer in _observers)
                    observer.OnNext(message);
            }
            catch (Exception e)
            {
                foreach (var observer in _observers)
                    observer.OnError(e);
            }
        }

    }
    
    private readonly List<IObserver<T>> _observers = new();
    public IDisposable Subscribe(IObserver<T> observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);

        return new Unsubscriber(_observers, observer);
    }
    private class Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer) : IDisposable
    {
        public void Dispose()
        {
            observers?.Remove(observer);
        }
    }

    public void Dispose()
    {
        foreach (var observer in _observers)
            observer.OnCompleted();
    }
}

public class ServerSideSubscriptionSubscriber<T>(Action<T> onNext, Action? onError = null, Action? onComplete = null) : IObserver<T>
{
 
    public void OnCompleted()
    {
        onComplete?.Invoke();
    }

    public void OnError(Exception error)
    {
        onError?.Invoke();
    }

    public void OnNext(T value)
    {
        onNext(value);
    }
}