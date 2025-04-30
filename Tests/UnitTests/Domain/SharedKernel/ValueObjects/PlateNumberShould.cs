using Domain.SharedKernel.Exceptions.PublicExceptions;
using Domain.SharedKernel.ValueObjects;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.SharedKernel.ValueObjects;

[TestSubject(typeof(PlateNumber))]
public class PlateNumberShould
{
    [Fact]
    public void CreateNewInstanceWithCorrectNumber()
    {
        // Arrange
        var input = "К398ОТ777";

        // Act
        var actual = PlateNumber.Create(input);

        // Assert
        Assert.Equal(input, actual.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ThrowValueIsRequiredExceptionIfNumberIsNullOrEmpty(string? invalidNumber)
    {
        // Arrange

        // Act
        void Act()
        {
            PlateNumber.Create(invalidNumber);
        }

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void TrimInputNumber()
    {
        // Arrange
        var input = "  К398ОТ777  ";

        // Act
        var actual = PlateNumber.Create(input);

        // Assert
        Assert.Equal(input.Trim(), actual.Value);
    }

    [Theory]
    [InlineData("К398ОТ7")]
    [InlineData("К398ОТ7777")]
    public void ThrowValueOutOfRangeExceptionIfNumberLessThan8orGreaterThan9(string invalidNumber)
    {
        // Arrange

        // Act
        void Act()
        {
            PlateNumber.Create(invalidNumber);
        }

        // Assert
        Assert.Throws<ValueIsUnsupportedException>(Act);
    }

    [Fact]
    public void ChangeCharactersToUpperCase()
    {
        // Arrange
        var input = "к398от777";

        // Act
        var actual = PlateNumber.Create(input);

        // Assert
        Assert.Equal(input.ToUpper(), actual.Value);
    }

    [Theory]
    [InlineData("398ОКТ77")]
    [InlineData("К398ОТE77")]
    [InlineData("К39ОТ777")]
    [InlineData("К98ОТ7777")]
    public void ThrowValueIsInvalidExceptionIfNumberIsInvalid(string invalidNumber)
    {
        // Arrange

        // Act
        void Act()
        {
            PlateNumber.Create(invalidNumber);
        }

        // Assert
        Assert.Throws<ValueIsInvalidException>(Act);
    }

    [Fact]
    public void ThrowValueOutOfRangeExceptionIfNumberContainsLatinCharacters()
    {
        // Arrange
        var input = "E214UG77";

        // Act
        void Act()
        {
            PlateNumber.Create(input);
        }

        // Assert
        Assert.Throws<ValueIsInvalidException>(Act);
    }

    [Fact]
    public void EqualOperatorReturnTrueForEqualNumbers()
    {
        // Arrange
        var input = "К398ОТ777";
        var number1 = PlateNumber.Create(input);
        var number2 = PlateNumber.Create(input);

        // Act
        var actual = number1 == number2;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void NotEqualOperatorReturnTrueForNotEqualNumbers()
    {
        // Arrange
        var number1 = PlateNumber.Create("К398ОТ777");
        var number2 = PlateNumber.Create("М124МК77");

        // Act
        var actual = number1 != number2;

        // Assert
        Assert.True(actual);
    }
}