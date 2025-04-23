using Domain.VehicleAddingSaga;
using From.BookingKafkaEvents;
using Infrastructure.Adapters.Postgres.Saga.ConsumingSagaEvents;
using MassTransit;

namespace Api.Adapters.Kafka;

public class VehicleAddingToBookingProcessedConsumer(
    ISagaConsumeProcessor sagaConsumeProcessor) : IConsumer<VehicleAddingToBookingProcessed>
{
    public async Task Consume(ConsumeContext<VehicleAddingToBookingProcessed> context)
    {
        var @event = context.Message;
        var sagaEvent = new VehicleAddingSagaEvent(
            @event.SagaId,
            @event.VehicleId,
            @event.IsSuccess,
            VehicleAddingSagaMicroservice.Booking);
        
        var processResult = await sagaConsumeProcessor.SagaVehicleAddingProcess(sagaEvent);
        if (processResult == false) throw new ConsumerCanceledException("Could not save event in inbox");

        await context.ConsumeCompleted;
    }
}