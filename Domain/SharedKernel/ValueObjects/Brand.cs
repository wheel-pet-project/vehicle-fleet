using CSharpFunctionalExtensions;
using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.SharedKernel.ValueObjects;

public class Brand : ValueObject
{
    private Brand()
    {
    }

    private Brand(string name) : this()
    {
        Name = name;
    }

    public string Name { get; } = null!;

    public static Brand Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ValueIsRequiredException("city name cannot be empty");

        var brandName = input.Trim();

        brandName = char.ToUpper(brandName[0]) + brandName[1..];

        return new Brand(brandName);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}