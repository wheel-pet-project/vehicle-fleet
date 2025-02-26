using Dapper;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;
using Npgsql;

namespace Application.UseCases.Queries.Vehicle.GetAllVehicles;

public class GetAllVehiclesQueryHandler(
    NpgsqlDataSource dataSource) : IRequestHandler<GetAllVehiclesQuery, Result<GetAllVehiclesQueryResponse>>
{
    public async Task<Result<GetAllVehiclesQueryResponse>> Handle(
        GetAllVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(_sql,
            new
            {
                StatusId = request.FilteringStatus.Id,
                Offset = (request.Page - 1) * request.PageSize, 
                Limit = request.PageSize
            });

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var vehiclesEnumerable = await connection.QueryAsync<VehicleAggregatedShortDapperModel>(command);
        var vehicles = vehiclesEnumerable.AsList();
        
        return Result.Ok(new GetAllVehiclesQueryResponse(vehicles.Select(x =>
                new VehicleAggregatedShortView(
                    x.Id,
                    x.Brand,
                    x.CarModel,
                    Color.FromName(x.Color),
                    x.Latitude,
                    x.Longitude)).ToList()));
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