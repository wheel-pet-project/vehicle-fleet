using Application.Ports.Postgres;
using Domain.SharedKernel.Errors;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.MarkAsReadiedForReleaseVehicle;

public class MarkAsReadiedForReleaseVehicleHandler(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<MarkAsReadiedForReleaseVehicleRequest, Result>
{
    public async Task<Result> Handle(MarkAsReadiedForReleaseVehicleRequest request, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetById(request.VehicleId);
        if (vehicle == null) return Result.Fail(new NotFound("Vehicle not found"));

        vehicle.MarkAsReadiedForRelease();

        vehicleRepository.Update(vehicle);

        return await unitOfWork.Commit();
    }
}