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
        var upperLeftLocation = Location.Create(request.UpperLeftLocation.Latitude, 
            request.UpperLeftLocation.Longitude);
        var lowerRightLocation = Location.Create(request.LowerRightLocation.Latitude, 
            request.LowerRightLocation.Longitude);
        
        var command = new CommandDefinition(_sql, new { StatusId = request.FilteringStatus.Id });

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var vehicles = (await connection.QueryAsync<VehicleAggregatedShortDapperModel>(command)).AsList();
        
        var vehiclesInSquare = vehicles
            .Where(x => Location.Create(x.Latitude, x.Longitude).InSquare(upperLeftLocation, lowerRightLocation))
            .ToList();

        return Result.Ok(new GetVehiclesInSquareQueryResponse(vehiclesInSquare.Select(x => 
                new VehicleInSquareShortView(
                    x.Id,
                    x.Brand,
                    x.CarModel,
                    Color.FromName(x.Color),
                    x.Latitude,
                    x.Longitude))
            .ToList()));
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