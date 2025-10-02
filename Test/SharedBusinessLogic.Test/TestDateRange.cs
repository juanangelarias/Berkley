using James.Shared.Model;

namespace SharedBusinessLogic.Test;

public class TestDateRange
{
    [Fact]
    public void Label_WhenStartAndEndNull_ReturnsEmptyString()
    {
        var range = new DateRange { Start = null, End = null };

        Assert.Equal(string.Empty, range.Label);
    }

    [Fact]
    public void Label_WhenOnlyStartSet_ReturnsSingleDateFormatted()
    {
        var date = new DateTime(2024, 3, 5);
        var range = new DateRange { Start = date, End = null };

        Assert.Equal("03/05/2024", range.Label);
    }

    [Fact]
    public void Label_WhenStartAndEndSet_ReturnsFormattedRange()
    {
        var start = new DateTime(2024, 3, 5);
        var end = new DateTime(2024, 4, 7);
        var range = new DateRange { Start = start, End = end };

        Assert.Equal("03/05/2024 - 04/07/2024", range.Label);
    }

    [Fact]
    public void Label_WhenEndSetButStartNull_ReturnsNullStartAsEmptyBeforeDash()
    {
        var end = new DateTime(2024, 4, 7);
        var range = new DateRange { Start = null, End = end };

        // Start?.ToString(...) yields null => $"{null} - 04/07/2024" => " - 04/07/2024"
        Assert.Equal(" - 04/07/2024", range.Label);
    }
}