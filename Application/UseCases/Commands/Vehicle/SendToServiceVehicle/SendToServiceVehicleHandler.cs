using Application.Ports.Postgres;
using Domain.SharedKernel.Errors;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.SendToServiceVehicle;

public class SendToServiceVehicleHandler(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SendToServiceVehicleRequest, Result>
{
    public async Task<Result> Handle(SendToServiceVehicleRequest request, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetById(request.VehicleId);
        if (vehicle == null) return Result.Fail(new NotFound("Vehicle not found"));
        
        vehicle.MarkAsServiced();
        
        vehicleRepository.Update(vehicle);

        return await unitOfWork.Commit();
    }
}