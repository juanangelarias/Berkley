namespace James.Shared.Model;

public class SubscriptionResult<T>
{
    public required string Identifier { get; set; }
    public required T Result { get; set; }
}