using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.AddVehicle;

public class AddVehicleHandler : IRequestHandler<AddVehicleRequest, Result>
{
    public Task<Result> Handle(AddVehicleRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}