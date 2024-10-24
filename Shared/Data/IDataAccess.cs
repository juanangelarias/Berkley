using James.Shared.Imaging;
using James.Shared.Model;

namespace James.Shared.Data
{
    public interface IDataAccess
    {
        public Task<IDataAccessResult<Account?>> GetAccountByNumber(string accountNumber);
        public Task<IDataAccessResult<List<AccountProgram>>> GetAccountProgramHistory(string accountNumber);
        public Task<IDataAccessResult<InforceAccountLOA>> GetInforceAccountLOAsByAccountNumber(string accountNumber);
        public Task<IDataAccessResult<List<Account>>> SearchAccounts(string searchString);
        public Task<IDataAccessResult<List<Account>>> GetAgencyAccounts(string agencyNumber);
        public Task<IDataAccessResult<Agency?>> GetAgencyByAgencyNumber(string agencyNumber);
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
        public Task<IDataAccessResult<List<AgentsInAgency>>> GetAgencyAgents(Guid agencyId);
        public Task<IDataAccessResult<List<AgencyStatusDm>>> GetAgencyStatuses();
        public Task<IDataAccessResult<List<AgencyStatusLog>>> GetAgencyStatusLog(string agencyNumber);
        public Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId);
        public Task<IDataAccessResult<List<PowerOfAttorneyDocumentNameDm>>> GetPOADocumentNames();
        //public Task<IDataAccessResult<PowerOfAttorneyDocumentStatus>> SetPowerOfAttorneyDocumentStatus(Guid id, DateTime? requested, DateTime? received, Guid documentTypeId, string? comments);
        public Task<IDataAccessResult<Agent>> GetAgent(Guid agentId);
        public Task<IDataAccessResult<List<Agent>>> SearchAgents(string searchString);
        public Task<IDataAccessResult<List<Agency>>> GetAgencyRelatedParties(Guid agencyId);
        public Task<IDataAccessResult<List<Agency>>> SearchAgencies(string? search);
        public Task<IDataAccessResult<List<PowerOfAttorneyStatusDm>>> GetAllPoaStatuses();
        public Task<IDataAccessResult<List<AgencyCommission>>> GetAgencyCommissionRates(Guid agencyId);
        public Task<IDataAccessResult<UserProfile>> GetUserProfileByUserName(string userName);
        public Task<IDataAccessResult<AccountProgram>> SetAccountProgram(Guid programId, DateTime effective, DateTime expritation, int single, int aggregate,
            string? comments, Guid statusId);
        public Task<IDataAccessResult<PowerOfAttorney>> SetPowerOfAttorney(Guid poaId, Guid insurerId, int? limit, string? referenceNumber,
            DateOnly? firstIssued, DateOnly? currentIssued, string? comments, string status);
        public Task<ISaveDataResult> SetPowerOfAttorneyDocumentLink(Guid poaId, Guid? imagingDocumentId);
        public Task<ISaveDataResult> SetAddress(Address address, string identifier);
        public Task<ISaveDataResult> CreateAddress(Guid addressId, string address1, string? address2,
            string? address3, string city, string? stateCode, string? postalCode, Guid legalEntityId, string addressType);
        public Task<ISaveDataResult> CreatePhoneNumber(Guid phoneId, string? countryCode, string mainNumber, string? extension,
            Guid legalEntityId, string phoneType);
        public Task<ISaveDataResult> DeleteAddress(Guid addressId);
        public Task<ISaveDataResult> DeletePhoneNumber(Guid phoneId);
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
        public Task<IDataAccessResult<AgencyLicense>> SetAgencyLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool appointingState,
            string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state, bool isActive);
        public Task<ISaveDataResult> SetAgencyInventory(Guid inventoryId, DateTime? sent, int? quantity, string documentType, string? addressee,
            Guid addressId, string address1, string? address2, string? address3, string city, string? stateCode, string? postalCode);
        public Task<ISaveDataResult> SetAgencyGeneralInfo(Guid agencyId, string agencyName, Guid parentId, string? taxId, string? npn, bool w9,
            bool need1099, bool nasbp, string branchKey);
        public Task<ISaveDataResult> DeleteLicense(Guid licenseId);
        public Task<ISaveDataResult> DeleteAgencyInventory(Guid inventoryId);
        public Task<ISaveDataResult> SetAgencyCommissionRates(Guid agencyId, AgencyCommission[] rates);
        public Task<IDataAccessResult<BondRequestNumberType>> GetBondRequestNumberType(string bondNumber);
        public Task<IDataAccessResult<string?>> GetBondNumber(string bondRequestNumber);

        public IDisposable AddressModified(Guid addressId, Action<SubscriptionResult<Address>> onNext, Action<Exception>? onError = null, Action? onComplete = null);

        public Task<IDataAccessResult<ImagingSearchCriteria>> GetImagingSearchCriteria(string id, ImagingDocumentCategory docCategory,
            bool useDocCategoryAsCriteria = true);

        public Task<IDataAccessResult<List<ImagingDocument>>> SearchDocuments(string id, ImagingDocumentCategory docCategory,
            string? documentType = null);
        public Task<IDataAccessResult<ImagingDocument?>> GetImagingDocumentsDetails(ImagingDocumentCategory docCategory,
            Guid documentId);

        public Task<IDataAccessResult<List<PowerOfAttorneyDocumentNameDm>>> GetPoaDocumentNames();

        public Task<IDataAccessResult<PowerOfAttorneyDocumentStatus>> SetPowerOfAttorneyDocumentStatus(Guid id,
            DateTime? requested, DateTime? received, Guid documentTypeId, string? comments);
    }

    public interface ISaveDataResult
    {
        public string[] Errors { get; }
        public bool Success { get; }
    }
    public interface IDataAccessResult<T>:ISaveDataResult
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

        public string[] Errors { get; init; } = [];

        public bool Success => Errors.Length == 0;
    }
    public class DataAccessResultString : IDataAccessResult<string?>
    {
        public string? Data { get; init; }

        public string[] Errors { get; init; } = [];

        public bool Success => Errors.Length == 0;
    }
}
