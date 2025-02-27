using Domain.SharedKernel.ValueObjects;

namespace Application.UseCases.Queries.Vehicle.GetAllVehicles;

public record GetAllVehiclesQueryResponse
{
    private readonly List<VehicleShortView> _vehicles;

    public GetAllVehiclesQueryResponse(List<VehicleShortView> vehicles)
    {
        _vehicles = vehicles;
    }

    public IReadOnlyList<VehicleShortView> Vehicles => _vehicles;
}

public record VehicleShortView(
    Guid Id,
    string Brand,
    string CarModel,
    Color Color,
    double Latitude,
    double Longitude);