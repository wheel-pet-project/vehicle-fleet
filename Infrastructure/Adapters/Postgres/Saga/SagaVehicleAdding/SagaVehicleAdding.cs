using Domain.SharedKernel.Exceptions.ArgumentException;
using Infrastructure.Adapters.Postgres.Saga.SagaSharedKernel;
using ISaga = Application.Ports.Postgres.Saga.ISaga;

namespace Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding;

public class SagaVehicleAdding : SagaSharedKernel.Saga, ISaga
{
    internal SagaVehicleAdding(Guid vehicleId) : base(1, new SagaVehicleAddingState())
    {
        VehicleId = vehicleId;
    }
    
    public Guid VehicleId { get; }

    public void ProcessSagaEvent(SagaVehicleAddingConsumerEvent sagaVehicleAddingEvent)
    {
        if (sagaVehicleAddingEvent.SagaId != SagaId || sagaVehicleAddingEvent.VehicleId != VehicleId)
            throw new ValueIsInvalidException(
                "Saga id and/or vehicle id are not matched with ids from saga event");

        SagaState.UpdateSagaState(sagaVehicleAddingEvent);
    }
}