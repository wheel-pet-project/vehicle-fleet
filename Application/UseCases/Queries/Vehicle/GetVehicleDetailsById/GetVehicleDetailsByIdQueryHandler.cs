using Dapper;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAggregate;
using FluentResults;
using MediatR;
using Npgsql;

namespace Application.UseCases.Queries.Vehicle.GetVehicleDetailsById;

public class GetVehicleDetailsByIdQueryHandler(
    NpgsqlDataSource dataSource)
    : IRequestHandler<GetVehicleDetailsByIdQuery, Result<GetVehicleDetailsByIdQueryResponse>>
{
    public async Task<Result<GetVehicleDetailsByIdQueryResponse>> Handle(
        GetVehicleDetailsByIdQuery request,
        CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(_sql, new { Id = request.VehicleId });

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var vehicle =
            await connection.QuerySingleOrDefaultAsync<VehicleDetailsDapperModel>(command);
        if (vehicle == null) return Result.Fail(new NotFound("Vehicle not found"));

        return Result.Ok(new GetVehicleDetailsByIdQueryResponse(
            vehicle.Id,
            Status.FromId(vehicle.StatusId),
            vehicle.Brand,
            vehicle.CarModel,
            Color.FromName(vehicle.Color),
            vehicle.PlateNumber,
            vehicle.Vin,
            vehicle.FuelLevelPercents,
            (double)vehicle.PricePerMinute,
            (double)vehicle.PricePerHour,
            (double)vehicle.PricePerDay));
    }

    private record VehicleDetailsDapperModel(
        Guid Id,
        int StatusId,
        string Brand,
        string CarModel,
        string Color,
        string PlateNumber,
        string Vin,
        int FuelLevelPercents,
        decimal PricePerMinute,
        decimal PricePerHour,
        decimal PricePerDay);

    private readonly string _sql =
        """
        SELECT vehicle.id AS Id,
               status_id AS StatusId,
               brand AS Brand, 
               car_model AS CarModel,
               color AS Color,
               plate_number AS PlateNumber,
               vin AS Vin,
               fuel_level_percents AS FuelLevelPercents,
               price_per_minute AS PricePerMinute,
               price_per_hour AS PricePerHour,
               price_per_day AS PricePerDay
        FROM vehicle
        JOIN model ON vehicle.model_id = model.id
        WHERE vehicle.id = @Id
        """;
}