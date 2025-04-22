using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAggregate;
using Infrastructure.Adapters.Postgres.Saga;
using Infrastructure.Adapters.Postgres.Saga.SagaSharedKernel;
using Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Infrastructure.Saga.SagaVehicleAdding;

[TestSubject(typeof(SagaVehicleAddingStateShould))]
public class SagaVehicleAddingStateShould
{
    private readonly global::Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding.SagaVehicleAdding _saga;
    private readonly ISagaState<IProcessState> _state;

    public SagaVehicleAddingStateShould()
    {
        var sagaCreator = new SagaCreator();
        _saga = (global::Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding.SagaVehicleAdding)sagaCreator
            .CreateSagaVehicleAdding(
                Vehicle.Create(Guid.NewGuid(),
                    PlateNumber.Create("К333ОТ77"), Color.White,
                    Vin.Create("SALYA2BN2KA791786"), Location.Create(10.0, 10.0)));

        _state = _saga.SagaState;
    }
    
    [Fact]
    public void MarkAsFaultedIfOneOfProcessesFault()
    {
        // Arrange
        var sagaEvent =
            new SagaVehicleAddingConsumerEvent(_saga.SagaId, _saga.VehicleId, false, SagaVehicleAddingService.Booking);

        // Act
        _state.UpdateSagaState(sagaEvent);

        // Assert
        Assert.True(_state.IsFaulted);
        Assert.False(_state.IsCompleted);
    }

    [Fact]
    public void NotMarkAsCompletedIfOneOfProcessesCompleted()
    {
        // Arrange
        var sagaEvent =
            new SagaVehicleAddingConsumerEvent(_saga.SagaId, _saga.VehicleId, true, SagaVehicleAddingService.Booking);

        // Act
        _state.UpdateSagaState(sagaEvent);

        // Assert
        Assert.False(_state.IsCompleted);
    }

    [Fact]
    public void NotMarkAsFaultedIfOneOfProcessesCompleted()
    {
        // Arrange
        var sagaEvent =
            new SagaVehicleAddingConsumerEvent(_saga.SagaId, _saga.VehicleId, true, SagaVehicleAddingService.Booking);

        // Act
        _state.UpdateSagaState(sagaEvent);

        // Assert
        Assert.False(_state.IsFaulted);
    }

    [Fact]
    public void MarkAsCompleteIfEachProcessesCompleted()
    {
        // Arrange
        var events = SagaVehicleAddingService.All().Select(x => new SagaVehicleAddingConsumerEvent(_saga.SagaId, _saga.VehicleId, true, x)).ToList();

        // Act
        events.ForEach(x => _state.UpdateSagaState(x));

        // Assert
        Assert.True(_state.IsCompleted);
    }
}