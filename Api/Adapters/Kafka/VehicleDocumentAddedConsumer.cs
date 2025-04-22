using From.VehicleDocumentsKafkaEvents;
using Infrastructure.Adapters.Postgres.Saga.ConsumingSagaEvents;
using Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding;
using MassTransit;

namespace Api.Adapters.Kafka;

public class VehicleDocumentAddedConsumer(
    ISagaConsumeProcessor sagaConsumeProcessor) : IConsumer<DocumentAddingCompleted>
{
    public async Task Consume(ConsumeContext<DocumentAddingCompleted> context)
    {
        // todo: заменить Guid.NewGuid() на SagaId
        var @event = context.Message;
        var sagaEvent = new SagaVehicleAddingConsumerEvent(
            @event.EventId,
            Guid.NewGuid(),
            true,
            SagaVehicleAddingService.VehicleDocuments);
        
        var processResult = await sagaConsumeProcessor.SagaVehicleAddingProcess(sagaEvent);
        if (processResult == false) throw new ConsumerCanceledException("Could not save event in inbox");

        await context.ConsumeCompleted;
    }
}