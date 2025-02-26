using FluentResults;
using MediatR;

namespace Application.UseCases.Queries.Model.GetModelById;

public record GetModelByIdQuery(Guid ModelId) : IRequest<Result<GetModelByIdQueryResponse>>;