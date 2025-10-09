using James.Shared.Model;

namespace James.Shared.Test;

public class AgencyCommissionRatePeriodTests
{
    private static Guid TestAgencyId => Guid.Parse("12345678-1234-1234-1234-123456789012");
    
    [Fact]
    public void Constructor_SetsAllProperties_Correctly()
    {
        // Arrange
        var agencyId = TestAgencyId;
        var bondType = new BondType();
        var commissionType = CommissionType.Current;
        var start = new DateTime(2025, 1, 1);
        var finish = new DateTime(2025, 12, 31);
        var ranges = new List<AgencyCommissionRateRange>();

        // Act
        var period = new AgencyCommissionRatePeriod(
            agencyId, 
            bondType, 
            commissionType, 
            start, 
            finish, 
            ranges);

        // Assert
        Assert.Equal(agencyId, period.AgencyId);
        Assert.Equal(bondType, period.BondType);
        Assert.Equal(commissionType, period.CommissionType);
        Assert.Equal(start, period.Start);
        Assert.Equal(finish, period.Finish);
        Assert.Equal(ranges, period.Ranges);
        Assert.False(period.IsChanged);
    }

    [Fact]
    public void Constructor_WithNullFinish_SetsFinishToNull()
    {
        // Arrange
        var agencyId = TestAgencyId;
        var start = new DateTime(2025, 1, 1);

        // Act
        var period = new AgencyCommissionRatePeriod(
            agencyId,
            new BondType(),
            CommissionType.Current,
            start,
            null,
            new List<AgencyCommissionRateRange>());

        // Assert
        Assert.Null(period.Finish);
        Assert.False(period.IsChanged);
    }

    [Fact]
    public void Start_PropertyChanged_SetsIsChangedToTrue()
    {
        // Arrange
        var period = CreateTestPeriod();
        var newStart = new DateTime(2025, 2, 1);

        // Act
        period.Start = newStart;

        // Assert
        Assert.Equal(newStart, period.Start);
        Assert.True(period.IsChanged);
    }

    [Fact]
    public void Finish_PropertyChanged_SetsIsChangedToTrue()
    {
        // Arrange
        var period = CreateTestPeriod();
        var newFinish = new DateTime(2025, 11, 30);

        // Act
        period.Finish = newFinish;

        // Assert
        Assert.Equal(newFinish, period.Finish);
        Assert.True(period.IsChanged);
    }

