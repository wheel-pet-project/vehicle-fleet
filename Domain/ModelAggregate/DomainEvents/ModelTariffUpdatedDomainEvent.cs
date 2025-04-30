using Domain.SharedKernel;

namespace Domain.ModelAggregate.DomainEvents;

public record ModelTariffUpdatedDomainEvent : DomainEvent
{
    public ModelTariffUpdatedDomainEvent(
        Guid modelId,
        decimal pricePerMinute,
        decimal pricePerHour,
        decimal pricePerDay)
    {
        if (modelId == Guid.Empty)
            throw new ArgumentException($"{nameof(modelId)} cannot be empty");
        if (pricePerMinute <= 0 || pricePerHour <= 0 || pricePerDay <= 0)
            throw new ArgumentException("prices must be greater than zero");

        ModelId = modelId;
        PricePerMinute = pricePerMinute;
        PricePerHour = pricePerHour;
        PricePerDay = pricePerDay;
    }

    public Guid ModelId { get; }
    public decimal PricePerMinute { get; }
    public decimal PricePerHour { get; }
    public decimal PricePerDay { get; }
}