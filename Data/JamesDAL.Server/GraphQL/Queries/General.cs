using HotChocolate.Authorization;
using James.Shared.Dto;
using Microsoft.AspNetCore.Http;

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
        public async Task<PhoneNumber> GetPhoneNumber(Guid phoneId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.PhoneNumbers.Where(p => p.Id == phoneId).FirstOrDefaultAsync();

            return result ?? throw new GraphQLException($"No phone number found with PhoneID {phoneId}.");
        }
        [Authorize]
        public async Task<Employee> GetEmployeeByUserName(string userName, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Employees.Where(e => e.ActiveDirectoryAccount == userName).FirstOrDefaultAsync();

            return result ?? throw new GraphQLException($"No employee found with UserName {userName}.");
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
                    .ThenInclude(a => a!.TypeNavigation)
                    .Where(a => a.LegalEntityAddress!.LegalEntityId == legalEntityId)
                    .OrderBy(a => a.LegalEntityAddress!.TypeNavigation.Order)
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
        public async Task<List<LegalEntityEmail>> GetAllLegalEntityEmails(Guid legalEntityId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            var result = await ctx.LegalEntityEmails
                .Where(a => a.LegalEntityId == legalEntityId)
                .ToListAsync();
            
            return result;
        }
        
        [Authorize]
        public async Task<List<PhoneTypeDm>> GetPhoneTypes([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                var result = await ctx.PhoneTypeDms.OrderBy(a => a.Order).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error when retrieving Phone Types.", ex);
            }
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

        [Authorize]
        public async Task<List<EmailTypeDm>> GetEmailTypes(
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.EmailTypeDms
                .OrderBy(o => o.Type)
                .ToListAsync();
            
            return result;
        }
        
        [Authorize]
        public async Task<List<CountryDm>> GetAllCountries([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                var result = await ctx.CountryDms.ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error when retrieving country list.", ex);
            }
        }

        [Authorize]
        public async Task<List<WatchStatusDm>> GetAllWatchStatuses(
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.WatchStatusDms
                .ToListAsync();
            
            return result;
        }
        
        [Authorize]
        public async Task<List<KeyValuePair<string, string>>> GetUserSettings([Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] IHttpContextAccessor contextAccessor)
        {
            try
            {
                var username = contextAccessor.HttpContext?.User.FindFirst("nickname")?.Value;
                if (null == username)
                    throw new UnauthorizedAccessException("Must be logged in to get user settings.");
                var ctx = await contextFactory.CreateDbContextAsync();
                var ctx2 = await contextFactory.CreateDbContextAsync();
                var userSettingsTask = ctx.UserSettings.Where(up => up.Username == username).Select(up => new KeyValuePair<string, string>(up.Key, up.Value)).ToListAsync();
                var defaultSettingsTask = ctx2.UserSettings.Where(up => up.Username == "Default").Select(up => new KeyValuePair<string, string>(up.Key, up.Value)).ToListAsync();
                Task[] parallelTasks = [userSettingsTask, defaultSettingsTask];
                await Task.WhenAll(parallelTasks);
                var settings = defaultSettingsTask.Result.ToDictionary();
                foreach (var kvp in userSettingsTask.Result)
                    settings[kvp.Key] = kvp.Value;
                return settings.ToList();
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error when retrieving user settings.", ex);
            }
        }
        
        [Authorize]
        public async Task<List<BusinessTypeClassCodeDm>> GetAllBusinessTypeClassCodes([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            
            return await ctx.BusinessTypeClassCodeDms.ToListAsync();
        }
        
        [Authorize]
        public async Task<List<BusinessTypeDm>> GetAllBusinessTypes([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            
            return await ctx.BusinessTypeDms.ToListAsync();
        }
        
        [Authorize]
        public async Task<List<Sic>> GetAllSicCodes([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            
            return await ctx.Sics.ToListAsync();
        }
        
        [Authorize]
        public async Task<List<IndustryCodeDm>> GetAllIndustryCodes([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            
            return await ctx.IndustryCodeDms
                .OrderBy(o=>o.Code)
                .ToListAsync();
        }

        [Authorize]
        public async Task<AccountLOAsDto> GetAccountLOAs(string accountNum,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx1 = await contextFactory.CreateDbContextAsync();
            var ctx2 = await contextFactory.CreateDbContextAsync();
            
            var approvedLOAByAccountTask = GetApprovedLOAByAccount(accountNum, ctx1);
            var approvedAgencyLOAByAccountTask = GetApprovedAgencyLOAByAccount(accountNum, ctx2);
            
            await Task.WhenAll(approvedAgencyLOAByAccountTask, approvedLOAByAccountTask);

            var response = new AccountLOAsDto
            {
                AccountLOAs = approvedLOAByAccountTask.Result,
                AgencyLOAs = approvedAgencyLOAByAccountTask.Result
            };

            return response;
        }

        private async Task<List<AccountLOADetailDto>> GetApprovedLOAByAccount(string accountNum,
            JamesDatabaseContext ctx)
        {
            var data = await ctx.LineOfAuthorityLogs
                .OrderBy(o => o.AccountNum)
                .ThenBy(t => t.BondType)
                .ThenByDescending(t => t.Effective)
                .Where(r => r.AccountNum == accountNum && r.Status == "Approved")
                .Select(s=> new AccountLOADetailDto
                {
                    Aggregate = s.Loaaggregate,
                    BondType = s.BondType, 
                    Effective = s.Effective,
                    Expiration = s.Expiration,
                    Single = s.Loasingle,
                    Status = s.Status
                })
                .ToListAsync();
            
            var contract = data.FirstOrDefault(f => f.BondType == "Contract");
            var commercial = data.FirstOrDefault(f=>f.BondType == "Commercial");
            
            var result = new List<AccountLOADetailDto>();
            if(contract != null)
                result.Add(contract);
            if(commercial != null)
                result.Add(commercial);
            
            return result;
        }

        private async Task<List<AccountLOADetailDto>> GetApprovedAgencyLOAByAccount(string accountNum,
            JamesDatabaseContext ctx)
        {
            var data =  await ctx.AgencyLineOfAuthorityLogs
                .OrderBy(o => o.AccountNum)
                .ThenBy(t => t.BondType)
                .ThenByDescending(t => t.Effective)
                .Where(r => r.AccountNum == accountNum && r.Status == "Approved")
                .Select(s=> new AccountLOADetailDto
                {
                    Aggregate = s.Loaaggregate,
                    BondType = s.BondType, 
                    Effective = s.Effective,
                    Expiration = s.Expiration,
                    Single = s.Loasingle,
                    Status = s.Status
                })
                .ToListAsync();

            var contract = data.FirstOrDefault(f => f.BondType == "Contract");
            var commercial = data.FirstOrDefault(f=>f.BondType == "Commercial");
            
            var result = new List<AccountLOADetailDto>();
            if(contract != null)
                result.Add(contract);
            if(commercial != null)
                result.Add(commercial);
            
            return result;
        }
    }
}
