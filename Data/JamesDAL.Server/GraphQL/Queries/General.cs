using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        [Authorize]
        public async Task<Address> GetAddress(Guid addressId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Addresses.Where(a => a.Id == addressId).FirstOrDefaultAsync();

            return result ?? throw new GraphQLException($"No address found with AddressID {addressId}.");
        }

        [Authorize]
        public async Task<UserProfile> GetUserProfileByUserName(string userName, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.UserProfiles.Where(u => u.Username == userName).FirstOrDefaultAsync();

            return result ?? throw new GraphQLException($"No user found with UserName {userName}.");
        }
        [Authorize]
        public async Task<List<InventoryDocumentDm>> GetAllInventoryDocTypes([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();

                var result = await ctx.InventoryDocumentDms.ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error when retrieving InventoryDocumentDM", ex);
            }
        }
        [Authorize]
        public async Task<List<Branch>> GetAllBranches([Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                var result = await ctx.Branches.ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error when retrieving Branches.", ex);
            }
        }
        [Authorize]
        public async Task<List<Address>> GetAllLegalEntityAddresses(Guid legalEntityId, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                var result = await ctx.Addresses
                    .Include(a => a.LegalEntityAddress)
                    .ThenInclude(a => a.TypeNavigation)
                    .Where(a => a.LegalEntityAddress.LegalEntityId == legalEntityId)
                    .OrderBy(a => a.LegalEntityAddress.TypeNavigation.Order)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error when retrieving Addresses.", ex);
            }
        }
        [Authorize]
        public async Task<List<PhoneNumber>> GetAllLegalEntityPhoneNumbers(Guid legalEntityId, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.PhoneNumbers
                .Include(a => a.LegalEntityPhone)
                .ThenInclude(a => a.TypeNavigation)
                .Where(a => a.LegalEntityPhone.LegalEntityId == legalEntityId)
                .OrderBy(a => a.LegalEntityPhone.TypeNavigation.Order)
                .ToListAsync();

            return result;
        }
        [Authorize]
        public async Task<List<AddressTypeDm>> GetAddressTypes([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                var result = await ctx.AddressTypeDms.OrderBy(a => a.Order).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error when retrieving Address Types.", ex);
            }
        }
    }
}
