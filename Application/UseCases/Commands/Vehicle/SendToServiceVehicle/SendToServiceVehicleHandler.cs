using Application.Ports.Postgres;
using Domain.SharedKernel.Errors;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.SendToServiceVehicle;

public class SendToServiceVehicleHandler(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SendToServiceVehicleCommand, Result>
{
    public async Task<Result> Handle(
        SendToServiceVehicleCommand command,
        CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetById(command.VehicleId);
        if (vehicle == null) return Result.Fail(new NotFound("Vehicle not found"));

        vehicle.MarkAsServiced();

        vehicleRepository.Update(vehicle);

        return await unitOfWork.Commit();
    }
}