using James.Shared.Model;

namespace James.Shared.Dto;

public class CreditReportDto
{
    public List<CreditReportDm> CreditReportAgencies { get; set; } = [];
    public List<CreditReportHistory> CreditReports { get; set; } = [];
}