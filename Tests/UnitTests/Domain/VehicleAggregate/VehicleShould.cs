using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.Exceptions.DomainRulesViolationException;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAddingSaga;
using Domain.VehicleAggregate;
using Domain.VehicleAggregate.DomainEvents;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.VehicleAggregate;

[TestSubject(typeof(Vehicle))]
public class VehicleShould
{
    private readonly Guid _modelId = Guid.NewGuid();
    private readonly PlateNumber _plateNumber = PlateNumber.Create("К333ОТ77");
    private readonly Color _color = Color.Red;
    private readonly Vin _vin = Vin.Create("SALYA2BN2KA791786");
    private readonly FuelLevel _fuelLevel = FuelLevel.Create();
    private readonly Location _location = Location.Create(10.0, 10.0);

    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange

        // Act
        var (_, actual) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);

        // Assert
        Assert.NotNull(actual);
        Assert.NotEqual(Guid.Empty, actual.Id);
        Assert.Equal(_modelId, actual.ModelId);
        Assert.Equal(_plateNumber, actual.PlateNumber);
        Assert.Equal(_vin, actual.Vin);
        Assert.Equal(_fuelLevel, actual.FuelLevel);
        Assert.Equal(_location, actual.Location);
    }

    [Fact]
    public void CreateNewVehicleAddingSagaInstanceWhenCreated()
    {
        // Arrange

        // Act
        var (saga, _) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);

        // Assert
        Assert.IsType<global::Domain.VehicleAddingSaga.VehicleAddingSaga>(saga);
    }
    
    [Fact]
    public void ThrowValueIsRequiredExceptionIfModelIdIsEmpty()
    {
        // Arrange
        var emptyId = Guid.Empty;

        // Act
        void Act()
        {
            Vehicle.Create(emptyId, _plateNumber, _color, _vin, _location, _fuelLevel);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfPlateNumberIsNull()
    {
        // Arrange

        // Act
        void Act()
        {
            Vehicle.Create(_modelId, null!, _color, _vin, _location, _fuelLevel);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfColorIsNull()
    {
        // Arrange

        // Act
        void Act()
        {
            Vehicle.Create(_modelId, _plateNumber, null!, _vin, _location, _fuelLevel);
        }

        // Assert
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfVinIsNull()
    {
        // Arrange

        // Act
        void Act()
        {
            Vehicle.Create(_modelId, _plateNumber, _color, null!, _location, _fuelLevel);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void SetDefaultLocationIfLocationIsNull()
    {
        // Arrange

        // Act
        var (_, actual) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, null!, _fuelLevel);

        // Assert
        Assert.NotNull(actual.Location);
    }

    [Fact]
    public void SetDefaultFuelLevelIfFuelLevelIsNull()
    {
        // Arrange

        // Act
        var (_, actual) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location);

        // Assert
        Assert.NotNull(actual.FuelLevel);
    }

    [Fact]
    public void AddDomainEventWhenCreatingNewInstance()
    {
        // Arrange

        // Act
        var (_, actual) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);

        // Assert
        Assert.NotEmpty(actual.DomainEvents);
    }

    [Fact]
    public void MarkAsAddedChangeStatus()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location);

        // Act
        vehicle.MarkAsAdded();

        // Assert
        Assert.Equal(Status.Added, vehicle.Status);
    }

    [Fact]
    public void MarkAsAddedThrowDomainRulesViolationExceptionIfVehicleCannotBeChangedToThisStatus()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location);
        vehicle.MarkAsNotAdded();

        // Act
        void Act()
        {
            vehicle.MarkAsAdded();
        }

        // Assert
        Assert.Throws<DomainRulesViolationException>(Act);
    }

    [Fact]
    public void MarkAsNotAddedChangeStatus()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location);

        // Act
        vehicle.MarkAsNotAdded();

        // Assert
        Assert.Equal(Status.NotAdded, vehicle.Status);
    }

    [Fact]
    public void
        MarkAsNotAddedThrowDomainRulesViolationExceptionIfVehicleCannotBeChangedToThisStatus()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location);
        vehicle.MarkAsAdded();

        // Act
        void Act()
        {
            vehicle.MarkAsNotAdded();
        }

        // Assert
        Assert.Throws<DomainRulesViolationException>(Act);
    }

    [Fact]
    public void MarkAsReadiedForRelease()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();

        // Act
        vehicle.MarkAsReadiedForRelease();

        // Assert
        Assert.Equal(Status.ReadiedForRelease, vehicle.Status);
    }

    [Fact]
    public void AddDomainEventWhenMarkingAsReadiedForRelease()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();

        // Act
        vehicle.MarkAsReadiedForRelease();

        // Assert
        Assert.NotEmpty(vehicle.DomainEvents);
    }

    [Fact]
    public void ThrowDomainRulesViolationExceptionIfMarkingAsReadiedForInvalidStatus()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();
        vehicle.MarkAsReadiedForRelease();
        vehicle.Release();

        // Act
        void Act()
        {
            vehicle.MarkAsReadiedForRelease();
        }

        // Assert
        Assert.Throws<DomainRulesViolationException>(Act);
    }

    [Fact]
    public void Release()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();
        vehicle.MarkAsReadiedForRelease();

        // Act
        vehicle.Release();

        // Assert
        Assert.Equal(Status.Released, vehicle.Status);
    }

    [Fact]
    public void AddDomainEventWhenReleasing()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();
        vehicle.MarkAsReadiedForRelease();
        vehicle.ClearDomainEvents();

        // Act
        vehicle.Release();

        // Assert
        Assert.NotEmpty(vehicle.DomainEvents);
    }

    [Fact]
    public void ThrowDomainRulesViolationExceptionIfReleasingForInvalidStatus()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();

        // Act
        void Act()
        {
            vehicle.Release();
        }

        // Assert
        Assert.Throws<DomainRulesViolationException>(Act);
    }

    [Fact]
    public void Occupy()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();
        vehicle.MarkAsReadiedForRelease();
        vehicle.Release();

        // Act
        vehicle.Occupy(Guid.NewGuid());

        // Assert
        Assert.Equal(Status.Occupied, vehicle.Status);
    }

    [Fact]
    public void AddVehicleOccupyingProcessedDomainEventWithOccupiedPropertyIsTrueWhenOccupying()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();
        vehicle.MarkAsReadiedForRelease();
        vehicle.Release();
        vehicle.ClearDomainEvents();

        // Act
        vehicle.Occupy(Guid.NewGuid());

        // Assert
        Assert.NotEmpty(vehicle.DomainEvents);
        Assert.True(
            vehicle.DomainEvents.Count(x => x is VehicleOccupyingProcessedDomainEvent
            {
                IsOccupied: true
            }) == 1);
    }

    [Fact]
    public void
        AddVehicleOccupyingProcessedDomainEventWithOccupiedPropertyIsFalseIfOccupyingForInvalidStatus()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();

        // Act
        vehicle.Occupy(Guid.NewGuid());

        // Assert
        Assert.NotEmpty(vehicle.DomainEvents);
        Assert.True(
            vehicle.DomainEvents.Count(x => x is VehicleOccupyingProcessedDomainEvent
            {
                IsOccupied: false
            }) == 1);
    }

    [Fact]
    public void MarkAsServicedChangeStatus()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();

        // Act
        vehicle.MarkAsServiced();

        // Assert
        Assert.Equal(Status.Serviced, vehicle.Status);
    }

    [Fact]
    public void AddDomainEventWhenMarkingAsServiced()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();

        // Act
        vehicle.MarkAsServiced();

        // Assert
        Assert.NotEmpty(vehicle.DomainEvents);
    }

    [Fact]
    public void ThrowDomainRulesViolationExceptionIfMarkingAsServicedForInvalidStatus()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();
        vehicle.MarkAsReadiedForRelease();
        vehicle.Release();
        vehicle.Occupy(Guid.NewGuid());

        // Act
        void Act()
        {
            vehicle.MarkAsServiced();
        }

        // Assert
        Assert.Throws<DomainRulesViolationException>(Act);
    }

    [Fact]
    public void Delete()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();

        // Act
        vehicle.Delete();

        // Assert
        Assert.Equal(Status.Deleted, vehicle.Status);
    }

    [Fact]
    public void AddDomainEventWhenDeleting()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();

        // Act
        vehicle.Delete();

        // Assert
        Assert.NotEmpty(vehicle.DomainEvents);
    }

    [Fact]
    public void ThrowDomainRulesViolationExceptionIfDeletingForInvalidStatus()
    {
        // Arrange
        var (_, vehicle) = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _location, _fuelLevel);
        vehicle.MarkAsAdded();
        vehicle.MarkAsReadiedForRelease();
        vehicle.Release();
        vehicle.Occupy(Guid.NewGuid());

        // Act
        void Act()
        {
            vehicle.Delete();
        }

        // Assert
        Assert.Throws<DomainRulesViolationException>(Act);
    }
}