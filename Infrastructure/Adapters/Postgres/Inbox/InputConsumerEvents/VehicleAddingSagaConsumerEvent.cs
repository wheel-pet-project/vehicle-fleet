using Application.UseCases.Commands.Vehicle.ProcessSagaEvent;
using Domain.SharedKernel.Saga;
using FluentResults;
using MediatR;

namespace Infrastructure.Adapters.Postgres.Inbox.InputConsumerEvents;

public class VehicleAddingSagaConsumerEvent(
    Guid eventId,
    Guid sagaId,
    Guid vehicleId,
    bool isSuccess,
    SagaMicroservice microservice) : IInputConsumerEvent
{
    public Guid EventId { get; } = eventId;
    public Guid SagaId { get; } = sagaId;
    public Guid VehicleId { get; } = vehicleId;
    public bool IsSuccess { get; } = isSuccess;
    public SagaMicroservice Microservice { get; } = microservice;

    public IRequest<Result> ToCommand()
    {
        return new ProcessSagaEventCommand(SagaId, VehicleId, IsSuccess, Microservice);
    }
}