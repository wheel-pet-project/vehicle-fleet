using Domain.ModelAggregate;
using Domain.SharedKernel.Exceptions.ArgumentException;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.ModelAggregate;

[TestSubject(typeof(Tariff))]
public class TariffShould
{
    [Fact]
    public void ReturnNewInstanceWithCorrectValues()
    {
        // Arrange
        var pricePerMinute = 10.0M;
        var pricePerHour = 500.0M;
        var pricePerDay = 5000.0M;

        // Act
        var actual = Tariff.Create(pricePerMinute, pricePerHour, pricePerDay);

        // Assert
        Assert.NotNull(actual);
        Assert.NotEqual(Guid.Empty, actual.Id);
        Assert.Equal(pricePerMinute, actual.PricePerMinute);
        Assert.Equal(pricePerHour, actual.PricePerHour);
        Assert.Equal(pricePerDay, actual.PricePerDay);
    }

    [Theory]
    [InlineData(0.0, 2.2, 2.2)]
    [InlineData(2.2, 0.0, 2.2)]
    [InlineData(2.2, 2.2, 0.0)]
    public void ThrowValueOutOfRangeExceptionIfPriceLessThanZero(
        decimal pricePerMinute,
        decimal pricePerHour,
        decimal pricePerDay)
    {
        // Arrange

        // Act
        void Act()
        {
            Tariff.Create(pricePerMinute, pricePerHour, pricePerDay);
        }

        // Assert
        Assert.Throws<ValueOutOfRangeException>(Act);
    }

    [Fact]
    public void UpdateTariffPrices()
    {
        // Arrange
        var expectedPricePerMinute = 20.0M;
        var expectedPricePerHour = 600.0M;
        var expectedPricePerDay = 6000.0M;
        
        var tariff = Tariff.Create(10.0M, 500.0M, 5000.0M);

        // Act
        tariff.Update(expectedPricePerMinute, expectedPricePerHour, expectedPricePerDay);

        // Assert
        Assert.Equal(expectedPricePerMinute, tariff.PricePerMinute);
        Assert.Equal(expectedPricePerHour, tariff.PricePerHour);
        Assert.Equal(expectedPricePerDay, tariff.PricePerDay);
    }

    [Theory]
    [InlineData(20, 0, 0)]
    [InlineData(0, 600, 0)]
    [InlineData(0, 0, 6000)]
    public void UpdateOnlyPassedPrices(decimal pricePerMinute, decimal pricePerHour, decimal pricePerDay)
    {
        // Arrange
        var tariff = Tariff.Create(10.0M, 500.0M, 5000.0M);

        // Act
        tariff.Update(pricePerMinute, pricePerHour, pricePerDay);

        // Assert
        switch (pricePerMinute, pricePerHour, pricePerDay)
        {
            case (> 0, 0, 0): Assert.Equal(pricePerMinute, tariff.PricePerMinute); break;
            case (0, > 0, 0): Assert.Equal(pricePerHour, tariff.PricePerHour); break;
            case (0, 0, > 0): Assert.Equal(pricePerDay, tariff.PricePerDay); break;
        }
    }

    [Theory]
    [InlineData(-1, 0, 0)]
    [InlineData(0, -1, 0)]
    [InlineData(0, 0, -1)]
    public void ThrowValueOutOfRangeExceptionIfSamePriceLessThanZeroWhenUpdating(
        decimal pricePerMinute,
        decimal pricePerHour,
        decimal pricePerDay)
    {
        // Arrange
        var tariff = Tariff.Create(10.0M, 500.0M, 5000.0M);

        // Act
        void Act() => tariff.Update(pricePerMinute, pricePerHour, pricePerDay);
        
        // Assert
        Assert.Throws<ValueOutOfRangeException>(Act);
    }
}