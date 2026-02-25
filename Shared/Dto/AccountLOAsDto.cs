namespace James.Shared.Dto;

public class AccountLOAsDto
{
    public List<AccountLOADetailDto> AccountLOAs { get; set; } = [];
    public List<AccountLOADetailDto> AgencyLOAs { get; set; } = [];
    public int LOATotal { get; set; } = 0;
}