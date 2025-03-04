using Application.Ports.Postgres;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.AddVehicle;

public class AddVehicleHandler(
    IModelRepository modelRepository,
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddVehicleRequest, Result<AddVehicleResponse>>
{
    public async Task<Result<AddVehicleResponse>> Handle(AddVehicleRequest request, CancellationToken cancellationToken)
    {
        var model = await modelRepository.GetById(request.ModelId);
        if (model == null) return Result.Fail(new NotFound("Model not found"));

        var plateNumber = PlateNumber.Create(request.PlateNumber);
        var color = request.Color;
        var vin = Vin.Create(request.Vin);
        var location = request.Location != null
            ? Domain.SharedKernel.ValueObjects.Location.Create(request.Location.Latitude, request.Location.Longitude)
            : null;

        var vehicle = Domain.VehicleAggregate.Vehicle.Create(model.Id, plateNumber, color, vin, location);

        await vehicleRepository.Add(vehicle);

        var commitResult = await unitOfWork.Commit();

        return commitResult.IsSuccess ? Result.Ok(new AddVehicleResponse(vehicle.Id)) : commitResult;
    }
}