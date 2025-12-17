using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using James.Data.Server.Exceptions;

namespace James.Data.Server.GraphQL.Mutations;

[MutationType]
public class AccountMutation
{
    [Authorize]
    public async Task<bool> SetCurrentCreditReportLink(string accountNum, Guid? imagingDocumentId,
        [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var account = ctx.Accounts.FirstOrDefault(p => p.AccountNum == accountNum);
        if (account == null) return false;
        account.CreditReportImagingId = imagingDocumentId;
        await ctx.SaveChangesAsync();
        return true;
    }

    [Authorize]
    public async Task<bool> SetAccountGeneralInfo(Guid accountId, string? yearStarted, string? currentManagementYear,
        string? businessClass, string? businessType, string? priorSurety, int? estAnnualPremium,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var account = await ctx.Accounts
            .Include(a => a.IdNavigation)
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null) return false;
        account.YearOpened = yearStarted;
        account.CurrentManagementYear = currentManagementYear;
        account.BusinessTypeClass = businessClass;
        account.BusinessType = businessType;
        account.PriorSuretyCompany = priorSurety;

        await ctx.SaveChangesAsync();
        return true;
    }

    [Authorize]
    public async Task<bool> SetAccountGeneralInfoPanel(Guid accountId, string? fiscalYearEnd, string? businessType,
        string? industryCode, string? priorSuretyCompany,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var account = await ctx.Accounts
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null)
            throw new NotFoundException("Account not found");

        account.FiscalYearEnd = fiscalYearEnd;
        account.BusinessType = businessType;
        account.IndustryCode = industryCode;
        account.PriorSuretyCompany = priorSuretyCompany;

