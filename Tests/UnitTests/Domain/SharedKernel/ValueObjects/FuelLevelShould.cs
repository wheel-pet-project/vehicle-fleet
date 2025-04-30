using Domain.SharedKernel.Exceptions.PublicExceptions;
using Domain.SharedKernel.ValueObjects;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.SharedKernel.ValueObjects;

[TestSubject(typeof(FuelLevel))]
public class FuelLevelShould
{
    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange
        var percents = 50;

        // Act
        var actual = FuelLevel.Create(percents);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(percents, actual.Percents);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void ThrowValueOutOfRangeExceptionIfLevelPercentageIsLessThan0OrGreater100(
        int invalidPercent)
    {
        // Arrange

        // Act
        void Act()
        {
            FuelLevel.Create(invalidPercent);
        }

        // Assert
        Assert.Throws<ValueIsUnsupportedException>(Act);
    }

    [Fact]
    public void EqualOperatorReturnTrueForEqualLevels()
    {
        // Arrange
        var fuelLevel1 = FuelLevel.Create(50);
        var fuelLevel2 = FuelLevel.Create(50);

        // Act
        var actual = fuelLevel1 == fuelLevel2;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void NotEqualOperatorReturnTrueForDifferentLevels()
    {
        // Arrange
        var fuelLevel1 = FuelLevel.Create(5);
        var fuelLevel2 = FuelLevel.Create(100);

        // Act
        var actual = fuelLevel1 != fuelLevel2;

        // Assert
        Assert.True(actual);
    }
}