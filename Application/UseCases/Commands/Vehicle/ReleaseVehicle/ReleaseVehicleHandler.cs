using Application.Ports.Postgres;
using Domain.SharedKernel.Exceptions.InternalExceptions;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.ReleaseVehicle;

public class ReleaseVehicleHandler(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ReleaseVehicleCommand, Result>
{
    public async Task<Result> Handle(
        ReleaseVehicleCommand command,
        CancellationToken _)
    {
        var vehicle = await vehicleRepository.GetById(command.VehicleId);
        if (vehicle == null)
            throw new DataConsistencyViolationException(
                $"Vehicle with id: {command.VehicleId} not found");

        vehicle.Release();

        vehicleRepository.Update(vehicle);

        return await unitOfWork.Commit();
    }
}