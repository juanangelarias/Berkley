using James.Data.Server.Exceptions;
using James.Shared;
using James.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace James.Data.Server.GraphQL.Mutations;

public partial class GeneralMutation
{
    [Authorize(Policy = "InRoleCanSetAccountProgram")]
    public async Task<bool> SetAccountProgram(Guid programId, string accountNum, DateTime effective,
        DateTime? expiration, int single, int aggregate, string? comments, Guid statusId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] IUserShared userShared)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var employee = await GetEmployeeAsync(ctx, userShared);

        AccountProgramStatusHistory CreateStatusLog(AccountProgramStatusHistory? lastLog) => new()
        {
            Id = Guid.NewGuid(),
            AccountProgramId = programId,
            AccountNum = accountNum,
            OldStatus = lastLog?.NewStatus,
            NewStatus = statusId,
            StatusDate = DateTime.UtcNow,
            OldSingle = lastLog?.NewSingle,
            NewSingle = single,
            OldAggregate = lastLog?.NewAggregate,
            NewAggregate = aggregate,
            StatusChangeBy = employee.Id
        };

        var existing = await ctx.AccountPrograms
            .FirstOrDefaultAsync(f => f.Id == programId);
        if (existing != null)
        {
            existing.Effective = effective;
            existing.Expiration = expiration ?? new DateTime(9999, 12, 31, 0, 0, 0, DateTimeKind.Utc);
            existing.Single = single;
            existing.Aggregate = aggregate;
            existing.Modified = DateTime.UtcNow;
            existing.CreatedBy = employee.Id;
            existing.Comments = comments;

            var lastLog = await ctx.AccountProgramStatusHistories
                .OrderBy(o => o.AccountProgramId)
                .ThenByDescending(t => t.Created)
                .FirstOrDefaultAsync(f => f.AccountProgramId == programId);

            var newLog = CreateStatusLog(lastLog);
            ctx.AccountProgramStatusHistories.Add(newLog);
            ctx.Update(existing);
        }
        else
        {
            var program = new AccountProgram
            {
                Id = programId,
                AccountNum = accountNum,
                Effective = effective,
                Expiration = expiration ?? new DateTime(9999, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                Single = single,
                Aggregate = aggregate,
                StatusId = statusId,
                Created = DateTime.UtcNow,
                CreatedBy = employee.Id,
                Modified = DateTime.UtcNow,
                ApprovedBy = null,
                ApprovedDate = null,
                Comments = comments
            };
            ctx.AccountPrograms.Add(program);
            ctx.AccountProgramStatusHistories.Add(CreateStatusLog(null));
        }

        await ctx.SaveChangesAsync();
        return true;
    }

    [Authorize(Policy = "InRoleCanDeleteAccountProgram")]
    public async Task<bool> DeleteAccountProgram(Guid accountProgramId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var record = await ctx.AccountPrograms
            .FirstOrDefaultAsync(f => f.Id == accountProgramId);

        if (record == null)
            throw new GraphQLException("Account program status dms not found.");

        var history = await ctx.AccountProgramStatusHistories
            .Where(f => f.AccountProgramId == accountProgramId)
            .ToListAsync();

        ctx.AccountProgramStatusHistories.RemoveRange(history);
        ctx.AccountPrograms.Remove(record);

        await ctx.SaveChangesAsync();

        return true;
    }

    [Authorize(Policy = "InRoleCanSetAccountProgram")]
    public async Task<bool> AccountProgramChangeStatus(Guid accountProgramId, string newStatusTxt,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IUserShared userShared)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var employee = await GetEmployeeAsync(ctx, userShared);

        var newStatus = ctx.AccountProgramStatusDms
            .FirstOrDefault(f => f.Description.ToUpper() == newStatusTxt.ToUpper());

        if (newStatus == null)
            throw new GraphQLException($"'{newStatusTxt}' status not found");

        var program = await ctx.AccountPrograms.FindAsync(accountProgramId);
        if (program == null)
            throw new GraphQLException("Account program not found");

        var lastLog = await ctx.AccountProgramStatusHistories
            .OrderBy(o => o.AccountProgramId)
            .ThenByDescending(t => t.Created)
            .FirstOrDefaultAsync(f => f.AccountProgramId == accountProgramId);

        var newLog = new AccountProgramStatusHistory
        {
            Id = Guid.NewGuid(),
            AccountProgramId = program.Id,
            AccountNum = program.AccountNum,
            OldStatus = lastLog?.NewStatus,
            NewStatus = newStatus.Id,
            StatusDate = DateTime.UtcNow,
            OldSingle = lastLog?.NewSingle,
            NewSingle = program.Single,
            OldAggregate = lastLog?.NewAggregate,
            NewAggregate = program.Aggregate,
            StatusChangeBy = employee.Id
        };

        ctx.AccountProgramStatusHistories.Add(newLog);
        program.StatusId = newStatus.Id;
        program.Modified = DateTime.UtcNow;
        program.ApprovedDate = DateTime.UtcNow;

        if (newStatusTxt.ToUpper() == LOAStatus.Approved)
        {
            program.ApprovedBy = employee.Id;
            program.ApprovedDate = DateTime.UtcNow;
        }

        await ctx.SaveChangesAsync();

        if (newStatusTxt.ToUpper() == LOAStatus.ApprovalRequested)
        {
            // ToDo: Send email to approver
        }

        if (newStatusTxt.ToUpper() == LOAStatus.Approved)
        {
            // ToDo: Send approval email to the requester
        }

        if (newStatusTxt.ToUpper() == LOAStatus.Declined)
        {
            // ToDo: Send decline email to the requester
        }

        return true;
    }

    [Authorize]
    public async Task<bool> SetLoaLog(Guid id, string accountNum, DateTime effective, DateTime expiration,
        int loaSingle, int loaAggregate, string? comments, string status, string? division, string? bondType,
        string? conditions, bool homeOfficeApproved, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IUserShared userShared)
    {
        return await SetLoaLogInternal(id, accountNum, effective, expiration, loaSingle, loaAggregate, comments, status,
            division, bondType, conditions, homeOfficeApproved, contextFactory, userShared);
    }

    /// <summary>
    /// Renews or creates a new Line of Authority log.
    /// </summary>
    /// <param name="id">The unique identifier for the LOA log.</param>
    /// <param name="accountNum">The account number.</param>
    /// <param name="effective">The effective date.</param>
    /// <param name="expiration">The expiration date.</param>
    /// <param name="loaSingle">The single bond limit.</param>
    /// <param name="loaAggregate">The aggregate bond limit.</param>
    /// <param name="comments">Optional comments.</param>
    /// <param name="status">The status of the LOA log.</param>
    /// <param name="division">The division.</param>
    /// <param name="bondType">The bond type.</param>
    /// <param name="conditions">Optional conditions.</param>
    /// <param name="homeOfficeApproved">Whether home office has approved.</param>
    /// <param name="contextFactory">Database context factory.</param>
    /// <param name="userShared">User shared service.</param>
    /// <param name="existingCtx">Optional existing database context for transaction support.</param>
    /// <returns>True if successful.</returns>
    private async Task<bool> SetLoaLogInternal(Guid id, string accountNum, DateTime effective, DateTime expiration,
        int loaSingle, int loaAggregate, string? comments, string status, string? division, string? bondType,
        string? conditions, bool homeOfficeApproved, IDbContextFactory<JamesDatabaseContext> contextFactory,
        IUserShared userShared, JamesDatabaseContext? existingCtx = null)
    {
        var ctx = existingCtx ?? await contextFactory.CreateDbContextAsync();

        var employee = await GetEmployeeAsync(ctx, userShared);

        var loa = await ctx.LineOfAuthorityLogs.FindAsync(id);
        if (loa == null)
        {
            loa = new()
            {
                Id = id,
                AccountNum = accountNum,
                Effective = effective,
                Expiration = expiration,
                Loasingle = loaSingle,
                Loaaggregate = loaAggregate,
                Comments = comments,
                Status = status,
                Division = division,
                BondType = bondType,
                Conditions = conditions,
                HomeOfficeApproved = homeOfficeApproved,
                CreatedBy = employee.Id,
            };

            ctx.LineOfAuthorityLogs.Add(loa);
        }
        else
        {
            loa.AccountNum = accountNum;
            loa.Effective = effective;
            loa.Expiration = expiration;
            loa.Loasingle = loaSingle;
            loa.Loaaggregate = loaAggregate;
            loa.Comments = comments;
            loa.Status = status;
            loa.Division = division;
            loa.BondType = bondType;
            loa.Conditions = conditions;
            loa.HomeOfficeApproved = homeOfficeApproved;
            loa.CreatedBy = employee.Id;
        }

        // ToDo: add functionality to add the LoaHistory when creating or updating a LOA

        if (existingCtx == null)
        {
            await ctx.SaveChangesAsync();
        }

        return true;
    }

    [Authorize]
    public async Task<bool> LoaLogDelete(Guid accountLoaId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var loa = await ctx.LineOfAuthorityLogs.FindAsync(accountLoaId);
        if (loa == null)
            throw new NotFoundException("Line of authority log not found");

        ctx.LineOfAuthorityLogs.Remove(loa);
        await ctx.SaveChangesAsync();

        return true;
    }

    [Authorize]
    public async Task<bool> LoaLogChangeStatus(Guid accountLoaId, string newStatusTxt,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IUserShared userShared)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var loa = await ctx.LineOfAuthorityLogs.FindAsync(accountLoaId);
        if (loa == null)
            throw new GraphQLException("Line of authority log not found");

        var employee = await GetEmployeeAsync(ctx, userShared);

        // ToDo: add functionality to change the LoaHistory status when changing the LOA status

        loa.Status = newStatusTxt;
        loa.Modified = DateTime.UtcNow;
        await ctx.SaveChangesAsync();

        return true;
    }

    [Authorize]
    public async Task<bool> SetAgencyLoa(Guid id, string agencyNumber, string accountNum,
        DateTime effective, DateTime expiration, int loaSinge, int loaAggregate, string comments, string division,
        string bondType, string conditions, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IUserShared userShared)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var employee = await GetEmployeeAsync(ctx, userShared);

        var loa = await ctx.AgencyLineOfAuthorityLogs.FindAsync(id);
        if (loa == null)
        {
            loa = new()
            {
                Id = id,
                AgencyNumber = agencyNumber,
                AccountNum = accountNum,
                Effective = effective,
                Expiration = expiration,
                Loasingle = loaSinge,
                Loaaggregate = loaAggregate,
                Comments = comments,
                Division = division,
                BondType = bondType,
                CreatedBy = employee.Id,
                Conditions = conditions,
            };

            ctx.AgencyLineOfAuthorityLogs.Add(loa);
        }
        else
        {
            loa.AgencyNumber = agencyNumber;
            loa.AccountNum = accountNum;
            loa.Effective = effective;
            loa.Expiration = expiration;
            loa.Loasingle = loaSinge;
            loa.Loaaggregate = loaAggregate;
            loa.Comments = comments;
            loa.Division = division;
            loa.BondType = bondType;
            loa.Conditions = conditions;
        }

        await ctx.SaveChangesAsync();

        return true;
    }

    [Authorize]
    public async Task<bool> AgencyLoaDelete(Guid agencyLoaId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var loa = await ctx.AgencyLineOfAuthorityLogs.FindAsync(agencyLoaId);
        if (loa == null)
            throw new GraphQLException("Agency line of authority log not found");

        ctx.AgencyLineOfAuthorityLogs.Remove(loa);
        await ctx.SaveChangesAsync();

        return true;
    }


    // Reason - Renewal
    [Authorize]
    public async Task<bool> SetLOAReason(LineOfAuthorityReason loaReason,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] IUserShared userShared)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var employee = await GetEmployeeAsync(ctx, userShared);

        var existing = await ctx.LineOfAuthorityReasons
            .FirstOrDefaultAsync(f => f.Id == loaReason.Id);

        await using var transaction = await ctx.Database.BeginTransactionAsync();

        try
        {
            if (existing == null)
            {
                var newReason = new LineOfAuthorityReason
                {
                    Id = loaReason.Id,
                    AccountNum = loaReason.AccountNum,
                    CreatedBy = employee.Id,
                    Type = loaReason.Type,
                    Recommendation = loaReason.Recommendation,
                    BusinessOverview = loaReason.BusinessOverview,
                    BondRisk = loaReason.BondRisk,
                    FinancialAnalysis = loaReason.FinancialAnalysis,
                    DebtHighlights = loaReason.DebtHighlights,
                    FollowUpConditions = loaReason.FollowUpConditions,
                    KeyChanges = loaReason.KeyChanges,
                    Outlook = loaReason.Outlook,
                };
                ctx.LineOfAuthorityReasons.Add(newReason);
            }
            else
            {
                existing.AccountNum = loaReason.AccountNum;
                existing.CreatedBy = employee.Id;
                existing.Type = loaReason.Type;
                existing.Recommendation = loaReason.Recommendation;
                existing.BusinessOverview = loaReason.BusinessOverview;
                existing.BondRisk = loaReason.BondRisk;
                existing.FinancialAnalysis = loaReason.FinancialAnalysis;
                existing.DebtHighlights = loaReason.DebtHighlights;
                existing.FollowUpConditions = loaReason.FollowUpConditions;
                existing.KeyChanges = loaReason.KeyChanges;
                existing.Outlook = loaReason.Outlook;
                ctx.Update(existing);
            }

            foreach (var log in loaReason.LineOfAuthorityLogs)
            {
                await SetLoaLogInternal(log.Id, log.AccountNum, log.Effective, log.Expiration, log.Loasingle, log.Loaaggregate,
                    log.Comments, log.Status, log.Division, log.BondType, log.Conditions, log.HomeOfficeApproved,
                    contextFactory, userShared, ctx);
            }

            await ctx.SaveChangesAsync();

            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            throw;
        }
    }

    [Authorize]
    public async Task<bool> AssociateLOALogToReason(Guid reasonId, Guid loaLogId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var loaLog = await ctx.LineOfAuthorityLogs
            .FirstOrDefaultAsync(f => f.Id == loaLogId);

        if (loaLog == null)
            throw new NotFoundException("LOA Log not found");

        var logsAssociated = await ctx.LineOfAuthorityLogs
            .Where(r => r.ReasonId == reasonId)
            .ToListAsync();

        if (logsAssociated.Any(a => a.Id == loaLogId))
            return true;

        if (logsAssociated.Any(a => a.BondType == loaLog.BondType))
            throw new($"There is already a LOA Log associated with this reason and bond type ({loaLog.BondType})");

        loaLog.ReasonId = reasonId;
        ctx.Update(loaLog);
        await ctx.SaveChangesAsync();

        return true;
    }

    [Authorize]
    public async Task<bool> DisassociateLOALogToReason(Guid reasonId, Guid loaLogId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var loaLog = await ctx.LineOfAuthorityLogs
            .FirstOrDefaultAsync(f => f.Id == loaLogId);

        if (loaLog == null)
            throw new NotFoundException("Loa Log not found");

        if (loaLog.ReasonId != reasonId)
            throw new("LOA Log is not associated with this reason");

        loaLog.ReasonId = null;
        ctx.Update(loaLog);
        await ctx.SaveChangesAsync();

        return true;
    }

    [Authorize]
    public async Task<bool> ApproveAccountLOA(Guid reasonId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] IUserShared userShared)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var employee = await GetEmployeeAsync(ctx, userShared);

        await using var trn = await ctx.Database.BeginTransactionAsync();
        try
        {
            var loaLogs = ctx.LineOfAuthorityLogs
                .Where(l => l.ReasonId == reasonId)
                .ToList();

            foreach (var loaLog in loaLogs)
            {
                loaLog.Status = LOAStatus.Approved;
                loaLog.Approved = DateTime.UtcNow;
                loaLog.ApprovedBy = employee.Id;
                ctx.Update(loaLog);
            }

            await ctx.SaveChangesAsync();
            await trn.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await trn.RollbackAsync();
            throw;
        }
    }

    [Authorize]
    public async Task<bool> DeclineAccountLOA(Guid reasonId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] IUserShared userShared)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var employee = await GetEmployeeAsync(ctx, userShared);

        await using var trn = await ctx.Database.BeginTransactionAsync();
        try
        {
            var loaLogs = ctx.LineOfAuthorityLogs
                .Where(l => l.ReasonId == reasonId)
                .ToList();

            foreach (var loaLog in loaLogs)
            {
                loaLog.Status = LOAStatus.Declined;
                ctx.Update(loaLog);
            }

            await ctx.SaveChangesAsync();
            await trn.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await trn.RollbackAsync();
            throw;
        }
    }
}
