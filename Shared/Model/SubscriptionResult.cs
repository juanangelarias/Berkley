namespace James.Shared.Model;

public class SubscriptionResult<T>
{
    public string Identifier { get; set; }
    public T Result { get; set; }
}