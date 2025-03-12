using Application.DomainEventHandlers.Vehicle;
using Application.Ports.Kafka;
using Domain.VehicleAggregate.DomainEvents;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace UnitTests.Application.DomainEventHandlers.Vehicle;

[TestSubject(typeof(VehicleServicedHandler))]
public class VehicleServicedHandlerShould
{
    private readonly VehicleServicedDomainEvent _event = new(Guid.NewGuid());

    private readonly Mock<IMessageBus> _messageBusMock = new();

    [Fact]
    public async Task CallMessageBusPublish()
    {
        // Arrange
        var handler = new VehicleServicedHandler(_messageBusMock.Object);

        // Act
        await handler.Handle(_event, TestContext.Current.CancellationToken);

        // Assert
        _messageBusMock.Verify(
            x => x.Publish(It.IsAny<VehicleServicedDomainEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}