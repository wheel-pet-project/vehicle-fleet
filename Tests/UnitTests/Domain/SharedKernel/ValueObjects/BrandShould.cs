using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.ValueObjects;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.SharedKernel.ValueObjects;

[TestSubject(typeof(Brand))]
public class BrandShould
{
    [Fact]
    public void CreateNewInstanceWithCorrectName()
    {
        // Arrange
        var name = "Kia";

        // Act
        var brand = Brand.Create(name);

        // Assert
        Assert.NotNull(brand);
        Assert.Equal(name, brand.Name);
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
            Brand.Create(invalidName!);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void TrimName()
    {
        // Arrange
        var name = " Kia ";

        // Act
        var brand = Brand.Create(name);

        // Assert
        Assert.Equal("Kia", brand.Name);
    }

    [Fact]
    public void UpperFirstLetter()
    {
        // Arrange
        var name = "kia";

        // Act
        var brand = Brand.Create(name);

        // Assert
        Assert.Equal("Kia", brand.Name);
    }

    [Fact]
    public void EqualOperatorReturnTrueForEqualBrands()
    {
        // Arrange
        var brand1 = Brand.Create("Kia");
        var brand2 = Brand.Create("Kia");

        // Act
        var actual = brand1 == brand2;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void EqualOperatorReturnFalseForDifferentBrands()
    {
        // Arrange
        var brand1 = Brand.Create("Kia");
        var brand2 = Brand.Create("Volkswagen");

        // Act
        var actual = brand1 == brand2;

        // Assert
        Assert.False(actual);
    }
}