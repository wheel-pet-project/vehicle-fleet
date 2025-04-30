using Domain.SharedKernel;

namespace Domain.VehicleAggregate.DomainEvents;

public record VehicleAddingFaultedDomainEvent : DomainEvent
{
    public VehicleAddingFaultedDomainEvent(Guid vehicleId)
    {
        if (vehicleId == Guid.Empty) throw new ArgumentException($"{nameof(vehicleId)} cannot be empty");

        VehicleId = vehicleId;
    }

    public Guid VehicleId { get; }
}