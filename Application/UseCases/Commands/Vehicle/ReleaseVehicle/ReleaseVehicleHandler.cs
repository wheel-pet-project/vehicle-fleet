using Application.Ports.Postgres;
using Domain.SharedKernel.Errors;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.ReleaseVehicle;

public class ReleaseVehicleHandler(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ReleaseVehicleRequest, Result>
{
    public async Task<Result> Handle(ReleaseVehicleRequest request, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetById(request.VehicleId);
        if (vehicle == null) return Result.Fail(new NotFound("Vehicle not found"));

        vehicle.Release();

        vehicleRepository.Update(vehicle);

        return await unitOfWork.Commit();
    }
}