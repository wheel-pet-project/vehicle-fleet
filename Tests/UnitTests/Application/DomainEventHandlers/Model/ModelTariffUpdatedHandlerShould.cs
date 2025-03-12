using Application.DomainEventHandlers.Model;
using Application.Ports.Kafka;
using Domain.ModelAggregate.DomainEvents;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace UnitTests.Application.DomainEventHandlers.Model;

[TestSubject(typeof(ModelTariffUpdatedHandler))]
public class ModelTariffUpdatedHandlerShould
{
    private readonly ModelTariffUpdatedDomainEvent _event = new(Guid.NewGuid(), 1.0M, 1.0M, 1.0M);

    private readonly Mock<IMessageBus> _messageBusMock = new();

    [Fact]
    public async Task CallMessageBusPublish()
    {
        // Arrange
        var handler = new ModelTariffUpdatedHandler(_messageBusMock.Object);

        // Act
        await handler.Handle(_event, TestContext.Current.CancellationToken);

        // Assert
        _messageBusMock.Verify(x => x.Publish(It.IsAny<ModelTariffUpdatedDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}