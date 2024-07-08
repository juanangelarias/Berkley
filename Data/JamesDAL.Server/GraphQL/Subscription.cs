using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using James.Shared;

namespace James.Data.Server.GraphQL
{
    public partial class Subscription
    {
        [Subscribe(With = nameof(SubscribeToOnAddressModifiedAsync))]
        [Topic(nameof(OnAddressModified))]
        public Address OnAddressModified([ID]Guid addressId, [EventMessage] Address address, CancellationToken cancellationToken) => address;

        public async ValueTask<ISourceStream<Address>> SubscribeToOnAddressModifiedAsync(
            Guid addressId, [Service] ITopicEventReceiver eventReceiver, CancellationToken cancellationToken) =>
            await eventReceiver.SubscribeAsync<Address>("OnAddressModified_" + addressId, cancellationToken);

        [Subscribe]
        [Topic(nameof(Subscription.OnLicenseModified))]
        public AgencyLicense OnLicenseModified([EventMessage] AgencyLicense license) => license;
    }

    public static class SubscriptionExtensions
    {
        public static IAsyncEnumerable<T> ToConvertOutput<T, TSource>(this IAsyncEnumerable<TSource> source)
        {
            return new AsyncEnumerableConversion<T, TSource>(source).ConvertedResult();
        }
    }

    public class AsyncEnumerableConversion<T, TSource>(IAsyncEnumerable<TSource> source)
    {
        public async IAsyncEnumerable<T> ConvertedResult()
        {
            await foreach (var item in source)
                yield return (T)ThisToThat.ToEntityType(item, typeof(T));
        }
    }
}
