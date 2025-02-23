namespace Application.UseCases.Queries.Model.GetModelById;

public record GetModelByIdQueryResponse(
    Guid Id,
    string Brand,
    string CarModel,
    char Category,
    double PricePerMinute,
    double PricePerHour,
    double PricePerDay);