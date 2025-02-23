using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.AddModel;

public record AddModelRequest(
    Guid CorrelationId,
    string Brand,
    string CarModel,
    char Category,
    double PricePerMinute,
    double PricePerHour,
    double PricePerDay) : BaseRequest(CorrelationId), IRequest<Result>;