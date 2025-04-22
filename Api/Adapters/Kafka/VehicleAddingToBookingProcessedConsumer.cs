using From.BookingKafkaEvents;
using Infrastructure.Adapters.Postgres.Saga.ConsumingSagaEvents;
using Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding;
using MassTransit;

namespace Api.Adapters.Kafka;

public class VehicleAddingToBookingProcessedConsumer(
    ISagaConsumeProcessor sagaConsumeProcessor) : IConsumer<VehicleAddingToBookingProcessed>
{
    public async Task Consume(ConsumeContext<VehicleAddingToBookingProcessed> context)
    {
        // todo: заменить Guid.NewGuid() на SagaId
        var @event = context.Message;
        var sagaEvent = new SagaVehicleAddingConsumerEvent(
            @event.EventId,
            Guid.NewGuid(),
            @event.IsSuccess,
            SagaVehicleAddingService.Booking);
        
        var processResult = await sagaConsumeProcessor.SagaVehicleAddingProcess(sagaEvent);
        if (processResult == false) throw new ConsumerCanceledException("Could not save event in inbox");

        await context.ConsumeCompleted;
    }
}