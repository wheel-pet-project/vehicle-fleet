using Domain.SharedKernel.Exceptions.PublicExceptions;
using Domain.SharedKernel.ValueObjects;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.SharedKernel.ValueObjects;

[TestSubject(typeof(Location))]
public class LocationShould
{
    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange
        var latitude = 12.34561531;
        var longitude = 34.5678315;

        // Act
        var actual = Location.Create(latitude, longitude);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(latitude, actual.Latitude);
        Assert.Equal(longitude, actual.Longitude);
    }

    [Fact]
    public void ThrowValueOutOfRangeExceptionIfLatitudeLessThan0()
    {
        // Arrange

        // Act
        void Act()
        {
            Location.Create(-0.012423, 34.5678315);
        }

        // Assert
        Assert.Throws<ValueUnsupportedException>(Act);
    }

    [Fact]
    public void ThrowValueOutOfRangeExceptionIfLatitudeGreaterThan90()
    {
        // Arrange

        // Act
        void Act()
        {
            Location.Create(91.0124, 1.5678315);
        }

        // Assert
        Assert.Throws<ValueUnsupportedException>(Act);
    }

    [Fact]
    public void ThrowValueOutOfRangeExceptionIfLongitudeLessThan0()
    {
        // Arrange

        // Act
        void Act()
        {
            Location.Create(34.5678315, -0.012423);
        }

        // Assert
        Assert.Throws<ValueUnsupportedException>(Act);
    }

    [Fact]
    public void ThrowValueOutOfRangeExceptionIfLongitudeGreaterThan180()
    {
        // Arrange

        // Act
        void Act()
        {
            Location.Create(34.5678315, 180.924325);
        }

        // Assert
        Assert.Throws<ValueUnsupportedException>(Act);
    }

    [Fact]
    public void EqualOperatorReturnTrueIfLocationsIsEqual()
    {
        // Arrange
        var latitude = 12.34561531;
        var longitude = 34.5678315;
        var location1 = Location.Create(latitude, longitude);
        var location2 = Location.Create(latitude, longitude);

        // Act
        var actual = location1 == location2;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void NotEqualOperatorReturnTrueIfLocationsIsNotEqual()
    {
        // Arrange
        var location1 = Location.Create(12.34561531, 12.34561531);
        var location2 = Location.Create(34.5678315, 34.5678315);

        // Act
        var actual = location1 != location2;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void EqualOperatorRoundGeoTo1MeterPrecision()
    {
        // Arrange
        var location1 = Location.Create(1.00000999, 1.00000999);
        var location2 = Location.Create(1.00001444, 1.00001444);

        // Act
        var actual = location1 == location2;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void InSquareWithLocationsAsBordersReturnTrue()
    {
        // Arrange
        var location = Location.Create(1.00000, 1.00000);

        // Act
        var actual = location.InSquare(Location.Create(1.00001, 0.99999),
            Location.Create(0.99999, 1.00001));

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void InSquareWithLocationsAsBordersReturnFalse()
    {
        // Arrange
        var location = Location.Create(1.00000, 1.00000);

        // Act
        var actual = location.InSquare(Location.Create(1.00001, 1.1000), Location.Create(1.1, 1.2));

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void InSquareWithSizeInDegreeReturnTrue()
    {
        // Arrange
        var location = Location.Create(1.00000, 1.00000);

        // Act
        var actual = location.InSquare(0.00001, location);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void InSquareWithSizeInDegreeReturnFalse()
    {
        // Arrange
        var location = Location.Create(1.00000, 1.00000);

        // Act
        var actual = Location.Create(1.00002, 1.00002).InSquare(0.00001, location);

        // Assert
        Assert.False(actual);
    }
}