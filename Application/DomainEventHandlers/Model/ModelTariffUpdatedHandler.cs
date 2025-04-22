using Application.Ports.Kafka;
using Domain.ModelAggregate.DomainEvents;
using MediatR;

namespace Application.DomainEventHandlers.Model;

public class ModelTariffUpdatedHandler(IMessageBus messageBus)
    : INotificationHandler<ModelTariffUpdatedDomainEvent>
{
    public async Task Handle(
        ModelTariffUpdatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await messageBus.Publish(domainEvent, cancellationToken);
    }
}