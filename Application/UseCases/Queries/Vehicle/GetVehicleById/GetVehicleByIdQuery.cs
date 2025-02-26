using FluentResults;
using MediatR;

namespace Application.UseCases.Queries.Vehicle.GetVehicleById;

public record GetVehicleByIdQuery(Guid VehicleId) : IRequest<Result<GetVehicleByIdQueryResponse>>;