using James.Data.Client.GraphQL;
using James.Shared;
using James.Shared.Data;
using James.Shared.Model;
using StrawberryShake;
using System.Diagnostics;
using Severity = James.Shared.Model.Severity;

namespace James.Data.Client
{
    public class ClientDataAccess(IJamesClient jamesClient, ILoggingService logging) : IDataAccess
    {
        public async Task<IDataAccessResult<List<Account>>> GetAgencyAccounts(string agencyNumber)
        {
            try
            {
                var result = await jamesClient.AgencyAccounts.ExecuteAsync(agencyNumber);
                if (result.Errors.Any())
                    return new DataAccessResult<List<Account>> { Errors = result.Errors.Select(ErrorToString).ToArray() };
                Debug.Assert(result.Data != null, "result.Data != null");
                var accountList =
                    new List<Account>(
                        result.Data.AgencyAccounts.Select(ThisToThat.ToEntityType<Account>));
                return new DataAccessResult<List<Account>> { Data = accountList };
            }
            catch (Exception e)
            {
                logging.LogException(e, "GetAgencyByAgencyNumber returned exception",
                    e.ToText(), Severity.Error, "GraphQl");
                throw;
            }
        }

        public async Task<IDataAccessResult<Agency>> GetAgencyByAgencyNumber(string agencyNumber)
        {
            var result = await jamesClient.GetAgencyByAgencyNumber.ExecuteAsync(agencyNumber);
            if (result.Errors.Count == 0)
            {
                Debug.Assert(result.Data != null, "result.Data != null");
                return new DataAccessResult<Agency> { Data = ThisToThat.ToEntityType<Agency>(result.Data.AgencyByAgencyNumber) };
            }

            logging.LogWarning("GetAgencyByAgencyNumber returned error(s)",
                string.Join("\r\n", result.Errors.Select(ErrorToString)), "GraphQl");
            return new DataAccessResult<Agency> { Errors = result.Errors.Select(ErrorToString).ToArray() };
        }

        public async Task<IDataAccessResult<List<AgencyLicense>>> GetAgencyLicenses(Guid agencyId)
        {
            var result = await jamesClient.GetAgencyLicenses.ExecuteAsync(agencyId);
            return GraphQLResult<List<AgencyLicense>>(result);
        }

        public async Task<IDataAccessResult<List<Insurer>>> GetAllInsurers()
        {
            var result = await jamesClient.AllInsurers.ExecuteAsync();
            return GraphQLResult<List<Insurer>>(result);
        }

        public async Task<IDataAccessResult<List<State>>> GetAllStates()
        {
            var result = await jamesClient.GetAllStates.ExecuteAsync();
            return GraphQLResult<List<State>>(result);
        }

        public async Task<IDataAccessResult<List<Bond>>> GetAgencyBonds(Guid agencyId)
        {
            var result = await jamesClient.GetAgencyBonds.ExecuteAsync(agencyId);
            return GraphQLResult<List<Bond>>(result);
        }

        public async Task<IDataAccessResult<List<AgentsInAgency>>> GetAgencyAgents(Guid agencyId)
        {
            var result = await jamesClient.GetAgencyAgents.ExecuteAsync(agencyId);
            return GraphQLResult<List<AgentsInAgency>>(result);
        }

        public async Task<IDataAccessResult<List<AgencyStatusDm>>> GetAgencyStatuses()
        {
            var result = await jamesClient.GetAgencyStatuses.ExecuteAsync();
            return GraphQLResult<List<AgencyStatusDm>>(result);
        }

        public async Task<IDataAccessResult<Agent>> GetAgent(Guid agentId)
        {
            var result = await jamesClient.AgentByAgentId.ExecuteAsync(agentId);
            return GraphQLResult<Agent>(result);
        }

        public async Task<IDataAccessResult<List<Agency>>> GetAgencyRelatedParties(Guid agencyId)
        {
            var result = await jamesClient.GetAgencyRelatedParties.ExecuteAsync(agencyId);
            return GraphQLResult<List<Agency>>(result);
        }

        public async Task<IDataAccessResult<List<Agency>>> SearchAgencies(string? search)
        {
            try
            {
                var result = await jamesClient.SearchAgencies.ExecuteAsync(search);
                return GraphQLResult<List<Agency>>(result);
            }
            catch (Exception ex)
            {
                var exceptionDetail = ex.ToText();
                logging.LogException(ex, "GetAgencyByAgencyNumber returned exception",
                    exceptionDetail, Severity.Error, "GraphQl");
                return new DataAccessResult<List<Agency>> { Data = [], Errors = [ex.Message] };
            }
        }

        public async Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId)
        {
            //TODO:Refactor to call this type of method with all boiler plate.
            //try
            //{
            //    var result = await _jamesClient.GetAgencyPOAs.ExecuteAsync(agencyId);
            //    return GraphQLResult<List<PowerOfAttorney>>(result);
            //}
            //catch (Exception ex)
            //{
            //    var exceptionDetail = ex.ToText();
            //    _logging.LogException(ex, "GetAgencyPOAs returned exception",
            //        exceptionDetail, Severity.Error, "GraphQl");
            //    return new DataAccessResult<List<PowerOfAttorney>> { Data = [], Errors = [ex.Message] };
            //}
            return await ExecuteGet<List<PowerOfAttorney>>(async () => await jamesClient.GetAgencyPOAs.ExecuteAsync(agencyId), "GetAgencyPOAs");
        }

