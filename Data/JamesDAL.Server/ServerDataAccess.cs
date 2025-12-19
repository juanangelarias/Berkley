using HotChocolate.Subscriptions;
using James.Data.Imaging;
using James.Data.Server.Exceptions;
using James.Data.Server.GraphQL.Mutations;
using James.Data.Server.GraphQL.Queries;
using James.Shared;
using James.Shared.Data;
using James.Shared.Dto;
using James.Shared.Imaging;
using Microsoft.AspNetCore.Http;

namespace James.Data.Server
{
    public class ServerDataAccess(
        IDbContextFactory<JamesDatabaseContext> contextFactory,
        Query query,
        AccountMutation accountMutation,
        AgencyMutation agencyMutation,
        ObligeeMutation obligeeMutation,
        GeneralMutation generalMutation,
        ServerImagingAccess imagingAccess,
        ITopicEventSender eventSender,
        ITopicEventReceiver eventReceiver,
        ILoggingService loggingService,
        IHttpContextAccessor contextAccessor,
        IUserShared userShared, IBrowserStorageCache browserStorageCache) 
        : BaseDataAccess(browserStorageCache, loggingService), IDataAccess
    //TODO: Review if using this with injected classes causes any issues similar to GraphQl queries with injected classes
    {
        public async Task<IDataAccessResult<List<BusinessTypeClassCodeDm>>> GetAllBusinessTypeClassCodes()
        {
            return await ExecuteGet(async () => await query.GetAllBusinessTypeClassCodes(contextFactory));
        }

        public async Task<IDataAccessResult<List<BusinessTypeDm>>> GetAllBusinessTypes()
        {
            return await ExecuteGet(async () => await query.GetAllBusinessTypes(contextFactory));
        }

        public async Task<IDataAccessResult<List<Sic>>> GetAllSicCodes()
        {
            return await ExecuteGet(async () => await query.GetAllSicCodes(contextFactory));
        }

        public async Task<IDataAccessResult<Account>> GetAccountByNumber(string accountNumber)
        {
            return (await ExecuteGet(async () => await query.GetAccountByNumber(accountNumber, contextFactory)))!;
        }

        public async Task<IDataAccessResult<List<AccountProgram>>> GetAccountProgramHistory(string accountNumber)
        {
            return await ExecuteGet(async () => await query.GetAccountProgramHistory(accountNumber, contextFactory));
        }

        public async Task<IDataAccessResult<InforceAccountLOA>> GetInforceAccountLOAsByAccountNumber(
            string accountNumber)
        {
            return await ExecuteGet(async () =>
                await query.GetAccountActiveLinesOfAuthority(accountNumber, contextFactory));
        }

        public async Task<IDataAccessResult<List<AdditionalRelatedParty>>> GetAdditionalRelatedParties(
            string? accountNumber)
        {
            return await ExecuteGet(async () => await query.GetAdditionalRelatedParties(accountNumber, contextFactory));
        }

        public async Task<IDataAccessResult<List<AgencyAccountDto>>> GetAgencyAccounts(string agencyNumber)
        {
            return await ExecuteGet(async () => await query.GetAgencyAccounts(agencyNumber, contextFactory));
        }

        public async Task<IDataAccessResult<List<AgencyAccountBondDto>>> GetAgencyAccountBonds(string accountNum)
        {
            return await ExecuteGet(async () => await query.GetAgencyAccountBonds(accountNum, contextFactory));
        }

        public async Task<IDataAccessResult<Agency?>> GetAgencyByAgencyNumber(string agencyNumber)
        {
            return await ExecuteGet(async () => await query.GetAgencyByAgencyNumber(agencyNumber, contextFactory));
        }

        public async Task<IDataAccessResult<Agency?>> GetAgencyNameAndNumberById(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyById(agencyId, contextFactory));
        }

        public async Task<IDataAccessResult<List<Obligee>>> SearchObligees(string searchString)
        {
            return await ExecuteGet(async () => await query.SearchObligeesAsync(searchString, contextFactory));
        }

        public async Task<IDataAccessResult<List<Account>>> SearchAccounts(string searchString)
        {
            return await ExecuteGet(async () => await query.SearchAccounts(searchString, contextFactory));
        }

        public async Task<IDataAccessResult<Obligee?>> GetObligeeById(Guid id)
        {
            return await ExecuteGet(async () => await query.GetObligeeById(id, contextFactory));
        }

        public async Task<IDataAccessResult<Obligee?>> GetObligeeByObligeeNumber(string obligeeNumber)
        {
            return await ExecuteGet(async () => await query.GetObligeeByObligeeNumber(obligeeNumber, contextFactory));
        }

        public async Task<IDataAccessResult<List<ObligeeTypeDm>>> GetObligeeTypes()
        {
            return await ExecuteGet(async () => await query.GetObligeeTypes(contextFactory));
        }

        public async Task<IDataAccessResult<List<Bond>>> GetObligeePrimaryBonds(Guid obligeeId)
        {
            return await ExecuteGet(async () => await query.GetObligeePrimaryBonds(obligeeId, contextFactory));
        }

        public async Task<IDataAccessResult<List<Bond>>> GetObligeeSecondaryBonds(Guid obligeeId)
        {
            return await ExecuteGet(async () => await query.GetObligeeSecondaryBonds(obligeeId, contextFactory));
        }

