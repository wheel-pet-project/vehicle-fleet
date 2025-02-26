using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.SendToServiceVehicle;

public record SendToServiceVehicleRequest(Guid VehicleId) : IRequest<Result>;