        public async Task<IDataAccessResult<List<PowerOfAttorneyStatusDm>>> GetAllPoaStatuses()
        {
            return await ExecuteGet<List<PowerOfAttorneyStatusDm>>(async () => await jamesClient.GetAllPoaStatuses.ExecuteAsync(), "GetAllPoaStatuses");
        }

        public async Task<IDataAccessResult<PowerOfAttorney>> SetPowerOfAttorney(Guid poaId, Guid insurerId, int? limit, string? serial, DateOnly? firstIssued,
            DateOnly? currentIssued, string? comments, Guid status)
        {
            return await ExecuteGet<PowerOfAttorney>(async () =>
                await jamesClient.SetPowerOfAttorney.ExecuteAsync(new SetPowerOfAttorneyInput
                {
                    PoaId = poaId,
                    InsurerId = insurerId,
                    Limit = limit,
                    Serial = serial,
                    FirstIssued = firstIssued?.ToDateTime(TimeOnly.Parse("12:00 AM")),
                    CurrentIssued = currentIssued?.ToDateTime(TimeOnly.Parse("12:00 AM")),
                    Comments = comments,
                    Status = status
                }), "SetPowerOfAttorney");
        }

        public async Task<ISaveDataResult> DeleteLicense(Guid licenseId)
        {
            var result = await jamesClient.DeleteLicense.ExecuteAsync(new DeleteLicenseInput { LicenseId = licenseId });
            return GraphQLSaveResult(result);
        }

        public IDisposable AddressModified(Action<SubscriptionResult<Address>> onNext, Action? onError = null, Action? onComplete = null)
        {
            return jamesClient.AddressModified.Watch().Subscribe();
        }

        public async Task<ISaveDataResult> SetAddress(Address address)
        {
            var result = await jamesClient.SetAddress.ExecuteAsync(new SetAddressInput
            {
                AddressId = address.Id,
                Address1 = address.Address1,
                Address2 = address.Address2,
                Address3 = address.Address3,
                City = address.City,
                StateCode = address.StateCode,
                PostalCode = address.PostalCode
            });
            return GraphQLSaveResult(result);
        }

        public async Task<IDataAccessResult<AgencyLicense>> CreateLicense(Guid agencyId, Guid? agentId, bool? appointingState,
            string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state,
            bool isActive)
        {
            var result = await jamesClient.CreateLicense.ExecuteAsync(new CreateLicenseInput
            {
                AgencyId = agencyId,
                AgentId = agentId,
                AppointingState = appointingState,
                Comments = comments,
                Appointment = appointment?.ToDateTime(TimeOnly.Parse("12:00 AM")),
                Expiration = expiration?.ToDateTime(TimeOnly.Parse("12:00 AM")),
                Termination = termination?.ToDateTime(TimeOnly.Parse("12:00 AM")),
                InsurerId = insurerId,
                IsResident = isResident,
                LicenseNumber = licenseNumber,
                State = state,
                IsActive = isActive
            });
            return GraphQLResult<AgencyLicense>(result);
        }

        public async Task<IDataAccessResult<AgencyLicense>> SetAgencyLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool? appointingState, string? comments,
            DateOnly? appointment, DateOnly? expiration, DateOnly? termination, Guid insurerId, bool isResident,
            string? licenseNumber, string state, bool isActive)
        {
            var result = await jamesClient.SetAgencyLicense.ExecuteAsync(new SetLicenseInput
            {
                AgencyId = agencyId,
                AgentId = agentId,
                AppointingState = appointingState,
                Comments = comments,
                Appointment = appointment?.ToDateTime(TimeOnly.Parse("12:00 AM")),
                Expiration = expiration?.ToDateTime(TimeOnly.Parse("12:00 AM")),
                Termination = termination?.ToDateTime(TimeOnly.Parse("12:00 AM")),
                InsurerId = insurerId,
                IsResident = isResident,
                LicenseNumber = licenseNumber,
                State = state,
                IsActive = isActive
            });
            return GraphQLResult<AgencyLicense>(result);
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
            { Data = data, Errors = graphQLResult?.Errors.Select(ErrorToString).ToArray() ?? [] };
        }
        private static ISaveDataResult GraphQLSaveResult(IOperationResult? graphQLResult)
        {
            return new SaveDataResult
            { Errors = graphQLResult?.Errors.Select(ErrorToString).ToArray() ?? [] };
        }

        private async Task<IDataAccessResult<T>> ExecuteGet<T>(Func<Task<IOperationResult>> dataFunc, string graphQlFunctionName, T? defaultValue = null) where T : class, new()
        {
            try
            {
                var result = await dataFunc();
                return GraphQLResult<T>(result);
            }
            catch (Exception ex)
            {
                var exceptionDetail = ex.ToText();
                logging.LogException(ex, graphQlFunctionName + " returned exception",
                    exceptionDetail, Severity.Error, "GraphQl");
                return new DataAccessResult<T> { Data = defaultValue, Errors = [ex.Message] };
            }
        }
    }
}
