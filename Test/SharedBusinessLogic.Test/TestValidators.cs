using ClientBusinessLogic;
using James.Shared.Model;

namespace SharedBusinessLogic.Test;

public class TestValidators
{
    [Theory]
    [InlineData("")]            // empty
    [InlineData(" ")]           // whitespace
    [InlineData("no-at.com")]   // missing @
    [InlineData("@domain.com")] // missing local
    public void ValidateEmail_Invalid_ReturnsFalse(string email)
    {
        var result = Validators.ValidateEmail(email);
        Assert.False(result);
    }

    [Theory]
    [InlineData("a@b.co")]
    [InlineData("user.name+tag@domain.com")]
    [InlineData("firstname.lastname@sub.domain.org")]
    public void ValidateEmail_Valid_ReturnsTrue(string email)
    {
        var result = Validators.ValidateEmail(email);
        Assert.True(result);
    }

    [Fact]
    public void ValidateAddress_Null_ReturnsFalse()
    {
        Assert.False(Validators.ValidateAddress(null));
    }

    [Theory]
    [InlineData(null, "City", "ST", "12345")]
    [InlineData("123 Main", null, "ST", "12345")]
    [InlineData("123 Main", "City", null, "12345")]
    [InlineData("123 Main", "City", "ST", null)]
    [InlineData("  ", "City", "ST", "12345")]
    public void ValidateAddress_MissingRequired_ReturnsFalse(string a1, string city, string state, string zip)
    {
        var address = new Address { Address1 = a1, City = city, StateCode = state, PostalCode = zip };
        Assert.False(Validators.ValidateAddress(address));
    }

    [Fact]
    public void ValidateAddress_AllRequiredPresent_ReturnsTrue()
    {
        var address = new Address { Address1 = "123 Main", City = "City", StateCode = "ST", PostalCode = "12345" };
        Assert.True(Validators.ValidateAddress(address));
    }

