using Application.Ports.Kafka;
using Domain.VehicleAggregate.DomainEvents;
using MediatR;

namespace Application.DomainEventHandlers.Vehicle;

public class VehicleOccupiedHandler(IMessageBus messageBus) : INotificationHandler<VehicleOccupiedDomainEvent>
{
    public async Task Handle(VehicleOccupiedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await messageBus.Publish(domainEvent, cancellationToken);
    }
}