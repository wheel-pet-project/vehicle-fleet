using Application.Ports.Postgres;
using Domain.ModelAggregate;
using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.AddModel;

public class AddModelHandler(
    IModelRepository modelRepository, 
    IUnitOfWork unitOfWork) : IRequestHandler<AddModelRequest, Result>
{
    public async Task<Result> Handle(AddModelRequest request, CancellationToken cancellationToken)
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
        
        return await unitOfWork.Commit();
    }
}