using James.Shared.Model;

namespace James.Shared.Data
{
    public interface IDataAccess
    {
        public Task<IDataAccessResult<List<Account>>> GetAgencyAccounts(string agencyNumber);
        public Task<IDataAccessResult<Agency>> GetAgencyByAgencyNumber(string agencyNumber);
        public Task<IDataAccessResult<List<AgencyLicense>>> GetAgencyLicenses(Guid agencyId);
        public Task<IDataAccessResult<List<Insurer>>> GetAllInsurers();
        public Task<IDataAccessResult<List<State>>> GetAllStates();
        public Task<IDataAccessResult<Address>> GetAddress(Guid addressId);
        public Task<IDataAccessResult<List<Bond>>> GetAgencyBonds(Guid agencyId);
        public Task<IDataAccessResult<List<AgencyInventory>>> GetAgencyInventory(Guid agencyId);
        public Task<IDataAccessResult<List<AgentsInAgency>>> GetAgencyAgents(Guid agencyId);
        public Task<IDataAccessResult<List<AgencyStatusDm>>> GetAgencyStatuses();
        public Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId);
        public Task<IDataAccessResult<Agent>> GetAgent(Guid agentId);
        public Task<IDataAccessResult<List<Agency>>> GetAgencyRelatedParties(Guid agencyId);
        public Task<IDataAccessResult<List<Agency>>> SearchAgencies(string? search);
        public Task<IDataAccessResult<List<PowerOfAttorneyStatusDm>>> GetAllPoaStatuses();

        public Task<IDataAccessResult<PowerOfAttorney>> SetPowerOfAttorney(Guid poaId, Guid insurerId, int? limit, string? serial,
            DateOnly? firstIssued, DateOnly? currentIssued, string? comments, Guid status);
        public Task<ISaveDataResult> SetAddress(Address address);
        public Task<IDataAccessResult<AgencyLicense>> CreateLicense(Guid agencyId, Guid? agentId, bool? appointingState, 
            string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state, bool isActive);

        public Task<IDataAccessResult<AgencyLicense>> SetAgencyLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool? appointingState,
            string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state, bool isActive);
        public Task<ISaveDataResult> DeleteLicense(Guid licenseId);

        //TODO: Figure out subscriptions
        public Task<IDisposable> AddressModified();
    }

    public interface ISubscription<T>:IDisposable{}

    public interface ISaveDataResult
    {
        public string[] Errors { get; }
        public bool Success { get; }
    }
    public interface IDataAccessResult<T>:ISaveDataResult
    {
        public T? Data { get; }
    }
    
    //TODO: Discuss not using SaveDataResult as base class (I think there is no reason to ever view a DataAccessResult as a SaveDataResult)
    public class SaveDataResult : ISaveDataResult
    {
        public string[] Errors { get; init; } = [];

        public bool Success => Errors.Length==0;
    }
    public class DataAccessResult<T>: IDataAccessResult<T>
    {
        public T? Data { get; init; }

        public string[] Errors { get; init; } = [];

        public bool Success => Errors.Length==0;
    }
}
