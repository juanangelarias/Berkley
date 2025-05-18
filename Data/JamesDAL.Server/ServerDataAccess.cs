using HotChocolate.Subscriptions;
using James.Data.Imaging;
using James.Data.Server.GraphQL.Mutations;
using James.Data.Server.GraphQL.Queries;
using James.Shared;
using James.Shared.Data;
using James.Shared.Imaging;

namespace James.Data.Server
{
    //TODO: Review if using this with injected classes causes any issues similar to GraphQl queries with injected classes
    public class ServerDataAccess(IDbContextFactory<JamesDatabaseContext> contextFactory, Query query, AccountMutation accountMutation, AgencyMutation agencyMutation, ObligeeMutation obligeeMutation, GeneralMutation generalMutation, ServerImagingAccess imagingAccess, ITopicEventSender eventSender, ITopicEventReceiver eventReceiver, ILoggingService loggingService) : IDataAccess
    {
        public async Task<IDataAccessResult<Account>> GetAccountByNumber(string accountNumber)
        {
            return (await ExecuteGet(async () => await query.GetAccountByNumber(accountNumber, contextFactory)))!;
        }
        public async Task<IDataAccessResult<List<AccountProgram>>> GetAccountProgramHistory(string accountNumber)
        {
            return await ExecuteGet(async () => await query.GetAccountProgramHistory(accountNumber, contextFactory));
        }
        public async Task<IDataAccessResult<InforceAccountLOA>> GetInforceAccountLOAsByAccountNumber(string accountNumber)
        {
            return await ExecuteGet(async () => await query.GetAccountActiveLinesOfAuthority(accountNumber, contextFactory));
        }
        public async Task<IDataAccessResult<List<AdditionalRelatedParty>>> GetAdditionalRelatedParties(string? accountNumber)
        {
            return await ExecuteGet(async () => await query.GetAdditionalRelatedParties(accountNumber, contextFactory));
        }
        public async Task<IDataAccessResult<List<Account>>> GetAgencyAccounts(string agencyNumber)
        {
            return await ExecuteGet(async () => await query.GetAgencyAccounts(agencyNumber, contextFactory));
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
        public async Task<IDataAccessResult<List<AgencyLicense>>> GetAgencyLicenses(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyLicenses(agencyId, contextFactory));
        }
        public async Task<IDataAccessResult<List<Insurer>>> GetAllInsurers()
        {
            return await ExecuteGet(async () => await query.GetAllInsurers(contextFactory));
        }
        public async Task<IDataAccessResult<List<Branch>>> GetAllBranches()
        {
            return await ExecuteGet(async () => await query.GetAllBranches(contextFactory));
        }
        public async Task<IDataAccessResult<List<State>>> GetAllStates()
        {
            return await ExecuteGet(async () => await query.GetAllStates(contextFactory));
        }
        public async Task<IDataAccessResult<List<InventoryDocumentDm>>> GetAllInventoryDocTypes()
        {
            return await ExecuteGet(async () => await query.GetAllInventoryDocTypes(contextFactory));
        }
        public async Task<IDataAccessResult<List<Bond>>> GetAgencyBonds(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyBonds(agencyId, contextFactory));
        }
        public async Task<IDataAccessResult<List<Agent>>> GetAgencyAgents(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyAgents(agencyId, contextFactory));
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
        public async Task<IDataAccessResult<List<Agency>>> GetAgencyRelatedParties(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyRelatedParties(agencyId, contextFactory));
        }

        public async Task<IDataAccessResult<List<Agency>>> SearchAgencies(string? search, bool activeOnly)
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
            return await ExecuteGet(async () => await query.GetAllLegalEntityPhoneNumbers(legalEntityId, contextFactory));
        }
        public async Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyPOAs(agencyId, contextFactory));
        }
        public async Task<IDataAccessResult<List<PowerOfAttorneyDocumentNameDm>>> GetPOADocumentNames()
        {
            return await ExecuteGet(async () => await query.GetPOADocumentNames(contextFactory));
        }
        public async Task<IDataAccessResult<List<PowerOfAttorneyStatusDm>>> GetAllPoaStatuses()
        {
            return await ExecuteGet(async () => await query.GetAllPoaStatuses(contextFactory));
        }

        public async Task<IDataAccessResult<List<AgencyCommission>>> GetAgencyCommissionRates(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyCommissionRates(agencyId, contextFactory));
        }
        public async Task<IDataAccessResult<UserProfile>> GetUserProfileByUserName(string userName)
        {
            return await ExecuteGet(async () => await query.GetUserProfileByUserName(userName, contextFactory));
        }
        public async Task<ISaveDataResult> SetAccountGeneralInfo(Guid accountId, string? yearStarted, string? currentManagementYear, string? businessClass,
            string? businessType, string? priorSurety, int? estAnnualPremium)
        {
            return await ExecuteSave(async () => await accountMutation.SetAccountGeneralInfo(accountId, yearStarted, currentManagementYear, businessClass, businessType, priorSurety, estAnnualPremium, contextFactory));
        }
        public async Task<ISaveDataResult> SetAccountSystems(Guid accountId, string? estimatingSystem, string? estimatingSignoff, string? internalAccountingSystem,
            bool? interimWips, bool? interimPOCs)
        {
            return await ExecuteSave(async () => await accountMutation.SetAccountSystems(accountId, estimatingSystem, estimatingSignoff, internalAccountingSystem, interimWips, interimPOCs, contextFactory));
        }
        public async Task<ISaveDataResult> SetAccountAdditionalInformation(Guid accountId, bool? fullIndemnity, bool? corpIndemnity, bool? personalIndemnity,
            bool? keyManagementLifeInsurance, bool? managementIncentives, bool? fundedBuySell, bool? multipleActiveOwners,
            bool? trackCommAccount, bool? berkleyAffiliate, string? comments)
        {
            return await ExecuteSave(async () => await accountMutation.SetAccountAdditionalInformation(accountId, fullIndemnity, corpIndemnity, personalIndemnity, keyManagementLifeInsurance,
                managementIncentives, fundedBuySell, multipleActiveOwners, trackCommAccount, berkleyAffiliate, comments, contextFactory));
        }
        public async Task<IDataAccessResult<AccountProgram>> SetAccountProgram(Guid programId, DateTime effective, DateTime expritation, int single, int aggregate,
            string? comments, Guid statusId)
        {
            return await ExecuteGet(async () => await accountMutation.SetAccountProgram(programId, effective, expritation, single, aggregate, comments, statusId, contextFactory));
        }
        public async Task<IDataAccessResult<PowerOfAttorney>> SetPowerOfAttorney(Guid poaId, Guid insurerId, int? limit, string? referenceNumber, DateOnly? firstIssued,
            DateOnly? currentIssued, string? comments, string status)
        {
            return await ExecuteGet(async () => await agencyMutation.SetPowerOfAttorney(poaId, insurerId, limit, referenceNumber, firstIssued, currentIssued, comments, status, eventSender, contextFactory));
        }

        public async Task<ISaveDataResult> SetPowerOfAttorneyDocumentLink(Guid poaId, Guid? imagingDocumentId)
        {
            return await ExecuteSave(async () => await agencyMutation.CreateAgencyPOADocumentLink(poaId, imagingDocumentId, eventSender, contextFactory));
        }

        public async Task<ISaveDataResult> SetAgencyLicenseDocumentLink(Guid licenseId, Guid? imagingDocumentId)
        {
            return await ExecuteSave(async () =>
                await agencyMutation.CreateAgencyLicenseDocumentLink(licenseId, imagingDocumentId, eventSender,
                    contextFactory));
        }

        public async Task<ISaveDataResult> SetAccountCreditReportDocumentLink(Guid? documentId, string accountNum)
        {
            return await ExecuteSave(async () => await accountMutation.SetCurrentCreditReportLink(accountNum, documentId, eventSender, contextFactory));
        }

        public async Task<ISaveDataResult> SetAddress(Address address, string identifier)
        {
            return await ExecuteSave((async () => await generalMutation.SetAddress(address.Id, address.Address1, address.Address2,
                address.Address3, address.City, address.StateCode, address.PostalCode, identifier,
                eventSender, contextFactory, loggingService)));
        }
        public async Task<ISaveDataResult> CreateAddress(Guid addressId, string address1, string? address2,
            string? address3, string city, string? stateCode, string? postalCode,
            Guid legalEntityId, string addressType, string identifier)
        {
            return await ExecuteSave(async () =>
                await generalMutation.CreateAddress(addressId, address1, address2, address3, city, stateCode, postalCode, 
                legalEntityId, addressType, identifier, eventSender, contextFactory, loggingService));

        }
        public async Task<ISaveDataResult> CreatePhoneNumber(Guid phoneId, string? countryCode, string mainNumber, string? extension,
            Guid legalEntityId, string phoneType)
        {
            return await ExecuteSave(async () => await generalMutation.CreatePhoneNumber(phoneId, countryCode ?? "", mainNumber, extension, legalEntityId, phoneType, contextFactory));
        }
        public async Task<ISaveDataResult> DeleteAddress(Guid addressId, string identifier)
        {
            return await ExecuteSave(async () =>
            await generalMutation.DeleteAddress(addressId, identifier, eventSender, contextFactory, loggingService));
        }

        public async Task<IDataAccessResult<List<CountryDm>>> GetAllCountries()
        {
            return await ExecuteGet(async()=>await query.GetAllCountries(contextFactory));
        }
        public async Task<ISaveDataResult> DeletePhoneNumber(Guid phoneId)
        {
            return await ExecuteSave(async () => await generalMutation.DeletePhoneNumber(phoneId, contextFactory));
        }

        public async Task<ISaveDataResult> SetAgencyGeneralInfo(Guid agencyId, string agencyName, Guid parentId, string? taxId, string? npn, bool w9,
            bool need1099, bool nasbp, string branchKey)
        {
            try
            {
                var result = await agencyMutation.SetAgencyGeneralInfo(agencyId, agencyName, parentId, taxId, npn, w9, need1099, nasbp, branchKey, contextFactory);
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
        public async Task<ISaveDataResult> SetAgencyInventory(Guid inventoryId, DateTime? sent, int? quantity, string documentType, string? addressee,
            Guid addressId, string address1, string? address2, string? address3, string city, string? stateCode, string? postalCode)
        {
            try
            {
                await agencyMutation.SetAgencyInventory(inventoryId, sent, quantity, documentType, addressee, addressId, address1, address2, address3, city, stateCode, postalCode,
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
        public async Task<ISaveDataResult> CreateAgencyStatusLog(Guid id, string agencyNumber, DateTime effective, string oldStatus, string newStatus, Guid changedBy, string? comments)
        {
            try
            {
                var result = await agencyMutation.CreateAgencyStatusLog(id, agencyNumber, effective, oldStatus, newStatus, changedBy, comments, eventSender, contextFactory);
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
        public async Task<ISaveDataResult> CreateAgencyPOA(Guid poaId, Guid insurerId, Guid agencyId, int limit, string? referenceNumber, DateOnly? firstIssued, DateOnly? currentIssued, string? comments, string status)
        {
            try
            {
                var result = await agencyMutation.CreateAgencyPOA(poaId, insurerId, agencyId, limit, referenceNumber, firstIssued, currentIssued, comments, status, eventSender, contextFactory);
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
        public async Task<ISaveDataResult> DeleteAgencyPOA(Guid poaId)
        {
            try
            {
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
        public async Task<ISaveDataResult> CreateAgencyInventory(Guid inventoryId, Guid agencyId, DateTime dateSent, int quantity, string documentType, string addressee,
            string address1, string? address2, string? address3, string city, string? stateCode, string? postalCode, Guid approverId)
        {
            try
            {
                var result = await agencyMutation.CreateAgencyInventory(inventoryId, agencyId, dateSent, quantity, documentType, addressee,
                    address1, address2, address3, city, stateCode, postalCode, approverId, eventSender, contextFactory);
                return new DataAccessResult<bool>(); /*{  DataObject = result };*/
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

        public async Task<IDataAccessResult<Obligee>> CreateObligee(Guid id, string fullName, string obligeeType, bool printStatusLetter, string? notes,
            string address1, string? address2, string city, string state, string postalCode, string? phoneNumber, string? email)
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
        public async Task<IDataAccessResult<AgencyLicense>> SetAgencyLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool appointingState, string? comments,
            DateOnly? appointment, DateOnly? expiration, DateOnly? termination, Guid insurerId, bool isResident,
            string? licenseNumber, string state, bool isActive)
        {
            return await ExecuteGet(async () => await agencyMutation.SetAgencyLicense(licenseId, agencyId, agentId, appointingState,
                    comments, appointment, expiration, termination,
                    insurerId, isResident, licenseNumber, state, isActive, eventSender, contextFactory));

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
            return await ExecuteSave(async () => await agencyMutation.DeleteLicense(licenseId, eventSender, contextFactory));

        }
        public async Task<ISaveDataResult> DeleteAgencyInventory(Guid inventoryId)
        {
            return await ExecuteSave(async () => await agencyMutation.DeleteAgencyInventory(inventoryId, eventSender, contextFactory));
        }
        public async Task<ISaveDataResult> SetAgencyCommissionRates(Guid agencyId, AgencyCommission[] rates)
        {
            return await ExecuteSave(async () => await agencyMutation.SaveCommissionRates(agencyId, rates, eventSender, contextFactory));
        }

        public async Task<IDataAccessResult<BondRequestNumberType>> GetBondRequestNumberType(string bondNumber)
        {
            return (await ExecuteGet(async () => await query.GetBondRequestNumberType(bondNumber, contextFactory)))!;
        }

        public async Task<IDataAccessResult<string?>> GetBondNumber(string bondRequestNumber)
        {
            return await ExecuteGet(async () => await query.GetBondNumber(bondRequestNumber, contextFactory));
        }

        public async Task<IDataAccessResult<List<ImagingType>>> GetAllImagingTypes()
        {
            return await ExecuteGet(async () => await query.GetAllImagingTypes(contextFactory));
        }

        public async Task<IDataAccessResult<List<VImagingCategoryTabDivisionType>>> GetAllImagingCategoryTabDivisionTypes()
        {
            return await ExecuteGet(async () => await query.GetAllImagingCategoryTabDivisionType(contextFactory));
        }

        public async Task<IDataAccessResult<ImagingDocument?>> GetImagingDocumentsDetails(
            ImagingDocumentCategory docCategory, Guid documentId)
        {
            return await ExecuteGet(async () => await query.GetDocumentDetails(docCategory, documentId, imagingAccess));
        }

        public async Task<IDataAccessResult<List<PowerOfAttorneyDocumentNameDm>>> GetPoaDocumentNames()
        {
            return await ExecuteGet(async () => await query.GetPOADocumentNames(contextFactory));
        }

        public async Task<IDataAccessResult<PowerOfAttorneyDocumentStatus>> SetPowerOfAttorneyDocumentStatus(Guid id, DateTime? requested, DateTime? received, Guid documentTypeId,
            string? comments)
        {
            return await ExecuteGet(async () => await agencyMutation.SetPowerOfAttorneyDocumentStatus(id, requested,
                received, documentTypeId, comments, eventSender, contextFactory));
        }

        //UNDONE:  Refactor to DRY out the code
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

        public IDisposable AddressModified(Guid addressId, Action<SubscriptionResult<Address>> onNext, Action<Exception>? onError = null, Action? onComplete = null)
        {
            return OnAddressModified(addressId).Subscribe(new ServerSideSubscriptionSubscriber<SubscriptionResult<Address>>(onNext, onError, onComplete));
        }

        public IDisposable AddressCollectionModified(Guid addressId, Action<SubscriptionResult<Guid>> onNext, Action<Exception>? onError = null, Action? onComplete = null)
        {
            return OnAddressCollectionModified(addressId).Subscribe(new ServerSideSubscriptionSubscriber<SubscriptionResult<Guid>>(onNext, onError, onComplete));
        }

        public IDisposable SearchResultReady(string searchTerm, Action<SubscriptionResult<List<JamesSearchResult>>> onNext, Action<Exception>? onError = null, Action? onComplete = null)
        {
            return OnSearchResultReady(searchTerm).Subscribe(new ServerSideSubscriptionSubscriber<SubscriptionResult<List<JamesSearchResult>>> (onNext,onError,onComplete));
        }

        public Task<ISaveDataResult> StartSuperSearch(string searchTerm, SearchOptions options)
        {
            return ExecuteSave(async ()=> await query.Search(searchTerm, options, eventSender, contextFactory, loggingService));
        }

        public Task<IDataAccessResult<List<Account>>> GetIdAccountNumbers()
        {
            return ExecuteGet(async () => await query.GetIdAccountNumbers(contextFactory));
        }

        public Task<IDataAccessResult<List<Agency>>> GetIdAgencyNumbers()
        {
            return ExecuteGet(async () => await query.GetIdAgencyNumbers(contextFactory));
        }

        public async Task<IDataAccessResult<List<ImagingDocument>>> SearchDocuments(string imagingId, ImagingDocumentCategory docCategory, string? documentType = null)
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
                        $"({criteria.WhereClause} OR {(string.Equals(bidBondType?.Type, "CONTRACT", StringComparison.InvariantCultureIgnoreCase) ? ImagingAccessBase.ContBidId : ImagingAccessBase.CommBidId)} = '{bidBondType.BondRequestNumber}')";
                    break;
                case ImagingDocumentCategory.Agency: //3
                    criteria.WhereClause = $"{ImagingAccessBase.AgencyNo} = '{id}'";
                    break;
                case ImagingDocumentCategory.CommBid: //4
                    criteria.WhereClause = $"{ImagingAccessBase.CommBidId} = '{id}'";
                    bondNumber = (await GetBondNumber(id)).Data;
                    //TODO:Handle GraphQl errors
                    if (!string.IsNullOrWhiteSpace(bondNumber))
                        criteria.WhereClause = $"({criteria.WhereClause} OR {ImagingAccessBase.PolicyNo} = '{bondNumber}')";
                    break;
                case ImagingDocumentCategory.ContBid: //5
                    criteria.WhereClause = $"{ImagingAccessBase.ContBidId} = '{id}'";
                    bondNumber = (await GetBondNumber(id)).Data;
                    //TODO:Handle GraphQl errors
                    if (!string.IsNullOrWhiteSpace(bondNumber))
                        criteria.WhereClause = $"({criteria.WhereClause} OR {ImagingAccessBase.PolicyNo} = '{bondNumber}')";
                    break;
                case ImagingDocumentCategory.Billing: //SearchBillingDocuments
                    criteria.WhereClause = $"{ImagingAccessBase.AccountId} = '{id}''";
                    break;
                default:
                    throw new ArgumentException("Invalid docCategory", nameof(docCategory));
            }
            return criteria;
        }

        private readonly Dictionary<Guid, ServerSideSubscription<SubscriptionResult<Address>>> _onAddressModified = new();

        private ServerSideSubscription<SubscriptionResult<Address>> OnAddressModified(Guid addressId)
        {
            lock (_onAddressModified)
            {
                if (_onAddressModified.ContainsKey(addressId) == false)
                {
                    _onAddressModified[addressId] = new(eventReceiver
                        .SubscribeAsync<SubscriptionResult<Address>>("OnAddressModified_" + addressId).Result.ReadEventsAsync(), CancellationToken.None);
                }

                return _onAddressModified[addressId];
            }
        }
        private readonly Dictionary<Guid, ServerSideSubscription<SubscriptionResult<Guid>>> _onAddressCollectionModified = new();

        private ServerSideSubscription<SubscriptionResult<Guid>> OnAddressCollectionModified(Guid legalEntityId)
        {
            lock (_onAddressCollectionModified)
            {
                if (_onAddressCollectionModified.ContainsKey(legalEntityId) == false)
                {
                    _onAddressCollectionModified[legalEntityId] = new(eventReceiver
                        .SubscribeAsync<SubscriptionResult<Guid>>("OnAddressCollectionModified_" + legalEntityId).Result.ReadEventsAsync(), CancellationToken.None);
                }

                return _onAddressCollectionModified[legalEntityId];
            }
        }
        private readonly Dictionary<string, ServerSideSubscription<SubscriptionResult<List<JamesSearchResult>>>> _onSearchResultReady = new();

        private ServerSideSubscription<SubscriptionResult<List<JamesSearchResult>>> OnSearchResultReady(string searchTerm)
        {
            lock (_onSearchResultReady)
            {
                if (_onSearchResultReady.ContainsKey(searchTerm) == false)
                {
                    _onSearchResultReady[searchTerm] = new(eventReceiver
                        .SubscribeAsync<SubscriptionResult<List<JamesSearchResult>>>("Srch_" + searchTerm).Result.ReadEventsAsync(), CancellationToken.None);
                }

                return _onSearchResultReady[searchTerm];
            }
        }
    }
}