        public async Task<IDataAccessResult<List<AgencyInventory>>> GetAgencyInventory(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyInventory(agencyId, contextFactory));
        }

        public async Task<IDataAccessResult<List<AgencyLicense>>> GetAgencyLicenses(Guid agencyId, bool agents)
        {
            return await ExecuteGet(async () => await query.GetAgencyLicenses(agencyId, agents, contextFactory));
        }

        public async Task<IDataAccessResult<List<Insurer>>> GetAllInsurers()
        {
            return await ExecuteGet(async () => await query.GetAllInsurers(contextFactory));
        }

        public async Task<IDataAccessResult<List<Branch>>> GetAllBranches()
        {
            return await ExecuteGet(async () => await query.GetAllBranches(contextFactory));
        }

        public async Task<IDataAccessResult<List<DivisionDm>>> GetDivisions()
        {
            return await ExecuteGet(async () => await query.GetDivisions(contextFactory));
        }

        public async Task<IDataAccessResult<List<Underwriter>>> GetUnderwriters()
        {
            return await ExecuteGet(async () => await query.GetUnderwriters(contextFactory));
        }

        public async Task<IDataAccessResult<List<State>>> GetAllStates()
        {
            return await ExecuteGet(async () => await query.GetAllStates(contextFactory));
        }

        public async Task<IDataAccessResult<List<InventoryDocumentDm>>> GetAllInventoryDocTypes()
        {
            return await ExecuteGet(async () => await query.GetAllInventoryDocTypes(contextFactory));
        }
        public async Task<IDataAccessResult<List<Bond>>> GetAgencyBonds(Guid agencyId, int skip, int take)
        {
            return await ExecuteGet(async () => await query.GetAgencyBonds(agencyId, skip, take, contextFactory));
        }

        public async Task<IDataAccessResult<QueryCount>> GetAgencyBondsCount(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyBondsCount(agencyId, contextFactory));
        }

        public async Task<IDataAccessResult<List<WatchStatusDm>>> GetAllWatchStatuses()
        {
            return await ExecuteGet(async () => await query.GetAllWatchStatuses(contextFactory));
        }

        public async Task<IDataAccessResult<List<Agent>>> GetAgencyAgents(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyAgents(agencyId, contextFactory));
        }
        public async Task<IDataAccessResult<List<AgencyLicense>>> GetAgencyAgentLicenses(Guid agencyId, Guid agentId)
        {
            return await ExecuteGet(async () => await query.GetAgencyAgentLicenses(agencyId, agentId, contextFactory));
        }

        public async Task<IDataAccessResult<List<AgencyStatusDm>>> GetAgencyStatuses()
        {
            return await ExecuteGet(async () => await query.GetAgencyStatuses(contextFactory));
        }

        public async Task<IDataAccessResult<List<AgencyStatusLog>>> GetAgencyStatusLog(string agencyNumber)
        {
            return await ExecuteGet(async () => await query.GetAgencyStatusLog(agencyNumber, contextFactory));
        }

        public async Task<IDataAccessResult<Agent>> GetAgent(Guid agentId)
        {
            return await ExecuteGet(async () => await query.GetAgentByAgentId(agentId, contextFactory));
        }

        public async Task<IDataAccessResult<List<Agent>>> SearchAgents(string searchString)
        {
            return await ExecuteGet(async () => await query.SearchAgents(searchString, contextFactory));
        }
        public async Task<IDataAccessResult<List<AgencyLocationsDto>>> GetAgencyRelatedParties(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyRelatedParties(agencyId, contextFactory));
        }

        public async Task<IDataAccessResult<List<AgencySearchDto>>> SearchAgencies(string? search, bool activeOnly)
        {
            return await ExecuteGet(async () => await query.SearchAgencies(search, activeOnly, contextFactory));
        }

        public async Task<IDataAccessResult<Address>> GetAddress(Guid addressId)
        {
            return await ExecuteGet(async () => await query.GetAddress(addressId, contextFactory));
        }

        public async Task<IDataAccessResult<PhoneNumber>> GetPhoneNumber(Guid phoneId)
        {
            return await ExecuteGet(async () => await query.GetPhoneNumber(phoneId, contextFactory));
        }

        public async Task<IDataAccessResult<List<AddressTypeDm>>> GetAddressTypes()
        {
            return await ExecuteGet(async () => await query.GetAddressTypes(contextFactory));
        }

        public async Task<IDataAccessResult<List<PhoneTypeDm>>> GetPhoneTypes()
        {
            return await ExecuteGet(async () => await query.GetPhoneTypes(contextFactory));
        }

        public async Task<IDataAccessResult<List<Address>>> GetAllLegalEntityAddresses(Guid legalEntityId)
        {
            return await ExecuteGet(async () => await query.GetAllLegalEntityAddresses(legalEntityId, contextFactory));
        }

        public async Task<IDataAccessResult<List<PhoneNumber>>> GetAllLegalEntityPhoneNumbers(Guid legalEntityId)
        {
            return await ExecuteGet(async () =>
                await query.GetAllLegalEntityPhoneNumbers(legalEntityId, contextFactory));
        }

        public async Task<IDataAccessResult<List<LegalEntityEmail>>> GetAllLegalEntityEmails(Guid legalEntityId)
        {
            return await ExecuteGet(async () => await query.GetAllLegalEntityEmails(legalEntityId, contextFactory));
        }

        public async Task<IDataAccessResult<List<EmailTypeDm>>> GetAllEmailTypes()
        {
            return await ExecuteGet(async () => await query.GetEmailTypes(contextFactory));
        }

        public async Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId, bool activeOnly)
        {
            return await ExecuteGet(async () => await query.GetAgencyPOAs(agencyId, activeOnly, contextFactory));
        }

        public async Task<IDataAccessResult<List<PowerOfAttorneyStatusDm>>> GetAllPoaStatuses()
        {
            return await ExecuteGet(async () => await query.GetAllPoaStatuses(contextFactory));
        }

        public async Task<IDataAccessResult<List<AgencyCommission>>> GetAgencyCommissionRates(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyCommissionRates(agencyId, contextFactory));
        }

        public async Task<IDataAccessResult<Employee>> GetEmployeeByUserName(string userName)
        {
            return await ExecuteGet(async () => await query.GetEmployeeByUserName(userName, contextFactory));
        }

        public async Task<ISaveDataResult> SetAccountGeneralInfo(Guid accountId, string? yearStarted,
            string? currentManagementYear, string? businessClass,
            string? businessType, string? priorSurety, int? estAnnualPremium)
        {
            return await ExecuteSave(async () => await accountMutation.SetAccountGeneralInfo(accountId, yearStarted,
                currentManagementYear, businessClass, businessType, priorSurety, estAnnualPremium, contextFactory));
        }

        public async Task<ISaveDataResult> SetAccountGeneralInfoPanel(Guid accountId, string? fiscalYearEnd, 
            string? businessType, string? industryCode, string? priorSuretyCompany)
        {
            return await ExecuteSave(async () => await accountMutation.SetAccountGeneralInfoPanel(accountId, 
                fiscalYearEnd, businessType, industryCode, priorSuretyCompany, contextFactory));
        }
        
        public async Task<IDataAccessResult<AccountLOAsDto>> GetAccountLOAs(string accountNumber)
        {
            return await ExecuteGet(async () => await query.GetAccountLOAs(accountNumber, contextFactory));
        }

        public async Task<ISaveDataResult> SetAccountSystems(Guid accountId, string? estimatingSystem,
            string? estimatingSignoff, string? internalAccountingSystem,
            bool? interimWips, bool? interimPOCs)
        {
            return await ExecuteSave(async () => await accountMutation.SetAccountSystems(accountId, estimatingSystem,
                estimatingSignoff, internalAccountingSystem, interimWips, interimPOCs, contextFactory));
        }

        public async Task<ISaveDataResult> SetAccountAdditionalInformation(Guid accountId, bool? fullIndemnity,
            bool? corpIndemnity, bool? personalIndemnity,
            bool? keyManagementLifeInsurance, bool? managementIncentives, bool? fundedBuySell,
            bool? multipleActiveOwners,
            bool? trackCommAccount, bool? berkleyAffiliate, string? comments)
        {
            return await ExecuteSave(async () => await accountMutation.SetAccountAdditionalInformation(accountId,
                fullIndemnity, corpIndemnity, personalIndemnity, keyManagementLifeInsurance,
                managementIncentives, fundedBuySell, multipleActiveOwners, trackCommAccount, berkleyAffiliate, comments,
                contextFactory));
        }

        public async Task<IDataAccessResult<LegalEntityEmail>> SetLegalEntityEmail(Guid id, Guid legalEntityId,
            string emailAddress, string type)
        {
            return await ExecuteGet(async () =>
                await generalMutation.SetLegalEntityEmail(id, legalEntityId, emailAddress, type, contextFactory));
        }

        public async Task<ISaveDataResult> DeleteLegalEntityEmail(Guid id)
        {
            return await ExecuteSave(async () => await generalMutation.DeleteLegalEntityEmail(id, contextFactory));
        }

        public async Task<IDataAccessResult<AccountProgram>> SetAccountProgram(Guid programId, DateTime effective,
            DateTime expiration, int single, int aggregate,
            string? comments, Guid statusId)
        {
            return await ExecuteGet(async () => await accountMutation.SetAccountProgram(programId, effective,
                expiration, single, aggregate, comments, statusId, contextFactory));
        }

        public async Task<IDataAccessResult<PowerOfAttorney>> SetPowerOfAttorney(Guid poaId, Guid insurerId, int? limit,
            string? referenceNumber, DateOnly? firstIssued, DateOnly? currentIssued, string? comments, string status)
        {
            return await ExecuteGet(async () => await agencyMutation.SetPowerOfAttorney(poaId, insurerId, limit,
                referenceNumber, firstIssued, currentIssued, comments, status, eventSender, contextFactory));
        }

        public async Task<ISaveDataResult> SetPowerOfAttorneyDocumentLink(Guid poaId, Guid? imagingDocumentId)
        {
            return await ExecuteSave(async () =>
                await agencyMutation.CreateAgencyPOADocumentLink(poaId, imagingDocumentId, eventSender,
                    contextFactory));
        }

        public async Task<ISaveDataResult> SetAgencyLicenseDocumentLink(Guid licenseId, Guid? imagingDocumentId)
        {
            return await ExecuteSave(async () =>
                await agencyMutation.CreateAgencyLicenseDocumentLink(licenseId, imagingDocumentId, eventSender,
                    contextFactory));
        }

        public async Task<ISaveDataResult> SetAccountCreditReportDocumentLink(Guid? documentId, string accountNum)
        {
            return await ExecuteSave(async () =>
                await accountMutation.SetCurrentCreditReportLink(accountNum, documentId, eventSender, contextFactory));
        }

        public async Task<ISaveDataResult> SetAddress(Address address, string identifier)
        {
            return await ExecuteSave((async () => await generalMutation.SetAddress(address.Id, address.Address1,
                address.Address2, address.Address3, address.City, address.StateCode, address.PostalCode, identifier,
                eventSender, contextFactory, loggingService)));
        }

        public async Task<ISaveDataResult> CreateAddress(Guid addressId, string address1, string? address2,
            string? address3, string city, string? stateCode, string? postalCode,
            Guid legalEntityId, string addressType, string identifier)
        {
            return await ExecuteSave(async () =>
                await generalMutation.CreateAddress(addressId, address1, address2, address3, city, stateCode,
                    postalCode, legalEntityId, addressType, identifier, eventSender, contextFactory, loggingService));
        }

        public async Task<ISaveDataResult> CreatePhoneNumber(Guid phoneId, string? countryCode, string mainNumber,
            string? extension,
            Guid legalEntityId, string phoneType)
        {
            return await ExecuteSave(async () => await generalMutation.CreatePhoneNumber(phoneId, countryCode,
                mainNumber, extension, legalEntityId, phoneType, contextFactory));
        }

        public async Task<ISaveDataResult> DeleteAddress(Guid addressId, string identifier)
        {
            return await ExecuteSave(async () =>
                await generalMutation.DeleteAddress(addressId, identifier, eventSender, contextFactory,
                    loggingService));
        }

        public async Task<IDataAccessResult<List<CountryDm>>> GetAllCountries()
        {
            return await ExecuteGet(async () => await query.GetAllCountries(contextFactory));
        }

        public async Task<ISaveDataResult> DeletePhoneNumber(Guid phoneId)
        {
            return await ExecuteSave(async () => await generalMutation.DeletePhoneNumber(phoneId, contextFactory));
        }

        public async Task<ISaveDataResult> SetAgencyGeneralInfo(Guid agencyId, string agencyName, Guid parentId,
            string? taxId, string? npn, bool w9, bool need1099, bool nasbp, string branchKey)
        {
            try
            {
                var result = await agencyMutation.SetAgencyGeneralInfo(agencyId, agencyName, parentId, taxId, npn, w9,
                    need1099, nasbp, branchKey, contextFactory);
                return new SaveDataResult { Errors = result ? [] : ["SetAgencyGeneralInfo failed."] };
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }

        public async Task<ISaveDataResult> SetAgencyProfitSharingInfo(Guid agencyId, bool profitSharing,
            int? profitSharingMinimumPremium)
        {
            try
            {
                _ = await agencyMutation.SetAgencyProfitSharingInfo(agencyId, profitSharing,
                    profitSharingMinimumPremium, contextFactory);
                return new SaveDataResult();
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }

        public async Task<ISaveDataResult> SetAgencyInventory(Guid inventoryId, DateTime? sent, int? quantity,
            string documentType, string? addressee, Guid addressId, string address1, string? address2, string? address3,
            string city, string? stateCode, string? postalCode)
        {
            try
            {
                await agencyMutation.SetAgencyInventory(inventoryId, sent, quantity, documentType, addressee, addressId,
                    address1, address2, address3, city, stateCode, postalCode,
                    eventSender, contextFactory, loggingService);
                return new SaveDataResult();
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }

        public async Task<ISaveDataResult> CreateAgencyStatusLog(Guid id, string agencyNumber, DateTime effective,
            string oldStatus, string newStatus, Guid changedBy, string? comments)
        {
            try
            {
                var result = await agencyMutation.CreateAgencyStatusLog(id, agencyNumber, effective, oldStatus,
                    newStatus, changedBy, comments, eventSender, contextFactory);
                return new SaveDataResult { Errors = result ? [] : ["CreateAgencyStatusLog failed."] };
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }

        public async Task<ISaveDataResult> CreateAgencyPOA(Guid poaId, Guid insurerId, Guid agencyId, int limit,
            string? referenceNumber, DateOnly? firstIssued, DateOnly? currentIssued, string? comments, string status)
        {
            try
            {
                var result = await agencyMutation.CreateAgencyPOA(poaId, insurerId, agencyId, limit, referenceNumber,
                    firstIssued, currentIssued, comments, status, eventSender, contextFactory);
                return new SaveDataResult { Errors = result ? [] : ["CreateAgencyPOA failed."] };
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }

        public async Task<ISaveDataResult> DeleteAgencyPOA(Guid poaId)
        {
            try
            {
                var result = await ExecuteSave(async () =>
                    await agencyMutation.DeleteAgencyPOA(poaId, eventSender, contextFactory));

                return result;
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }

        public async Task<ISaveDataResult> AgencyLicenseBulkDelete(List<Guid> licenseIds)
        {
            var result = await ExecuteSave(async () =>
                await agencyMutation.AgencyLicenseBulkDelete(licenseIds, contextFactory));

            return result;
        }

        public async Task<ISaveDataResult> AgencyLicenseBulkInsert(List<AgencyLicenseBulk> licenses)
        {
            var result = await ExecuteSave((async () =>
                await agencyMutation.AgencyLicenseBulkInsert(licenses, contextFactory)));

            return result;
        }

        public async Task<ISaveDataResult> AgencyLicenseBulkUpdate(List<AgencyLicenseBulk> licenses)
        {
            var result = await ExecuteSave(async () =>
                await agencyMutation.AgencyLicenseBulkUpdate(licenses, contextFactory));

            return result;
        }

        public async Task<ISaveDataResult> CreateLicense(Guid licenseId, Guid agencyId, Guid? agentId,
            bool appointingState, string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state, bool isActive)
        {
            try
            {
                var result = await agencyMutation.CreateLicense(licenseId, agencyId, agentId, appointingState,
                    comments, appointment, expiration, termination,
                    insurerId, isResident, licenseNumber, state, isActive, eventSender, contextFactory, loggingService);
                return new DataAccessResult<bool> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<bool> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<bool> { Errors = [ex.Message] };
            }
        }

        public async Task<ISaveDataResult> CreateAgencyInventory(Guid inventoryId, Guid agencyId, DateTime dateSent,
            int quantity, string documentType, string addressee, string address1, string? address2, string? address3,
            string city, string? stateCode, string? postalCode, Guid approverId)
        {
            try
            {
                var result = await agencyMutation.CreateAgencyInventory(inventoryId, agencyId, dateSent, quantity,
                    documentType, addressee, address1, address2, address3, city, stateCode, postalCode, approverId,
                    eventSender, contextFactory);
                return new SaveDataResult { Errors = result ? [] : ["CreateAgencyInventory failed."] };
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }

        public async Task<IDataAccessResult<Obligee>> CreateObligee(Guid id, string fullName, string obligeeType,
            bool printStatusLetter, string? notes,
            string address1, string? address2, string city, string state, string postalCode, string? phoneNumber,
            string? email)
        {
            try
            {
                var result = await obligeeMutation.CreateObligee(id, fullName, obligeeType, printStatusLetter, notes,
                    address1, address2, city, state, postalCode, phoneNumber, email, eventSender, contextFactory);
                return new DataAccessResult<Obligee>() { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<Obligee> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<Obligee> { Errors = [ex.Message] };
            }
        }

        public async Task<IDataAccessResult<AgencyLicense>> SetAgencyLicense(Guid licenseId, Guid agencyId,
            Guid? agentId, bool appointingState, string? comments,
            DateOnly? appointment, DateOnly? expiration, DateOnly? termination, Guid insurerId, bool isResident,
            string? licenseNumber, string state, bool isActive)
        {
            return await ExecuteGet(async () => await agencyMutation.SetAgencyLicense(licenseId, agencyId, agentId,
                appointingState, comments, appointment, expiration, termination, insurerId, isResident, licenseNumber,
                state, isActive, eventSender, contextFactory));

            //try
            //{
            //    var result = await agencyMutation.SetAgencyLicense(licenseId,agencyId, agentId, appointingState,
            //        comments, appointment, expiration, termination,
            //        insurerId, isResident, licenseNumber, state, isActive, eventSender, contextFactory);
            //    return new DataAccessResult<AgencyLicense> { DataObject = result };
            //}
            //catch (AggregateException ae)
            //{
            //    return new DataAccessResult<AgencyLicense> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            //}
            //catch (Exception ex)
            //{
            //    return new DataAccessResult<AgencyLicense> { Errors = [ex.Message] };
            //}
        }

        public async Task<ISaveDataResult> DeleteLicense(Guid licenseId)
        {
            return await ExecuteSave(async () =>
                await agencyMutation.DeleteLicense(licenseId, eventSender, contextFactory));
        }

        public async Task<ISaveDataResult> DeleteAgencyInventory(Guid inventoryId)
        {
            return await ExecuteSave(async () =>
                await agencyMutation.DeleteAgencyInventory(inventoryId, eventSender, contextFactory, loggingService));
        }

        public async Task<ISaveDataResult> SetAgencyCommissionRate(AgencyCommission rate)
        {
            return await ExecuteSave(async () =>
                await agencyMutation.SaveCommissionRates(rate, eventSender, contextFactory));
        }

        public async Task<ISaveDataResult> DeleteAgencyCommissionRate(Guid commRateId)
        {
            return await ExecuteSave(async () =>
                await agencyMutation.DeleteAgencyCommissionRate(commRateId, eventSender, contextFactory));
        }

        public async Task<IDataAccessResult<List<AgencyDto>>> GetAllActiveAgencies()
        {
            var response = await ExecuteGet(async () => await query.GetAllActiveAgencies(contextFactory));
            return response!;
        }

        public async Task<IDataAccessResult<BondRequestNumberType>> GetBondRequestNumberType(string bondNumber)
        {
            var response =
                await ExecuteGet(async () => await query.GetBondRequestNumberType(bondNumber, contextFactory));

            if (response == null)
                throw new NotFoundException("Bond Request Number Type not found");

            return response!;
        }

        public async Task<IDataAccessResult<string?>> GetBondNumber(string bondRequestNumber)
        {
            return await ExecuteGet(async () => await query.GetBondNumber(bondRequestNumber, contextFactory));
        }
        public async Task<IDataAccessResult<List<BondBlock>>> GetBondBlocksByAgency(Guid agencyId, DateTime start, DateTime end, string filter)
        {
            return await ExecuteGet(async () => await query.GetBondBlocksByAgency(agencyId, start, end, filter, contextFactory));
        }
        public async Task<IDataAccessResult<List<Bond>>> GetBondsByBlock(Guid bondBlockId)
        {
            return await ExecuteGet(async () => await query.GetBondsByBlock(bondBlockId, contextFactory));
        }

        public async Task<IDataAccessResult<List<ImagingType>>> GetAllImagingTypes()
        {
            return await ExecuteGet(async () => await query.GetAllImagingTypes(contextFactory));
        }

        public async Task<IDataAccessResult<List<VImagingCategoryTabDivisionType>>>
            GetAllImagingCategoryTabDivisionTypes()
        {
            return await ExecuteGet(async () => await query.GetAllImagingCategoryTabDivisionType(contextFactory));
        }

        public async Task<IDataAccessResult<ImagingDocument?>> GetImagingDocumentsDetails(
            ImagingDocumentCategory docCategory, Guid documentId)
        {
            return await ExecuteGet(async () => await query.GetDocumentDetails(docCategory, documentId, imagingAccess));
        }

        public async Task<IDataAccessResult<List<SecurityRole>>> GetAllSecurityRoles()
        {
            return await ExecuteGet(async () => await query.GetAllSecurityRoles(contextFactory));
        }

        public async Task<IDataAccessResult<List<SecurityRole>>> GetSecurityRolesByUserId(Guid userId)
        {
            return await ExecuteGet(async () => await query.GetSecurityRolesByUser(userId, contextFactory));
        }

        public async Task<IDataAccessResult<List<SecurityRoleMember>>> GetSecurityRoleMembers(string role)
        {
            return await ExecuteGet(async () => await query.GetSecurityRoleMembers(role, contextFactory));
        }

        public async Task<ISaveDataResult> AddPrincipalToSecurityRole(Guid principalId, string role)
        {
            return await ExecuteGet(async () =>
                await generalMutation.AddPrincipalToSecurityRole(principalId, role, contextFactory, loggingService));
        }

        public async Task<ISaveDataResult> RemovePrincipalFromSecurityRole(Guid principalId, string role)
        {
            return await ExecuteGet(async () =>
                await generalMutation.RemovePrincipalFromSecurityRole(principalId, role, contextFactory,
                    loggingService));
        }

        public async Task<ISaveDataResult> AddSecurityRole(SecurityRole role)
        {
            return await ExecuteSave(async () =>
                await generalMutation.AddSecurityRole(role.Role, role.Description!, role.Ord, contextFactory,
                    loggingService));
        }

        public async Task<IDataAccessResult<List<Employee>>> GetEmployees(bool activeOnly = true)
        {
            return await ExecuteGet(async () => await query.GetEmployees(activeOnly, contextFactory));
        }

        public async Task<IDataAccessResult<List<PotentialEmployeeActiveDirectoryInfo>>> GetActiveDirectoryUsers(
            string usernameSearchText)
        {
            return await ExecuteGet(async () => await query.GetActiveDirectoryUsers(usernameSearchText));
        }

        public async Task<ISaveDataResult> CreateEmployee(string username, string fullName, string initials,
            string title, string email)
        {
            return await ExecuteSave(async () =>
                await generalMutation.CreateEmployee(username, fullName, initials, title, email, contextFactory));
        }

        public async Task<ISaveDataResult> SetEmployeeEmail(Guid employeeId, string email)
        {
            return await ExecuteSave(async () =>
                await generalMutation.SetEmployeeEmail(employeeId, email, contextFactory));
        }

        public async Task<IDataAccessResult<List<PowerOfAttorneyDocumentNameDm>>> GetPoaDocumentNames()
        {
            return await ExecuteGet(async () => await query.GetPOADocumentNames(contextFactory));
        }

        public async Task<IDataAccessResult<PowerOfAttorneyDocumentStatus>> SetPowerOfAttorneyDocumentStatus(Guid id,
            DateTime? requested, DateTime? received, Guid documentTypeId, string? comments)
        {
            return await ExecuteGet(async () => await agencyMutation.SetPowerOfAttorneyDocumentStatus(id, requested,
                received, documentTypeId, comments, eventSender, contextFactory));
        }

        public async Task<IDataAccessResult<AccountWatch>> CreateAccountWatch(Guid id, string accountNum,
            DateTime watchDate,
            string watchStatus, string reason, string actionPlan)
        {
            return await ExecuteGet(async () => await accountMutation.CreateAccountWatch(id, accountNum, watchDate,
                watchStatus, reason, actionPlan, contextFactory));
        }

        public async Task<IDataAccessResult<AccountWatch>> UpdateAccountWatch(Guid id, string watchStatus,
            string reason, string actionPlan)
        {
            var response = await ExecuteGet(async () =>
                await accountMutation.UpdateAccountWatch(id, watchStatus, reason, actionPlan, contextFactory));

            return response.Data != null
                ? response
                : new DataAccessResult<AccountWatch>
                {
                    Data = null,
                    Errors = ["Account Watch not found"]
                };
        }

        public async Task<ISaveDataResult> SetAccountCommercialInfo(Guid accountId, string fullName, Guid underwriterId,
            string branchKey, string divisionCode, Guid sicCodeId, Guid hoLead)
        {
            var response = await ExecuteSave(async () => await accountMutation.SetAccountCommercialInfo(accountId,
                fullName, underwriterId, branchKey, divisionCode, sicCodeId, hoLead, contextFactory));

            return response;
        }

        public async Task<ISaveDataResult> SetAccountAgencyAndAgent(Guid accountId, string agencyNumber, Guid? agentId)
        {
            var response = await ExecuteSave(async () =>
                await accountMutation.SetAccountAgencyAndAgent(accountId, agencyNumber, agentId, contextFactory));

            return response;
        }

        public async Task<ISaveDataResult> DeleteAccountWatch(Guid id)
        {
            var response = await ExecuteGet(async () => await accountMutation.DeleteAccountWatch(id, contextFactory));

            return response;
        }

        public async Task<IDataAccessResult<List<AccountWatch>>> GetAccountWatches(string accountNum)
        {
            return await ExecuteGet(async () => await query.GetAllAccountWatches(accountNum, contextFactory));
        }

        public async Task<IDataAccessResult<DateOnly?>> GetFirstIndemnityDate(string accountNum)
        {
            return await ExecuteGet(async () => await query.GetFirstIndemnity(accountNum, contextFactory));
        }

        public async Task<IDataAccessResult<AccountAlertPackageDto>> GetAccountAlerts(int period, string accountNum)
        {
            return await ExecuteGet(async () => await query.GetAccountAlerts(period, accountNum, contextFactory));
        }
        
        public async Task<IDataAccessResult<PrivateEquity?>> GetLastPrivateEquityByAccount(string accountNum)
        {
            return await ExecuteGet(async () => await query.GetLastPrivateEquityByAccount(accountNum, contextFactory));
        }
        
        public async Task<IDataAccessResult<Indemnitor?>> GetLastIndemnitorByAccount(string accountNum)
        {
            return await ExecuteGet(async () => await query.GetLastIndemnitorByAccount(accountNum, contextFactory));
        }

        private async Task<IDataAccessResult<T>> ExecuteGet<T>(Func<Task<T>> dataFunc)
        {
            try
            {
                var result = await dataFunc();
                return new DataAccessResult<T> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<T> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<T> { Errors = [ex.Message] };
            }
        }

        private static async Task<ISaveDataResult> ExecuteSave(Func<Task> saveAction)
        {
            try
            {
                await saveAction();
                return new SaveDataResult();
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }

        public IDisposable AddressModified(Guid addressId, Action<SubscriptionResult<Address>> onNext,
            Action<Exception>? onError = null, Action? onComplete = null)
        {
            return OnAddressModified(addressId)
                .Subscribe(new ServerSideSubscriptionSubscriber<SubscriptionResult<Address>>(onNext, onError,
                    onComplete));
        }

        public IDisposable AddressCollectionModified(Guid addressId, Action<SubscriptionResult<Guid>> onNext,
            Action<Exception>? onError = null, Action? onComplete = null)
        {
            return OnAddressCollectionModified(addressId)
                .Subscribe(new ServerSideSubscriptionSubscriber<SubscriptionResult<Guid>>(onNext, onError, onComplete));
        }

        public IDisposable SearchResultReady(string searchTerm,
            Action<SubscriptionResult<List<JamesSearchResult>>> onNext, Action<Exception>? onError = null,
            Action? onComplete = null)
        {
            return OnSearchResultReady(searchTerm)
                .Subscribe(new ServerSideSubscriptionSubscriber<SubscriptionResult<List<JamesSearchResult>>>(onNext,
                    onError, onComplete));
        }

        public Task<ISaveDataResult> StartSuperSearch(string searchTerm, SearchOptions options)
        {
            return ExecuteSave(async () =>
                await query.Search(searchTerm, options, eventSender, contextFactory, loggingService));
        }

        public Task<IDataAccessResult<List<Account>>> GetIdAccountNumbers()
        {
            return ExecuteGet(async () => await query.GetIdAccountNumbers(contextFactory));
        }

        public Task<IDataAccessResult<List<Agency>>> GetIdAgencyNumbers()
        {
            return ExecuteGet(async () => await query.GetIdAgencyNumbers(contextFactory));
        }

        public async Task<IDataAccessResult<List<ImagingDocument>>> SearchDocuments(string imagingId,
            ImagingDocumentCategory docCategory, string? documentType = null)
        {
            return await ExecuteGet(async () =>
                await query.SearchDocumentsAsync(imagingId, docCategory, documentType, contextFactory,
                    imagingAccess));
        }

        public async Task<IDataAccessResult<ImagingSearchCriteria>> GetImagingSearchCriteria(string id,
            ImagingDocumentCategory docCategory,
            bool useDocCategoryAsCriteria = true)
        {
            return await ExecuteGet(async () => await ImagingSearchCriteria(id, docCategory, useDocCategoryAsCriteria));
        }
        
        public async Task<IDataAccessResult<List<IndustryCodeDm>>> GetAllIndustryCodes()
        {
            return await ExecuteGet(async () => await query.GetAllIndustryCodes(contextFactory));
        }
        
        #region User Settings

        public async Task<ISaveDataResult> SetUserSettings(string key, string value)
        {
            return await ExecuteSave(async () =>
                await generalMutation.SetUserSetting(key, value, contextFactory, contextAccessor, loggingService));
        }
        
        public async Task<ISaveDataResult> ResetUserSettings()
        {
            //HACK:  This was written for developer testing and has not been fully tested to be used in the actual application.
            return await ExecuteSave(async () =>
                await generalMutation.ResetUserSettings(contextFactory, contextAccessor));
        }

        public async Task<ISaveDataResult> ResetUserSetting(string key)
        {
            //HACK:  This was written for developer testing and has not been fully tested to be used in the actual application.
            return await ExecuteSave(async () => await generalMutation.ResetUserSetting(key, contextFactory, contextAccessor));
        }
        
        public async Task<IDataAccessResult<List<KeyValue>>> GetAllUserSettings()
        {
            return await ExecuteGet(async () => new List<KeyValue>(await query.GetAllUserSettings(contextFactory, contextAccessor)));
        }

        public async Task<ISaveDataResult> SetUserSetting(string key, string? value)
        {
            return await ExecuteSave(async () => await generalMutation.SetUserSetting(key, value, contextFactory, contextAccessor, loggingService));
        }
        
        public async Task<ISaveDataResult> SetDefaultUserSetting(string key, string? value)
        {
            return await ExecuteSave(async () => await generalMutation.SetDefaultUserSetting(key, value, contextFactory, loggingService ));
        }

        #endregion
        
        #region BondBlock

        public async Task<ISaveDataResult> SetBondBlock(BondBlock block)
        {
            return await ExecuteSave(async () => await generalMutation.SetBondBlock(block.Id, null, block.Prefix,
                block.FirstNumber, block.LastNumber, block.AgencyRestricted, block.IssuedBy, block.Comments, 
                block.AgencyId, block.Enabled, contextFactory));
        }
        
        public async Task<ISaveDataResult> DeleteBondBlock(Guid id)
        {
            return await ExecuteSave(async () => await generalMutation.DeleteBondBlock(id, contextFactory));
        }

        public Task<IDataAccessResult<int>> NextBondBlockInitialNumber(string prefix)
        {
            return ExecuteGet(async () => await query.GetNextBondBlockInitialNumber(prefix, contextFactory));
        }

        #endregion

        private async Task<ImagingSearchCriteria> ImagingSearchCriteria(string id, ImagingDocumentCategory docCategory,
            bool useDocCategoryAsCriteria = true)
        {
            id = id.Trim();
            var criteria = new ImagingSearchCriteria()
            {
                MaxResults = 2000,
                Fields = string.Join(",", ImagingAccessBase.DocumentPropertyFields)
            };
            if (useDocCategoryAsCriteria)
            {
                criteria.DocClass = docCategory.DocumentCategory();
            }

            string? bondNumber;
            switch (docCategory)
            {
                case ImagingDocumentCategory.Account: //1
                    criteria.WhereClause = $"{ImagingAccessBase.AccountId} = '{id}'";
                    break;
                case ImagingDocumentCategory.Bond: //2
                    criteria.WhereClause = $"{ImagingAccessBase.PolicyNo} = '{id}'";
                    var bidBondType = (await GetBondRequestNumberType(id)).Data;
                    //TODO: Handle errors above
                    criteria.WhereClause =
                        $"({criteria.WhereClause} OR {(string.Equals(bidBondType?.Type, "CONTRACT", StringComparison.InvariantCultureIgnoreCase) ? ImagingAccessBase.ContBidId : ImagingAccessBase.CommBidId)} = '{bidBondType?.BondRequestNumber}')";
                    break;
                case ImagingDocumentCategory.Agency: //3
                    criteria.WhereClause = $"{ImagingAccessBase.AgencyNo} = '{id}'";
                    break;
                case ImagingDocumentCategory.CommBid: //4
                    criteria.WhereClause = $"{ImagingAccessBase.CommBidId} = '{id}'";
                    bondNumber = (await GetBondNumber(id)).Data;
                    //TODO:Handle GraphQl errors
                    if (!string.IsNullOrWhiteSpace(bondNumber))
                        criteria.WhereClause =
                            $"({criteria.WhereClause} OR {ImagingAccessBase.PolicyNo} = '{bondNumber}')";
                    break;
                case ImagingDocumentCategory.ContBid: //5
                    criteria.WhereClause = $"{ImagingAccessBase.ContBidId} = '{id}'";
                    bondNumber = (await GetBondNumber(id)).Data;
                    //TODO:Handle GraphQl errors
                    if (!string.IsNullOrWhiteSpace(bondNumber))
                        criteria.WhereClause =
                            $"({criteria.WhereClause} OR {ImagingAccessBase.PolicyNo} = '{bondNumber}')";
                    break;
                case ImagingDocumentCategory.Billing: //SearchBillingDocuments
                    criteria.WhereClause = $"{ImagingAccessBase.AccountId} = '{id}''";
                    break;
                default:
                    throw new ArgumentException("Invalid docCategory", nameof(docCategory));
            }

            return criteria;
        }

        private readonly Dictionary<Guid, ServerSideSubscription<SubscriptionResult<Address>>> _onAddressModified =
            new();

        private ServerSideSubscription<SubscriptionResult<Address>> OnAddressModified(Guid addressId)
        {
            lock (_onAddressModified)
            {
                if (_onAddressModified.ContainsKey(addressId) == false)
                {
                    _onAddressModified[addressId] = new(eventReceiver
                        .SubscribeAsync<SubscriptionResult<Address>>("OnAddressModified_" + addressId).Result
                        .ReadEventsAsync(), CancellationToken.None);
                }

                return _onAddressModified[addressId];
            }
        }

        private readonly Dictionary<Guid, ServerSideSubscription<SubscriptionResult<Guid>>>
            _onAddressCollectionModified = new();

        private ServerSideSubscription<SubscriptionResult<Guid>> OnAddressCollectionModified(Guid legalEntityId)
        {
            lock (_onAddressCollectionModified)
            {
                if (_onAddressCollectionModified.ContainsKey(legalEntityId) == false)
                {
                    _onAddressCollectionModified[legalEntityId] = new(eventReceiver
                        .SubscribeAsync<SubscriptionResult<Guid>>("OnAddressCollectionModified_" + legalEntityId).Result
                        .ReadEventsAsync(), CancellationToken.None);
                }

                return _onAddressCollectionModified[legalEntityId];
            }
        }

        private readonly Dictionary<string, ServerSideSubscription<SubscriptionResult<List<JamesSearchResult>>>>
            _onSearchResultReady = new();

        private ServerSideSubscription<SubscriptionResult<List<JamesSearchResult>>> OnSearchResultReady(
            string searchTerm)
        {
            lock (_onSearchResultReady)
            {
                if (_onSearchResultReady.ContainsKey(searchTerm) == false)
                {
                    _onSearchResultReady[searchTerm] = new(eventReceiver
                        .SubscribeAsync<SubscriptionResult<List<JamesSearchResult>>>("Srch_" + searchTerm).Result
                        .ReadEventsAsync(), CancellationToken.None);
                }

                return _onSearchResultReady[searchTerm];
            }
        }
    }
}