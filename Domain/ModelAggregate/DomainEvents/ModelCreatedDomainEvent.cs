using Domain.SharedKernel;

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
            throw new ArgumentException($"{nameof(modelId)} cannot be empty");
        if (char.IsBetween(category, 'A', 'Z') is false)
            throw new ArgumentException(
                $"{nameof(category)} category character must be between 'A' and 'Z'");
        if (pricePerMinute <= 0 || pricePerHour <= 0 || pricePerDay <= 0)
            throw new ArgumentException("prices must be greater than zero");

        ModelId = modelId;
        Category = category;
        PricePerMinute = pricePerMinute;
        PricePerHour = pricePerHour;
        PricePerDay = pricePerDay;
    }

    public Guid ModelId { get; }
    public char Category { get; }
    public decimal PricePerMinute { get; }
    public decimal PricePerHour { get; }
    public decimal PricePerDay { get; }
}