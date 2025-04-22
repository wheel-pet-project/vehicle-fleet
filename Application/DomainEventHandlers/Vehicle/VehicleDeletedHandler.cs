using Application.Ports.Kafka;
using Domain.VehicleAggregate.DomainEvents;
using MediatR;

namespace Application.DomainEventHandlers.Vehicle;

public class VehicleDeletedHandler(IMessageBus messageBus)
    : INotificationHandler<VehicleDeletedDomainEvent>
{
    public async Task Handle(
        VehicleDeletedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await messageBus.Publish(domainEvent, cancellationToken);
    }
}