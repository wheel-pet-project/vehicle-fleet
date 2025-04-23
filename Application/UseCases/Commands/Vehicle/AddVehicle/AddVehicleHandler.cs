using Application.Ports.Postgres;
using Application.Ports.Postgres.Saga;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.AddVehicle;

public class AddVehicleHandler(
    IModelRepository modelRepository,
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork,
    IVehicleAddingSagaSaveOnlyRepository vehicleAddingSagaSaveOnlyRepository) : IRequestHandler<AddVehicleCommand, Result<AddVehicleResponse>>
{
    public async Task<Result<AddVehicleResponse>> Handle(
        AddVehicleCommand command,
        CancellationToken cancellationToken)
    {
        var model = await modelRepository.GetById(command.ModelId);
        if (model == null) return Result.Fail(new NotFound("Model not found"));

        var plateNumber = PlateNumber.Create(command.PlateNumber);
        var color = command.Color;
        var vin = Vin.Create(command.Vin);
        var location = command.Location != null
            ? Domain.SharedKernel.ValueObjects.Location.Create(
                command.Location.Latitude,
                command.Location.Longitude)
            : null;

        var (saga, vehicle) = Domain.VehicleAggregate.Vehicle.Create(model.Id, plateNumber, color, vin, location);

        await vehicleRepository.Add(vehicle);
        await vehicleAddingSagaSaveOnlyRepository.Add(saga);

        var commitResult = await unitOfWork.Commit();

        return commitResult.IsSuccess
            ? Result.Ok(new AddVehicleResponse(vehicle.Id))
            : commitResult;
    }
}