namespace James.Shared.Dto;

public class AccountCollateralDto
{
    public string? BondNumber { get; set; }
    public string Bank { get; set; } = null!;
    public string? Type { get; set; }
    public int Amount { get; set; }
    public DateOnly? ExpirationDate { get; set; }
}