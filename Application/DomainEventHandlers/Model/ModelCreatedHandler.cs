using Application.Ports.Kafka;
using Domain.ModelAggregate.DomainEvents;
using MediatR;

namespace Application.DomainEventHandlers.Model;

public class ModelCreatedHandler(IMessageBus messageBus)
    : INotificationHandler<ModelCreatedDomainEvent>
{
    public async Task Handle(
        ModelCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await messageBus.Publish(domainEvent, cancellationToken);
    }
}