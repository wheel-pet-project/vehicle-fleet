using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.UpdateModelCategory;

public record UpdateModelCategoryCommand(
    Guid ModelId,
    char Category) : IRequest<Result>;