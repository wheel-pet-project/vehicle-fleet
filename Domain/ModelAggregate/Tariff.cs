using CSharpFunctionalExtensions;
using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.ModelAggregate;

public sealed class Tariff : Entity<Guid>
{
    private Tariff()
    {
    }

    private Tariff(decimal pricePerMinute, decimal pricePerHour, decimal pricePerDay) : this()
    {
        Id = Guid.NewGuid();
        PricePerMinute = pricePerMinute;
        PricePerHour = pricePerHour;
        PricePerDay = pricePerDay;
    }

    public decimal PricePerMinute { get; private set; }
    public decimal PricePerHour { get; private set; }
    public decimal PricePerDay { get; private set; }

    public void Update(decimal pricePerMinute, decimal pricePerHour, decimal pricePerDay)
    {
        if (pricePerMinute < 0 || pricePerHour < 0 || pricePerDay < 0)
            throw new ValueOutOfRangeException("prices for tariff must be greater than 0");
        
        if (pricePerMinute > 0) PricePerMinute = pricePerMinute;
        if (pricePerHour > 0) PricePerHour = pricePerHour;
        if (pricePerDay > 0) PricePerDay = pricePerDay;
    }

    public static Tariff Create(decimal pricePerMinute, decimal pricePerHour, decimal pricePerDay)
    {
        if (pricePerMinute <= 0)
            throw new ValueOutOfRangeException($"{nameof(pricePerMinute)} must be greater than zero");
        if (pricePerHour <= 0)
            throw new ValueOutOfRangeException($"{nameof(pricePerHour)} must be greater than zero");
        if (pricePerDay <= 0)
            throw new ValueOutOfRangeException($"{nameof(pricePerDay)} must be greater than zero");

        return new Tariff(pricePerMinute, pricePerHour, pricePerDay);
    }
}