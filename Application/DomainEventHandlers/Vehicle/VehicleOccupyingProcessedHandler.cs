using Application.Ports.Kafka;
using Domain.VehicleAggregate.DomainEvents;
using MediatR;

namespace Application.DomainEventHandlers.Vehicle;

public class VehicleOccupyingProcessedHandler(
    IMessageBus messageBus) : INotificationHandler<VehicleOccupyingProcessedDomainEvent>
{
    public async Task Handle(
        VehicleOccupyingProcessedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await messageBus.Publish(domainEvent, cancellationToken);
    }
}