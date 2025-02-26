using Domain.SharedKernel.ValueObjects;

namespace Application.UseCases.Queries.Vehicle.GetAllVehicles;

public record GetAllVehiclesQueryResponse
{
    private readonly List<VehicleAggregatedShortView> _vehicles;
    public GetAllVehiclesQueryResponse(List<VehicleAggregatedShortView> vehicles)
    {
        _vehicles = vehicles;
    }
    
    public IReadOnlyList<VehicleAggregatedShortView> Vehicles => _vehicles;
}

public record VehicleAggregatedShortView(
    Guid Id,
    string Brand,
    string CarModel,
    Color Color,
    double Latitude,
    double Longitude);