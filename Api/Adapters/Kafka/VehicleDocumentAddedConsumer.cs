using Domain.VehicleAddingSaga;
using From.VehicleDocumentsKafkaEvents;
using Infrastructure.Adapters.Postgres.Saga.ConsumingSagaEvents;
using MassTransit;

namespace Api.Adapters.Kafka;

public class VehicleDocumentAddedConsumer(
    ISagaConsumeProcessor sagaConsumeProcessor) : IConsumer<DocumentAddingCompleted>
{
    public async Task Consume(ConsumeContext<DocumentAddingCompleted> context)
    {
        var @event = context.Message;
        var sagaEvent = new VehicleAddingSagaEvent(
            @event.SagaId,
            @event.VehicleId, 
            true,
            VehicleAddingSagaMicroservice.VehicleDocuments);
        
        var processResult = await sagaConsumeProcessor.SagaVehicleAddingProcess(sagaEvent);
        if (processResult == false) throw new ConsumerCanceledException("Could not save event in inbox");

        await context.ConsumeCompleted;
    }
}