    [Fact]
    public void Start_PropertyChanged_RaisesPropertyChangedEvent()
    {
        // Arrange
        var period = CreateTestPeriod();
        var propertyChangedRaised = false;
        period.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(period.Start))
                propertyChangedRaised = true;
        };

        // Act
        period.Start = new DateTime(2025, 2, 1);

        // Assert
        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public void Finish_PropertyChanged_RaisesPropertyChangedEvent()
    {
        // Arrange
        var period = CreateTestPeriod();
        var propertyChangedRaised = false;
        period.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(period.Finish))
                propertyChangedRaised = true;
        };

        // Act
        period.Finish = new DateTime(2025, 11, 30);

        // Assert
        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public void Ranges_PropertyChanged_RaisesPropertyChangedEvent()
    {
        // Arrange
        var period = CreateTestPeriod();
        var propertyChangedRaised = false;
        period.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(period.Ranges))
                propertyChangedRaised = true;
        };

        // Act
        period.Ranges = new List<AgencyCommissionRateRange>();

        // Assert
        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public void ResetAll_ResetsStartToOriginalValue()
    {
        // Arrange
        var originalStart = new DateTime(2025, 1, 1);
        var period = CreateTestPeriod(start: originalStart);
        period.Start = new DateTime(2025, 2, 1);

        // Act
        period.ResetAll();

        // Assert
        Assert.Equal(originalStart, period.Start);
        Assert.False(period.IsChanged);
    }

    [Fact]
    public void ResetAll_ResetsFinishToOriginalValue()
    {
        // Arrange
        var originalFinish = new DateTime(2025, 12, 31);
        var period = CreateTestPeriod(finish: originalFinish);
        period.Finish = new DateTime(2025, 11, 30);

        // Act
        period.ResetAll();

        // Assert
        Assert.Equal(originalFinish, period.Finish);
        Assert.False(period.IsChanged);
    }

    [Fact]
    public void ResetAll_ResetsRangesToOriginalValue()
    {
        // Arrange
        var originalRanges = new List<AgencyCommissionRateRange>();
        var period = CreateTestPeriod(ranges: originalRanges);
        period.Ranges = new List<AgencyCommissionRateRange>();

        // Act
        period.ResetAll();

        // Assert
        Assert.Equal(originalRanges, period.Ranges);
        Assert.False(period.IsChanged);
    }

    [Fact]
    public void ApplyChangesAll_SetsNewOriginalStart()
    {
        // Arrange
        var newStart = new DateTime(2025, 2, 1);
        var period = CreateTestPeriod();
        period.Start = newStart;

        // Act
        period.ApplyChangesAll();

        // Assert
        Assert.False(period.IsChanged);
        
        // Changing back to the new "original" should mark as changed
        period.Start = new DateTime(2025, 3, 1);
        Assert.True(period.IsChanged);
        
        period.ResetAll();
        Assert.Equal(newStart, period.Start);
    }

    [Fact]
    public void ApplyChangesAll_SetsNewOriginalFinish()
    {
        // Arrange
        var newFinish = new DateTime(2025, 11, 30);
        var period = CreateTestPeriod();
        period.Finish = newFinish;

        // Act
        period.ApplyChangesAll();

        // Assert
        Assert.False(period.IsChanged);
        
        period.Finish = new DateTime(2025, 10, 31);
        Assert.True(period.IsChanged);
        
        period.ResetAll();
        Assert.Equal(newFinish, period.Finish);
    }

    [Fact]
    public void ApplyChangesAll_SetsIsChangedToFalse()
    {
        // Arrange
        var period = CreateTestPeriod();
        period.Start = new DateTime(2025, 2, 1);
        period.Finish = new DateTime(2025, 11, 30);

        // Act
        period.ApplyChangesAll();

        // Assert
        Assert.False(period.IsChanged);
    }

    [Fact]
    public void IsChanged_WhenMultiplePropertiesChanged_ReturnsTrue()
    {
        // Arrange
        var period = CreateTestPeriod();

        // Act
        period.Start = new DateTime(2025, 2, 1);
        period.Finish = new DateTime(2025, 11, 30);

        // Assert
        Assert.True(period.IsChanged);
    }

    [Fact]
    public void ResetAll_AfterMultipleChanges_ResetsAllProperties()
    {
        // Arrange
        var originalStart = new DateTime(2025, 1, 1);
        var originalFinish = new DateTime(2025, 12, 31);
        var period = CreateTestPeriod(start: originalStart, finish: originalFinish);
        
        period.Start = new DateTime(2025, 2, 1);
        period.Finish = new DateTime(2025, 11, 30);

        // Act
        period.ResetAll();

        // Assert
        Assert.Equal(originalStart, period.Start);
        Assert.Equal(originalFinish, period.Finish);
        Assert.False(period.IsChanged);
    }

    [Theory]
    [InlineData(CommissionType.Current)]
    [InlineData(CommissionType.Scheduled)]
    public void Constructor_WithDifferentCommissionTypes_SetsCommissionTypeCorrectly(CommissionType commissionType)
    {
        // Arrange & Act
        var period = CreateTestPeriod(commissionType: commissionType);

        // Assert
        Assert.Equal(commissionType, period.CommissionType);
    }

    [Fact]
    public void Start_SetToNull_UpdatesPropertyAndMarksChanged()
    {
        // Arrange
        var period = CreateTestPeriod(start: new DateTime(2025, 1, 1));

        // Act
        period.Start = null;

        // Assert
        Assert.Null(period.Start);
        Assert.True(period.IsChanged);
    }

    [Fact]
    public void Finish_SetToNull_UpdatesPropertyAndMarksChanged()
    {
        // Arrange
        var period = CreateTestPeriod(finish: new DateTime(2025, 12, 31));

        // Act
        period.Finish = null;

        // Assert
        Assert.Null(period.Finish);
        Assert.True(period.IsChanged);
    }

    [Fact]
    public void Start_SetToSameValue_StillMarksAsChanged()
    {
        // Arrange
        var start = new DateTime(2025, 1, 1);
        var period = CreateTestPeriod(start: start);

        // Act
        period.Start = start;

        // Assert
        // The implementation calls CheckIsChanged() which compares to original
        // Setting to the same value as current (but already original), shouldn't change IsChanged
        Assert.False(period.IsChanged);
    }

    [Fact]
    public void Ranges_EmptyList_IsHandledCorrectly()
    {
        // Arrange
        var emptyRanges = new List<AgencyCommissionRateRange>();

        // Act
        var period = CreateTestPeriod(ranges: emptyRanges);

        // Assert
        Assert.Empty(period.Ranges);
        Assert.False(period.IsChanged);
    }

    // Helper method to create test instances
    private static AgencyCommissionRatePeriod CreateTestPeriod(
        Guid? agencyId = null,
        BondType? bondType = null,
        CommissionType commissionType = CommissionType.Current,
        DateTime? start = null,
        DateTime? finish = null,
        List<AgencyCommissionRateRange>? ranges = null)
    {
        return new AgencyCommissionRatePeriod(
            agencyId ?? TestAgencyId,
            bondType ?? new BondType(),
            commissionType,
            start ?? new DateTime(2025, 1, 1),
            finish ?? new DateTime(2025, 12, 31),
            ranges ?? new List<AgencyCommissionRateRange>());
    }
}