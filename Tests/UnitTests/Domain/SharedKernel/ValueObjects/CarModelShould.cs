using Domain.SharedKernel.Exceptions.PublicExceptions;
using Domain.SharedKernel.ValueObjects;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.SharedKernel.ValueObjects;

[TestSubject(typeof(CarModel))]
public class CarModelShould
{
    [Fact]
    public void CreateNewInstanceWithCorrectName()
    {
        // Arrange
        var name = "Rio";

        // Act
        var model = CarModel.Create(name);

        // Assert
        Assert.NotNull(model);
        Assert.Equal(name, model.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ThrowValueIsRequiredExceptionIfNameIsInvalid(string? invalidName)
    {
        // Arrange

        // Act
        void Act()
        {
            CarModel.Create(invalidName!);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void TrimName()
    {
        // Arrange
        var name = " Rio ";

        // Act
        var model = CarModel.Create(name);

        // Assert
        Assert.Equal("Rio", model.Name);
    }

    [Fact]
    public void UpperFirstLetter()
    {
        // Arrange
        var name = "rio";

        // Act
        var model = CarModel.Create(name);

        // Assert
        Assert.Equal("Rio", model.Name);
    }

    [Fact]
    public void EqualOperatorReturnTrueForEqualBrands()
    {
        // Arrange
        var model1 = CarModel.Create("Rio");
        var model2 = CarModel.Create("Rio");

        // Act
        var actual = model1 == model2;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void EqualOperatorReturnFalseForDifferentBrands()
    {
        // Arrange
        var model1 = CarModel.Create("Rio");
        var model2 = CarModel.Create("Polo");

        // Act
        var actual = model1 == model2;

        // Assert
        Assert.False(actual);
    }
}