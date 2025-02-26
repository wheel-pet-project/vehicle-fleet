using FluentResults;
using MediatR;

namespace Application.UseCases.Queries.Model.GetAllModels;

public record GetAllModelsQuery(
    int? Page = 1,
    int? PageSize = 10) : IRequest<Result<GetAllModelsQueryResponse>>;