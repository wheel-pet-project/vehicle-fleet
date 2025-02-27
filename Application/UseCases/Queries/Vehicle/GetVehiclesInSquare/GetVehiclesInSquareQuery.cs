using Domain.VehicleAggregate;
using FluentResults;
using MediatR;

namespace Application.UseCases.Queries.Vehicle.GetVehiclesInSquare;

public record GetVehiclesInSquareQuery(
    Status FilteringStatus,
    LocationDto UpperLeftLocation,
    LocationDto LowerRightLocation) : IRequest<Result<GetVehiclesInSquareQueryResponse>>;

public record LocationDto(double Latitude, double Longitude);