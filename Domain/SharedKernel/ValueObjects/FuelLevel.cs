using CSharpFunctionalExtensions;
using Domain.SharedKernel.Exceptions.PublicExceptions;

namespace Domain.SharedKernel.ValueObjects;

/// <summary>
/// Уровень топлива ТС
/// </summary>
public class FuelLevel : ValueObject
{
    private FuelLevel()
    {
    }

    private FuelLevel(int levelPercents) : this()
    {
        Percents = levelPercents;
    }

    public int Percents { get; }

    public static FuelLevel Create(int levelPercents = 0)
    {
        if (levelPercents is < 0 or > 100)
            throw new ValueIsUnsupportedException(
                $"{nameof(levelPercents)} for fuel tank must be between 0 and 100");

        return new FuelLevel(levelPercents);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Percents;
    }
}