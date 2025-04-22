using Application.Ports.Kafka;
using Domain.VehicleAggregate.DomainEvents;
using MediatR;

namespace Application.DomainEventHandlers.Vehicle;

public class VehicleReadiedForReleaseHandler(IMessageBus messageBus)
    : INotificationHandler<VehicleReadiedForReleaseDomainEvent>
{
    public async Task Handle(
        VehicleReadiedForReleaseDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await messageBus.Publish(domainEvent, cancellationToken);
    }
}