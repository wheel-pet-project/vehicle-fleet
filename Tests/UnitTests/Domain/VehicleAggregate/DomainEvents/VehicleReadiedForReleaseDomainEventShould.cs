using Domain.VehicleAggregate.DomainEvents;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.VehicleAggregate.DomainEvents;

[TestSubject(typeof(VehicleReadiedForReleaseDomainEvent))]
public class VehicleReadiedForReleaseDomainEventShould
{
    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();

        // Act
        var actual = new VehicleReadiedForReleaseDomainEvent(vehicleId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(vehicleId, actual.VehicleId);
    }

    [Fact]
    public void ThrowArgumentExceptionIfVehicleIdIsEmpty()
    {
        // Arrange

        // Act
        void Act()
        {
            new VehicleReadiedForReleaseDomainEvent(Guid.Empty);
        }

        // Assert
        Assert.Throws<ArgumentException>(Act);
    }
}