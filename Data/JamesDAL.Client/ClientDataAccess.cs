using James.Data.Client.GraphQL;
using James.Shared;
using James.Shared.Data;
using James.Shared.Model;
using StrawberryShake;
using System.Diagnostics;
using Severity = James.Shared.Model.Severity;

namespace James.Data.Client
{
    public class ClientDataAccess : IDataAccess
    {
        private readonly IJamesClient _jamesClient;
        private readonly ILoggingService _logging;

        public ClientDataAccess(IJamesClient jamesClient, ILoggingService logging)
        {
            _jamesClient = jamesClient;
            _logging = logging;
        }

        public async Task<IDataAccessResult<List<Account>>> GetAgencyAccounts(string agencyNumber)
        {
            try
            {
                var result = await _jamesClient.AgencyAccounts.ExecuteAsync(agencyNumber);
                if (result.Errors.Any())
                    return new DataAccessResult<List<Account>> { Errors = result.Errors.Select(ErrorToString).ToArray() };
                Debug.Assert(result.Data != null, "result.Data != null");
                var accountList =
                    new List<Account>(
                        result.Data.AgencyAccounts.Select(acnt =>
                            ThisToThat.ToEntityType<Account>(acnt)));
                return new DataAccessResult<List<Account>> { Data = accountList };
            }
            catch (Exception e)
            {
                _logging.LogException(e, "GetAgencyByAgencyNumber returned exception",
                    e.ToText(), Severity.Error, "GraphQl");
                throw;
            }
        }

        public async Task<IDataAccessResult<Agency>> GetAgencyByAgencyNumber(string agencyNumber)
        {
            var result = await _jamesClient.GetAgencyByAgencyNumber.ExecuteAsync(agencyNumber);
            if (result.Errors.Count == 0)
            {
                Debug.Assert(result.Data != null, "result.Data != null");
                return new DataAccessResult<Agency> { Data = ThisToThat.ToEntityType<Agency>(result.Data.AgencyByAgencyNumber) };
            }

            _logging.LogWarning("GetAgencyByAgencyNumber returned error(s)",
                string.Join("\r\n", result.Errors.Select(ErrorToString)), "GraphQl");
            return new DataAccessResult<Agency> { Errors = result.Errors.Select(ErrorToString).ToArray() };
        }

        public async Task<IDataAccessResult<List<AgencyLicense>>> GetAgencyLicenses(Guid agencyId)
        {
            var result = await _jamesClient.GetAgencyLicenses.ExecuteAsync(agencyId);
            return GraphQLResult<List<AgencyLicense>>(result);
        }

        public async Task<IDataAccessResult<List<Insurer>>> GetAllInsurers()
        {
            var result = await _jamesClient.AllInsurers.ExecuteAsync();
            return GraphQLResult<List<Insurer>>(result);
        }

        public async Task<IDataAccessResult<List<State>>> GetAllStates()
        {
            var result = await _jamesClient.GetAllStates.ExecuteAsync();
            return GraphQLResult<List<State>>(result);
        }

        public async Task<IDataAccessResult<List<Bond>>> GetAgencyBonds(Guid agencyId)
        {
            var result = await _jamesClient.GetAgencyBonds.ExecuteAsync(agencyId);
            return GraphQLResult<List<Bond>>(result);
        }

        public async Task<IDataAccessResult<List<AgentsInAgency>>> GetAgencyAgents(Guid agencyId)
        {
            var result = await _jamesClient.GetAgencyAgents.ExecuteAsync(agencyId);
            return GraphQLResult<List<AgentsInAgency>>(result);
        }

        public async Task<IDataAccessResult<List<AgencyStatusDm>>> GetAgencyStatuses()
        {
            var result = await _jamesClient.GetAgencyStatuses.ExecuteAsync();
            return GraphQLResult<List<AgencyStatusDm>>(result);
        }

        public async Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId)
        {
            var result = await _jamesClient.GetAgencyPOAs.ExecuteAsync(agencyId);
            return GraphQLResult<List<PowerOfAttorney>>(result);
        }

        public async Task<IDataAccessResult<Agent>> GetAgent(Guid agentId)
        {
            var result = await _jamesClient.AgentByAgentId.ExecuteAsync(agentId);
            return GraphQLResult<Agent>(result);
        }

        public async Task<IDataAccessResult<List<Agency>>> SearchAgencies(string? search)
        {
            try
            {
                var result = await _jamesClient.SearchAgencies.ExecuteAsync(search);
                return GraphQLResult<List<Agency>>(result);
            }
            catch (Exception ex)
            {
                var exceptionDetail = ex.ToText();
                _logging.LogException(ex, "GetAgencyByAgencyNumber returned exception",
                    exceptionDetail, Severity.Error, "GraphQl");
                return new DataAccessResult<List<Agency>> { Data = [],Errors = [ex.Message] };
            }
        }

        public async Task<IDisposable> AddressModified()
        {
            return _jamesClient.AddressModified.Watch().Subscribe();
        }

        public async Task SetAddress(Address address)
        {
            await _jamesClient.SetAddress.ExecuteAsync(new SetAddressInput
            {
                AddressId = address.Id, Address1 = address.Address1, Address2 = address.Address2,
                Address3 = address.Address3, City=address.City, StateCode = address.StateCode, 
                PostalCode = address.PostalCode
            });
        }

        private static string ErrorToString(IClientError error)
        {
            //TODO: Make this better and put somewhere where the code can be shared.
            return
                $"Message: {error.Message}\r\nCode: {error.Code}\r\nException: {error.Exception}\r\nPath: {error.Path}\r\nExtensions: {error.Extensions}";
        }
        private static IDataAccessResult<T> GraphQLResult<T>(IOperationResult? graphQLResult)
            where T : new()
        {
            var data = ThisToThat.ToEntityType<T>(graphQLResult?.Data);
            return new DataAccessResult<T>
            { Data = data, Errors = graphQLResult?.Errors.Select(ErrorToString).ToArray()?? [] };
        }
    }
}
