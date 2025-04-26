using Domain.SharedKernel.Saga;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.ProcessSagaEvent;

public record ProcessSagaEventCommand(
    Guid SagaId,
    Guid VehicleId,
    bool IsSuccess,
    SagaMicroservice Service) : IRequest<Result>;