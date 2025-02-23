using Domain.ModelAggregate.DomainEvents;
using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.ValueObjects;

namespace Domain.ModelAggregate;

public class Model : Aggregate
{
    private Model()
    {
    }

    private Model(Brand brand, CarModel model, Category category, Tariff tariff) : this()
    {
        Id = Guid.NewGuid();
        Brand = brand;
        CarModel = model;
        Category = category;
        Tariff = tariff;
    }

    public Guid Id { get; private set; }
    public Brand Brand { get; private set; } = null!;
    public CarModel CarModel { get; private set; } = null!;
    public Category Category { get; private set; } = null!;
    public Tariff Tariff { get; private set; } = null!;

    public void UpdateCategory(Category potentialCategory)
    {
        if (potentialCategory == null)
            throw new ValueIsRequiredException($"{nameof(potentialCategory)} cannot be null");

        Category = potentialCategory;

        AddDomainEvent(new ModelCategoryUpdatedDomainEvent(Id, Category.Symbol));
    }

    public void UpdateTariff(decimal pricePerMinute = 0, decimal pricePerHour = 0, decimal pricePerDay = 0)
    {
        Tariff.Update(pricePerMinute, pricePerHour, pricePerDay);

        AddDomainEvent(new ModelTariffUpdatedDomainEvent(
            Id,
            Tariff.PricePerMinute,
            Tariff.PricePerHour,
            Tariff.PricePerDay));
    }

    public static Model Create(Brand brand, CarModel carModel, Category category, Tariff tariff)
    {
        if (brand == null) throw new ValueIsRequiredException($"{nameof(brand)} cannot be null");
        if (carModel == null) throw new ValueIsRequiredException($"{nameof(carModel)} cannot be null");
        if (category == null) throw new ValueIsRequiredException($"{nameof(category)} cannot be null");
        if (tariff == null) throw new ValueIsRequiredException($"{nameof(tariff)} cannot be null");

        var model = new Model(brand, carModel, category, tariff);

        model.AddDomainEvent(new ModelCreatedDomainEvent(
            model.Id,
            model.Category.Symbol,
            model.Tariff.PricePerMinute,
            model.Tariff.PricePerHour,
            model.Tariff.PricePerDay));

        return model;
    }
}