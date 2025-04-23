using Domain.VehicleAddingSaga;

namespace Infrastructure.Adapters.Postgres.Saga.ConsumingSagaEvents;

public interface ISagaConsumeProcessor
{
    Task<bool> SagaVehicleAddingProcess(VehicleAddingSagaEvent @event);
}