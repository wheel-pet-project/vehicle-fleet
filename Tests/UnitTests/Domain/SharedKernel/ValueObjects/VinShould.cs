using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.ValueObjects;
using JetBrains.Annotations;
using Xunit;

namespace UnitTests.Domain.SharedKernel.ValueObjects;

[TestSubject(typeof(Vin))]
public class VinShould
{
    [Fact]
    public void CreateNewInstanceWithCorrectNumber()
    {
        // Arrange
        var input = "1DW21OPD9VKM231MN";

        // Act
        var actual = Vin.Create(input);

        // Assert
        Assert.Equal(input, actual.Number);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ThrowValueIsRequiredExceptionIfNumberIsNullOrEmpty(string? invalidNumber)
    {
        // Arrange

        // Act
        void Act() => Vin.Create(invalidNumber!);

        // Assert
        Assert.Throws<ValueIsRequiredException>(Act);
    }

    [Fact]
    public void TrimINputNumber()
    {
        // Arrange
        var input = "  1DW21OPD9VKM231MN  ";

        // Act
        var actual = Vin.Create(input);

        // Assert
        Assert.Equal(input.Trim(), actual.Number);
    }

    [Fact]
    public void ThrowValueOutOfRangeExceptionIfNumberLengthIsNot17()
    {
        // Arrange
        var numberWithInvalidLength = "1DW21OPD9VKM";

        // Act
        void Act() => Vin.Create(numberWithInvalidLength);

        // Assert
        Assert.Throws<ValueOutOfRangeException>(Act);
    }

    [Fact]
    public void ChangeCharatersToUpperCase()
    {
        // Arrange
        var input = "1dw21OPD9VKm231Mn";

        // Act
        var actual = Vin.Create(input);

        // Assert
        Assert.Equal(input.ToUpper(), actual.Number);
    }
}