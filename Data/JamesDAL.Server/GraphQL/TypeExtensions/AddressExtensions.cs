using System.Diagnostics;

namespace James.Data.Server.GraphQL.TypeExtensions
{
    [ExtendObjectType(typeof(Address))]
    public class AddressExtensions
    {
        public async Task<string> GetState([Parent] Address address,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            JamesDatabaseContext? ctx = null;
            if (null == address.StateCodeNavigation)
            {
                ctx = await contextFactory.CreateDbContextAsync();
                address.StateCodeNavigation = await ctx.States.Include(s => s.CountryCodeNavigation).SingleAsync(ad => ad.Code == address.StateCode);
            }
            else if (null == address.StateCodeNavigation.CountryCodeNavigation)
            {
                ctx ??= await contextFactory.CreateDbContextAsync();
                address.StateCodeNavigation.CountryCodeNavigation =
                    await ctx.CountryDms.SingleAsync(c => c.Code == address.StateCodeNavigation.CountryCode);
            }
            return address.StateCodeNavigation.Name;
        }
        public async Task<string> GetCountry([Parent] Address address,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            await GetState(address, contextFactory);
            Debug.Assert(address.StateCodeNavigation!.CountryCodeNavigation != null, "address.StateCodeNavigation.CountryCodeNavigation != null");
            return address.StateCodeNavigation.CountryCodeNavigation.Name;
        }
        public async Task<string> GetCountryCode([Parent] Address address,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (null == address.StateCodeNavigation?.CountryCodeNavigation)
                await GetState(address, contextFactory);
            Debug.Assert(address.StateCodeNavigation!.CountryCodeNavigation != null, "address.StateCodeNavigation.CountryCodeNavigation != null");
            return address.StateCodeNavigation.CountryCodeNavigation.Code;
        }
    }

    public class StateExtensions
    {
        public async Task<CountryDm> GetCountry([Parent] State state,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (null == state.CountryCodeNavigation)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                state.CountryCodeNavigation = await ctx.CountryDms.SingleAsync(c => c.Code == state.CountryCode);
            }
            return state.CountryCodeNavigation;
        }
    }
}
