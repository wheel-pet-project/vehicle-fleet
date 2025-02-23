using FluentResults;
using MediatR;

namespace Application.UseCases.Queries.Model.GetAllModels;

public record GetAllModelsQuery(
    Guid CorrelationId,
    int? Page = null,
    int? PageSize = null) : BaseRequest(CorrelationId), IRequest<Result<GetAllModelsQueryResponse>>;