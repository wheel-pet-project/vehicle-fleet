using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.VehicleAggregate.DomainEvents;

public record VehicleReleasedDomainEvent : DomainEvent
{
    public VehicleReleasedDomainEvent(Guid vehicleId)
    {
        if (vehicleId == Guid.Empty) throw new ValueIsRequiredException($"{nameof(vehicleId)} cannot be empty");
        
        VehicleId = vehicleId;
    }
    
    public Guid VehicleId { get; }
}