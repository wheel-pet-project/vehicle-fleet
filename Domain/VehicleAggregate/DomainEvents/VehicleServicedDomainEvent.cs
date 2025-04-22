using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.VehicleAggregate.DomainEvents;

public record VehicleServicedDomainEvent : DomainEvent
{
    public VehicleServicedDomainEvent(Guid vehicleId)
    {
        if (vehicleId == Guid.Empty)
            throw new ValueIsRequiredException($"{nameof(vehicleId)} cannot be empty");

        VehicleId = vehicleId;
    }

    public Guid VehicleId { get; }
}