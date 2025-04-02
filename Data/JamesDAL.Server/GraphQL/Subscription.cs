using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types.Relay;
using James.Shared;

namespace James.Data.Server.GraphQL
{
    public partial class Subscription
    {
        [Subscribe(With = nameof(SubscribeToOnAddressModifiedAsync))]
        [Topic(nameof(OnAddressModified))]
        public SubscriptionResult<Address> OnAddressModified([ID] Guid addressId, [EventMessage] SubscriptionResult<Address> addressResult, CancellationToken cancellationToken) => addressResult;

        public async ValueTask<ISourceStream<SubscriptionResult<Address>>> SubscribeToOnAddressModifiedAsync(
            Guid addressId, [Service] ITopicEventReceiver eventReceiver, CancellationToken cancellationToken) =>
            await eventReceiver.SubscribeAsync<SubscriptionResult<Address>>("OnAddressModified_" + addressId, cancellationToken);

        [Subscribe(With = nameof(SubscribeToOnAddressCollectionModifiedAsync))]
        [Topic(nameof(OnAddressCollectionModified))]
        public SubscriptionResult<Guid> OnAddressCollectionModified([ID] Guid legalEntityId, [EventMessage] SubscriptionResult<Guid>  result, CancellationToken cancellationToken) => result;

        public async ValueTask<ISourceStream<SubscriptionResult<Guid>>> SubscribeToOnAddressCollectionModifiedAsync(
            Guid legalEntityId, [Service] ITopicEventReceiver eventReceiver, CancellationToken cancellationToken) =>
            await eventReceiver.SubscribeAsync<SubscriptionResult<Guid>>("OnAddressCollectionModified_" + legalEntityId, cancellationToken);

        [Subscribe]
        [Topic(nameof(OnLicenseModified))]
        public AgencyLicense OnLicenseModified([EventMessage] AgencyLicense license) => license;//TODO: Implement

        [Subscribe(With = nameof(SubscribeToSearchResultReadyAsync))]
        [Topic(nameof(OnSearchResultReady))]
        public SubscriptionResult<List<JamesSearchResult>> OnSearchResultReady(string searchTerm,
            [EventMessage] SubscriptionResult<List<JamesSearchResult>> results, CancellationToken cancellationToken) => results;
        public async ValueTask<ISourceStream<SubscriptionResult<List<JamesSearchResult>>>> SubscribeToSearchResultReadyAsync(
            string searchTerm, [Service] ITopicEventReceiver eventReceiver, CancellationToken cancellationToken) =>
            await eventReceiver.SubscribeAsync<SubscriptionResult<List<JamesSearchResult>>>("Srch_" + searchTerm, cancellationToken);

    }

    public static class SubscriptionExtensions
    {
        public static IAsyncEnumerable<T> ToConvertOutput<T, TSource>(this IAsyncEnumerable<TSource> source) where T : new()
        {
            return new AsyncEnumerableConversion<T, TSource>(source).ConvertedResult();
        }
    }

    public class AsyncEnumerableConversion<T, TSource>(IAsyncEnumerable<TSource> source) where T : new()
    {
        public async IAsyncEnumerable<T> ConvertedResult()
        {
            await foreach (var item in source)
                yield return ThisToThat.ToEntityType<T>(item);
        }
    }
}
