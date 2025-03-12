using Application.DomainEventHandlers.Vehicle;
using Application.Ports.Kafka;
using Domain.VehicleAggregate.DomainEvents;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace UnitTests.Application.DomainEventHandlers.Vehicle;

[TestSubject(typeof(VehicleReleasedHandler))]
public class VehicleReleasedHandlerShould
{
    private readonly VehicleReleasedDomainEvent _event = new(Guid.NewGuid());

    private readonly Mock<IMessageBus> _messageBusMock = new();

    [Fact]
    public async Task CallMessageBusPublish()
    {
        // Arrange
        var handler = new VehicleReleasedHandler(_messageBusMock.Object);

        // Act
        await handler.Handle(_event, TestContext.Current.CancellationToken);

        // Assert
        _messageBusMock.Verify(
            x => x.Publish(It.IsAny<VehicleReleasedDomainEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}