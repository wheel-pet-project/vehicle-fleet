using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAggregate;

namespace Application.UseCases.Queries.Vehicle.GetVehicleDetailsById;

public record GetVehicleDetailsByIdQueryResponse(
    Guid Id,
    Status Status,
    string Brand,
    string CarModel,
    Color Color,
    string PlateNumber,
    string Vin,
    int FuelLevelPercents,
    double PricePerMinute,
    double PricePerHour,
    double PricePerDay);