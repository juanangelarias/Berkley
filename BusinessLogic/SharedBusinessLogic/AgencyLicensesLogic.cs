using James.Shared.Model;

namespace SharedBusinessLogic;

public static class AgencyLicensesLogic
{
    public static List<string> Validate(List<AgencyLicense> licenses, bool isNew)
    {
        var errors = new List<string>();

        var stateErrs = 0;
        var insurerErrs = 0;
        foreach (var license in licenses)
        {
            if (string.IsNullOrWhiteSpace(license.State))
                stateErrs++;

            if (license.Insurer.Id == Guid.Empty)
                insurerErrs++;
        }

        if (stateErrs > 0)
            errors.Add($"State has not been defined in {stateErrs} rows. Please correct and try again.");

        if (insurerErrs > 0)
            errors.Add($"Insurer has not been defined in {insurerErrs} rows. Please correct and try again.");

        return errors;
    }
}