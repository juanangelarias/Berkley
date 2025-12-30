namespace James.Shared.Dto;

public class AccountCollateralDto
{
    public string BondNumber { get; set; } = null!;
    public string Bank { get; set; } = null!;
    public int IlocOrCash { get; set; }
    public DateOnly ExpireationDate { get; set; }
}