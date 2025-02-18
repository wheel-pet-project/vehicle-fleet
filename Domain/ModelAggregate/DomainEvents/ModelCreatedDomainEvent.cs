using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.ValueObjects;

namespace Domain.ModelAggregate.DomainEvents;

public record ModelCreatedDomainEvent : DomainEvent
{
    public ModelCreatedDomainEvent(
        Guid modelId,
        char category,
        decimal pricePerMinute,
        decimal pricePerHour,
        decimal pricePerDay)
    {
        if (modelId == Guid.Empty) 
            throw new ValueIsRequiredException($"'{nameof(modelId)}' cannot be empty");
        if (SharedKernel.ValueObjects.Category.GetSupportedCategories().Contains(category) is false) 
            throw new ValueOutOfRangeException($"'{nameof(category)}' category is not supported");
        if (pricePerMinute <= 0 || pricePerHour <= 0 || pricePerDay <= 0)
            throw new ValueOutOfRangeException($"prices must be greater than zero");
        
        ModelId = modelId;
        Category = category;
    }
    
    public Guid ModelId { get; private set; }
    public char Category { get; private set; }
    public decimal PricePerMinute { get; private set; }
    public decimal PricePerHour { get; private set; }
    public decimal PricePerDay { get; private set; }
}