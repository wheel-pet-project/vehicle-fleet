using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.VehicleAggregate.DomainEvents;

public record VehicleAddedDomainEvent : DomainEvent
{
    public VehicleAddedDomainEvent(Guid vehicleId, Guid modelId, Guid sagaId)
    {
        if (vehicleId == Guid.Empty)
            throw new ValueIsRequiredException($"{nameof(vehicleId)} cannot be empty");
        if (modelId == Guid.Empty)
            throw new ValueIsRequiredException($"{nameof(modelId)} cannot be empty");
        if (sagaId == Guid.Empty)
            throw new ValueIsRequiredException($"{nameof(sagaId)} cannot be empty");

        VehicleId = vehicleId;
        ModelId = modelId;
        SagaId = sagaId;
    }

    public Guid VehicleId { get; }
    public Guid ModelId { get; }
    public Guid SagaId { get; }
}