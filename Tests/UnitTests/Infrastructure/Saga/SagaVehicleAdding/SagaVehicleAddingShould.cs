using Application.Ports.Postgres.Saga;
using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAggregate;
using Infrastructure.Adapters.Postgres.Saga;
using Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Infrastructure.Saga.SagaVehicleAdding;

[TestSubject(typeof(global::Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding.SagaVehicleAdding))]
public class SagaVehicleAddingShould
{
    private ISaga _saga;

    public SagaVehicleAddingShould()
    {
        var sagaCreator = new SagaCreator();
        _saga = sagaCreator.CreateSagaVehicleAdding(
            Vehicle.Create(Guid.NewGuid(),
            PlateNumber.Create("К333ОТ77"), Color.White,
            Vin.Create("SALYA2BN2KA791786"), Location.Create(10.0, 10.0)));
    }
    
    [Fact]
    public void ProcessSagaEventThrowValueIsInvalidExceptionIfEventFromAnotherSagaAndIdMismatch()
    {
        // Arrange
        var sagaEvent =
            new SagaVehicleAddingConsumerEvent(Guid.NewGuid(), Guid.NewGuid(), true, SagaVehicleAddingService.Booking);

        // Act
        void Act() =>
            ((global::Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding.SagaVehicleAdding)_saga)
            .ProcessSagaEvent(sagaEvent);

        // Assert
        Assert.Throws<ValueIsInvalidException>(Act);
    }

    [Fact]
    public void ProcessSagaEventChangeSagaState()
    {
        // Arrange
        var vehicleAddingSaga = (global::Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding.SagaVehicleAdding)_saga;
        var sagaEvent =
            new SagaVehicleAddingConsumerEvent(vehicleAddingSaga.SagaId, vehicleAddingSaga.VehicleId, false, SagaVehicleAddingService.Booking);

        // Act
        vehicleAddingSaga.ProcessSagaEvent(sagaEvent);

        // Assert
        Assert.True(vehicleAddingSaga.SagaState.IsFaulted);
    }
}