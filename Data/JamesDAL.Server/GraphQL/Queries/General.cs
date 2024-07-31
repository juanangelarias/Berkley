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
    }
}
