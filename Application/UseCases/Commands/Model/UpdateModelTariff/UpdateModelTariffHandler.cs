using Application.Ports.Postgres;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.UpdateModelTariff;

public class UpdateModelTariffHandler(
    IModelRepository modelRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateModelTariffRequest, Result>
{
    public async Task<Result> Handle(UpdateModelTariffRequest request, CancellationToken cancellationToken)
    {
        var model = await modelRepository.GetById(request.ModelId);
        if (model == null) return Result.Fail(new NotFound("Model not found"));

        var newTariff = Tariff.Create(
            new decimal(request.PricePerMinute),
            new decimal(request.PricePerHour),
            new decimal(request.PricePerDay));

        model.UpdateTariff(newTariff);

        modelRepository.Update(model);

        return await unitOfWork.Commit();
    }
}