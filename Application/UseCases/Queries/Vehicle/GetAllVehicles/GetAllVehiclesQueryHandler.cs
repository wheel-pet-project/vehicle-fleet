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
        GetAllVehiclesQuery query,
        CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(_sql,
            new
            {
                StatusId = query.FilteringStatus.Id,
                Offset = CalculateOffset(query.Page, query.PageSize),
                Limit = query.PageSize
            });

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var vehiclesEnumerable = await connection.QueryAsync<VehicleAggregatedShortDapperModel>(command);
        var vehicles = vehiclesEnumerable.AsList();

        return Result.Ok(new GetAllVehiclesQueryResponse(vehicles.Select(x =>
                new VehicleShortView(
                    x.Id,
                    x.Brand,
                    x.CarModel,
                    Color.FromName(x.Color),
                    x.Latitude,
                    x.Longitude))
            .ToList()));
    }
    
    private int CalculateOffset(int? page, int? pageSize)
    {
        page ??= 1;
        pageSize ??= 10;
        
        return page.Value < 1
            ? 1
            : (page.Value - 1) * pageSize.Value;
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