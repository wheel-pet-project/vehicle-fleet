using Application.Ports.Kafka;
using Domain.VehicleAggregate.DomainEvents;
using MediatR;

namespace Application.DomainEventHandlers.Vehicle;

public class VehicleServicedHandler(IMessageBus messageBus) : INotificationHandler<VehicleServicedDomainEvent>
{
    public async Task Handle(VehicleServicedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await messageBus.Publish(domainEvent, cancellationToken);
    }
}