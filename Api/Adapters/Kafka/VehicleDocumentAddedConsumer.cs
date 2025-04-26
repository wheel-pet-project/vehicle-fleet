using Domain.VehicleAddingSaga;
using From.VehicleDocumentsKafkaEvents;
using Infrastructure.Adapters.Postgres.Inbox;
using Infrastructure.Adapters.Postgres.Inbox.InputConsumerEvents;
using MassTransit;

namespace Api.Adapters.Kafka;

public class VehicleDocumentAddedConsumer(IInbox inbox) : IConsumer<DocumentAddingCompleted>
{
    public async Task Consume(ConsumeContext<DocumentAddingCompleted> context)
    {
        var @event = context.Message;
        var sagaConsumerEvent = new VehicleAddingSagaConsumerEvent(
            @event.EventId,
            @event.SagaId,
            @event.VehicleId, 
            true,
            VehicleAddingSagaMicroservice.VehicleDocuments);
        
        var isSaved = await inbox.Save(sagaConsumerEvent);
        if (isSaved == false) throw new ConsumerCanceledException("Could not save event in inbox");

        await context.ConsumeCompleted;
    }
}