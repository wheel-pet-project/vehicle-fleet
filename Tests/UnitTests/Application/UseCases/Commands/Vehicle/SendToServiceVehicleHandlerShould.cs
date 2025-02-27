using Application.Ports.Postgres;
using Application.UseCases.Commands.Vehicle.SendToServiceVehicle;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.Commands.Vehicle;

[TestSubject(typeof(SendToServiceVehicleHandler))]
public class SendToServiceVehicleHandlerShould
{
    private readonly SendToServiceVehicleRequest _request = new(Guid.NewGuid());

    private readonly global::Domain.VehicleAggregate.Vehicle _vehicleFromDb =
        global::Domain.VehicleAggregate.Vehicle.Create(Guid.NewGuid(), PlateNumber.Create("К333ОТ77"), Color.White,
            Vin.Create("SALYA2BN2KA791786"), Location.Create(10.0, 10.0));

    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly SendToServiceVehicleHandler _handler;

    public SendToServiceVehicleHandlerShould()
    {
        _vehicleRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(_vehicleFromDb);
        _unitOfWorkMock.Setup(x => x.Commit()).ReturnsAsync(Result.Ok);
        _handler = new SendToServiceVehicleHandler(_vehicleRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ReturnSuccess()
    {
        // Arrange

        // Act
        var actual = await _handler.Handle(_request, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(actual.IsSuccess);
    }

    [Fact]
    public async Task ReturnFailIfVehicleNotFound()
    {
        // Arrange
        _vehicleRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync((global::Domain.VehicleAggregate.Vehicle?)null);

        // Act
        var actual = await _handler.Handle(_request, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(actual.IsFailed);
        Assert.IsType<NotFound>(actual.Errors[0]);
    }

    [Fact]
    public async Task ReturnFailIfCommitFailed()
    {
        // Arrange
        _unitOfWorkMock.Setup(x => x.Commit()).ReturnsAsync(Result.Fail("error"));

        // Act
        var actual = await _handler.Handle(_request, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(actual.IsFailed);
    }

    [Fact]
    public async Task VerifyCommitCall()
    {
        // Arrange

        // Act
        await _handler.Handle(_request, TestContext.Current.CancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
    }
}