using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.DeleteVehicle;

public record DeleteVehicleRequest(Guid VehicleId) : IRequest<Result>;