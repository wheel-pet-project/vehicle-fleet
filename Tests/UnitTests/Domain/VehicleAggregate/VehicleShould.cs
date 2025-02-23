using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.Exceptions.DomainRulesViolationException;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAggregate;
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
        var actual = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);

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
    public void ThrowValueIsRequiredExceptionIfModelIdIsEmpty()
    {
        // Arrange
        var emptyId = Guid.Empty;

        // Act
        void Act() => Vehicle.Create(emptyId, _plateNumber, _color, _vin, _fuelLevel, _location);

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfPlateNumberIsNull()
    {
        // Arrange

        // Act
        void Act() => Vehicle.Create(_modelId, null!, _color, _vin, _fuelLevel, _location);

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfColorIsNull()
    {
        // Arrange

        // Act
        void Act() => Vehicle.Create(_modelId, _plateNumber, null!, _vin, _fuelLevel, _location);

        // Assert
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfVinIsNull()
    {
        // Arrange

        // Act
        void Act() => Vehicle.Create(_modelId, _plateNumber, _color, null!, _fuelLevel, _location);

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfFuelLevelIsNull()
    {
        // Arrange

        // Act
        void Act() => Vehicle.Create(_modelId, _plateNumber, _color, _vin, null!, _location);

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfLocationIsNull()
    {
        // Arrange

        // Act
        void Act() => Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, null!);

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void AddDomainEventWhenCreatingNewInstance()
    {
        // Arrange

        // Act
        var actual = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);

        // Assert
        Assert.NotEmpty(actual.DomainEvents);
    }

    [Fact]
    public void MarkAsReadiedForRelease()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);

        // Act
        vehicle.MarkAsReadiedForRelease();

        // Assert
        Assert.Equal(Status.ReadiedForRelease, vehicle.Status);
    }

    [Fact]
    public void AddDomainEventWhenMarkingAsReadiedForRelease()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);

        // Act
        vehicle.MarkAsReadiedForRelease();

        // Assert
        Assert.NotEmpty(vehicle.DomainEvents);
    }

    [Fact]
    public void ThrowDomainRulesViolationExceptionIfMarkingAsReadiedForInvalidStatus()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);
        vehicle.MarkAsReadiedForRelease();
        vehicle.Release();

        // Act
        void Act() => vehicle.MarkAsReadiedForRelease();

        // Assert
        Assert.Throws<DomainRulesViolationException>(Act);
    }

    [Fact]
    public void Release()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);
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
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);
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
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);

        // Act
        void Act() => vehicle.Release();

        // Assert
        Assert.Throws<DomainRulesViolationException>(Act);
    }

    [Fact]
    public void Occupy()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);
        vehicle.MarkAsReadiedForRelease();
        vehicle.Release();
        
        // Act
        vehicle.Occupy();

        // Assert
        Assert.Equal(Status.Occupied, vehicle.Status);
    }

    [Fact]
    public void AddDomainEventWhenOccupying()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);
        vehicle.MarkAsReadiedForRelease();
        vehicle.Release();
        vehicle.ClearDomainEvents();

        // Act
        vehicle.Occupy();

        // Assert
        Assert.NotEmpty(vehicle.DomainEvents);
    }

    [Fact]
    public void ThrowDomainRulesViolationExceptionIfOccupyingForInvalidStatus()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);

        // Act
        void Act() => vehicle.Occupy();

        // Assert
        Assert.Throws<DomainRulesViolationException>(Act);
    }

    [Fact]
    public void MarkAsServiced()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);

        // Act
        vehicle.MarkAsServiced();

        // Assert
        Assert.Equal(Status.Serviced, vehicle.Status);
    }

    [Fact]
    public void AddDomainEventWhenMarkingAsServiced()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);

        // Act
        vehicle.MarkAsServiced();

        // Assert
        Assert.NotEmpty(vehicle.DomainEvents);
    }
    
    [Fact]
    public void ThrowDomainRulesViolationExceptionIfMarkingAsServicedForInvalidStatus()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);
        vehicle.MarkAsReadiedForRelease();
        vehicle.Release();
        vehicle.Occupy();

        // Act
        void Act() => vehicle.MarkAsServiced();

        // Assert
        Assert.Throws<DomainRulesViolationException>(Act);
    }

    [Fact]
    public void Delete()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);

        // Act
        vehicle.Delete();

        // Assert
        Assert.Equal(Status.Deleted, vehicle.Status);
    }

    [Fact]
    public void AddDomainEventWhenDeleting()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);

        // Act
        vehicle.Delete();

        // Assert
        Assert.NotEmpty(vehicle.DomainEvents);
    }
    
    [Fact]
    public void ThrowDomainRulesViolationExceptionIfDeletingForInvalidStatus()
    {
        // Arrange
        var vehicle = Vehicle.Create(_modelId, _plateNumber, _color, _vin, _fuelLevel, _location);
        vehicle.MarkAsReadiedForRelease();
        vehicle.Release();
        vehicle.Occupy();

        // Act
        void Act() => vehicle.Delete();

        // Assert
        Assert.Throws<DomainRulesViolationException>(Act);
    }
}