using Application.Ports.Postgres;
using Domain.SharedKernel.Errors;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.DeleteVehicle;

public class DeleteVehicleHandler(
    IVehicleRepository vehicleRepository, 
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteVehicleRequest, Result>
{
    public async Task<Result> Handle(DeleteVehicleRequest request, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetById(request.VehicleId);
        if (vehicle == null) return Result.Fail(new NotFound("Vehicle not found"));
        
        vehicle.Delete();
        
        vehicleRepository.Update(vehicle);

        return await unitOfWork.Commit();
    }
}