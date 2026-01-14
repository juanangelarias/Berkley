using HotChocolate.Authorization;
using James.Shared.Dto;
using SharedBusinessLogic;
using static System.DateTime;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<Account?> GetAccountByNumber(string? accountNumber,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        if (accountNumber == null)
            return null;
        var ctx = await contextFactory.CreateDbContextAsync();
        var data = await ctx.Accounts
                       // Legal Entity
                       .Include(a => a.IdNavigation)
                       // Addresses
                       .Include(a => a.IdNavigation.LegalEntityAddresses)
                       .ThenInclude(a => a.Address)
                       // Phones
                       .Include(a => a.IdNavigation.LegalEntityPhones)
                       .ThenInclude(a => a.PhoneNumber)
                       // Emails
                       .Include(a => a.IdNavigation.LegalEntityEmails)
                       // Agency
                       .Include(a => a.AgencyNumberNavigation)
                       // Agency Legal Entity
                       .ThenInclude(ag => ag!.IdNavigation)
                       // Agency Addresses
                       .ThenInclude(agi => agi.LegalEntityAddresses)
                       .ThenInclude(agia => agia.Address)
                       // Underwriter
                       .Include(a => a.Underwriter)
                       // Underwriter Employee
                       .ThenInclude(uw => uw!.IdNavigation)
                       // Agent
                       .Include(a => a.Agent)
                       .ThenInclude(ag => ag!.IdNavigation)
                       // Home Office Review By
                       .Include(a => a.HomeOfficeReviewByNavigation)
                       // Branch Review By
                       .Include(a => a.BranchReviewByNavigation)
                       // Bank Phone
                       .Include(a => a.BankPhone)
                       // CPA firm (Legal Entity)
                       .Include(a => a.Cpafirm)
                       // CPA firm Phones
                       .ThenInclude(c => c!.LegalEntityPhones)
                       // CPA Contact (Legal Entity)
                       .Include(a => a.Cpacontact)
                       // Business Type
                       .Include(a => a.BusinessTypeNavigation)
                       // Business Type Class
                       .Include(a => a.BusinessTypeClassNavigation)
                       // Law Firm (Law Entity)
                       .Include(a => a.LawFirm)
                       // Law Firm (Legal Entity)
                       .ThenInclude(a => a!.IdNavigation)
                       // Account Statuses
                       .Include(i => i.AccountStatusLogs)
                       .ThenInclude(i => i.AccountStatusNavigation)
                       .FirstOrDefaultAsync(a => a.AccountNum.Trim() == accountNumber.Trim())
                   ?? throw new GraphQLException("No account with this account number exists.");

        return data;
    }

    [Authorize]
    public async Task<DateOnly?> GetFirstIndemnity(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var firstIndemnity = ctx.Indemnitors
            .OrderBy(o => o.AgreementDate)
            .FirstOrDefault(r => r.AccountNum == accountNum)?
            .AgreementDate;

        return firstIndemnity;
    }

    [Authorize]
    public async Task<List<Account>> SearchAccounts(string searchString,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            //TODO: Improve search with fuzzy logic.
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.Accounts
                .Where(a => a.IdNavigation.FullName.Contains(searchString) || a.AccountNum.Contains(searchString))
                .Include(a => a.IdNavigation)
                .ToListAsync();
        }
        else
        {
            return [];
        }
    }

    [Authorize]
    public async Task<PrivateEquity?> GetLastPrivateEquityByAccount(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var result = await ctx.PrivateEquities
            .Include(i => i.EnteredByNavigation)
            .OrderBy(o => o.AccountNum)
            .ThenByDescending(o => o.Created)
            .FirstOrDefaultAsync(r => r.AccountNum == accountNum);

        return result;
    }

    [Authorize]
    public async Task<Indemnitor?> GetLastIndemnitorByAccount(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var result = await ctx.Indemnitors
            .OrderBy(o => o.AccountNum)
            .ThenByDescending(o => o.AgreementDate)
            .FirstOrDefaultAsync(r => r.AccountNum == accountNum && r.AgreementType == "GIA");

        return result;
    }

    [Authorize]
    public async Task<InforceAccountLOA> GetAccountActiveLinesOfAuthority(string accountNumber,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var contractLOA = await ctx.LineOfAuthorityLogs
            .Where(l => l.AccountNum == accountNumber && l.Effective <= Today && l.BondType == "Contract")
            .OrderByDescending(l => l.Created)
            .FirstOrDefaultAsync();

        var commercialLOA = await ctx.LineOfAuthorityLogs
            .Where(l => l.AccountNum == accountNumber && l.Effective <= Today &&
                        l.BondType == "Commercial")
            .OrderByDescending(l => l.Created)
            .FirstOrDefaultAsync();

        InforceAccountLOA inforceLOAs = new InforceAccountLOA()
        {
            AccountNum = accountNumber,
            ContractLOA = contractLOA,
            CommercialLOA = commercialLOA
        };


        return inforceLOAs;
    }

    [Authorize]
    public async Task<List<AccountProgram>> GetAccountProgramHistory(string accountNumber,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        return await ctx.AccountPrograms
            .Where(a => a.AccountNum == accountNumber)
            .Include(a => a.AccountProgramStatusHistories)
            .Include(a => a.Status)
            .OrderByDescending(a => a.Expiration)
            .ToListAsync();
    }

    [Authorize]
    public async Task<Account?> GetAccountOnly(string? accountNumber,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        if (accountNumber == null)
            return null;
        var ctx = await contextFactory.CreateDbContextAsync();
        return await ctx.Accounts
            .FirstOrDefaultAsync(a => a.AccountNum.Trim() == accountNumber.Trim());
    }

    [Authorize]
    public async Task<List<AdditionalRelatedParty>> GetAdditionalRelatedParties(string? accountNumber,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            return null!;
        }

        var ctx = await contextFactory.CreateDbContextAsync();
        return await ctx.AdditionalRelatedParties
            .Include(a => a.IdNavigation)
            .Where(a => a.AccountNum == accountNumber)
            .ToListAsync();
    }

    [Authorize]
    public async Task<List<Account>> GetIdAccountNumbers(
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var acctList = await ctx.Accounts.ToListAsync();
        return acctList;
    }

    [Authorize]
    public async Task<List<Account>> SearchAccountsByAccountNumber(string accountNumberFragment, bool activeOnly,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var matchingAccounts = await ctx.Accounts
            .Include(a => a.IdNavigation)
            .ThenInclude(le => le.LegalEntityAddresses)
            .ThenInclude(lea => lea.Address)
            .ThenInclude(ad => ad.StateCodeNavigation)
            .ThenInclude(sc => sc!.CountryCodeNavigation)
            .Include(a => a.IdNavigation.LegalEntityPhones)
            .ThenInclude(lep => lep.PhoneNumber)
            .Include(a => a.IdNavigation.LegalEntityEmails)
            .Join(ctx.VAccountStatuses, act => act.AccountNum, vact => vact.AccountNum,
                (act, vact) => new { Account = act, Active = vact.AccountStatus == "Active" })
            .Where(a => EF.Functions.Like(a.Account.AccountNum, $"%{accountNumberFragment}%") &&
                        (activeOnly == false || a.Active))
            .Select(a => a.Account)
            .ToListAsync();
        return matchingAccounts;
    }

    [Authorize]
    public async Task<List<Account>> SearchAccountsByName(string searchString, bool activeOnly,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var matchingAccounts = await ctx.Accounts
            .Include(a => a.IdNavigation)
            .ThenInclude(le => le.LegalEntityAddresses)
            .ThenInclude(lea => lea.Address)
            .ThenInclude(ad => ad.StateCodeNavigation)
            .ThenInclude(sc => sc!.CountryCodeNavigation)
            .Include(a => a.IdNavigation.LegalEntityPhones)
            .ThenInclude(lep => lep.PhoneNumber)
            .Include(a => a.IdNavigation.LegalEntityEmails)
            .Join(ctx.VAccountStatuses, act => act.AccountNum, vact => vact.AccountNum,
                (act, vact) => new { Account = act, Active = vact.AccountStatus == "Active" })
            .Where(a => EF.Functions.Like(a.Account.IdNavigation.FullName, $"%{searchString}%") &&
                        (activeOnly == false || a.Active))
            .Select(a => a.Account)
            .ToListAsync();
        return matchingAccounts;
    }

    [Authorize]
    public async Task<List<AccountWatch>> GetAllAccountWatches(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var result = await ctx.AccountWatches
            .Include(i => i.WatchStatusNavigation)
            .Where(r => r.AccountNum == accountNum)
            .ToListAsync();

        return result;
    }
    
    [Authorize]
    public async Task<List<LineOfAuthorityLog>> GetAllLineOfAuthorityLogs(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var result = await ctx.LineOfAuthorityLogs
            .Where(r => r.AccountNum == accountNum)
            .OrderByDescending(o=>o.Effective)
            .ToListAsync();

        return result;
    }
    
    [Authorize]
    public async Task<AccountAnnualPremiumDto> GetAccountAnnualPremiums(string accountNum, string type,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var startDate = Today.AddYears(-1);
        var endDate = Today;
        var yearStart = new DateTime(endDate.Year, 1, 1);
        
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var account = ctx.Accounts
            .FirstOrDefault(f=>f.AccountNum == accountNum);

        if (account == null)
            return new();
        
        var accountList = (type.ToUpper() == "ACCOUNT ONLY")
             ? [account.AccountNum]
             : await GetRelatedAccounts(account.Id,  type.ToUpper() == "PARENT ONLY", contextFactory);
        
        var premiums = await ctx.BondTransactions
            .OrderBy(o => o.Effective)
            .Where(r => r.Effective >= startDate &&
                        r.Effective <= endDate &&
                        accountList.Contains(r.AccountNum))
            .Select(s => new {s.Effective,s.Premium})
            .ToListAsync();
        
        var response = new AccountAnnualPremiumDto
        {
            TrailingTwelveMonths = premiums.Sum(s => s.Premium),
            YearToDate = premiums.Where(r => r.Effective >= yearStart).Sum(s => s.Premium)
        };
        
        return response;
    }

    private async Task<List<string>> GetRelatedAccounts(Guid accountId, bool onlyParent, 
        IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var parentId = await GetParent(accountId, contextFactory);
        if(parentId == null) 
            return [];
        
        var ctx = await contextFactory.CreateDbContextAsync();

        var parentAccountNum = ctx.Accounts.FirstOrDefault(f => f.Id == parentId)?.AccountNum;
        
        if (onlyParent || parentAccountNum == null)
        {
            return parentAccountNum == null
                ? []
                : [parentAccountNum];
        }

        List<string> related = [parentAccountNum];
        related.AddRange(await GetLegalEntityChildren(parentAccountNum, ctx));
        
        return related;
    }

    private async Task<List<string>> GetLegalEntityChildren(string accountNum, 
        JamesDatabaseContext ctx)
    {
        var  result = new List<string>();
        var children = await ctx.LegalEntityChildren
            .FromSqlInterpolated($"EXECUTE dbo.GetLegalEntityChildren {accountNum}")
            .ToListAsync();
        
        result.AddRange(children.Select(s => s.ChildAccountNum));

        foreach (var child in children)
        {
            var newChildren = await GetLegalEntityChildren(child.ChildAccountNum, ctx);
            result.AddRange(newChildren);       
        }
        
        return result;
    }
    
    private async Task<Guid?> GetParent(Guid legalEntityId, IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var legalEntity = await ctx.LegalEntities
            .FirstOrDefaultAsync(r => r.Id == legalEntityId);
        
        if(legalEntity == null)
            return null;
        
        return legalEntity.Parent != legalEntity.Id 
            ? await GetParent(legalEntity.Parent, contextFactory) 
            : legalEntity.Parent;
    }

    [Authorize]
    public async Task<List<AccountCollateralDto>> GetAccountBondCollaterals(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var collaterals = ctx.Collaterals
            .Where(r =>
                r.AccountNum == accountNum &&
                r.Released != null &&
                r.Released.Value < Today)
            .Select(s => new AccountCollateralDto
            {
                BondNumber = s.BondNumber,
                //Bank = s.Bank         // ToDo: After the field "Bank" is added to the table this should be uncommented
                Bank = "Bank ???",      // ToDo: After the field "Bank" is added to the table this should be removed
                Type = s.Type,
                Amount = s.Amount ?? 0,
                ExpirationDate = s.Expiration
            })
            .ToList();
        
        return collaterals;
    }

    [Authorize]
    public async Task<AccountOutstandingLiabilityDto> GetAccountOutstandingLiability(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var accountId = (await ctx.Accounts.FirstOrDefaultAsync(f => f.AccountNum == accountNum))?.Id;
        if(accountId == null) 
            throw new GraphQLException("No account with this account number exists.");
        
        var relatedAccounts = await GetRelatedAccounts(accountId.Value, false, contextFactory);

        var bonds = await ctx.Bonds
            .Include(i => i.BondType)
            .OrderBy(o => o.AccountNum)
            .ThenBy(t => t.BondType!.BondType)
            .ThenBy(t => t.BondType!.BondClass)
            .Where(r => relatedAccounts.Contains(r.AccountNum))
            .Select(s => new
            {
                s.BondNumber,
                s.Status,
                s.BondType!.BondType,
                s.BondType!.BondClass,
                s.CurrentBondLiability
            })
            .ToListAsync();
        
        var bondNumbers = bonds
            .Where(r=> r.Status == "Open")
            .Select(s => s.BondNumber).ToList();
        
        var bondMods = await ctx.BondModTransactions
            .OrderBy(o=>o.BondNumber)
            .ThenByDescending(t=>t.Effective)
            .Where(r => bondNumbers.Contains(r.BondNumber))
            .ToListAsync();

        var result = new AccountOutstandingLiabilityDto();
        foreach (var bond in bonds)
        {
            var mod = bondMods.FirstOrDefault(f => f.BondNumber == bond.BondNumber);
            var proratedAmount = bond.BondType == "Contract"
                ? mod == null
                    ? 0
                    : AccountProgramBusinessLogic.CalculateProratedBondAmount(bond.CurrentBondLiability, mod.Effective,
                        mod.Expiration)
                : bond.CurrentBondLiability;
            
            if(proratedAmount == 0) 
                continue;

            switch (bond.BondType)
            {
                case "Commercial":
                    result.OutstandingCommercialLiability += proratedAmount;
                    break;
                case "Contract":
                    result.OutstandingContractLiability += proratedAmount;
                    break;
            }

            var exist = result.LargestOutstandingBonds
                .FirstOrDefault(f => f.BondType == bond.BondType &&
                                     f.BondClass == bond.BondClass);
            
            if (exist != null && proratedAmount > exist.Amount)
                exist.Amount = proratedAmount;

            if (exist == null)
                result.LargestOutstandingBonds.Add(new()
                {
                    BondType = bond.BondType,
                    BondClass = bond.BondClass,
                    Amount = proratedAmount
                });
        }

        result.LargestBondEver = bonds
            .OrderByDescending(o => o.CurrentBondLiability)
            .FirstOrDefault()?.CurrentBondLiability ?? 0;
        
        return result;
    }
}