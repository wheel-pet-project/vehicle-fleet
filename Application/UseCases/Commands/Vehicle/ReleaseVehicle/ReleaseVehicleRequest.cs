using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.ReleaseVehicle;

public record ReleaseVehicleRequest(Guid VehicleId) :  IRequest<Result>;