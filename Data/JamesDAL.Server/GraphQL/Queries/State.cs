namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        /// <summary>
        /// Get all states from our states table
        /// </summary>
        /// <param name="contextFactory">service</param>
        /// <returns>All states in out database</returns>
        /// <remarks>Does not require authorizations</remarks>
        public async Task<List<State>> GetAllStates([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.States.ToListAsync();
        }
    }
}
