using Application.Ports.Postgres;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.UpdateModelCategory;

public class UpdateModelCategoryHandler(
    IModelRepository modelRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateModelCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateModelCategoryCommand command, CancellationToken cancellationToken)
    {
        var model = await modelRepository.GetById(command.ModelId);
        if (model == null) return Result.Fail(new NotFound("Model not found"));

        var category = Category.Create(command.Category);

        model.UpdateCategory(category);

        modelRepository.Update(model);

        return await unitOfWork.Commit();
    }
}