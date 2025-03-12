using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.SendToServiceVehicle;

public record SendToServiceVehicleCommand(Guid VehicleId) : IRequest<Result>;