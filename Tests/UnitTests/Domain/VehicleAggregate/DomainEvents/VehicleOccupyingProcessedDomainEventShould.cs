using Domain.VehicleAggregate.DomainEvents;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.VehicleAggregate.DomainEvents;

[TestSubject(typeof(VehicleOccupyingProcessedDomainEvent))]
public class VehicleOccupyingProcessedDomainEventShould
{
    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();

        // Act
        var actual = new VehicleOccupyingProcessedDomainEvent(vehicleId, bookingId, true);

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
            new VehicleOccupyingProcessedDomainEvent(Guid.Empty, Guid.NewGuid(), true);
        }

        // Assert
        Assert.Throws<ArgumentException>(Act);
    }

    [Fact]
    public void ThrowArgumentExceptionIfBookingIdIsEmpty()
    {
        // Arrange

        // Act
        void Act()
        {
            new VehicleOccupyingProcessedDomainEvent(Guid.NewGuid(), Guid.Empty, true);
        }

        // Assert
        Assert.Throws<ArgumentException>(Act);
    }
}