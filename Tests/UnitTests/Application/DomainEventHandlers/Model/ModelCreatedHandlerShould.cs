using Application.DomainEventHandlers.Model;
using Application.Ports.Kafka;
using Domain.ModelAggregate.DomainEvents;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace UnitTests.Application.DomainEventHandlers.Model;

[TestSubject(typeof(ModelCreatedHandler))]
public class ModelCreatedHandlerShould
{
    private readonly ModelCreatedDomainEvent _event = new(Guid.NewGuid(), 'B', 1.0M, 1.0M, 1.0M);
    
    private readonly Mock<IMessageBus> _messageBusMock = new();
    
    [Fact]
    public async Task CallMessageBusPublish()
    {
        // Arrange
        var handler = new ModelCreatedHandler(_messageBusMock.Object);

        // Act
        await handler.Handle(_event, TestContext.Current.CancellationToken);

        // Assert
        _messageBusMock.Verify(x => x.Publish(It.IsAny<ModelCreatedDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}