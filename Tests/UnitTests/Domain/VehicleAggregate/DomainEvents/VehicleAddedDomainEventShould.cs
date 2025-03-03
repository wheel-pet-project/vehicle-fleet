using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.VehicleAggregate.DomainEvents;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.VehicleAggregate.DomainEvents;

[TestSubject(typeof(VehicleAddedDomainEvent))]
public class VehicleAddedDomainEventShould
{
private readonly Guid _vehicleId = Guid.NewGuid();
private readonly Guid _modelId = Guid.NewGuid();
    
    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange
        

        // Act
        var actual = new VehicleAddedDomainEvent(_vehicleId, _modelId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(_vehicleId, actual.VehicleId);
        Assert.Equal(_modelId, actual.ModelId);
    }
    
    [Fact]
    public void ThrowValueIsRequiredExceptionIfVehicleIdIsEmpty()
    {
        // Arrange
        
        // Act
        void Act()
        {
            new VehicleAddedDomainEvent(Guid.Empty,  _modelId);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }
    
    [Fact]
    public void ThrowValueIsRequiredExceptionIfModelIdIsEmpty()
    {
        // Arrange
        
        // Act
        void Act()
        {
            new VehicleAddedDomainEvent(_vehicleId, Guid.Empty);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }
}