namespace James.Shared.Model;

public class AgencyLicenseBulk
{
    public Guid Id { get; set; }
    public Guid AgencyId { get; set; }
    public string State { get; set; } = null!;
    public string? LicenseNumber { get; set; }
    public bool IsResident { get; set; }
    public Guid InsurerId { get; set; }
    public DateOnly? Expiration { get; set; }
    public string? Comments { get; set; }
    public DateOnly? Appointment { get; set; }
    public DateOnly? Termination { get; set; }
    public bool AppointingState { get; set; }
    public bool IsActive { get; set; }
}