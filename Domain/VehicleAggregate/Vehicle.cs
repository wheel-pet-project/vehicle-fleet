using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.Exceptions.DomainRulesViolationException;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAggregate.DomainEvents;

namespace Domain.VehicleAggregate;

public sealed class Vehicle : Aggregate
{
    private Vehicle() { }

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
        Status = Status.Added;
        PlateNumber = plateNumber;
        Color = color;
        Vin = vin;
        FuelLevel = fuelLevel;
        Location = location;
    }
    
    public Guid Id { get; }
    public Guid ModelId { get; private set; }
    public Status Status { get; private set; }
    public PlateNumber PlateNumber { get; private set; }
    public Color Color { get; private set; }
    public Vin Vin { get; private set; }
    public FuelLevel FuelLevel { get; private set; }
    public Location Location { get; private set; }

    public void MarkAsReadiedForRelease()
    {
        if (Status.CanBeChangedToThisStatus(Status.ReadiedForRelease) == false)
            throw new DomainRulesViolationException("Cannot change the to this status of the vehicle");
        
        Status = Status.ReadiedForRelease;
        
        AddDomainEvent(new VehicleReadiedForReleaseDomainEvent(Id));
    }

    public void Release()
    {
        if (Status.CanBeChangedToThisStatus(Status.Released) == false)
            throw new DomainRulesViolationException("Cannot change the to this status of the vehicle");
        
        Status = Status.Released;
        
        AddDomainEvent(new VehicleReleasedDomainEvent(Id));
    }

    public void Occupy()
    {
        if (Status.CanBeChangedToThisStatus(Status.Occupied) == false)
            throw new DomainRulesViolationException("Cannot change the to this status of the vehicle");
        
        Status = Status.Occupied;
        
        AddDomainEvent(new VehicleOccupiedDomainEvent(Id));
    }

    public void MarkAsServiced()
    {
        if (Status.CanBeChangedToThisStatus(Status.Serviced) == false)
            throw new DomainRulesViolationException("Cannot change the to this status of the vehicle");
        
        Status = Status.Serviced;
        
        AddDomainEvent(new VehicleServicedDomainEvent(Id));
    }

    public void Delete()
    {
        if (Status.CanBeChangedToThisStatus(Status.Deleted) == false)
            throw new DomainRulesViolationException("Cannot change the to this status of the vehicle");
        
        Status = Status.Deleted;
        
        AddDomainEvent(new VehicleDeletedDomainEvent(Id));
    }
    
    public static Vehicle Create(
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
        
        vehicle.AddDomainEvent(new VehicleAddedDomainEvent(vehicle.Id, vehicle.ModelId));
        
        return vehicle;
    }
}