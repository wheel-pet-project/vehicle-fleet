using FluentResults;
using MediatR;

namespace Application.UseCases.Queries.Vehicle.GetVehicleDetailsById;

public record GetVehicleDetailsByIdQuery(Guid VehicleId) : IRequest<Result<GetVehicleDetailsByIdQueryResponse>>;