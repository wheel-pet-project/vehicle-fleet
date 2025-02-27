using Dapper;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAggregate;
using FluentResults;
using MediatR;
using Npgsql;

namespace Application.UseCases.Queries.Vehicle.GetVehicleById;

public class GetVehicleByIdQueryHandler(
    NpgsqlDataSource dataSource) : IRequestHandler<GetVehicleByIdQuery, Result<GetVehicleByIdQueryResponse>>
{
    public async Task<Result<GetVehicleByIdQueryResponse>> Handle(
        GetVehicleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(_sql, new { Id = request.VehicleId });

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var vehicle = await connection.QuerySingleOrDefaultAsync<VehicleDapperModel>(command);
        if (vehicle == null) return Result.Fail(new NotFound("Vehicle not found"));

        return Result.Ok(new GetVehicleByIdQueryResponse(
            vehicle.Id,
            Status.FromId(vehicle.StatusId),
            vehicle.Brand,
            vehicle.CarModel,
            Color.FromName(vehicle.Color),
            vehicle.PlateNumber,
            vehicle.FuelLevelPercents,
            (double)vehicle.PricePerMinute,
            (double)vehicle.PricePerHour,
            vehicle.Latitude,
            vehicle.Longitude));
    }

    private record VehicleDapperModel(
        Guid Id,
        int StatusId,
        string Brand,
        string CarModel,
        string Color,
        string PlateNumber,
        int FuelLevelPercents,
        decimal PricePerMinute,
        decimal PricePerHour,
        double Latitude,
        double Longitude);

    private readonly string _sql =
        """
        SELECT vehicle.id AS Id,
               status_id AS StatusId,
               brand AS Brand, 
               car_model AS CarModel,
               color AS Color,
               plate_number AS PlateNumber,
               fuel_level_percents AS FuelLevelPercents,
               price_per_minute AS PricePerMinute,
               price_per_hour AS PricePerHour,
               location_latitude AS Latitude,
               location_longitude AS Longitude
        FROM vehicle
        JOIN model ON vehicle.model_id = model.id
        WHERE vehicle.id = @Id
        """;
}