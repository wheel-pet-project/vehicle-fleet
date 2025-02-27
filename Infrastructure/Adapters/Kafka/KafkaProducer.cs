using Application.Ports.Kafka;
using Domain.ModelAggregate.DomainEvents;
using Domain.VehicleAggregate.DomainEvents;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Adapters.Kafka;

public class KafkaProducer(IKafkaRider kafkaRider) : IMessageBus
{
    public Task Publish(ModelCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Publish(ModelCategoryUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Publish(ModelTariffUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Publish(VehicleAddedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Publish(VehicleDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Publish(VehicleReadiedForReleaseDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Publish(VehicleOccupiedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Publish(VehicleReleasedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Publish(VehicleServicedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}