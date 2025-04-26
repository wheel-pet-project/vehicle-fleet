using Application.Ports.Postgres;
using Application.UseCases.Commands.Vehicle.OccupyVehicle;
using Domain.SharedKernel.Exceptions.DataConsistencyViolationException;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.Commands.Vehicle;

[TestSubject(typeof(OccupyVehicleHandler))]
public class OccupyVehicleHandlerShould
{
    private readonly OccupyVehicleCommand _command = new(Guid.NewGuid(), Guid.NewGuid());

    private readonly global::Domain.VehicleAggregate.Vehicle _vehicleFromDb =
        global::Domain.VehicleAggregate.Vehicle.Create(Guid.NewGuid(),
                PlateNumber.Create("К333ОТ77"), Color.White,
                Vin.Create("SALYA2BN2KA791786"), Location.Create(10.0, 10.0))
            .vehicle;

    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly OccupyVehicleHandler _handler;

    public OccupyVehicleHandlerShould()
    {
        _vehicleFromDb.MarkAsAdded();
        _vehicleFromDb.MarkAsReadiedForRelease(); // for state machine rules

        _vehicleRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(_vehicleFromDb);
        _unitOfWorkMock.Setup(x => x.Commit()).ReturnsAsync(Result.Ok);
        _handler = new OccupyVehicleHandler(_vehicleRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ReturnSuccess()
    {
        // Arrange

        // Act
        var actual = await _handler.Handle(_command, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(actual.IsSuccess);
    }

    [Fact]
    public async Task ThrowDataConsistencyExceptionIfVehicleNotFound()
    {
        // Arrange
        _vehicleRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync((global::Domain.VehicleAggregate.Vehicle?)null);

        // Act
        async Task Act()
        {
            await _handler.Handle(_command, TestContext.Current.CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DataConsistencyViolationException>(Act);
    }

    [Fact]
    public async Task ReturnFailIfCommitFailed()
    {
        // Arrange
        _unitOfWorkMock.Setup(x => x.Commit()).ReturnsAsync(Result.Fail("error"));

        // Act
        var actual = await _handler.Handle(_command, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(actual.IsFailed);
    }

    [Fact]
    public async Task VerifyCommitCall()
    {
        // Arrange

        // Act
        await _handler.Handle(_command, TestContext.Current.CancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
    }
}