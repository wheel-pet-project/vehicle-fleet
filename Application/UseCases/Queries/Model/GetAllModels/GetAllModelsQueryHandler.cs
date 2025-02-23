using FluentResults;
using MediatR;

namespace Application.UseCases.Queries.Model.GetAllModels;

public class GetAllModelsQueryHandler : IRequestHandler<GetAllModelsQuery, Result<GetAllModelsQueryResponse>>
{
    public Task<Result<GetAllModelsQueryResponse>> Handle(GetAllModelsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}