using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.VehicleAggregate.DomainEvents;

public record VehicleAddedDomainEvent : DomainEvent
{
    public VehicleAddedDomainEvent(Guid vehicleId, Guid modelId)
    {
        if (vehicleId == Guid.Empty)
            throw new ValueIsRequiredException($"{nameof(vehicleId)} cannot be empty");
        if (modelId == Guid.Empty)
            throw new ValueIsRequiredException($"{nameof(modelId)} cannot be empty");

        VehicleId = vehicleId;
        ModelId = modelId;
    }

    public Guid VehicleId { get; }
    public Guid ModelId { get; }
}