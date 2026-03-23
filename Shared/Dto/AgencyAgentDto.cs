using James.Shared.Model;

namespace James.Shared.Dto;

public class AgencyAgentDto
{
    public Guid Id { get; set; }
    public Guid? AgencyId { get; set; }
    public Guid AgentId { get; set; }
    public string AgencyName { get; set; } = string.Empty;
    public string AgencyNum { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? GivenName { get; set; } = string.Empty;
    public string? MiddleInitial { get; set; } = string.Empty;
    public string? FamilyName { get; set; } = string.Empty;
    public string? NationalProducerNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public bool AIF { get; set; }
    public bool Active { get; set; }
}