        await ctx.SaveChangesAsync();
        return true;
    }

    [Authorize]
    public async Task<bool> SetAccountSystems(Guid accountId, string? estimatingSystem, string? estimatingSignoff,
        string? internalAccountingSystem, bool? interimWips, bool? interimPOCs,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var account = await ctx.Accounts
            .Include(a => a.IdNavigation)
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null) return false;
        account.EstimatingSystem = estimatingSystem;
        account.EstimatingSignoff = estimatingSignoff;
        account.AccountingSystem = internalAccountingSystem;
        account.InterimWips = interimWips ?? false;
        account.Pocinterims = interimPOCs ?? false;

        await ctx.SaveChangesAsync();
        return true;
    }

    [Authorize]
    public async Task<bool> SetAccountAdditionalInformation(Guid accountId, bool? fullIndemnity, bool? corpIndemnity,
        bool? personalIndemnity, bool? keyManagementLifeInsurance, bool? managementIncentives, bool? fundedBuySell,
        bool? multipleActiveOwners, bool? trackCommAccount, bool? berkleyAffiliate, string? comments,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var account = await ctx.Accounts.Include(a => a.IdNavigation).FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null) return false;
        account.IndemnityFull = fullIndemnity ?? false;
        account.IndemnityCorp = corpIndemnity ?? false;
        account.IndemnityPerson = personalIndemnity ?? false;
        account.ContinuityKeyManagementLifeInsurance = keyManagementLifeInsurance ?? false;
        account.ContinuityManagementIncentives = managementIncentives ?? false;
        account.ContinuityFundedBuySell = fundedBuySell ?? false;
        account.ContinuityActiveMultipleOwners = multipleActiveOwners ?? false;
        //TODO: Deal with "trackCommAccount." Seems to be missing from DB.
        account.BerkleyAffiliate = berkleyAffiliate ?? false;
        account.IndemnityComments = comments;

        await ctx.SaveChangesAsync();
        return true;
    }

    [Authorize]
    public async Task<AccountProgram> SetAccountProgram(Guid programId, DateTime effective, DateTime expritation,
        int single, int aggregate, string? comments, Guid statusId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        //TODO: Implement. Need to deal with status changes via business logic
        var ctx = await contextFactory.CreateDbContextAsync();

        var oldProgram = await ctx.AccountPrograms.FirstOrDefaultAsync(a => a.Id == programId);

        //Update Status history before we set the new values for the program
        //TODO: Add StatusChangeBy
        AccountProgramStatusHistory newHistory = new AccountProgramStatusHistory
        {
            OldSingle = oldProgram?.Single,
            NewSingle = single,
            OldAggregate = oldProgram?.Aggregate,
            NewAggregate = aggregate,
            OldStatus = oldProgram?.StatusId,
            NewStatus = statusId,
            StatusDate = DateTime.Now,
            AccountProgramId = programId
        };

        oldProgram!.Effective = effective;
        oldProgram.Expiration = expritation;
        oldProgram.Single = single;
        oldProgram.Aggregate = aggregate;
        oldProgram.Comments = comments;
        oldProgram.StatusId = statusId;


        ctx.Update(oldProgram);
        await ctx.SaveChangesAsync();

        return oldProgram;
    }

    [Authorize]
    public async Task<AccountWatch> CreateAccountWatch(Guid id, string accountNum, DateTime watchDate,
        string watchStatus,
        string reason, string actionPlan, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var accountWatch = new AccountWatch
        {
            Id = id,
            AccountNum = accountNum,
            WatchDate = watchDate,
            WatchStatus = watchStatus,
            Reason = reason,
            ActionPlan = actionPlan,
            Created = DateTime.Now,
            Modified = DateTime.Now
        };

        ctx.AccountWatches.Add(accountWatch);
        await ctx.SaveChangesAsync();

        return accountWatch;
    }

    [Authorize]
    public async Task<AccountWatch> UpdateAccountWatch(Guid id, string watchStatus, string reason, string actionPlan,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var accountWatch = await ctx.AccountWatches.FirstOrDefaultAsync(a => a.Id == id);
        if (accountWatch == null)
            return null!;

        accountWatch.WatchStatus = watchStatus;
        accountWatch.Reason = reason;
        accountWatch.ActionPlan = actionPlan;
        accountWatch.Modified = DateTime.Now;

        await ctx.SaveChangesAsync();

        return accountWatch;
    }

    [Authorize]
    public async Task<bool> DeleteAccountWatch(Guid id,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var accountWatch = await ctx.AccountWatches.FirstOrDefaultAsync(a => a.Id == id);
        if (accountWatch == null)
            return false;

        ctx.AccountWatches.Remove(accountWatch);
        await ctx.SaveChangesAsync();

        return true;
    }

    #region Commercial

    [Authorize]
    public async Task<bool> SetAccountCommercialInfo(Guid accountId, string fullName, Guid underwriterId,
        string branchKey,
        string divisionCode, Guid hoLead, Guid sicCodeId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var account = await ctx.Accounts
            .Include(i => i.IdNavigation)
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null)
            return false;

        var validBranch = ctx.Branches.Any(b => b.BranchKey == branchKey);
        var validDivision = ctx.DivisionDms.Any(d => d.DivisionCode == divisionCode);

        if (!validBranch || !validDivision)
            return false;

        account.UnderwriterId = underwriterId;
        account.Branch = branchKey;
        account.Division = divisionCode;

        if (account.IdNavigation.FullName != fullName)
        {
            account.IdNavigation.FullName = fullName;
        }

        // ToDo: When the fields HOLead and SICCodeId are added to the Account table,
        //       uncomment the following lines. And maybe will need to be validated like
        //       branchKey and divisionCode.
        //account.HOLead = hoLead;
        //account.SICCodeId = sicCodeId;

        await ctx.SaveChangesAsync();
        return true;
    }

    [Authorize]
    public async Task<bool> SetAccountAgencyAndAgent(Guid accountId, string agencyNumber, Guid? agentId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var account = await ctx.Accounts
            .Include(i => i.IdNavigation)
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null)
            return false;

        account.AgencyNumber = agencyNumber;
        account.AgentId = agentId;

        await ctx.SaveChangesAsync();

        return true;
    }

    #endregion
}