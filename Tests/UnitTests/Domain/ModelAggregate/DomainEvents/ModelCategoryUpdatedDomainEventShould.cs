using Domain.ModelAggregate.DomainEvents;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.ModelAggregate.DomainEvents;

[TestSubject(typeof(ModelCategoryUpdatedDomainEvent))]
public class ModelCategoryUpdatedDomainEventShould
{
    private readonly Guid _modelId = Guid.NewGuid();
    private readonly char _category = 'B';

    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange

        // Act
        var actual = new ModelCategoryUpdatedDomainEvent(_modelId, _category);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(_modelId, actual.ModelId);
        Assert.Equal(_category, actual.Category);
    }

    [Fact]
    public void ThrowArgumentExceptionIfModelIdIsEmpty()
    {
        // Arrange

        // Act
        void Act()
        {
            new ModelCategoryUpdatedDomainEvent(Guid.Empty, _category);
        }

        // Assert
        Assert.Throws<ArgumentException>(Act);
    }

    [Theory]
    [InlineData('d')]
    [InlineData('1')]
    [InlineData('-')]
    public void ThrowArgumentExceptionIfCategoryIsInvalid(char invalidCharacter)
    {
        // Arrange

        // Act
        void Act()
        {
            new ModelCategoryUpdatedDomainEvent(_modelId, invalidCharacter);
        }


        // Assert

        Assert.Throws<ArgumentException>(Act);
    }
}