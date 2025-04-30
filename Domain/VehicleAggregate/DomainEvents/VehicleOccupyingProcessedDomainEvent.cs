using Domain.SharedKernel;

namespace Domain.VehicleAggregate.DomainEvents;

public record VehicleOccupyingProcessedDomainEvent : DomainEvent
{
    public VehicleOccupyingProcessedDomainEvent(
        Guid vehicleId,
        Guid bookingId,
        bool isOccupied,
        string? reason = "Vehicle already booked")
    {
        if (vehicleId == Guid.Empty) throw new ArgumentException($"{nameof(vehicleId)} cannot be empty");
        if (bookingId == Guid.Empty) throw new ArgumentException($"{nameof(bookingId)} cannot be empty");

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