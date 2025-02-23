using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.UpdateModelTariff;

public record UpdateModelTariffRequest(
    Guid CorrelationId,
    Guid ModelId,
    double PricePerMinute,
    double PricePerHour,
    double PricePerDay) : BaseRequest(CorrelationId), IRequest<Result>;