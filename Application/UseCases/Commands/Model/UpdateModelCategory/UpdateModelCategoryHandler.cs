using Application.Ports.Postgres;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.UpdateModelCategory;

public class UpdateModelCategoryHandler(
    IModelRepository modelRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateModelCategoryRequest, Result>
{
    public async Task<Result> Handle(UpdateModelCategoryRequest request, CancellationToken cancellationToken)
    {
        var model = await modelRepository.GetById(request.ModelId);
        if (model == null) return Result.Fail(new NotFound("Model not found"));

        var category = Category.Create(request.Category);

        model.UpdateCategory(category);

        modelRepository.Update(model);

        return await unitOfWork.Commit();
    }
}