using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAddingSaga;
using Domain.VehicleAggregate;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.VehicleAddingSaga;

[TestSubject(typeof(global::Domain.VehicleAddingSaga.VehicleAddingSaga))]
public class VehicleAddingSagaShould
{
    private readonly global::Domain.VehicleAddingSaga.VehicleAddingSaga _saga;
    
    public VehicleAddingSagaShould()
    {
        var (saga, _) = Vehicle.Create(Guid.NewGuid(),
            PlateNumber.Create("К333ОТ77"), Color.White,
            Vin.Create("SALYA2BN2KA791786"), Location.Create(10.0, 10.0));

        _saga = saga;
    }
    
    [Fact]
    public void ProcessSagaEventThrowValueIsInvalidExceptionIfEventFromAnotherSagaAndIdMismatch()
    {
        // Arrange
        var sagaEvent =
            new VehicleAddingSagaEvent(Guid.NewGuid(), Guid.NewGuid(), true, VehicleAddingSagaMicroservice.Booking);

        // Act
        void Act() => _saga.ProcessSagaEvent(sagaEvent);

        // Assert
        Assert.Throws<ValueIsInvalidException>(Act);
    }

    [Fact]
    public void ProcessSagaEventChangeSagaState()
    {
        // Arrange
        var vehicleAddingSaga = _saga;
        var sagaEvent =
            new VehicleAddingSagaEvent(vehicleAddingSaga.SagaId, vehicleAddingSaga.VehicleId, false, VehicleAddingSagaMicroservice.Booking);

        // Act
        vehicleAddingSaga.ProcessSagaEvent(sagaEvent);

        // Assert
        Assert.True(vehicleAddingSaga.SagaState.IsFaulted);
    }
}