using Domain.SharedKernel.Exceptions.PublicExceptions;
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
        var pricePerDay = 5000.0M;

        // Act
        var actual = Tariff.Create(pricePerMinute, pricePerHour, pricePerDay);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(pricePerMinute, actual.PricePerMinute);
        Assert.Equal(pricePerHour, actual.PricePerHour);
        Assert.Equal(pricePerDay, actual.PricePerDay);
    }

    [Theory]
    [InlineData(0.0, 2.2, 2.2)]
    [InlineData(2.2, 0.0, 2.2)]
    [InlineData(2.2, 2.2, 0.0)]
    public void ThrowValueOutOfRangeExceptionIfOneOfPriceLessThanZero(
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
        Assert.Throws<ValueIsUnsupportedException>(Act);
    }

    [Fact]
    public void EqualOperatorReturnTrueForEqualTariffs()
    {
        // Arrange
        var tariff1 = Tariff.Create(1M, 2M, 3M);
        var tariff2 = Tariff.Create(1M, 2M, 3M);

        // Act
        var actual = tariff1 == tariff2;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void NotEqualOperatorReturnTrurForDifferentTariffs()
    {
        // Arrange
        var tariff1 = Tariff.Create(1M, 2M, 3M);
        var tariff2 = Tariff.Create(5M, 10M, 30M);

        // Act
        var actual = tariff1 != tariff2;

        // Assert
        Assert.True(actual);
    }
}