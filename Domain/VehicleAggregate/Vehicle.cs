using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.Exceptions.DomainRulesViolationException;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAggregate.DomainEvents;

namespace Domain.VehicleAggregate;

public sealed class Vehicle : Aggregate
{
    private Vehicle()
    {
    }

    private Vehicle(
        Guid modelId,
        PlateNumber plateNumber,
        Color color,
        Vin vin,
        FuelLevel fuelLevel,
        Location location)
        : this()
    {
        Id = Guid.NewGuid();
        ModelId = modelId;
        Status = Status.AddingInProgress;
        PlateNumber = plateNumber;
        Color = color;
        Vin = vin;
        FuelLevel = fuelLevel;
        Location = location;
    }

    public Guid Id { get; }
    public Guid ModelId { get; private set; }
    public Status Status { get; private set; } = null!;
    public PlateNumber PlateNumber { get; private set; } = null!;
    public Color Color { get; private set; } = null!;
    public Vin Vin { get; private set; } = null!;
    public FuelLevel FuelLevel { get; private set; } = null!;
    public Location Location { get; private set; } = null!;

    public void MarkAsAdded()
    {
        if (Status.CanBeChangedToThisStatus(Status.Added) == false)
            throw new DomainRulesViolationException(
                $"Vehicle cannot be marked as added in '{Status.Name}' status");

        Status = Status.Added;
    }

    public void MarkAsNotAdded()
    {
        if (Status.CanBeChangedToThisStatus(Status.NotAdded) == false)
            throw new DomainRulesViolationException(
                $"Vehicle cannot be marked as not added in '{Status.Name}' status");

        Status = Status.NotAdded;
    }

    public void MarkAsReadiedForRelease()
    {
        if (Status.CanBeChangedToThisStatus(Status.ReadiedForRelease) == false)
            throw new DomainRulesViolationException(
                $"Vehicle cannot be marked as readied for release in '{Status.Name}' status");

        Status = Status.ReadiedForRelease;

        AddDomainEvent(new VehicleReadiedForReleaseDomainEvent(Id));
    }

    public void Release()
    {
        if (Status.CanBeChangedToThisStatus(Status.Released) == false)
            throw new DomainRulesViolationException(
                $"Vehicle cannot release in '{Status.Name}' status");

        Status = Status.Released;

        AddDomainEvent(new VehicleReleasedDomainEvent(Id));
    }

    public void Occupy(Guid bookingId)
    {
        if (Status.CanBeChangedToThisStatus(Status.Occupied))
        {
            Status = Status.Occupied;
            AddDomainEvent(new VehicleOccupyingProcessedDomainEvent(Id, bookingId, true));
        }
        else
        {
            AddDomainEvent(new VehicleOccupyingProcessedDomainEvent(Id, bookingId, false));
        }
    }

    public void MarkAsServiced()
    {
        if (Status.CanBeChangedToThisStatus(Status.Serviced) == false)
            throw new DomainRulesViolationException(
                $"Vehicle cannot be serviced in '{Status.Name}' status");

        Status = Status.Serviced;

        AddDomainEvent(new VehicleServicedDomainEvent(Id));
    }

    public void Delete()
    {
        if (Status.CanBeChangedToThisStatus(Status.Deleted) == false)
            throw new DomainRulesViolationException(
                $"Vehicle cannot be deleted in '{Status.Name}' status");

        Status = Status.Deleted;

        AddDomainEvent(new VehicleDeletedDomainEvent(Id));
    }

    public static (VehicleAddingSaga.VehicleAddingSaga saga, Vehicle vehicle) Create(
        Guid modelId,
        PlateNumber plateNumber,
        Color color,
        Vin vin,
        Location? location = null,
        FuelLevel? fuelLevel = null)
    {
        if (modelId == Guid.Empty) throw new ValueIsRequiredException($"{nameof(modelId)} cannot be empty");
        if (plateNumber == null) throw new ValueIsRequiredException($"{nameof(plateNumber)} cannot be null");
        if (color == null) throw new ValueIsRequiredException($"{nameof(color)} cannot be null");
        if (vin == null) throw new ValueIsRequiredException($"{nameof(vin)} cannot be null");

        if (location == null) location = Location.Create(56.0357178825, 37.7567042515); // Учинское вдхр.
        if (fuelLevel == null) fuelLevel = FuelLevel.Create();

        var vehicle = new Vehicle(modelId, plateNumber, color, vin, fuelLevel, location);

        var saga = new VehicleAddingSaga.VehicleAddingSaga(vehicle.Id);
        
        vehicle.AddDomainEvent(new VehicleAddedDomainEvent(vehicle.Id, vehicle.ModelId, saga.SagaId));

        return (saga, vehicle);
    }
}