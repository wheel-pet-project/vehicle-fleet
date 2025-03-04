using Dapper;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;
using Npgsql;

namespace Application.UseCases.Queries.Vehicle.GetVehiclesInSquare;

public class GetVehiclesInSquareQueryHandler(
    NpgsqlDataSource dataSource) : IRequestHandler<GetVehiclesInSquareQuery, Result<GetVehiclesInSquareQueryResponse>>
{
    public async Task<Result<GetVehiclesInSquareQueryResponse>> Handle(
        GetVehiclesInSquareQuery request,
        CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(_sql, new { StatusId = request.FilteringStatus.Id });

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var vehiclesEnumerable = await connection.QueryAsync<VehicleAggregatedShortDapperModel>(command);
        var vehicles = vehiclesEnumerable.AsList();

        var vehiclesInSquare = GetVehiclesInSquare(vehicles,
            (request.UpperLeftLocation.Latitude, request.UpperLeftLocation.Longitude),
            (request.LowerRightLocation.Latitude, request.LowerRightLocation.Longitude));

        return Result.Ok(new GetVehiclesInSquareQueryResponse(vehiclesInSquare.Select(x =>
                new VehicleInSquareShortView(
                    x.Id,
                    x.Brand,
                    x.CarModel,
                    Color.FromName(x.Color),
                    x.Latitude,
                    x.Longitude))
            .ToList()));

        List<VehicleAggregatedShortDapperModel> GetVehiclesInSquare(
            List<VehicleAggregatedShortDapperModel> vehicles,
            (double latitude, double longitude) upperLeftLocation,
            (double latitude, double longitude) lowerRightLocation)
        {
            return vehicles.Where(x =>
                    x.Latitude <= upperLeftLocation.latitude && x.Latitude >= lowerRightLocation.latitude &&
                    x.Longitude <= lowerRightLocation.longitude && x.Longitude >= upperLeftLocation.longitude)
                .ToList();
        }
    }

    private record VehicleAggregatedShortDapperModel(
        Guid Id,
        string Brand,
        string CarModel,
        string Color,
        double Latitude,
        double Longitude);

    private readonly string _sql =
        """
        SELECT vehicle.id AS Id,
               brand AS Brand, 
               car_model AS CarModel,
               color AS Color,
               location_latitude AS Latitude,
               location_longitude AS Longitude
        FROM vehicle
        JOIN model ON vehicle.model_id = model.id
        WHERE vehicle.status_id = @StatusId
        """;
}