using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.SharedKernel.ValueObjects;

public class Vin
{
    private Vin()
    {
    }

    private Vin(string vinNumber)
    {
        Number = vinNumber;
    }
    
    public string Number { get; private set; }

    public static Vin Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ValueIsRequiredException($"{nameof(input)} vin number is required");
        
        var vinNumber = input.Trim();

        if (vinNumber.Length != 17)
            throw new ValueOutOfRangeException($"{nameof(vinNumber)} must be 17 characters long");

        vinNumber = vinNumber.ToUpper();
        
        // todo: мб добавить regex
        return new Vin(vinNumber);
    }
}