using Domain.SharedKernel.ValueObjects;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.SharedKernel.ValueObjects;

[TestSubject(typeof(Tariff))]
public class TariffShould
{
    [Fact]
    public void ReturnNewInstanceWithCorrectValues()
    {
        // Arrange
        var pricePerMinute = 10.0M;
        var pricePerHour = 500.0M;
        var pricePerDay = 5000.5M;

        // Act
        var actual = Tariff.Create(pricePerMinute, pricePerHour, pricePerDay);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(pricePerMinute, actual.PricePerMinute);
        Assert.Equal(pricePerHour, actual.PricePerHour);
        Assert.Equal(pricePerDay, actual.PricePerDay);
    }

    [Fact]
    public void EqualOperatorReturnTrueForEqualTariffs()
    {
        // Arrange
        var pricePerMinute = 10.0M;
        var pricePerHour = 500.0M;
        var pricePerDay = 5000.5M;
        var tariff1 = Tariff.Create(pricePerMinute, pricePerHour, pricePerDay);
        var tariff2 = Tariff.Create(pricePerMinute, pricePerHour, pricePerDay);

        // Act
        var actual = tariff1 == tariff2;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void NotEqualOperatorReturnTrueForNotEqualTariffs()
    {
        var tariff1 = Tariff.Create(10.0M, 200.0M, 4000.0M);
        var tariff2 = Tariff.Create(10.0M, 200.0M, 10000.0M);

        // Act
        var actual = tariff1 != tariff2;

        // Assert
        Assert.True(actual);
    }
}