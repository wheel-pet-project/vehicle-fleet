using Domain.SharedKernel.Saga;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAddingSaga;
using Domain.VehicleAggregate;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.VehicleAddingSaga;

[TestSubject(typeof(VehicleAddingStateShould))]
public class VehicleAddingStateShould
{
    private readonly global::Domain.VehicleAddingSaga.VehicleAddingSaga _saga;
    private readonly ISagaState<IProcessState> _state;

    public VehicleAddingStateShould()
    {
        var (saga, _) =
            Vehicle.Create(Guid.NewGuid(),
                PlateNumber.Create("К333ОТ77"), Color.White,
                Vin.Create("SALYA2BN2KA791786"), Location.Create(10.0, 10.0));

        _saga = saga;
        _state = _saga.State;
    }
    
    [Fact]
    public void MarkAsFaultedIfOneOfProcessesFault()
    {
        // Arrange
        var sagaEvent =
            new VehicleAddingSagaEvent(_saga.SagaId, _saga.VehicleId, false, VehicleAddingSagaMicroservice.Booking);

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
            new VehicleAddingSagaEvent(_saga.SagaId, _saga.VehicleId, true, VehicleAddingSagaMicroservice.Booking);

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
            new VehicleAddingSagaEvent(_saga.SagaId, _saga.VehicleId, true, VehicleAddingSagaMicroservice.Booking);

        // Act
        _state.UpdateSagaState(sagaEvent);

        // Assert
        Assert.False(_state.IsFaulted);
    }

    [Fact]
    public void MarkAsCompleteIfEachProcessesCompleted()
    {
        // Arrange
        var events = VehicleAddingSagaMicroservice.All().Select(x => new VehicleAddingSagaEvent(_saga.SagaId, _saga.VehicleId, true, x)).ToList();

        // Act
        events.ForEach(x => _state.UpdateSagaState(x));

        // Assert
        Assert.True(_state.IsCompleted);
    }
}