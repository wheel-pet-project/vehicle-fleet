using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.UpdateModelTariff;

public record UpdateModelTariffRequest(
    Guid ModelId,
    double PricePerMinute,
    double PricePerHour,
    double PricePerDay) : IRequest<Result>;