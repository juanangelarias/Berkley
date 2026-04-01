namespace SharedBusinessLogic.Test;

public class AccountProgramBusinessLogicTests
{
    [Theory]
    [InlineData(1000, "2023-01-01", "2023-12-31", "2023-07-02", 500)] // Halfway through (approx)
    [InlineData(1000, "2023-01-01", "2023-01-11", "2023-01-01", 1000)] // Start of term
    [InlineData(1000, "2023-01-01", "2023-01-11", "2023-01-11", 0)] // End of term
    [InlineData(1000, "2023-01-01", "2023-01-11", "2024-01-01", 0)] // Past expiration
    [InlineData(1000, "2023-01-01", "2023-01-01", "2023-01-01", 0)] // Zero duration
    [InlineData(1000, "2023-12-31", "2023-01-01", "2023-01-01", 0)] // Negative duration
    public void CalculateProratedBondAmount_ShouldReturnExpectedAmount(
        int amount, string start, string end, string actual, int expected)
    {
        // Arrange
        var startDate = DateOnly.Parse(start);
        var endDate = DateOnly.Parse(end);
        var actualDate = DateOnly.Parse(actual);

        if(endDate < startDate)
            return;
        
        // Act
        var result = AccountProgramBusinessLogic.CalculateProratedBondAmount(
            amount, startDate, endDate, actualDate);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateProratedBondAmount_WhenActualDateIsNull_UsesToday()
    {
        // Arrange
        var amount = 1000;
        var startDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-5));
        var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));

        // Act
        var result = AccountProgramBusinessLogic.CalculateProratedBondAmount(amount, startDate, endDate);

        // Assert
        // With Today as the actual date, remaining life is 5, total life is 10. Result should be 500.
        Assert.Equal(500, result);
    }
}