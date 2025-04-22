using Application.Ports.Kafka;
using Domain.ModelAggregate.DomainEvents;
using MediatR;

namespace Application.DomainEventHandlers.Model;

public class ModelCategoryUpdatedHandler(IMessageBus messageBus)
    : INotificationHandler<ModelCategoryUpdatedDomainEvent>
{
    public async Task Handle(
        ModelCategoryUpdatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await messageBus.Publish(domainEvent, cancellationToken);
    }
}