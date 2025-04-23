using Domain.VehicleAddingSaga;
using From.RentKafkaEvents;
using Infrastructure.Adapters.Postgres.Saga.ConsumingSagaEvents;
using MassTransit;

namespace Api.Adapters.Kafka;

public class VehicleAddingToRentProcessedConsumer(
    ISagaConsumeProcessor sagaConsumeProcessor) : IConsumer<VehicleAddingToRentProcessed>
{
    public async Task Consume(ConsumeContext<VehicleAddingToRentProcessed> context)
    {
        // todo: заменить Guid.NewGuid() на SagaId
        var @event = context.Message;
        var sagaEvent = new VehicleAddingSagaEvent(
            @event.SagaId,
            @event.VehicleId,
            @event.IsSuccess,
            VehicleAddingSagaMicroservice.Rent);
        
        var processResult = await sagaConsumeProcessor.SagaVehicleAddingProcess(sagaEvent);
        if (processResult == false) throw new ConsumerCanceledException("Could not save event in inbox");

        await context.ConsumeCompleted;
    }
}