using Domain.VehicleAggregate;
using FluentResults;
using MediatR;

namespace Application.UseCases.Queries.Vehicle.GetAllVehicles;

public record GetAllVehiclesQuery(
    Status FilteringStatus,
    int? Page = 1,
    int? PageSize = 10) : IRequest<Result<GetAllVehiclesQueryResponse>>;