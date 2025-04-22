using Dapper;
using Domain.SharedKernel.Errors;
using FluentResults;
using MediatR;
using Npgsql;

namespace Application.UseCases.Queries.Model.GetModelById;

public class GetModelByIdQueryHandler(
    NpgsqlDataSource dataSource)
    : IRequestHandler<GetModelByIdQuery, Result<GetModelByIdQueryResponse>>
{
    public async Task<Result<GetModelByIdQueryResponse>> Handle(
        GetModelByIdQuery request,
        CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(_sql, new { Id = request.ModelId });

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var model = await connection.QuerySingleOrDefaultAsync<ModelDapperModel>(command);
        if (model == null) return Result.Fail(new NotFound("Model not found"));

        return Result.Ok(new GetModelByIdQueryResponse(model.Id,
            model.Brand,
            model.CarModel,
            model.Category,
            (double)model.PricePerMinute,
            (double)model.PricePerHour,
            (double)model.PricePerDay));
    }

    private record ModelDapperModel(
        Guid Id,
        string Brand,
        string CarModel,
        char Category,
        decimal PricePerMinute,
        decimal PricePerHour,
        decimal PricePerDay);

    private readonly string _sql =
        """
        SELECT id AS Id,
               brand AS Brand,
               car_model AS CarModel,
               category AS Category,
               price_per_minute AS PricePerMinute,
               price_per_hour AS PricePerHour,
               price_per_day AS PricePerDay
        FROM model
        WHERE id = @Id
        """;
}