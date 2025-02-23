using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;

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
            throw new ValueIsRequiredException($"'{nameof(modelId)}' cannot be empty");
        if (pricePerMinute <= 0 || pricePerHour <= 0 || pricePerDay <= 0)
            throw new ValueOutOfRangeException($"prices must be greater than zero");

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