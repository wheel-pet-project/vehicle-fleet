using Dapper;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;
using Npgsql;

namespace Application.UseCases.Queries.Vehicle.GetVehiclesInSquare;

public class GetVehiclesInSquareQueryHandler(
    NpgsqlDataSource dataSource)
    : IRequestHandler<GetVehiclesInSquareQuery, Result<GetVehiclesInSquareQueryResponse>>
{
    public async Task<Result<GetVehiclesInSquareQueryResponse>> Handle(
        GetVehiclesInSquareQuery query,
        CancellationToken cancellationToken)
    {
        var (upperLeftLocation, lowerRightLocation) = CreateBoundaries(query);

        var command = new CommandDefinition(Sql, new { StatusId = query.FilteringStatus.Id },
            cancellationToken: cancellationToken);
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var vehicles = (await connection.QueryAsync<VehicleAggregatedShortDapperModel>(command)).AsList();

        var vehiclesInSquare = vehicles
            .Where(x => LocationInBoundaries(x, upperLeftLocation, lowerRightLocation))
            .ToList();

        return Result.Ok(MapToResponse(vehiclesInSquare));
    }

    private (Location upperLeftLocation, Location lowerRightLocation) CreateBoundaries(GetVehiclesInSquareQuery query)
    {
        var upperLeftLocation = Location.Create(
            query.UpperLeftLocation.Latitude,
            query.UpperLeftLocation.Longitude);
        var lowerRightLocation = Location.Create(
            query.LowerRightLocation.Latitude,
            query.LowerRightLocation.Longitude);

        return (upperLeftLocation, lowerRightLocation);
    }

    private bool LocationInBoundaries(
        VehicleAggregatedShortDapperModel x,
        Location upperLeftLocation,
        Location lowerRightLocation)
    {
        return Location.Create(x.Latitude, x.Longitude)
            .InSquare(upperLeftLocation, lowerRightLocation);
    }

    private GetVehiclesInSquareQueryResponse MapToResponse(List<VehicleAggregatedShortDapperModel> vehiclesInSquare)
    {
        return new GetVehiclesInSquareQueryResponse(vehiclesInSquare.Select(x =>
                new VehicleInSquareShortView(
                    x.Id,
                    x.Brand,
                    x.CarModel,
                    Color.FromName(x.Color),
                    x.Latitude,
                    x.Longitude))
            .ToList());
    }

    private record VehicleAggregatedShortDapperModel(
        Guid Id,
        string Brand,
        string CarModel,
        string Color,
        double Latitude,
        double Longitude);

    private const string Sql =
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