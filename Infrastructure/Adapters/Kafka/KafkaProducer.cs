using Application.Ports.Kafka;
using Domain.ModelAggregate.DomainEvents;
using Domain.SharedKernel;
using Domain.VehicleAggregate.DomainEvents;
using From.VehicleFleetKafkaEvents.Model;
using From.VehicleFleetKafkaEvents.Vehicle;
using MassTransit;
using Microsoft.Extensions.Options;
using Uri = System.Uri;

namespace Infrastructure.Adapters.Kafka;

public class KafkaProducer(
    ITopicProducerProvider topicProducerProvider,
    IOptions<KafkaTopicsConfiguration> topicsConfiguration) : IMessageBus
{
    private readonly KafkaTopicsConfiguration _topicsConfiguration = topicsConfiguration.Value;

    public async Task Publish(
        ModelCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var producer = topicProducerProvider.GetProducer<string, ModelCreated>(
            new Uri($"topic:{_topicsConfiguration.ModelCreatedTopic}"));

        await producer.Produce(domainEvent.EventId.ToString(),
            new ModelCreated(domainEvent.EventId,
                domainEvent.ModelId,
                domainEvent.Category,
                (double)domainEvent.PricePerMinute,
                (double)domainEvent.PricePerHour,
                (double)domainEvent.PricePerDay),
            SetMessageId<ModelCreated, ModelCreatedDomainEvent>(domainEvent), cancellationToken);
    }

    public async Task Publish(
        ModelCategoryUpdatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var producer = topicProducerProvider.GetProducer<string, ModelCategoryUpdated>(
            new Uri($"topic:{_topicsConfiguration.ModelCategoryUpdatedTopic}"));

        await producer.Produce(domainEvent.EventId.ToString(),
            new ModelCategoryUpdated(domainEvent.EventId, domainEvent.ModelId,
                domainEvent.Category),
            SetMessageId<ModelCategoryUpdated, ModelCategoryUpdatedDomainEvent>(domainEvent),
            cancellationToken);
    }

    public async Task Publish(
        ModelTariffUpdatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var producer = topicProducerProvider.GetProducer<string, ModelTariffUpdated>(
            new Uri($"topic:{_topicsConfiguration.ModelTariffUpdatedTopic}"));

        await producer.Produce(domainEvent.EventId.ToString(),
            new ModelTariffUpdated(
                domainEvent.EventId,
                domainEvent.ModelId,
                (double)domainEvent.PricePerMinute,
                (double)domainEvent.PricePerHour,
                (double)domainEvent.PricePerDay),
            SetMessageId<ModelTariffUpdated, ModelTariffUpdatedDomainEvent>(domainEvent),
            cancellationToken);
    }

    public async Task Publish(
        VehicleAddedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var producer = topicProducerProvider.GetProducer<string, VehicleAdded>(
            new Uri($"topic:{_topicsConfiguration.VehicleAddedTopic}"));

        await producer.Produce(domainEvent.EventId.ToString(),
            new VehicleAdded(domainEvent.EventId, domainEvent.VehicleId, domainEvent.ModelId),
            SetMessageId<VehicleAdded, VehicleAddedDomainEvent>(domainEvent),
            cancellationToken);
    }

    public async Task Publish(
        VehicleDeletedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var producer = topicProducerProvider.GetProducer<string, VehicleDeleted>(
            new Uri($"topic:{_topicsConfiguration.VehicleDeletedTopic}"));

        await producer.Produce(domainEvent.EventId.ToString(),
            new VehicleDeleted(domainEvent.EventId, domainEvent.VehicleId),
            SetMessageId<VehicleDeleted, VehicleDeletedDomainEvent>(domainEvent),
            cancellationToken);
    }

    public async Task Publish(
        VehicleReadiedForReleaseDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var producer = topicProducerProvider.GetProducer<string, VehicleReadiedForRelease>(
            new Uri($"topic:{_topicsConfiguration.VehicleReadiedForReleaseTopic}"));

        await producer.Produce(domainEvent.EventId.ToString(),
            new VehicleReadiedForRelease(domainEvent.EventId, domainEvent.VehicleId),
            SetMessageId<VehicleReadiedForRelease, VehicleReadiedForReleaseDomainEvent>(
                domainEvent),
            cancellationToken);
    }

    public async Task Publish(
        VehicleOccupyingProcessedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var producer = topicProducerProvider.GetProducer<string, VehicleOccupyingProcessed>(
            new Uri($"topic:{_topicsConfiguration.VehicleOccupyingProcessedTopic}"));

        await producer.Produce(domainEvent.EventId.ToString(),
            new VehicleOccupyingProcessed(
                domainEvent.EventId,
                domainEvent.VehicleId,
                domainEvent.BookingId,
                domainEvent.IsOccupied,
                domainEvent.Reason),
            SetMessageId<VehicleOccupyingProcessed, VehicleOccupyingProcessedDomainEvent>(
                domainEvent),
            cancellationToken);
    }

    public async Task Publish(
        VehicleReleasedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var producer = topicProducerProvider.GetProducer<string, VehicleReleased>(
            new Uri($"topic:{_topicsConfiguration.VehicleReleasedTopic}"));

        await producer.Produce(domainEvent.EventId.ToString(),
            new VehicleReleased(domainEvent.EventId, domainEvent.VehicleId),
            SetMessageId<VehicleReleased, VehicleReleasedDomainEvent>(domainEvent),
            cancellationToken);
    }

    public async Task Publish(
        VehicleServicedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var producer = topicProducerProvider.GetProducer<string, VehicleServiced>(
            new Uri($"topic:{_topicsConfiguration.VehicleServicedTopic}"));

        await producer.Produce(domainEvent.EventId.ToString(),
            new VehicleServiced(domainEvent.EventId, domainEvent.VehicleId),
            SetMessageId<VehicleServiced, VehicleServicedDomainEvent>(domainEvent),
            cancellationToken);
    }

    private IPipe<KafkaSendContext<string, TContractEvent>> SetMessageId<TContractEvent,
        TDomainEvent>(
        TDomainEvent domainEvent)
        where TDomainEvent : DomainEvent
        where TContractEvent : class
    {
        return Pipe.Execute<KafkaSendContext<string, TContractEvent>>(ctx =>
            ctx.MessageId = domainEvent.EventId);
    }
}