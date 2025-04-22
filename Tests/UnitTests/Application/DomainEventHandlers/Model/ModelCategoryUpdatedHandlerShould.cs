using Application.DomainEventHandlers.Model;
using Application.Ports.Kafka;
using Domain.ModelAggregate.DomainEvents;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace UnitTests.Application.DomainEventHandlers.Model;

[TestSubject(typeof(ModelCategoryUpdatedHandler))]
public class ModelCategoryUpdatedHandlerShould
{
    private readonly ModelCategoryUpdatedDomainEvent _event = new(Guid.NewGuid(), 'B');

    private readonly Mock<IMessageBus> _messageBusMock = new();

    [Fact]
    public async Task CallMessageBusPublish()
    {
        // Arrange
        var handler = new ModelCategoryUpdatedHandler(_messageBusMock.Object);

        // Act
        await handler.Handle(_event, TestContext.Current.CancellationToken);

        // Assert
        _messageBusMock.Verify(
            x => x.Publish(It.IsAny<ModelCategoryUpdatedDomainEvent>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}