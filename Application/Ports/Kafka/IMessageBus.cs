using Domain.ModelAggregate.DomainEvents;
using Domain.VehicleAggregate.DomainEvents;

namespace Application.Ports.Kafka;

public interface IMessageBus
{
    Task Publish(ModelCreatedDomainEvent domainEvent, CancellationToken cancellationToken);

    Task Publish(ModelCategoryUpdatedDomainEvent domainEvent, CancellationToken cancellationToken);

    Task Publish(ModelTariffUpdatedDomainEvent domainEvent, CancellationToken cancellationToken);

    Task Publish(VehicleAddedDomainEvent domainEvent, CancellationToken cancellationToken);

    Task Publish(VehicleDeletedDomainEvent domainEvent, CancellationToken cancellationToken);

    Task Publish(VehicleReadiedForReleaseDomainEvent domainEvent, CancellationToken cancellationToken);

    Task Publish(VehicleOccupiedDomainEvent domainEvent, CancellationToken cancellationToken);

    Task Publish(VehicleReleasedDomainEvent domainEvent, CancellationToken cancellationToken);

    Task Publish(VehicleServicedDomainEvent domainEvent, CancellationToken cancellationToken);
}