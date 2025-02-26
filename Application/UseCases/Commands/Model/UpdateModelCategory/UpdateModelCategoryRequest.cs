using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.UpdateModelCategory;

public record UpdateModelCategoryRequest(
    Guid ModelId,
    char Category) : IRequest<Result>;