    [Theory]
    [InlineData("file.txt", true)]
    [InlineData("C:\\path\\to\\file.txt", true)]
    [InlineData("notes.md", true)]
    [InlineData("bad*name.txt", false)]
    [InlineData("noext", false)]
    public void ValidateFileName_WindowsPattern(string filename, bool expected)
    {
        var result = Validators.ValidateFileName(filename);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ValidatePoaFields_AllInvalid_ReturnsThreeErrors()
    {
        var poa = new PowerOfAttorney
        {
            AgencyId = Guid.Empty,
            Status = null,
            Insurer = new Insurer { Id = Guid.Empty }
        };

        var errors = Validators.ValidatePoaFields(poa);

        Assert.Contains("Agency has not been defined. Please correct and try again.", errors);
        Assert.Contains("Status has not been defined. Please correct and try again.", errors);
        Assert.Contains("Insurer has not been defined. Please correct and try again.", errors);
        Assert.Equal(3, errors.Count);
    }

    [Fact]
    public void ValidatePoaFields_AllValid_ReturnsNoErrors()
    {
        var poa = new PowerOfAttorney
        {
            AgencyId = Guid.NewGuid(),
            Status = "Active",
            Insurer = new Insurer { Id = Guid.NewGuid() }
        };

        var errors = Validators.ValidatePoaFields(poa);

        Assert.Empty(errors);
    }

    [Fact]
    public void ValidatePeriodForAgencyCommissions_Current_IncludedAndBeforeMax_ReturnsBothErrors()
    {
        var periods = new List<DateRange>
        {
            new() { Start = new DateTime(2024,1,1), End = new DateTime(2024,6,30) },
            new() { Start = new DateTime(2024,7,1), End = new DateTime(2024,12,31) },
        };

        var effective = new DateTime(2024, 6, 15); // inside first period and <= max start (2024-07-01)

        var errors = Validators.ValidatePeriodForAgencyCommissions(effective, null, periods, CommissionType.Current);

        Assert.Contains("The dates provided are inside of previous periods.", errors);
        Assert.Contains("Effective date cannot be before the previous effective date.", errors);
        Assert.Equal(2, errors.Count);
    }

    [Fact]
    public void ValidatePeriodForAgencyCommissions_Current_AfterMaxStart_NoErrorsWhenNotOverlapping()
    {
        var periods = new List<DateRange>
        {
            new() { Start = new DateTime(2024,1,1), End = new DateTime(2024,6,30) },
        };

        var effective = new DateTime(2024, 7, 1);

        var errors = Validators.ValidatePeriodForAgencyCommissions(effective, null, periods, CommissionType.Current);

        Assert.Empty(errors);
    }

    [Fact]
    public void ValidatePeriodForAgencyCommissions_Scheduled_MissingExpire_Errors()
    {
        var periods = new List<DateRange>
        {
            new() { Start = new DateTime(2024,1,1), End = new DateTime(2024,6,30) },
        };

        var errors = Validators.ValidatePeriodForAgencyCommissions(new DateTime(2024,7,1), null, periods, CommissionType.Scheduled);

        Assert.Contains("An expiration date is required for scheduled commissions.", errors);
    }

    [Fact]
    public void ValidatePeriodForAgencyCommissions_Scheduled_EffectiveAfterExpire_Error()
    {
        var periods = new List<DateRange>
        {
            new() { Start = new DateTime(2024,1,1), End = new DateTime(2024,6,30) },
        };

        var errors = Validators.ValidatePeriodForAgencyCommissions(
            new DateTime(2024,8,1),
            new DateTime(2024,7,31),
            periods,
            CommissionType.Scheduled);

        Assert.Contains("Effective date cannot be after the expire date.", errors);
    }

    [Fact]
    public void ValidatePeriodForAgencyCommissions_Scheduled_OverlapsExisting_Error()
    {
        var periods = new List<DateRange>
        {
            new() { Start = new DateTime(2024,1,1), End = new DateTime(2024,12,31) },
        };

        var errors = Validators.ValidatePeriodForAgencyCommissions(
            new DateTime(2024,6,1),
            new DateTime(2024,6,30),
            periods,
            CommissionType.Scheduled);

        Assert.Contains("The dates provided are inside of previous periods.", errors);
    }

    [Fact]
    public void ValidatePeriodForAgencyCommissions_Scheduled_NoOverlap_NoErrors()
    {
        var periods = new List<DateRange>
        {
            new() { Start = new DateTime(2024,1,1), End = new DateTime(2024,6,30) },
        };

        var errors = Validators.ValidatePeriodForAgencyCommissions(
            new DateTime(2024,7,1),
            new DateTime(2024,12,31),
            periods,
            CommissionType.Scheduled);

        Assert.Empty(errors);
    }

    [Fact]
    public void LookForGaps_WithGap_FindsError()
    {
        var ranges = new List<AgencyCommissionRateRange>
        {
            new() { Id = 1, From = 1, To = 5, Rate = 0.1d },
            new() { Id = 2, From = 7, To = 10, Rate = 0.2d }, // gap between 5 and 7
        };

        var errors = Validators.LookForGaps(ranges);

        Assert.Contains(errors, e => e.Key.Contains("gap between lines 1 and 2"));
        Assert.All(errors.Values, v => Assert.False(v));
    }

    [Fact]
    public void LookForGaps_FirstNotStartingAt1_FindsError()
    {
        var ranges = new List<AgencyCommissionRateRange>
        {
            new() { Id = 1, From = 2, To = 5, Rate = 0.1d },
        };

        var errors = Validators.LookForGaps(ranges);

        Assert.Contains("The first line must start at 1.", errors.Keys);
    }

    [Fact]
    public void GetFirstGap_NoGap_ReturnsZero()
    {
        var ranges = new List<AgencyCommissionRateRange>
        {
            new() { Id = 1, From = 1, To = 5, Rate = 0.1d },
            new() { Id = 2, From = 6, To = 10, Rate = 0.2d },
        };

        var id = Validators.GetFirstGap(ranges);

        Assert.Equal(0, id);
    }

    [Fact]
    public void GetFirstGap_WithGap_ReturnsIdOfPrevious()
    {
        var ranges = new List<AgencyCommissionRateRange>
        {
            new() { Id = 10, From = 1, To = 5, Rate = 0.1d },
            new() { Id = 20, From = 7, To = 10, Rate = 0.2d },
        };

        var id = Validators.GetFirstGap(ranges);

        Assert.Equal(10, id);
    }

    [Fact]
    public void ValidateAgencyCommissionRange_MinMustFollowPrevious()
    {
        var existing = new List<AgencyCommissionRateRange>
        {
            new() { Id = 1, From = 1, To = 5, Rate = 0.1d },
            new() { Id = 2, From = 6, To = 10, Rate = 0.2d },
        };
        var edited = new AgencyCommissionRateRange { Id = 2, From = 8, To = 12, Rate = 0.2d };

        var errors = Validators.ValidateAgencyCommissionRange(existing, edited);

        Assert.Contains("The minimum must be the previous range's maximum plus 1.", errors.Keys);
    }

    [Fact]
    public void ValidateAgencyCommissionRange_MaxMustNotExceedNextMinMinus1()
    {
        var existing = new List<AgencyCommissionRateRange>
        {
            new() { Id = 1, From = 1, To = 5, Rate = 0.1d },
            new() { Id = 3, From = 11, To = 20, Rate = 0.3d },
        };
        var edited = new AgencyCommissionRateRange { Id = 2, From = 6, To = 12, Rate = 0.2d };

        var errors = Validators.ValidateAgencyCommissionRange(existing, edited);

        Assert.Contains("The maximum must not exceed the next range's minimum minus 1.", errors.Keys);
    }

    [Fact]
    public void ValidateAgencyCommissionRange_MinLessThanMax_AndPositive()
    {
        var existing = new List<AgencyCommissionRateRange>();
        var edited = new AgencyCommissionRateRange { Id = 1, From = 5, To = 4, Rate = 0.1d };

        var errors = Validators.ValidateAgencyCommissionRange(existing, edited);

        Assert.Contains("The minimum must be less than the maximum.", errors.Keys);
    }

    [Fact]
    public void ValidateAgencyCommissionRange_MinAndMaxMustBeGreaterThanZero()
    {
        var existing = new List<AgencyCommissionRateRange>();
        var edited = new AgencyCommissionRateRange { Id = 1, From = 0, To = 0, Rate = 0.1d };

        var errors = Validators.ValidateAgencyCommissionRange(existing, edited);

        Assert.Contains("The minimum must be greater than 0.", errors.Keys);
        Assert.Contains("The maximum must be greater than 0.", errors.Keys);
    }

    [Fact]
    public void ValidateAgencyCommissionRange_RateBounds()
    {
        var existing = new List<AgencyCommissionRateRange>();

        var negativeRate = new AgencyCommissionRateRange { Id = 1, From = 1, To = 2, Rate = -0.01d };
        var overOneRate  = new AgencyCommissionRateRange { Id = 2, From = 3, To = 4, Rate = 1.01d };

        var e1 = Validators.ValidateAgencyCommissionRange(existing, negativeRate);
        var e2 = Validators.ValidateAgencyCommissionRange(existing, overOneRate);

        Assert.Contains("The rate must be greater than or equal to 0.", e1.Keys);
        Assert.Contains("The rate must be less than or equal to 100%.", e2.Keys);
    }

    [Fact]
    public void ValidateAgencyCommissionRange_NoErrorsForValidRange()
    {
        var existing = new List<AgencyCommissionRateRange>
        {
            new() { Id = 1, From = 1, To = 5, Rate = 0.1d },
            new() { Id = 3, From = 11, To = 20, Rate = 0.3d },
        };
        var edited = new AgencyCommissionRateRange { Id = 2, From = 6, To = 10, Rate = 0.2d };

        var errors = Validators.ValidateAgencyCommissionRange(existing, edited);

        Assert.Empty(errors);
    }
}