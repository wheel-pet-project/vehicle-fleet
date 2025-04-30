using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Domain.SharedKernel.Exceptions.PublicExceptions;

namespace Domain.SharedKernel.ValueObjects;

/// <summary>
/// Номерной знак ТС 
/// </summary>
public class PlateNumber : ValueObject
{
    private PlateNumber()
    {
    }

    private PlateNumber(string plateNumber) : this()
    {
        Value = plateNumber;
    }

    public string Value { get; }

    public static PlateNumber Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ValueIsRequiredException(
                $"{nameof(input)} plate number cannot be null or whitespace");

        var plateNumber = input.Trim();

        if (plateNumber.Length is < 8 or > 9)
            throw new ValueIsUnsupportedException($"{nameof(plateNumber)} must be 8-9 characters");

        plateNumber = plateNumber.ToUpper();

        // паттерн для обычных номерных знаков, знаком с регионом из 3 цифр и для классических (ретро) автомобилей 
        if (Regex.IsMatch(
                plateNumber,
                @"^(([АВЕКМНОРСТУХ]\d{3}(?<!000)[АВЕКМНОРСТУХ]{1,2})(\d{2,3}))|([ТСК][АВЕКМНОРСТУХ]{2}\d{3}(?<!000))(\d{2})$")
            == false)
            throw new ValueIsInvalidException($"{nameof(plateNumber)} must be a valid number");

        return new PlateNumber(plateNumber);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}