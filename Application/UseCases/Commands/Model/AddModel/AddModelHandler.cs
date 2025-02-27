using Application.Ports.Postgres;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.AddModel;

public class AddModelHandler(
    IModelRepository modelRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddModelRequest, Result<AddModelResponse>>
{
    public async Task<Result<AddModelResponse>> Handle(AddModelRequest request, CancellationToken cancellationToken)
    {
        var brand = Brand.Create(request.Brand);
        var carModel = CarModel.Create(request.CarModel);
        var category = Category.Create(request.Category);
        var tariff = Tariff.Create(
            new decimal(request.PricePerMinute),
            new decimal(request.PricePerHour),
            new decimal(request.PricePerDay));

        var model = Domain.ModelAggregate.Model.Create(brand, carModel, category, tariff);

        await modelRepository.Add(model);

        var transactionResult = await unitOfWork.Commit();

        return transactionResult.IsSuccess ? Result.Ok(new AddModelResponse(model.Id)) : transactionResult;
    }
}