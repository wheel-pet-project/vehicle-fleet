using Domain.ModelAggregate.DomainEvents;
using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.ValueObjects;

namespace Domain.ModelAggregate;

public class Model : Aggregate
{
    private Model(){}

    private Model(Brand brand, CarModel model, Category category) : this()
    {
        Id = Guid.NewGuid();
        Brand = brand;
        CarModel = model;
        Category = category;
    }
    
    public Guid Id { get; private set; }
    public Brand Brand { get; private set; } = null!;
    public CarModel CarModel { get; private set; } = null!;
    public Category Category { get; private set; } = null!;
    
    public Tariff Tariff { get; private set; } = null!;


    public static Model Create(Brand brand, CarModel model, Category category, Tariff tariff)
    {
        if (brand == null) throw new ValueIsRequiredException($"{nameof(brand)} cannot be null");
        if (model == null) throw new ValueIsRequiredException($"{nameof(model)} cannot be null");
        if (category == null) throw new ValueIsRequiredException($"{nameof(category)} cannot be null");
        if (tariff == null) throw new ValueIsRequiredException($"{nameof(tariff)} cannot be null");
        
        var newModel = new Model(brand, model, category);
        newModel.AddDomainEvent(new ModelCreatedDomainEvent(
            newModel.Id, 
            newModel.Category.Symbol,
            newModel.Tariff.PricePerMinute, 
            tariff.PricePerHour, 
            tariff.PricePerDay));
        
        return newModel;
    }
}