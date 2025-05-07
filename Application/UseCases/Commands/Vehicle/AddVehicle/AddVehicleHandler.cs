using Application.Ports.Postgres;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAddingSaga;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.AddVehicle;

public class AddVehicleHandler(
    IModelRepository modelRepository,
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork,
    IVehicleAddingSagaRepository vehicleAddingSagaRepository)
    : IRequestHandler<AddVehicleCommand, Result<AddVehicleResponse>>
{
    public async Task<Result<AddVehicleResponse>> Handle(AddVehicleCommand command, CancellationToken _)
    {
        var model = await modelRepository.GetById(command.ModelId);
        if (model == null) return Result.Fail(new NotFound("Model not found"));

        var (plateNumber, color, vin, location) = CreateValueObjects(command);
        var (saga, vehicle) = Domain.VehicleAggregate.Vehicle.Create(model.Id, plateNumber, color, vin, location);

        await AddVehicleAndSagaToDatabase(vehicle, saga);
        var commitResult = await unitOfWork.Commit();

        return commitResult.IsSuccess
            ? Result.Ok(new AddVehicleResponse(vehicle.Id))
            : commitResult;
    }

    private (PlateNumber plateNumber, Color color, Vin vin, Domain.SharedKernel.ValueObjects.Location? location)
        CreateValueObjects(
            AddVehicleCommand command)
    {
        var plateNumber = PlateNumber.Create(command.PlateNumber);
        var color = command.Color;
        var vin = Vin.Create(command.Vin);
        var location = command.Location != null
            ? Domain.SharedKernel.ValueObjects.Location.Create(
                command.Location.Latitude,
                command.Location.Longitude)
            : null;
        return (plateNumber, color, vin, location);
    }

    private async Task AddVehicleAndSagaToDatabase(Domain.VehicleAggregate.Vehicle vehicle, VehicleAddingSaga saga)
    {
        await vehicleRepository.Add(vehicle);
        await vehicleAddingSagaRepository.Add(saga);
    }
}