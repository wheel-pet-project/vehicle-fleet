using Application.DomainEventHandlers.Vehicle;
using Application.Ports.Kafka;
using Domain.VehicleAggregate.DomainEvents;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace UnitTests.Application.DomainEventHandlers.Vehicle;

[TestSubject(typeof(VehicleOccupyingProcessedHandler))]
public class VehicleOccupyingProcessedHandlerShould
{
    private readonly VehicleOccupyingProcessedDomainEvent _event = new(Guid.NewGuid(),
        Guid.NewGuid(), true);

    private readonly Mock<IMessageBus> _messageBusMock = new();

    [Fact]
    public async Task CallMessageBusPublish()
    {
        // Arrange
        var handler = new VehicleOccupyingProcessedHandler(_messageBusMock.Object);

        // Act
        await handler.Handle(_event, TestContext.Current.CancellationToken);

        // Assert
        _messageBusMock.Verify(
            x => x.Publish(It.IsAny<VehicleOccupyingProcessedDomainEvent>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}