using Application.Ports.Postgres;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Model.UpdateModelTariff;

public class UpdateModelTariffHandler(
    IModelRepository modelRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateModelTariffRequest, Result>
{
    public Task<Result> Handle(UpdateModelTariffRequest request, CancellationToken cancellationToken)
    {
        var 
    }
}