using Application.Ports.Postgres;
using Domain.SharedKernel.Exceptions.InternalExceptions;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.OccupyVehicle;

public class OccupyVehicleHandler(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<OccupyVehicleCommand, Result>
{
    public async Task<Result> Handle(OccupyVehicleCommand command, CancellationToken _)
    {
        var vehicle = await vehicleRepository.GetById(command.VehicleId);
        if (vehicle == null)
            throw new DataConsistencyViolationException(
                $"Vehicle with id: {command.VehicleId} not found");

        vehicle.Occupy(command.BookingId);

        vehicleRepository.Update(vehicle);

        return await unitOfWork.Commit();
    }
}