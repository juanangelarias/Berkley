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
        public Task<IDataAccessResult<List<Bond>>> GetAgencyBonds(Guid agencyId);
        public Task<IDataAccessResult<List<AgentsInAgency>>> GetAgencyAgents(Guid agencyId);
        public Task<IDataAccessResult<List<AgencyStatusDm>>> GetAgencyStatuses();
        public Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId);
        public Task<IDataAccessResult<Agent>> GetAgent(Guid agentId);
        public Task<IDataAccessResult<List<Agency>>> SearchAgencies(string? search);

        public Task SetAddress(Address address);//TODO:  Change to Task<ISaveData>

        //TODO: Figure out subscriptions
        public Task<IDisposable> AddressModified();
    }

    public interface ISubscription<T>:IDisposable{}

    public int
    public interface IDataAccessResult<T>
    {
        public T? Data { get; }
        public string[] Errors { get; }
        public bool Success { get; }
    }
    

    //TODO: Create SaveDataResult with just Errors and Succcess
    public class DataAccessResult<T>: IDataAccessResult<T>
    {
        public T? Data { get; init; }

        public string[] Errors { get; init; } = [];

        public bool Success => Errors.Length==0;
    }
}
