using Application.Ports.Postgres;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.AddModel;

public class AddModelHandler(
    IModelRepository modelRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddModelCommand, Result<AddModelResponse>>
{
    public async Task<Result<AddModelResponse>> Handle(AddModelCommand command, CancellationToken _)
    {
        var (brand, carModel, category, tariff) = CreateValueObjects(command);
        var model = Domain.ModelAggregate.Model.Create(brand, carModel, category, tariff);

        await modelRepository.Add(model);

        var commitResult = await unitOfWork.Commit();

        return commitResult.IsSuccess
            ? new AddModelResponse(model.Id)
            : commitResult;
    }

    private (Brand brand, CarModel carModel, Category category, Tariff tariff) CreateValueObjects(
        AddModelCommand command)
    {
        var brand = Brand.Create(command.Brand);
        var carModel = CarModel.Create(command.CarModel);
        var category = Category.Create(command.Category);
        var tariff = Tariff.Create(
            new decimal(command.PricePerMinute),
            new decimal(command.PricePerHour),
            new decimal(command.PricePerDay));

        return (brand, carModel, category, tariff);
    }
}