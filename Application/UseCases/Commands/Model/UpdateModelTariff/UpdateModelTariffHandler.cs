using Application.Ports.Postgres;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.UpdateModelTariff;

public class UpdateModelTariffHandler(
    IModelRepository modelRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateModelTariffCommand, Result>
{
    public async Task<Result> Handle(UpdateModelTariffCommand command, CancellationToken _)
    {
        var model = await modelRepository.GetById(command.ModelId);
        if (model == null) return Result.Fail(new NotFound("Model not found"));

        var newTariff = CreateNewTariff(command);

        model.UpdateTariff(newTariff);

        modelRepository.Update(model);

        return await unitOfWork.Commit();
    }

    private Tariff CreateNewTariff(UpdateModelTariffCommand command)
    {
        var newTariff = Tariff.Create(
            new decimal(command.PricePerMinute),
            new decimal(command.PricePerHour),
            new decimal(command.PricePerDay));

        return newTariff;
    }
}