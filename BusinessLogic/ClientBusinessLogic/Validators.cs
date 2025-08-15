using James.Shared.Model;
using System.Net.Mail;
using SharedBusinessLogic;

namespace ClientBusinessLogic;

public static class Validators
{
    /// <summary>
    /// Validate whether an email is a valid format.
    /// </summary>
    /// <param name="email">The email to validate.</param>
    /// <returns>True if the email is valid, false if it is invalid.</returns>
    public static bool ValidateEmail(string email)
    {
        if ("" == email)
        {
            return false;
        }
        else
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

    public static bool ValidateAddress(Address address)
    {
        if (address == null)
        {
            return false;
        }
        else if (string.IsNullOrWhiteSpace(address.Address1) ||
                 string.IsNullOrWhiteSpace(address.City) ||
                 string.IsNullOrWhiteSpace(address.StateCode) ||
                 string.IsNullOrWhiteSpace(address.PostalCode))
        {
            return false;
        }
        else
        {
            //TODO: Check for valid StateCode.
            return true;
        }
    }

    /// <summary>
    /// Validates if a filename is a valid Windows filename with an extension
    /// </summary>
    /// <param name="filename">The filename</param>
    /// <returns>True if valid</returns>
    /// <remarks>Ignores any path before the file name.</remarks>
    public static bool ValidateFileName(string filename)
    {
        return ImagingBusinessLogic.ValidWindowsFilenamePattern.IsMatch(filename);
    }

    /// <summary>
    /// Validates the fields of a Power of Attorney object and returns a list of errors if any required fields are invalid or missing.
    /// </summary>
    /// <param name="poa">The Power of Attorney object to validate.</param>
    /// <returns>A list of error messages indicating the fields that are invalid or missing. Returns an empty list if all fields are valid.</returns>
    public static List<string> ValidatePoaFields(PowerOfAttorney poa)
    {
        var errors = new List<string>();

        if (poa.AgencyId == Guid.Empty)
        {
            errors.Add("Agency has not been defined. Please correct and try again.");
        }

        if (string.IsNullOrWhiteSpace(poa.Status))
        {
            errors.Add("Status has not been defined. Please correct and try again.");
        }

        if (poa.Insurer.Id == Guid.Empty)
        {
            errors.Add("Insurer has not been defined. Please correct and try again.");
        }

        return errors;
    }

    #region Agency Commission Period Validation

    /// <summary>
    /// Validates a new period for agency commissions to ensure it does not overlap with existing periods
    /// and adheres to rules based on the commission type (current or scheduled).
    /// </summary>
    /// <param name="effectiveDate">The effective start date of the new period.</param>
    /// <param name="expireDate">The expiration date of the new period, if applicable.</param>
    /// <param name="existingPeriods">A list of existing date ranges for agency commissions.</param>
    /// <param name="commissionType">The type of commission (current or scheduled) being validated.</param>
    /// <returns>A list of error messages if validation issues are detected; an empty list if the period is valid.</returns>
    public static List<string> ValidatePeriodForAgencyCommissions(DateTime effectiveDate, DateTime? expireDate,
        List<DateRange> existingPeriods, CommissionType commissionType)
    {
        var errors = new List<string>();

        var maxEffectiveDate = existingPeriods.Max(a => a.Start);

        if (commissionType == CommissionType.Current)
        {
            ValidateCurrentCommissionDate(effectiveDate, existingPeriods, errors, maxEffectiveDate);
        }
        else
        {
            ValidateScheduleCommissionDates(effectiveDate, expireDate, existingPeriods, errors);
        }

        return errors;
    }

    /// <summary>
    /// Validates the schedule commission dates to ensure they adhere to specified business rules and do not conflict with existing periods.
    /// </summary>
    /// <param name="effectiveDate">The start date for the commission period being validated.</param>
    /// <param name="expireDate">The end date for the commission period being validated.</param>
    /// <param name="existingPeriods">A collection of existing commission date ranges to compare against.</param>
    /// <param name="errors">A collection to which any validation error messages will be added.</param>
    private static void ValidateScheduleCommissionDates(DateTime effectiveDate, DateTime? expireDate,
        List<DateRange> existingPeriods, List<string> errors)
    {
        if (effectiveDate > expireDate)
            errors.Add("Effective date cannot be after the expire date.");

        var included = existingPeriods.Any(a =>
            (effectiveDate >= a.Start && effectiveDate <= a.End) ||
            (expireDate >= a.Start && expireDate <= a.End));

        if (included)
            errors.Add("The dates provided are inside of previous periods.");
    }

    /// <summary>
    /// Validates the current commission date against existing periods and checks for conflicts or invalid dates.
    /// </summary>
    /// <param name="effectiveDate">The effective date to validate.</param>
    /// <param name="existingPeriods">A list of existing date ranges to validate against.</param>
    /// <param name="errors">A list to which validation error messages will be added.</param>
    /// <param name="maxEffectiveDate">The maximum effective date of the previous periods.</param>
    private static void ValidateCurrentCommissionDate(DateTime effectiveDate,
        List<DateRange> existingPeriods, List<string> errors, DateTime? maxEffectiveDate)
    {
        var included = existingPeriods.Any(a =>
            effectiveDate >= a.Start && effectiveDate <= a.End);

        if (included)
            errors.Add("The dates provided are inside of previous periods.");

        if (effectiveDate <= maxEffectiveDate)
            errors.Add("Effective date cannot be before the previous effective date.");
    }

    #endregion

    #region Agency Commission Range and Rate validation

    /// <summary>
    /// Identifies gaps in a list of agency commission rate ranges and returns any errors found as a dictionary.
    /// </summary>
    /// <param name="ranges">The list of agency commission rate ranges to validate for gaps.</param>
    /// <returns>A dictionary containing error messages with their associated boolean status indicating invalid ranges.</returns>
    public static Dictionary<string, bool> LookForGaps(List<AgencyCommissionRateRange> ranges)
    {
        var errors = new Dictionary<string, bool>();

        ranges = ranges.OrderBy(a => a.From).ToList();

        var first = true;
        var previousMax = 0;
        var previousLine = 0;
        foreach (var range in ranges)
        {
            if (first)
            {
                if (range.From != 1)
                    errors.Add($"The first line must start at 1.", false);

                previousLine = range.Id;
                previousMax = range.To ?? 0;
                first = false;
                continue;
            }

            if (range.From != previousMax + 1)
            {
                errors.Add($"There is a gap between lines {previousLine} and {range.Id}.", false);
            }

            previousLine = range.Id;
            previousMax = range.To ?? 0;
        }

        return errors;
    }

    /// <summary>
    /// Determines the ID of the first gap in a sequence of agency commission rate ranges.
    /// </summary>
    /// <param name="ranges">The list of agency commission rate ranges to evaluate.</param>
    /// <returns>The ID of the range where the first gap is found, or 0 if no gap exists.</returns>
    public static int GetFirstGap(List<AgencyCommissionRateRange> ranges)
    {
        var previousLine = 0;
        var previousLineMax = 0;
        foreach (var range in ranges)
        {
            if (previousLineMax != 0 && range.From != previousLineMax + 1)
            {
                return previousLine;
            }

            previousLineMax = range.To ?? 0;
            previousLine = range.Id;
        }

        return 0;
    }

    /// <summary>
    /// Validates the range of agency commission rates and checks for potential conflicts or invalid values.
    /// </summary>
    /// <param name="existingRanges">A list of existing agency commission rate ranges.</param>
    /// <param name="editedRange">The agency commission rate range being validated.</param>
    /// <returns>A dictionary containing error messages as keys, with a boolean indicating the presence of an error.</returns>
    public static Dictionary<string, bool> ValidateAgencyCommissionRange(List<AgencyCommissionRateRange> existingRanges,
        AgencyCommissionRateRange editedRange)
    {
        var errors = new Dictionary<string, bool>();

        existingRanges = existingRanges.OrderBy(a => a.To).ToList();
        var previous = existingRanges
            .OrderByDescending(o => o.To)
            .FirstOrDefault(f => f.To < editedRange.From);
        var next = existingRanges
            .OrderBy(o => o.Id)
            .FirstOrDefault(f => f.Id > editedRange.Id);

        if (previous != null && previous.To + 1 != editedRange.From)
            errors.Add("The minimum must be the previous range's maximum plus 1.", true);

        if (next != null && next.From <= editedRange.To)
            errors.Add("The maximum must not exceed the next range's minimum minus 1.", true);

        if (editedRange.From >= editedRange.To)
            errors.Add("The minimum must be less than the maximum.", true);

        if (editedRange.From < 1)
            errors.Add("The minimum must be greater than 0.", true);

        if (editedRange.To < 1)
            errors.Add("The maximum must be greater than 0.", true);

        switch (editedRange.Rate)
        {
            case < 0:
                errors.Add("The rate must be greater than or equal to 0.", true);
                break;
            case > 1:
                errors.Add("The rate must be less than or equal to 100%.", true);
                break;
        }

        return errors;
    }

    #endregion
}