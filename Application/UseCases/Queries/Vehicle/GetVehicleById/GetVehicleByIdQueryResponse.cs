using Domain.VehicleAggregate;
using Color = Domain.SharedKernel.ValueObjects.Color;

namespace Application.UseCases.Queries.Vehicle.GetVehicleById;

public record GetVehicleByIdQueryResponse(
    Guid Id,
    Status Status,
    string Brand,
    string CarModel,
    Color Color,
    string PlateNumber,
    int FuelLevelPercents,
    double PricePerMinute,
    double PricePerHour,
    double Latitude,
    double Longitude);