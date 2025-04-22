using Domain.ModelAggregate.DomainEvents;
using Domain.SharedKernel.Exceptions.ArgumentException;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.ModelAggregate.DomainEvents;

[TestSubject(typeof(ModelCreatedDomainEvent))]
public class ModelCreatedDomainEventShould
{
    private readonly Guid _modelId = Guid.NewGuid();
    private readonly char _category = 'B';
    private readonly decimal _pricePerMinute = 1.0M;
    private readonly decimal _pricePerHour = 2.0M;
    private readonly decimal _pricePerDay = 3.0M;

    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange

        // Act
        var actual = new ModelCreatedDomainEvent(_modelId, _category, _pricePerMinute,
            _pricePerHour, _pricePerDay);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(_modelId, actual.ModelId);
        Assert.Equal(_category, actual.Category);
        Assert.Equal(_pricePerMinute, actual.PricePerMinute);
        Assert.Equal(_pricePerHour, actual.PricePerHour);
        Assert.Equal(_pricePerDay, actual.PricePerDay);
    }

    [Fact]
    public void ThrowValueIsRequiredExceptionIfModelIdIsEmpty()
    {
        // Arrange

        // Act
        void Act()
        {
            new ModelCreatedDomainEvent(Guid.Empty, _category, _pricePerMinute, _pricePerHour,
                _pricePerDay);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Theory]
    [InlineData('d')]
    [InlineData('1')]
    [InlineData('-')]
    public void ThrowOutOfRangeExceptionIfCategoryIsInvalid(char invalidCharacter)
    {
        // Arrange

        // Act
        void Act()
        {
            new ModelCreatedDomainEvent(_modelId, invalidCharacter, _pricePerMinute, _pricePerHour,
                _pricePerDay);
        }


        // Assert

        Assert.Throws<ValueOutOfRangeException>(Act);
    }

    [Theory]
    [InlineData(-1, 0, 0)]
    [InlineData(0, -1, 0)]
    [InlineData(0, 0, -1)]
    public void ThrowOutOfRangeExceptionIfOneOfPricesLessThanZero(
        decimal pricePerMinute,
        decimal pricePerHour,
        decimal pricePerDay)
    {
        // Arrange

        // Act
        void Act()
        {
            new ModelCreatedDomainEvent(_modelId, _category, pricePerMinute, pricePerHour,
                pricePerDay);
        }


        // Assert

        Assert.Throws<ValueOutOfRangeException>(Act);
    }
}