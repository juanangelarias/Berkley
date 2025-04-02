using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using James.Shared;
using James.Shared.Data;
using Microsoft.IdentityModel.Tokens;

namespace James.Data.Server.GraphQL.Mutations
{
    [MutationType]
    public class GeneralMutation
    {
        [Authorize]
        public async Task<bool> CreateAddress(Guid addressId, string address1, string? address2,
            string? address3, string city, string? stateCode, string? postalCode,
            Guid legalEntityId, string addressType, string identifier,
            [Service] ITopicEventSender eventSender, 
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            try
            {
                Address NewAddress = new Address()
                {
                    Id = addressId,
                    Address1 = address1,
                    Address2 = address2,
                    Address3 = address3,
                    City = city,
                    StateCode = stateCode,
                    PostalCode = postalCode
                };

                var ctx = await contextFactory.CreateDbContextAsync();

                ctx.Addresses.Add(NewAddress);

                var newLEAddress = new LegalEntityAddress
                {
                    LegalEntityId = legalEntityId,
                    AddressId = addressId,
                    Type = addressType
                };

                ctx.LegalEntityAddresses.Add(newLEAddress);
                await ctx.SaveChangesAsync();
                eventSender.SendAsync($"{nameof(Subscription.OnAddressCollectionModified)}_{legalEntityId}",
                    new SubscriptionResult<string>() { Identifier = identifier, Result = addressId.ToString() });

                return true;
            }
            catch (Exception ex)
            {

                loggingService.LogException(ex, "Exception saving created address to database", category:StandardLoggingCategories.DataAccess);
                return false;
            }
        }

        [Authorize]
        public async Task<bool> DeleteAddress(Guid addressId, string identifier,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();

                var address = await ctx.Addresses.SingleOrDefaultAsync(x => x.Id == addressId);
                var leAddress = await ctx.LegalEntityAddresses.SingleOrDefaultAsync(x => x.AddressId == addressId);

                if (leAddress != null)
                {
                    ctx.LegalEntityAddresses.Remove(leAddress);
                }
                if (address != null)
                {
                    ctx.Addresses.Remove(address);
                }

                await ctx.SaveChangesAsync();
                eventSender.SendAsync($"{nameof(Subscription.OnAddressCollectionModified)}_{leAddress.LegalEntityId}",
                    new SubscriptionResult<string>() { Identifier = identifier, Result = identifier });
                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception deleting address to database", category: StandardLoggingCategories.DataAccess);
                return false;
            }
        }

        [Authorize]
        public async Task<Address> SetAddress(Guid addressId, string address1, string? address2, 
            string? address3, string city, string? stateCode, string? postalCode, string identifier,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var oldAddress = await ctx.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId)
                ;

            if (oldAddress == null)
                throw new GraphQLException("Invalid AddressId");


            oldAddress.Address1 = address1;
            oldAddress.Address2 = address2;
            oldAddress.Address3 = address3;
            oldAddress.City = city;
            oldAddress.StateCode = stateCode;
            oldAddress.PostalCode = postalCode;

            ctx.Update(oldAddress);
            try
            {
                await ctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception saving address to database", category: StandardLoggingCategories.DataAccess);
            }

            await eventSender.SendAsync($"{nameof(Subscription.OnAddressModified)}_{addressId}", new SubscriptionResult<Address>{Identifier = identifier, Result = oldAddress });

            return oldAddress;
        }
    }
}
