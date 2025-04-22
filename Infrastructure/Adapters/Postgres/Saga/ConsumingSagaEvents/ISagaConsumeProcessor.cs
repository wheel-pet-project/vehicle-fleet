using Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding;

namespace Infrastructure.Adapters.Postgres.Saga.ConsumingSagaEvents;

public interface ISagaConsumeProcessor
{
    Task<bool> SagaVehicleAddingProcess(SagaVehicleAddingConsumerEvent @event);
}