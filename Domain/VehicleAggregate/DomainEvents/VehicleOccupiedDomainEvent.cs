using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.VehicleAggregate.DomainEvents;

public record VehicleOccupiedDomainEvent : DomainEvent
{
    public VehicleOccupiedDomainEvent(Guid vehicleId)
    {
        if (vehicleId == Guid.Empty) throw new ValueIsRequiredException($"{nameof(vehicleId)} cannot be empty");

        VehicleId = vehicleId;
    }

    public Guid VehicleId { get; }
}