using Domain.SharedKernel.Saga;

namespace Domain.VehicleAddingSaga;

public class VehicleAddingSaga : Saga
{
    internal VehicleAddingSaga(Guid vehicleId) : base(1, new VehicleAddingState())
    {
        VehicleId = vehicleId;
    }

    public Guid VehicleId { get; }

    public void ProcessSagaEvent(VehicleAddingSagaEvent saga)
    {
        if (saga.SagaId != SagaId || saga.VehicleId != VehicleId)
            throw new ArgumentException(
                $"Saga id and/or vehicle id are not matched with ids from saga event, current saga id: {SagaId}, current vehicle id: {VehicleId}");

        State.UpdateSagaState(saga);
    }
}