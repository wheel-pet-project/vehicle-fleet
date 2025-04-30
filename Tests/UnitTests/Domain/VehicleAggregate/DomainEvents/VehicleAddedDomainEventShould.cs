using Domain.VehicleAggregate.DomainEvents;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.VehicleAggregate.DomainEvents;

[TestSubject(typeof(VehicleAddedDomainEvent))]
public class VehicleAddedDomainEventShould
{
    private readonly Guid _vehicleId = Guid.NewGuid();
    private readonly Guid _modelId = Guid.NewGuid();
    private readonly Guid _sagaId = Guid.NewGuid();

    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange


        // Act
        var actual = new VehicleAddedDomainEvent(_vehicleId, _modelId, _sagaId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(_vehicleId, actual.VehicleId);
        Assert.Equal(_modelId, actual.ModelId);
    }

    [Fact]
    public void ThrowArgumentExceptionIfVehicleIdIsEmpty()
    {
        // Arrange

        // Act
        void Act()
        {
            new VehicleAddedDomainEvent(Guid.Empty, _modelId, _sagaId);
        }

        // Assert
        Assert.Throws<ArgumentException>(Act);
    }

    [Fact]
    public void ThrowArgumentExceptionIfModelIdIsEmpty()
    {
        // Arrange

        // Act
        void Act()
        {
            new VehicleAddedDomainEvent(_vehicleId, Guid.Empty, _sagaId);
        }

        // Assert
        Assert.Throws<ArgumentException>(Act);
    }

    [Fact]
    public void ThrowArgumentExceptionIfSagaIdIsEmpty()
    {
        // Arrange

        // Act
        void Act()
        {
            new VehicleAddedDomainEvent(_vehicleId, _sagaId, Guid.Empty);
        }

        // Assert
        Assert.Throws<ArgumentException>(Act);
    }
}