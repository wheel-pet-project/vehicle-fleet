using Domain.SharedKernel.Saga;

namespace Domain.VehicleAggregate.VehicleSagas.VehicleAddingSaga;

public sealed class VehicleAddingSaga : Saga
{
    private VehicleAddingSagaState _latterState = VehicleAddingSagaState.ReadiedForRelease;

    private VehicleAddingSaga()
    {
    }

    private VehicleAddingSaga(Guid vehicleId) : this()
    {
        VehicleId = vehicleId;
        State = VehicleAddingSagaState.Added;
    }

    public Guid VehicleId { get; }
    public VehicleAddingSagaState State { get; private set; }

    public void ChangeState(VehicleAddingSagaState potentialState)
    {
        State = potentialState;
    }
}