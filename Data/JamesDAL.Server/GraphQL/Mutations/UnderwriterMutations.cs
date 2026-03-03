using Microsoft.AspNetCore.Authorization;

namespace James.Data.Server.GraphQL.Mutations;

public partial class GeneralMutation
{
    [Authorize(Policy = "InRoleSetAddUnderwritingRecommendation")]
    public async Task<bool> SetUnderwriterRecommendation(Guid id, string accountNum, Guid postedBy, string comments,
        string description, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var actualRecord = await ctx.UnderwriterRecommendations
            .FirstOrDefaultAsync(f => f.Id == id);

        if (actualRecord != null)
        {
            actualRecord.AccountNum = accountNum;
            actualRecord.PostedBy = postedBy;
            actualRecord.Comments = comments;
            actualRecord.Description = description;
        }
        else
        {
            var newRecommendation = new UnderwriterRecommendation
            {
                Id = id,
                AccountNum = accountNum,
                PostedBy = postedBy,
                Comments = comments,
                Description = description
            };
            ctx.UnderwriterRecommendations.Add(newRecommendation);
        }

        return await ctx.SaveChangesAsync() > 0;
    }

    [Authorize]
    public async Task<bool> DeleteUnderwriterRecommendation(Guid id,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var actualRecord = await ctx.UnderwriterRecommendations
            .FirstOrDefaultAsync(f => f.Id == id);
        
        if(actualRecord == null)
            throw new GraphQLException("Underwriter recommendation not found");
        
        ctx.UnderwriterRecommendations.Remove(actualRecord);
        await ctx.SaveChangesAsync();
        
        return true;
    }
}