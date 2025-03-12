using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.AddModel;

public record AddModelCommand(
    string Brand,
    string CarModel,
    char Category,
    double PricePerMinute,
    double PricePerHour,
    double PricePerDay) : IRequest<Result<AddModelResponse>>;