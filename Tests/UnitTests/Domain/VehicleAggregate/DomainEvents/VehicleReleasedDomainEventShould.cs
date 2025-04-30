using Domain.VehicleAggregate.DomainEvents;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.VehicleAggregate.DomainEvents;

[TestSubject(typeof(VehicleReleasedDomainEvent))]
public class VehicleReleasedDomainEventShould
{
    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();

        // Act
        var actual = new VehicleReleasedDomainEvent(vehicleId);

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
            new VehicleReleasedDomainEvent(Guid.Empty);
        }

        // Assert
        Assert.Throws<ArgumentException>(Act);
    }
}