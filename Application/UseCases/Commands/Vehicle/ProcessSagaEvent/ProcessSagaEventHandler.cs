using Application.Ports.Postgres;
using Domain.SharedKernel.Exceptions.InternalExceptions;
using Domain.SharedKernel.Saga;
using Domain.VehicleAddingSaga;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.ProcessSagaEvent;

public class ProcessSagaEventHandler(
    IVehicleAddingSagaRepository sagaRepository,
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ProcessSagaEventCommand, Result>
{
    public async Task<Result> Handle(ProcessSagaEventCommand command, CancellationToken _)
    {
        var saga = await GetSagaOrThrowIfNotFound(command);

        saga.ProcessSagaEvent(MapToSagaEvent(command));

        sagaRepository.Update(saga);
        await UpdateRespectiveVehicleIfNeeded(saga.State, saga.VehicleId);

        return await unitOfWork.Commit();
    }

    private async Task<VehicleAddingSaga> GetSagaOrThrowIfNotFound(ProcessSagaEventCommand command)
    {
        var saga = await sagaRepository.GetById(command.SagaId);
        if (saga == null)
            throw new DataConsistencyViolationException(
                $"Saga with id: {command.SagaId} and vehicle id: {command.VehicleId} doesn't exist");

        return saga;
    }

    private async ValueTask UpdateRespectiveVehicleIfNeeded(ISagaState<IProcessState> sagaState, Guid vehicleId)
    {
        if (sagaState is { IsCompleted: true } or { IsFaulted: true })
        {
            var vehicle = await vehicleRepository.GetById(vehicleId);
            if (vehicle == null)
                throw new DataConsistencyViolationException(
                    $"Vehicle with id: {vehicleId} doesn't exist");

            if (sagaState is { IsCompleted: true }) vehicle.MarkAsAdded();
            else if (sagaState is { IsFaulted: true }) vehicle.MarkAsNotAdded();

            vehicleRepository.Update(vehicle);
        }
    }

    private VehicleAddingSagaEvent MapToSagaEvent(ProcessSagaEventCommand command)
    {
        return new VehicleAddingSagaEvent(
            command.SagaId,
            command.VehicleId,
            command.IsSuccess,
            command.Service);
    }
}