using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.UpdateModelCategory;

public record UpdateModelCategoryRequest(
    Guid CorrelationId,
    Guid ModelId,
    char Category) : BaseRequest(CorrelationId), IRequest<Result>;