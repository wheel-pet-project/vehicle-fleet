using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.VehicleAggregate.DomainEvents;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.VehicleAggregate.DomainEvents;

[TestSubject(typeof(VehicleServicedDomainEvent))]
public class VehicleServicedDomainEventShould
{
    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        
        // Act
        var actual = new VehicleServicedDomainEvent(vehicleId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(vehicleId, actual.VehicleId);
    }
    
    [Fact]
    public void ThrowValueIsRequiredExceptionIfVehicleIdIsEmpty()
    {
        // Arrange
        
        // Act
        void Act()
        {
            new VehicleServicedDomainEvent(Guid.Empty);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }
}