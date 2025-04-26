using Domain.VehicleAddingSaga;
using From.RentKafkaEvents;
using Infrastructure.Adapters.Postgres.Inbox;
using Infrastructure.Adapters.Postgres.Inbox.InputConsumerEvents;
using MassTransit;

namespace Api.Adapters.Kafka;

public class VehicleAddingToRentProcessedConsumer(IInbox inbox) : IConsumer<VehicleAddingToRentProcessed>
{
    public async Task Consume(ConsumeContext<VehicleAddingToRentProcessed> context)
    {
        var @event = context.Message;
        var sagaConsumerEvent = new VehicleAddingSagaConsumerEvent(
            @event.EventId,
            @event.SagaId,
            @event.VehicleId,
            @event.IsSuccess,
            VehicleAddingSagaMicroservice.Rent);

        var isSaved = await inbox.Save(sagaConsumerEvent);
        if (isSaved == false) throw new ConsumerCanceledException("Could not save event in inbox");

        await context.ConsumeCompleted;
    }
}