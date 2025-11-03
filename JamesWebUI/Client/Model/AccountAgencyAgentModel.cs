using James.Shared;

namespace JamesWebUI.Client.Model;

public class AccountAgencyAgentModel(string? agencyNumber, Guid? agentId)
{
    public string? AgencyNumber { get; set; } = agencyNumber;
    public Guid? AgentId { get; set; } = agentId;
}