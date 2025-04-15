using Domain.SharedKernel.Exceptions.AlreadyHaveThisState;
using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.VehicleAggregate;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.VehicleAggregate;

[TestSubject(typeof(Status))]
public class StatusShould
{
    [Fact]
    public void FromNameReturnCorrectStatus()
    {
        // Arrange
        var name = Status.Added.Name;

        // Act
        var actual = Status.FromName(name);

        // Assert
        Assert.Equal(Status.Added, actual);
    }

    [Fact]
    public void FromNameThrowValueOutOfRangeExceptionIfStatusNameIsNotSupported()
    {
        // Arrange
        var name = "unsupported";

        // Act
        void Act()
        {
            Status.FromName(name);
        }

        // Assert
        Assert.Throws<ValueOutOfRangeException>(Act);
    }

    [Fact]
    public void FromIdReturnCorrectStatus()
    {
        // Arrange
        var id = Status.Added.Id;

        // Act
        var actual = Status.FromId(id);

        // Assert
        Assert.Equal(Status.Added, actual);
    }

    [Fact]
    public void FromIdThrowValueOutOfRangeExceptionIfStatusIdIsNotSupported()
    {
        // Arrange
        var id = 434;

        // Act
        void Act()
        {
            Status.FromId(id);
        }

        // Assert
        Assert.Throws<ValueOutOfRangeException>(Act);
    }

    [Fact]
    public void EqualOperatorShouldReturnTrueIfStatusesIsEqual()
    {
        // Arrange
        var status1 = Status.Added;
        var status2 = Status.Added;

        // Act
        var actual = status1 == status2;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void NotEqualOperatorShouldReturnTrueIfStatusesIsDifferent()
    {
        // Arrange
        var status1 = Status.Added;
        var status2 = Status.Deleted;

        // Act
        var actual = status1 != status2;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void CanBeChangedToThisStatusThrowAlreadyHaveThisStateExceptionIfPotentialStatusIsEqualCurrent()
    {
        // Arrange
        var added = Status.Added;

        // Act
        void Act()
        {
            added.CanBeChangedToThisStatus(Status.Added);
        }

        // Assert
        Assert.Throws<AlreadyHaveThisStateException>(Act);
    }

    [Fact]
    public void CanBeChangedToThisStatusThrowValueIsRequiredExceptionIfPotentialStatusIsNull()
    {
        // Arrange
        var added = Status.Added;

        // Act
        void Act()
        {
            added.CanBeChangedToThisStatus(null);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void CanBeChangedToThisStatusReturnFalseForThisStatuses()
    {
        // Arrange
        var added = Status.Added;
        var readiedForRelease = Status.ReadiedForRelease;
        var released = Status.Released;
        var occupied = Status.Occupied;
        var serviced = Status.Serviced;
        var deleted = Status.Deleted;

        // Act
        var addedToReleased = added.CanBeChangedToThisStatus(released);
        var addedToOccupied = added.CanBeChangedToThisStatus(occupied);

        var readiedForReleasedToAdded = readiedForRelease.CanBeChangedToThisStatus(added);
        var readiedForReleasedToOccupied = readiedForRelease.CanBeChangedToThisStatus(occupied);

        var releasedToAdded = released.CanBeChangedToThisStatus(added);
        var releasedToDeleted = released.CanBeChangedToThisStatus(deleted);
        var releasedToReadiedForRelease = released.CanBeChangedToThisStatus(readiedForRelease);

        var occupiedToAdded = occupied.CanBeChangedToThisStatus(added);
        var occupiedToReadiedForRelease = occupied.CanBeChangedToThisStatus(readiedForRelease);
        var occupiedToServiced = occupied.CanBeChangedToThisStatus(serviced);
        var occupiedToDeleted = occupied.CanBeChangedToThisStatus(deleted);

        var servicedToAdded = serviced.CanBeChangedToThisStatus(added);
        var servicedToReleased = serviced.CanBeChangedToThisStatus(released);
        var servicedToOccupied = serviced.CanBeChangedToThisStatus(occupied);

        var deletedToAdded = deleted.CanBeChangedToThisStatus(added);
        var deletedToReadiedForRelease = deleted.CanBeChangedToThisStatus(readiedForRelease);
        var deletedToReleased = deleted.CanBeChangedToThisStatus(released);
        var deletedToOccupied = deleted.CanBeChangedToThisStatus(occupied);
        var deletedToServiced = deleted.CanBeChangedToThisStatus(serviced);

        // Assert
        Assert.False(addedToReleased);
        Assert.False(addedToOccupied);

        Assert.False(readiedForReleasedToAdded);
        Assert.False(readiedForReleasedToOccupied);

        Assert.False(releasedToAdded);
        Assert.False(releasedToDeleted);
        Assert.False(releasedToReadiedForRelease);

        Assert.False(occupiedToAdded);
        Assert.False(occupiedToReadiedForRelease);
        Assert.False(occupiedToServiced);
        Assert.False(occupiedToDeleted);

        Assert.False(servicedToAdded);
        Assert.False(servicedToReleased);
        Assert.False(servicedToOccupied);

        Assert.False(deletedToAdded);
        Assert.False(deletedToReadiedForRelease);
        Assert.False(deletedToReleased);
        Assert.False(deletedToOccupied);
        Assert.False(deletedToServiced);
    }

    [Fact]
    public void CanBeChangedToThisStatusReturnTrueForThisStatuses()
    {
        // Arrange
        var added = Status.Added;
        var readiedForRelease = Status.ReadiedForRelease;
        var released = Status.Released;
        var occupied = Status.Occupied;
        var serviced = Status.Serviced;
        var deleted = Status.Deleted;

        // Act
        var addedToReadiedForRelease = added.CanBeChangedToThisStatus(readiedForRelease);
        var addedToDeleted = added.CanBeChangedToThisStatus(deleted);
        var addedToServiced = added.CanBeChangedToThisStatus(serviced);

        var readiedForReleasedToReleased = readiedForRelease.CanBeChangedToThisStatus(released);
        var readiedForReleasedToDeleted = readiedForRelease.CanBeChangedToThisStatus(deleted);
        var readiedForReleasedToServiced = readiedForRelease.CanBeChangedToThisStatus(serviced);

        var releasedToOccupied = released.CanBeChangedToThisStatus(occupied);
        var releasedToServiced = released.CanBeChangedToThisStatus(serviced);

        var occupiedToReleased = occupied.CanBeChangedToThisStatus(released);

        var servicedToReadiedForRelease = serviced.CanBeChangedToThisStatus(readiedForRelease);
        var servicedToDeleted = serviced.CanBeChangedToThisStatus(deleted);

        // Assert
        Assert.True(addedToReadiedForRelease);
        Assert.True(addedToDeleted);
        Assert.True(addedToServiced);

        Assert.True(readiedForReleasedToReleased);
        Assert.True(readiedForReleasedToDeleted);
        Assert.True(readiedForReleasedToServiced);

        Assert.True(releasedToOccupied);
        Assert.True(releasedToServiced);

        Assert.True(occupiedToReleased);

        Assert.True(servicedToReadiedForRelease);
        Assert.True(servicedToDeleted);
    }
}