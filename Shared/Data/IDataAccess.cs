using James.Shared.Dto;
using James.Shared.Imaging;
using James.Shared.Model;

namespace James.Shared.Data
{
    public interface IDataAccess
    {
        #region Data Caching functionality from BaseDataAccess

        public IBrowserStorageCache BrowserStorageCache { get; }
        public Task GetCacheOrLoadDataAsync(LoadItem loadItem);

        /// <summary>
        /// Load multiple items in parallel and then run the afterAllLoaded when loading complete
        /// </summary>
        /// <param name="afterAllLoaded">Action to execute after all items have been loaded.</param>
        /// <param name="loadItems">The LoadItem to load</param>
        /// <remarks>Use this is you need multiple data sets before contracting a final output.</remarks>
        public Task ParallelGetCacheOrDataAsync(Action? afterAllLoaded, params LoadItem[] loadItems);

        /// <summary>
        /// Load multiple items in parallel
        /// </summary>
        /// <param name="loadItems">The LoadItem to load</param>
        public Task ParallelGetCacheOrDataAsync(params LoadItem[] loadItems);

        /// <summary>
        /// Clears cache
        /// </summary>
        /// <remarks>Use cautiously as this clears the cache for the entire server if it is called server-side</remarks>
        public void Clear();

        /// <summary>
        /// Clears cache for one cache key
        /// </summary>
        /// <param name="key">Cache key to clear</param>
        public void Clear(string key);

        /// <summary>
        /// Updates or adds an item to the cache with an optional cache duration.
        /// </summary>
        /// <param name="key">The unique identifier for the cached item.</param>
        /// <param name="data">The data object to be cached.</param>
        /// <param name="cacheDuration">
        /// Optional duration for which the item should remain in the cache. 
        /// Defaults to 1 hour if not specified.
        /// </param>
        /// <remarks>
        /// If the key already exists in the cache, the existing entry is updated.
        /// If the key does not exist, a new cache entry is created.
        /// </remarks>
        public void UpdateCache(string key, object data, TimeSpan? cacheDuration = null);
        #endregion

