using Domain.SharedKernel;
using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.ModelAggregate.DomainEvents;

public record ModelCategoryUpdatedDomainEvent : DomainEvent
{
    public ModelCategoryUpdatedDomainEvent(Guid modelId, char category)
    {
        if (modelId == Guid.Empty)
            throw new ValueIsRequiredException($"'{nameof(modelId)}' cannot be empty");
        if (char.IsBetween(category, 'A', 'Z') is false)
            throw new ValueOutOfRangeException($"'{nameof(category)}' category character must be between 'A' and 'Z'");

        ModelId = modelId;
        Category = category;
    }

    public Guid ModelId { get; }
    public char Category { get; }
}