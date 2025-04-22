using Application.Ports.Postgres;
using Domain.SharedKernel.Errors;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.MarkAsReadiedForReleaseVehicle;

public class MarkAsReadiedForReleaseVehicleHandler(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<MarkAsReadiedForReleaseVehicleCommand, Result>
{
    public async Task<Result> Handle(
        MarkAsReadiedForReleaseVehicleCommand command,
        CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetById(command.VehicleId);
        if (vehicle == null) return Result.Fail(new NotFound("Vehicle not found"));

        vehicle.MarkAsReadiedForRelease();

        vehicleRepository.Update(vehicle);

        return await unitOfWork.Commit();
    }
}