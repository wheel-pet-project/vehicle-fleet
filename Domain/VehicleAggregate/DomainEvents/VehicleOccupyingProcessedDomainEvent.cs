using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.VehicleAggregate.DomainEvents;

public record VehicleOccupyingProcessedDomainEvent : DomainEvent
{
    public VehicleOccupyingProcessedDomainEvent(
        Guid vehicleId,
        Guid bookingId,
        bool isOccupied,
        string? reason = "Vehicle already booked")
    {
        if (vehicleId == Guid.Empty)
            throw new ValueIsRequiredException($"{nameof(vehicleId)} cannot be empty");
        if (bookingId == Guid.Empty)
            throw new ValueIsRequiredException($"{nameof(bookingId)} cannot be empty");

        VehicleId = vehicleId;
        BookingId = bookingId;
        IsOccupied = isOccupied;
        Reason = reason;
    }

    public Guid VehicleId { get; }
    public Guid BookingId { get; }
    public bool IsOccupied { get; }
    public string? Reason { get; }
}