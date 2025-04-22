using Dapper;
using FluentResults;
using MediatR;
using Npgsql;

namespace Application.UseCases.Queries.Model.GetAllModels;

public class GetAllModelsQueryHandler(
    NpgsqlDataSource dataSource)
    : IRequestHandler<GetAllModelsQuery, Result<GetAllModelsQueryResponse>>
{
    public async Task<Result<GetAllModelsQueryResponse>> Handle(
        GetAllModelsQuery query,
        CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(_sql,
            new
            {
                Offset = CalculateOffset(query.Page, query.PageSize),
                Limit = query.PageSize
            });

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var modelsEnumerable = await connection.QueryAsync<ModelShortDapperModel>(command);
        var models = modelsEnumerable.AsList();

        var response = new GetAllModelsQueryResponse(models.Select(x =>
                new GetAllModelsQueryResponse.ModelShortView(x.Id, x.Brand, x.CarModel))
            .ToList());

        return response;
    }

    private int CalculateOffset(int? page, int? pageSize)
    {
        page ??= 1;
        pageSize ??= 10;

        return page.Value < 1
            ? 1
            : (page.Value - 1) * pageSize.Value;
    }

    private record ModelShortDapperModel(Guid Id, string Brand, string CarModel);

    private readonly string _sql =
        """
        SELECT id AS Id, 
               brand AS Brand, 
               car_model AS CarModel
        FROM model
        ORDER BY id
        OFFSET @Offset
        LIMIT @Limit
        """;
}