using From.BookingKafkaEvents;
using Infrastructure.Adapters.Postgres.Inbox;
using Infrastructure.Adapters.Postgres.Inbox.InputConsumerEvents;
using MassTransit;

namespace Api.Adapters.Kafka;

public class BookingCreatedConsumer(IInbox inbox) : IConsumer<BookingCreated>
{
    public async Task Consume(ConsumeContext<BookingCreated> context)
    {
        var @event = context.Message;
        var consumerEvent = new BookingCreatedInputConsumerEvent(
            @event.EventId,
            @event.BookingId,
            @event.VehicleId);

        var isSaved = await inbox.Save(consumerEvent);
        if (isSaved == false) throw new ConsumerCanceledException("Could not save event in inbox");

        await context.ConsumeCompleted;
    }
}