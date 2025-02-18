using CSharpFunctionalExtensions;
using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.SharedKernel.ValueObjects;

public class CarModel : ValueObject
{
    private CarModel(){}

    private CarModel(string name) : this()
    {
        Name = name;
    }
    
    public string Name { get; private set; } = null!;

    public static CarModel Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ValueIsRequiredException("city name cannot be empty");
        
        var modelName = input.Trim();
        
        modelName = char.ToUpper(modelName[0]) + modelName[1..];
        
        return new CarModel(modelName);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}