using Domain.ModelAggregate.DomainEvents;
using Domain.SharedKernel.Exceptions.ArgumentException;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.ModelAggregate.DomainEvents;

[TestSubject(typeof(ModelTariffUpdatedDomainEvent))]
public class ModelTariffUpdateDomainEventShould
{
    private readonly Guid _modelId = Guid.NewGuid();
    private readonly decimal _pricePerMinute = 1.0M;
    private readonly decimal _pricePerHour = 2.0M;
    private readonly decimal _pricePerDay = 3.0M;

    [Fact]
    public void CreateNewInstanceWithCorrectValues()
    {
        // Arrange

        // Act
        var actual = new ModelTariffUpdatedDomainEvent(_modelId, _pricePerMinute, _pricePerHour, _pricePerDay);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(_modelId, actual.ModelId);
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
            new ModelTariffUpdatedDomainEvent(Guid.Empty, _pricePerMinute, _pricePerHour, _pricePerDay);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
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
            new ModelTariffUpdatedDomainEvent(_modelId, pricePerMinute, pricePerHour, pricePerDay);
        }


        // Assert

        Assert.Throws<ValueOutOfRangeException>(Act);
    }
}