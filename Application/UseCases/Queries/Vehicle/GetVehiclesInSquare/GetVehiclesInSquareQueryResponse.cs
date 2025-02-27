using Color = Domain.SharedKernel.ValueObjects.Color;

namespace Application.UseCases.Queries.Vehicle.GetVehiclesInSquare;

public record GetVehiclesInSquareQueryResponse
{
    private readonly List<VehicleInSquareShortView> _vehicles;

    public GetVehiclesInSquareQueryResponse(List<VehicleInSquareShortView> vehicles)
    {
        _vehicles = vehicles;
    }

    public IReadOnlyList<VehicleInSquareShortView> Vehicles => _vehicles;
}

public record VehicleInSquareShortView(
    Guid Id,
    string Brand,
    string CarModel,
    Color Color,
    double Latitude,
    double Longitude);