        //DataAccess
        public Task<IDataAccessResult<Account>> GetAccountByNumber(string accountNumber);
        public Task<IDataAccessResult<List<AccountProgram>>> GetAccountProgramHistory(string accountNumber);
        public Task<IDataAccessResult<InforceAccountLOA>> GetInforceAccountLOAsByAccountNumber(string accountNumber);
        public Task<IDataAccessResult<List<Account>>> SearchAccounts(string searchString);
        public Task<IDataAccessResult<List<AdditionalRelatedParty>>> GetAdditionalRelatedParties(string? accountNumber);
        public Task<IDataAccessResult<List<AgencyAccountDto>>> GetAgencyAccounts(string agencyNumber);
        public Task<IDataAccessResult<List<AgencyAccountBondDto>>> GetAgencyAccountBonds(string accountNum);
        public Task<IDataAccessResult<Agency?>> GetAgencyByAgencyNumber(string agencyNumber);
        public Task<IDataAccessResult<Agency?>> GetAgencyNameAndNumberById(Guid agencyId);
        public Task<IDataAccessResult<List<Obligee>>> SearchObligees(string searchString);
        public Task<IDataAccessResult<Obligee?>> GetObligeeById(Guid id);
        public Task<IDataAccessResult<Obligee?>> GetObligeeByObligeeNumber(string obligeeNumber);
        public Task<IDataAccessResult<List<ObligeeTypeDm>>> GetObligeeTypes();
        public Task<IDataAccessResult<List<Bond>>> GetObligeePrimaryBonds(Guid obligeeId);
        public Task<IDataAccessResult<List<Bond>>> GetObligeeSecondaryBonds(Guid obligeeId);
        public Task<IDataAccessResult<List<AgencyLicense>>> GetAgencyLicenses(Guid agencyId);
        public Task<IDataAccessResult<List<Insurer>>> GetAllInsurers();
        public Task<IDataAccessResult<List<State>>> GetAllStates();
        public Task<IDataAccessResult<List<Branch>>> GetAllBranches();
        public Task<IDataAccessResult<Address>> GetAddress(Guid addressId);
        public Task<IDataAccessResult<PhoneNumber>> GetPhoneNumber(Guid phoneId);
        public Task<IDataAccessResult<List<PhoneTypeDm>>> GetPhoneTypes();
        public Task<IDataAccessResult<List<AddressTypeDm>>> GetAddressTypes();
        public Task<IDataAccessResult<List<Address>>> GetAllLegalEntityAddresses(Guid legalEntityId);
        public Task<IDataAccessResult<List<PhoneNumber>>> GetAllLegalEntityPhoneNumbers(Guid legalEntityId);
        public Task<IDataAccessResult<List<Bond>>> GetAgencyBonds(Guid agencyId);
        public Task<IDataAccessResult<List<AgencyInventory>>> GetAgencyInventory(Guid agencyId);
        public Task<IDataAccessResult<List<InventoryDocumentDm>>> GetAllInventoryDocTypes();
        public Task<IDataAccessResult<List<Agent>>> GetAgencyAgents(Guid agencyId);
        public Task<IDataAccessResult<List<AgencyStatusDm>>> GetAgencyStatuses();
        public Task<IDataAccessResult<List<AgencyStatusLog>>> GetAgencyStatusLog(string agencyNumber);
        public Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId);
        public Task<IDataAccessResult<Agent>> GetAgent(Guid agentId);
        public Task<IDataAccessResult<List<Agent>>> SearchAgents(string searchString);
        public Task<IDataAccessResult<List<Agency>>> GetAgencyRelatedParties(Guid agencyId);
        public Task<IDataAccessResult<List<Agency>>> SearchAgencies(string? search, bool activeOnly);
        public Task<IDataAccessResult<List<PowerOfAttorneyStatusDm>>> GetAllPoaStatuses();
        public Task<IDataAccessResult<List<AgencyCommission>>> GetAgencyCommissionRates(Guid agencyId);
        public Task<IDataAccessResult<UserProfile>> GetUserProfileByUserName(string userName);
        public Task<ISaveDataResult> SetAccountGeneralInfo(Guid accountId, string? yearStarted, string? currentManagementYear, string? businessClass,
            string? businessType, string? priorSurety, int? estAnnualPremium);
        public Task<ISaveDataResult> SetAccountSystems(Guid accountId, string? estimatingSystem, string? estimatingSignoff, string? internalAccountingSystem, bool? interimWips, bool? interimPOCs);
        public Task<ISaveDataResult> SetAccountAdditionalInformation(Guid accountId, bool? fullIndemnity, bool? corpIndemnity, bool? personalIndemnity,
            bool? keyManagementLifeInsurance, bool? managementIncentives, bool? fundedBuySell, bool? multipleActiveOwners,
            bool? trackCommAccount, bool? berkleyAffiliate, string? comments);
        public Task<IDataAccessResult<AccountProgram>> SetAccountProgram(Guid programId, DateTime effective, DateTime expriration, int single, int aggregate,
            string? comments, Guid statusId);
        public Task<IDataAccessResult<PowerOfAttorney>> SetPowerOfAttorney(Guid poaId, Guid insurerId, int? limit, string? referenceNumber,
            DateOnly? firstIssued, DateOnly? currentIssued, string? comments, string status);
        public Task<ISaveDataResult> SetPowerOfAttorneyDocumentLink(Guid poaId, Guid? imagingDocumentId);
        public Task<ISaveDataResult> SetAgencyLicenseDocumentLink(Guid licenseId, Guid? imagingDocumentId);
        public Task<ISaveDataResult> SetAccountCreditReportDocumentLink(Guid? documentId, string accountNum);
        public Task<ISaveDataResult> SetAddress(Address address, string identifier);
        public Task<ISaveDataResult> CreateAddress(Guid addressId, string address1, string? address2,
            string? address3, string city, string? stateCode, string? postalCode, Guid legalEntityId, string addressType, string identifier);
        public Task<ISaveDataResult> DeleteAddress(Guid addressId, string identifier);
        public Task<ISaveDataResult> CreatePhoneNumber(Guid phoneId, string? countryCode, string mainNumber, string? extension,
            Guid legalEntityId, string phoneType);
        public Task<ISaveDataResult> DeletePhoneNumber(Guid phoneId);
        public Task<IDataAccessResult<List<CountryDm>>> GetAllCountries();
        public Task<ISaveDataResult> CreateAgencyStatusLog(Guid id, string agencyNumber, DateTime effective, string oldStatus, string newStatus,
            Guid changedBy, string? comments);
        public Task<ISaveDataResult> CreateLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool appointingState,
            string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state, bool isActive);
        public Task<IDataAccessResult<Obligee>> CreateObligee(Guid id, string fullName, string obligeeType, bool printStatusLetter, string? notes,
            string address1, string? address2, string city, string state, string postalCode, string? phoneNumber, string? email);
        public Task<ISaveDataResult> CreateAgencyInventory(Guid inventoryId, Guid agencyId, DateTime dateSent, int quantity, string documentType, 
            string addressee, string address1, string? address2, string? address3, string city, string? stateCode, string? postalCode, Guid approverId);
        public Task<ISaveDataResult> CreateAgencyPOA(Guid poaId, Guid insurerId, Guid agencyId, int limit, string? referenceNumber, DateOnly? firstIssued,
            DateOnly? currentIssued, string? comments, string status);
        public Task<ISaveDataResult> DeleteAgencyPOA(Guid poaId);
        public Task<ISaveDataResult> AgencyLicenseBulkDelete(List<Guid> licenseIds);
        public Task<ISaveDataResult> AgencyLicenseBulkInsert(List<AgencyLicenseBulk> licenses);
        public Task<ISaveDataResult> AgencyLicenseBulkUpdate(List<AgencyLicenseBulk> licenses);
        public Task<IDataAccessResult<AgencyLicense>> SetAgencyLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool appointingState,
            string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state, bool isActive);
        public Task<ISaveDataResult> SetAgencyInventory(Guid inventoryId, DateTime? sent, int? quantity, string documentType, string? addressee,
            Guid addressId, string address1, string? address2, string? address3, string city, string? stateCode, string? postalCode);
        public Task<ISaveDataResult> SetAgencyGeneralInfo(Guid agencyId, string agencyName, Guid parentId, string? taxId, string? npn, bool w9,
            bool need1099, bool nasbp, string branchKey);
        public Task<ISaveDataResult> SetAgencyProfitSharingInfo(Guid agencyId, bool profitSharing,
            int? profitSharingMinimumPremium);
        public Task<ISaveDataResult> DeleteLicense(Guid licenseId);
        public Task<ISaveDataResult> DeleteAgencyInventory(Guid inventoryId);
        public Task<ISaveDataResult> SetAgencyCommissionRate(AgencyCommission rate);
        public Task<ISaveDataResult> DeleteAgencyCommissionRate(Guid commRateId);
        public Task<IDataAccessResult<BondRequestNumberType>> GetBondRequestNumberType(string bondNumber);
        public Task<IDataAccessResult<string?>> GetBondNumber(string bondRequestNumber);
        public Task<IDataAccessResult<List<ImagingType>>> GetAllImagingTypes();
        public Task<IDataAccessResult<List<VImagingCategoryTabDivisionType>>> GetAllImagingCategoryTabDivisionTypes();

        #region UserSettings
        public Task<IDataAccessResult<Dictionary<string, string>>> GetAllUserSettings();
        public Task<ISaveDataResult> SetUserSetting(string key, string? value);
        public Task<ISaveDataResult> SetDefaultUserSetting(string key, string? value);
        #endregion

        public IDisposable AddressModified(Guid addressId, Action<SubscriptionResult<Address>> onNext, Action<Exception>? onError = null, Action? onComplete = null);

        public IDisposable AddressCollectionModified(Guid legalEntityId, Action<SubscriptionResult<Guid>> onNext,
            Action<Exception>? onError = null, Action? onComplete = null);

        /// <summary>
        /// Returns super search results when they are ready
        /// </summary>
        /// <param name="searchTerm">The search term to search for</param>
        /// <param name="onNext">handle of the returned results</param>
        /// <param name="onError">error handler</param>
        /// <param name="onComplete">handle to dispose of the subscription after all results are returned.</param>
        /// <returns>IDisposable reference to the subscription object</returns>
        public IDisposable SearchResultReady(string searchTerm,
            Action<SubscriptionResult<List<JamesSearchResult>>> onNext,
            Action<Exception>? onError = null, Action? onComplete = null);

        /// <summary>
        /// Call this after subscribing to the SearchResultsReady subscription
        /// </summary>
        /// <param name="searchTerm">The search term to search for</param>
        /// <param name="options"></param>
        /// <returns>A simple ISaveDataResult.  Actual results will come through the SearchResultsReady subscription</returns>
        public Task<ISaveDataResult> StartSuperSearch(string searchTerm, SearchOptions options);

        public Task<IDataAccessResult<List<Account>>> GetIdAccountNumbers();
        public Task<IDataAccessResult<List<Agency>>> GetIdAgencyNumbers();

        public Task<IDataAccessResult<ImagingSearchCriteria>> GetImagingSearchCriteria(string id, ImagingDocumentCategory docCategory,
            bool useDocCategoryAsCriteria = true);

        /// <summary>
        /// Returns metadata of documents contained in a given document class, document type and imaging id
        /// </summary>
        /// <param name="imagingId">the id of the object associated with the document category</param>
        /// <param name="docCategory">The document category to search</param>
        /// <param name="documentType">The document type to search for</param>
        /// <returns>List of the metadata for the <see cref="ImagingDocument" />s found</returns>
        public Task<IDataAccessResult<List<ImagingDocument>>> SearchDocuments(string imagingId, ImagingDocumentCategory docCategory,
            string? documentType = null);
        public Task<IDataAccessResult<ImagingDocument?>> GetImagingDocumentsDetails(ImagingDocumentCategory docCategory,
            Guid documentId);

        public Task<IDataAccessResult<List<SecurityRole>>> GetAllSecurityRoles();
        public Task<IDataAccessResult<List<SecurityRole>>> GetSecurityRolesByUserId(Guid userId);
        public Task<IDataAccessResult<List<SecurityRoleMember>>> GetSecurityRoleMembers(string role);
        public Task<ISaveDataResult> AddPrincipalToSecurityRole(Guid principalId, string role);
        public Task<ISaveDataResult> RemovePrincipalFromSecurityRole(Guid principalId, string role);
        public Task<ISaveDataResult> AddSecurityRole(SecurityRole role);
        public Task<IDataAccessResult<List<Employee>>> GetEmployees(bool activeOnly = true);

        public Task<IDataAccessResult<List<PowerOfAttorneyDocumentNameDm>>> GetPoaDocumentNames();

        public Task<IDataAccessResult<PowerOfAttorneyDocumentStatus>> SetPowerOfAttorneyDocumentStatus(Guid id,
            DateTime? requested, DateTime? received, Guid documentTypeId, string? comments);
    }

    public interface ISaveDataResult
    {
        public string[] Errors { get; }
        public bool Success { get; }
    }

    public interface IDataAccessResult : ISaveDataResult
    {
        public object? DataObject { get; }
    }
    public interface IDataAccessResult<T>: IDataAccessResult
    {
        public T? Data { get; }
    }
    
    public class SaveDataResult : ISaveDataResult
    {
        public string[] Errors { get; init; } = [];

        public bool Success => Errors.Length==0;
    }
    public class DataAccessResult<T> : IDataAccessResult<T>
    {
        public T? Data { get; init; }
        public object? DataObject => Data;

        public string[] Errors { get; init; } = [];

        public bool Success => Errors.Length == 0;
    }
    public class DataAccessResultString : IDataAccessResult<string?>
    {
        public string? Data { get; init; }
        public object? DataObject => Data;

        public string[] Errors { get; init; } = [];

        public bool Success => Errors.Length == 0;
    }

    public class Multisubscription : List<IDisposable>, IDisposable
    {
        public Multisubscription()
        {
        }
        public Multisubscription(IEnumerable<IDisposable> subscriptions)
        {
            AddRange(subscriptions);
        }

        public void Dispose()
        {
            foreach(var subscription in this)
                subscription.Dispose();
        }
    }
}
