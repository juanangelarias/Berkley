using James.Shared.Model;

namespace SharedBusinessLogic.Test;

public class TestAgencyCommissionRateRange
{
    [Fact]
    public void Ctor_IsNew_InitializesOriginalsToZero_AndIsChangedReflectsSetters()
    {
        // Arrange
        var recordId = Guid.NewGuid();

        // Act
        var range = new AgencyCommissionRateRange(recordId, id: 1, from: 100, to: 200, rate: 0.1234, isNew: true);

        // Assert
        Assert.Equal(recordId, range.RecordId);
        Assert.Equal(1, range.Id);
        Assert.Equal(100, range.From);
        Assert.Equal(200, range.To);
        Assert.Equal(0.1234, range.Rate, 5);

        range.Rate = 0.25;
        Assert.True(range.IsChanged);
    }

    [Fact]
    public void Ctor_NotNew_OriginalsMatchValues_AndIsChangedIsFalse()
    {
        // Arrange
        var recordId = Guid.NewGuid();

        // Act
        var range = new AgencyCommissionRateRange(recordId, id: 2, from: 100, to: 200, rate: 0.05, isNew: false);

        // Assert
        Assert.False(range.IsChanged);
    }

    [Fact]
    public void Property_Setters_UpdateAndMarkIsChanged()
    {
        // Arrange
        var range = new AgencyCommissionRateRange(Guid.NewGuid(), 3, 10, 20, 0.10, isNew: false);
        Assert.False(range.IsChanged);

        // Act
        range.From = 11;

        // Assert
        Assert.Equal(11, range.From);
        Assert.True(range.IsChanged);

        // Act
        range.To = 21;

        // Assert
        Assert.Equal(21, range.To);
        Assert.True(range.IsChanged);

        // Act
        range.Rate = 0.11;

        // Assert
        Assert.Equal(0.11, range.Rate, 5);
        Assert.True(range.IsChanged);
    }

    [Fact]
    public void ApplyChanges_SetsOriginalsToCurrent_AndClearsIsChanged()
    {
        // Arrange
        var range = new AgencyCommissionRateRange(Guid.NewGuid(), 4, 100, 200, 0.2, isNew: false);
        range.From = 110;
        range.To = 220;
        range.Rate = 0.25;
        Assert.True(range.IsChanged);

        // Act
        range.ApplyChanges();

        // Assert
        Assert.False(range.IsChanged);

        // Flip back to originals should not set IsChanged now
        range.From = 110;
        range.To = 220;
        range.Rate = 0.25;
        Assert.False(range.IsChanged);
    }

    [Fact]
    public void Reset_RestoresOriginals_AndClearsIsChanged()
    {
        // Arrange
        var range = new AgencyCommissionRateRange(Guid.NewGuid(), 5, 50, 100, 0.07, isNew: false);
        range.From = 60;
        range.To = 120;
        range.Rate = 0.08;
        Assert.True(range.IsChanged);

        // Act
        range.Reset();

        // Assert
        Assert.Equal(50, range.From);
        Assert.Equal(100, range.To);
        Assert.Equal(0.07, range.Rate, 5);
        Assert.False(range.IsChanged);
    }

    [Fact]
    public void To_AllowsNull_AndAffectsIsChanged()
    {
        // Arrange
        var range = new AgencyCommissionRateRange(Guid.NewGuid(), 6, 0, null, 0.01, isNew: false);
        Assert.False(range.IsChanged);

        // Act
        range.To = 500;

        // Assert
        Assert.Equal(500, range.To);
        Assert.True(range.IsChanged);

        // Act
        range.Reset();

        // Assert
        Assert.Null(range.To);
        Assert.False(range.IsChanged);
    }

    [Fact]
    public void IsChanged_UsesEpsilonForRateComparison()
    {
        // Arrange
        var range = new AgencyCommissionRateRange(Guid.NewGuid(), 7, 1, 2, 0.100000, isNew: false);
        Assert.False(range.IsChanged);

        // Change within epsilon
        range.Rate = 0.100000004;

        // Assert
        Assert.False(range.IsChanged);

        // Change beyond epsilon
        range.Rate = 0.1001;

        // Assert
        Assert.True(range.IsChanged);
    }

    [Fact]
    public void Formatting_FromTxt_ToTxt_RateTxt()
    {
        // Arrange
        var range = new AgencyCommissionRateRange(Guid.NewGuid(), 8, 1234, null, 0.2567, isNew: false);

        // Act & Assert
        Assert.Equal(1234.ToString("C0"), range.FromText);
        Assert.Equal("Unlimited", range.ToText); // null -> "0"
        Assert.Equal(0.2567.ToString("P2"), range.RateText);

        // Also verify ToTxt when not null
        range.To = 9876;
        Assert.Equal(9876.ToString("C0"), range.ToText);
    }

    [Fact]
    public void RecordId_And_Id_AreSimpleSettable()
    {
        // Arrange
        var range = new AgencyCommissionRateRange();
        var id = Guid.NewGuid();

        // Act
        range.RecordId = id;
        range.Id = 42;

        // Assert
        Assert.Equal(id, range.RecordId);
        Assert.Equal(42, range.Id);
    }
}