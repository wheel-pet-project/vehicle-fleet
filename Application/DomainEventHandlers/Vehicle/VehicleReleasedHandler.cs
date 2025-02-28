using Application.Ports.Kafka;
using Domain.VehicleAggregate.DomainEvents;
using MediatR;

namespace Application.DomainEventHandlers.Vehicle;

public class VehicleReleasedHandler(IMessageBus messageBus) : INotificationHandler<VehicleReleasedDomainEvent>
{
    public async Task Handle(VehicleReleasedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await messageBus.Publish(domainEvent, cancellationToken);
    }
}