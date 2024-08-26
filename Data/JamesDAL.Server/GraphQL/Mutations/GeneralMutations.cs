using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Mutations
{
    [MutationType]
    public class GeneralMutations
    {
        [Authorize]
        public async Task<bool> CreateAddress(Guid addressId, string address1, string? address2,
            string? address3, string city, string? stateCode, string? postalCode,
            Guid legalEntityId, string addressType, 
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            //TODO: Address Validator?
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

            return true;
        }

        [Authorize]
        public async Task<bool> DeleteAddress(Guid addressId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            //TODO: Error Handling
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
            return true;
        }
    }
}
