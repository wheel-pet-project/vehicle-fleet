using Domain.VehicleAddingSaga;
using From.BookingKafkaEvents;
using Infrastructure.Adapters.Postgres.Inbox;
using Infrastructure.Adapters.Postgres.Inbox.InputConsumerEvents;
using MassTransit;

namespace Api.Adapters.Kafka;

public class VehicleAddingToBookingProcessedConsumer(IInbox inbox) : IConsumer<VehicleAddingToBookingProcessed>
{
    public async Task Consume(ConsumeContext<VehicleAddingToBookingProcessed> context)
    {
        var @event = context.Message;
        var sagaConsumerEvent = new VehicleAddingSagaConsumerEvent(
            @event.EventId,
            @event.SagaId,
            @event.VehicleId,
            @event.IsSuccess,
            VehicleAddingSagaMicroservice.Booking);
        
        var isSaved = await inbox.Save(sagaConsumerEvent);
        if (isSaved == false) throw new ConsumerCanceledException("Could not save event in inbox");

        await context.ConsumeCompleted;
    }
}